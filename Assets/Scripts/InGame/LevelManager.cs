using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Data;
using General;
using MusaUtils;
using MusaUtils.Pooling;
using MusaUtils.Templates.HyperCasual;
using UnityEngine;
using Random = UnityEngine.Random;

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

        public GameStates currState = GameStates.Enterance;
        [HideInInspector] public LevelData currentLevel;
        private int totalBlockCount;
        private int currLevelIndex;
        public float gameTimer;

        private void Start()
        {
            dataContainer = DataContainer.dataContainer;

            currLevelIndex = PlayerPrefs.GetInt("Level");
            currentLevel =
                dataContainer.levels[
                    currLevelIndex > (dataContainer.levels.Count - 1)
                        ? Random.Range(0, dataContainer.levels.Count)
                        : currLevelIndex];
            SetLevel(currentLevel);
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
                            totalBlockCount++;
                        }
                    }
                }
            }
            
            SetBlocks(data);
        }

        private void SetBlocks(LevelData data)
        {
            var totalCount = blocks.Count / 3;

            for (var i = 0; i < totalCount; i++)
            {
                var blockInfo = i < data.blockTypes.Count ? data.blockTypes[i] : data.blockTypes.FromList();
                for (var j = 0; j < 3; j++)
                {
                    var block = blocks.FromList();
                    Debug.Log(blockInfo.blockIndex + " => " + block.GetInstanceID());
                    block.myInfo = blockInfo;
                    block.SetMe();
                    blocks.Remove(block);
                }
            }
        }

        private async void CheckLevelWin()
        {
            totalBlockCount -= 3;
            if (totalBlockCount <= 0)
            {
                currentLevel.isCompleted = true;
                if (currLevelIndex < (dataContainer.levels.Count - 1))
                {
                    dataContainer.levels[currLevelIndex + 1].isUnlocked = true;
                }
                
                if (gameTimer <= currentLevel.twoStarTimeLimit) {currentLevel.earnedStars = gameTimer <= currentLevel.threeStarTimeLimit ? 3 : 2; }
                else { currentLevel.earnedStars = 1; }
                
                WalletManager.IncreaseCurrency(CurrencyType.Cup, currentLevel.earnedStars * 6);
                currentLevel.Save();

                await UniTask.Delay(TimeSpan.FromSeconds(2f));
                GameEvents.StateChanged(GameStates.Win);
            }
        }

        private void GameStateChanged(GameStates state)
        {
            currState = state;
        }

        private void OnEnable()
        {
            GameEvents.onCheckLevelWin += CheckLevelWin;
            GameEvents.onStateChanged += GameStateChanged;
        }

        private void OnDisable()
        {
            GameEvents.onCheckLevelWin -= CheckLevelWin;
            GameEvents.onStateChanged -= GameStateChanged;
        }
    }
}