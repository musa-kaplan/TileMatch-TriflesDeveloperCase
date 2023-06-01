using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Data;
using DG.Tweening;
using General;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Enterance
{
    public class LeaderboardUi : MonoBehaviour
    {
        [SerializeField] private Animator leaderboardButtonAnimator;
        [SerializeField] private Button leaderboardButton;
        [SerializeField] private Button exitButton;
        [SerializeField] private GameObject leaderboardCanvas;
        [SerializeField] private RectTransform contentCanvas;

        [SerializeField] private List<LeaderboardUiElement> leaderboardUiElements;

        private DataContainer dataContainer;
        private bool isCanvasOpen;
        private bool buttonInteractableState;
        private static readonly int IsOpened = Animator.StringToHash("IsOpened");

        private void Start()
        {
            dataContainer = DataContainer.dataContainer;
            LeaderboardManager.SendPlayfabLeaderboard(dataContainer.walletManager.GetCurrency(CurrencyType.Cup).amount);
        }

        private async void CheckLeaderboardState()
        {
            if(buttonInteractableState) return;
            buttonInteractableState = true;
            exitButton.interactable = leaderboardButton.interactable = false;
            isCanvasOpen = !isCanvasOpen;
            leaderboardButtonAnimator.SetBool(IsOpened, isCanvasOpen);
            
            if (isCanvasOpen)
            {
                var list = dataContainer.leaderboardManager.GiveLeaderboardPlayers();
                await UniTask.Delay(TimeSpan.FromSeconds(.5f));
                for (var i = 0; i < 10; i++)
                {
                    if (i < list.Count)
                    {
                        leaderboardUiElements[i].SetMe(list[i].rank, list[i].playerName, list[i].rankScore);
                    }
                    else
                    {
                        leaderboardUiElements[i].ClearMe();
                    }
                }
            }
            
            leaderboardCanvas.transform.DOScale(isCanvasOpen ? Vector3.one : Vector3.zero, .25f);
            await UniTask.Delay(TimeSpan.FromSeconds(.25f));
            contentCanvas.anchoredPosition = Vector2.zero;
            buttonInteractableState = false;
            exitButton.interactable = leaderboardButton.interactable = true;
        }

        private void OnEnable()
        {
            leaderboardButton.onClick.RemoveAllListeners();
            leaderboardButton.onClick.AddListener(CheckLeaderboardState);
            
            exitButton.onClick.RemoveAllListeners();
            exitButton.onClick.AddListener(CheckLeaderboardState);
        }
    }
}
