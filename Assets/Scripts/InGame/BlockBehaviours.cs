using System;
using Cysharp.Threading.Tasks;
using Data;
using DG.Tweening;
using General;
using MoreMountains.NiceVibrations;
using MusaUtils.Templates.HyperCasual;
using UnityEngine;

namespace InGame
{
    public class BlockBehaviours : MonoBehaviour
    {
        [HideInInspector] public BlockInfo myInfo;

        [SerializeField] private SpriteRenderer mySprite;
        [SerializeField] private SpriteRenderer myLockedSprite;
        [SerializeField] private Color myLockedColor;

        private DataContainer dataContainer;
        private GameStates currState;
        private Tweener moveTween;
        private bool isLocked = true;
        private bool isDone;

        public void SetMe()
        {
            dataContainer = DataContainer.dataContainer;
            mySprite.sprite = myInfo.blockSprite;
        }

        private void OnMouseDown()
        {
            if(isLocked) return;
            if (CanHopUp())
            {
                if (!currState.Equals(GameStates.Started)) { GameEvents.StateChanged(GameStates.Started); }
                dataContainer.BlockClicked();
                isDone = true;
                LetsHopUp();
            }
        }

        private async void LetsHopUp()
        {
            if(PlayerPrefs.GetInt("Haptic").Equals(1)){MMVibrationManager.Haptic(HapticTypes.Selection);}
            transform.DOPunchScale(Vector3.one * .1f, .2f, 1);
            transform.DOMove(dataContainer.boardManager.GiveSlot().position, .4f);

            await UniTask.Delay(TimeSpan.FromSeconds(.4f));
            transform.DOScale(Vector3.one * .75f, .1f);

            await UniTask.Delay(TimeSpan.FromSeconds(.1f));
            GameEvents.BlockFlied(this);
        }

        private void LetsBlast(BlockBehaviours b)
        {
            if (b.Equals(this))
            {
                if(PlayerPrefs.GetInt("Haptic").Equals(1)){MMVibrationManager.Haptic(HapticTypes.HeavyImpact);}
                var position = transform.position;
                CurrencyInteraction.CurrencyGained(CurrencyType.Gold, 1, position);
                dataContainer.particleManager.PlayParticle(ParticleTypes.BlockBlast, position);
                gameObject.SetActive(false);
            }
        }

        private bool CanHopUp()
        {
            return !isDone && !currState.Equals(GameStates.Lose) && !currState.Equals(GameStates.Win) &&
                   dataContainer.boardManager.HaveSlot() && !dataContainer.isBlockClicked;
        }

        private RaycastHit hit;
        
        private async void CheckIfAnyBlockAboveMe(BlockBehaviours b)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(.5f));
            Physics.Raycast(transform.position, -transform.forward, out hit);

            if (hit.transform != null)
            {
                if (hit.transform.CompareTag("Block"))
                {
                    myLockedSprite.DOColor(myLockedColor, .25f);
                    isLocked = true;
                }
            }
            else
            {
                myLockedSprite.DOColor(Color.white, .25f);
                isLocked = false;
            }
        }

        private void GameStateChanged(GameStates state) => currState = state;

        private void OnEnable()
        {
            GameEvents.onStateChanged += GameStateChanged;
            GameEvents.onBlockBlasted += LetsBlast;
            GameEvents.onBlockFlied += CheckIfAnyBlockAboveMe;

            CheckIfAnyBlockAboveMe(this);
        }

        private void OnDisable()
        {
            GameEvents.onStateChanged -= GameStateChanged;
            GameEvents.onBlockBlasted -= LetsBlast;
            GameEvents.onBlockFlied -= CheckIfAnyBlockAboveMe;
        }
    }
}