using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MusaUtils.Templates.HyperCasual;
using UnityEngine;

namespace InGame
{
    public class BoardManager : MonoBehaviour
    {
        [SerializeField] private List<Transform> slots;

        private List<BlockBehaviours> blocks = new List<BlockBehaviours>();
        private int currBlockCount;

        public Transform GiveSlot()
        {
            currBlockCount++;
            return slots[currBlockCount - 1];
        }

        private void Update()
        {
            SetBlockPositions();
        }

        public bool HaveSlot()
        {
            return currBlockCount < slots.Count;
        }

        private void AddBlock(BlockBehaviours b)
        {
            blocks.Add(b);
            
            CheckBlocks();
        }

        private void CheckBlocks()
        {
            for (var i = 0; i < blocks.Count; i++)
            {
                int count = 0;
                var blockList = new List<BlockBehaviours>();
                for (var j = 0; j < blocks.Count; j++)
                {
                    if (blocks[i].myInfo.blockIndex.Equals(blocks[j].myInfo.blockIndex))
                    {
                        count++;
                        blockList.Add(blocks[j]);
                    }
                }

                if (count >= 3)
                {
                    blockList.Add(blocks[i]);

                    for (var j = 0; j < 3; j++)
                    {
                        blocks.Remove(blockList[j]);
                    }
                    
                    BlastBlocks(blockList);
                }
            }

            if (currBlockCount >= slots.Count)
            {
                GameEvents.StateChanged(GameStates.Lose);
            }
        }

        private void SetBlockPositions()
        {
            for (var i = 0; i < blocks.Count; i++)
            {
                blocks[i].transform.position = Vector3.Lerp(blocks[i].transform.position, slots[i].position, .35f);
            }
        }

        private async void BlastBlocks(List<BlockBehaviours> blastBlocks)
        {
            for (int i = 0; i < blastBlocks.Count; i++)
            {
                GameEvents.BlockBlasted(blastBlocks[i]);
                await UniTask.Delay(TimeSpan.FromSeconds(.15f));
            }
            currBlockCount -= 3;
        }

        private void OnEnable()
        {
            GameEvents.onBlockFlied += AddBlock;
        }

        private void OnDisable()
        {
            GameEvents.onBlockFlied -= AddBlock;
        }
    }
}
