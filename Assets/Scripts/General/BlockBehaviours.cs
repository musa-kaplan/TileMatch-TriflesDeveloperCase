using UnityEngine;

namespace General
{
    public class BlockBehaviours : MonoBehaviour
    {
        [HideInInspector] public BlockInfo myInfo;

        [SerializeField] private SpriteRenderer mySprite;

        public void SetMe()
        {
            mySprite.sprite = myInfo.blockSprite;
        }
    }
}