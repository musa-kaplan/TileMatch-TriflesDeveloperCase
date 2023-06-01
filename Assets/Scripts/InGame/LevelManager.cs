using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Data;
using General;
using MusaUtils;
using MusaUtils.Templates.HyperCasual;
using UnityEngine;

namespace InGame
{
    [Serializable]
    public class LayerInfo
    {
        public List<PileInfo> piles;
    }

    [Serializable]
    public class PileInfo
    {
        public GameObject pileObject;
        public Vector2 pilePosition;
    }

    [Serializable]
    public class BlockInfo
    {
        public int blockIndex;
        public Sprite blockSprite;
    }

    public class LevelManager : MonoBehaviour
    {
        private DataContainer dataContainer;
        private List<BlockBehaviours> blocks = new List<BlockBehaviours>();

        public GameStates currState;
        public float gameTimer;

        private void Start()
        {
            dataContainer = DataContainer.dataContainer;
            SetLevel(dataContainer.levels[PlayerPrefs.GetInt("Level")]);
        }

        private void Update()
        {
            if (currState.Equals(GameStates.Started))
            {
                gameTimer += Time.deltaTime;
            }
        }

        private void SetLevel(LevelData data)
        {
            for (var i = 0; i < data.layers.Count; i++)
            {
                for (var j = 0; j < data.layers[i].piles.Count; j++)
                {
                    var pile = Instantiate(data.layers[i].piles[j].pileObject);
                    pile.transform.parent = transform;
                    pile.transform.position = new Vector3(data.layers[i].piles[j].pilePosition.x,
                        data.layers[i].piles[j].pilePosition.y,
                        -data.layers.Count + i);

                    var childCount = pile.transform.childCount;
                    for (var k = 0; k < childCount; k++)
                    {
                        if (pile.transform.GetChild(k).TryGetComponent(out BlockBehaviours blockBehaviours))
                        {
                            blocks.Add(blockBehaviours);
                        }
                    }
                }
            }
            
            SetBlocks(data);
        }

        private void SetBlocks(LevelData data)
        {
            var totalCount = 0;
            for (var i = 0; i < (blocks.Count / 3); i++)
            {
                var blockInfo = i < data.blockTypes.Count ? data.blockTypes[i] : data.blockTypes.FromList();
                for (var j = 0; j < 3; j++)
                {
                    var block = blocks[totalCount];
                    block.myInfo = blockInfo;
                    block.SetMe();
                    totalCount++;
                }
            }
        }

        private async void CheckLevelWin(BlockBehaviours b)
        {
            blocks.Remove(b);
            if (blocks.Count <= 0)
            {
                var currLevel = dataContainer.levels[PlayerPrefs.GetInt("Level")];
                currLevel.isCompleted = true;
                
                if (gameTimer <= currLevel.twoStarTimeLimit) {currLevel.earnedStars = gameTimer <= currLevel.threeStarTimeLimit ? 3 : 2; }
                else { currLevel.earnedStars = 1; }
                
                await UniTask.Delay(TimeSpan.FromSeconds(1f));
                GameEvents.StateChanged(GameStates.Win);
            }
        }

        private void GameStateChanged(GameStates state)
        {
            currState = state;
        }

        private void OnEnable()
        {
            GameEvents.onBlockBlasted += CheckLevelWin;
            GameEvents.onStateChanged += GameStateChanged;
        }

        private void OnDisable()
        {
            GameEvents.onBlockBlasted -= CheckLevelWin;
            GameEvents.onStateChanged -= GameStateChanged;
        }
    }
}