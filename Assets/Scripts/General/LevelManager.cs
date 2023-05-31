using System;
using System.Collections.Generic;
using Data;
using MusaUtils.Pooling;
using UnityEngine;

namespace General
{
    [Serializable]
    public class LayerInfo
    {
        public List<BlockInfo> blocks;
        public List<PileInfo> piles;
    }

    [Serializable]
    public class PileInfo
    {
        public MonoPools pileObject;
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

        private void Start()
        {
            dataContainer = DataContainer.dataContainer;
            SetLevel(dataContainer.levels[PlayerPrefs.GetInt("Level")]);
        }

        private void SetLevel(LevelData data)
        {
            for (var i = 0; i < data.layers.Count; i++)
            {
                for (var j = 0; j < data.layers[i].piles.Count; j++)
                {
                    var block = AquaPoolManager.PoolInit().GetObject(dataContainer.blockPool);
                    block.transform.parent = transform;
                    block.transform.position = new Vector3(data.layers[i].piles[j].pilePosition.x,
                        data.layers[i].piles[j].pilePosition.y,
                        -data.layers.Count + i);

                    if (block.TryGetComponent(out BlockBehaviours blockBehaviours))
                    {
                        blocks.Add(blockBehaviours);
                    }
                }
            }
        }

        private void SetBlocks()
        {
            
        }

        private void OnEnable()
        {
        }

        private void OnDisable()
        {
        }
    }
}