using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Data;
using DG.Tweening;
using General;
using InGame;
using MusaUtils.Templates.HyperCasual;
using TMPro;
using Ui.Enterance;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Ui.InGame
{
    public class InGameGeneralUi : MonoBehaviour
    {
        private DataContainer dataContainer;

        [SerializeField] private RectTransform winCanvas;
        [SerializeField] private RectTransform loseCanvas;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button homeButton;
        [SerializeField] private Button retryButton;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI timerText;

        [Header("Win Canvas Elements")] 
        [SerializeField] private Image[] nonStarImages;
        [SerializeField] private Image[] starImages;
        [SerializeField] private TextMeshProUGUI earnedMoneyCountText;
        [SerializeField] private TextMeshProUGUI earnedCupCountText;

        [Header("Top Bar Ui Elements")] [SerializeField]
        private List<CurrencyUiElements> currencyElements;

        private int currLevelIndex;
        private int inGameMoneyCount;

        private void Start()
        {
            dataContainer = DataContainer.dataContainer;
            SetCurrencyUis();
            currLevelIndex = PlayerPrefs.GetInt("Level");
            levelText.text = "Level " + (currLevelIndex + 1);
        }

        private void Update()
        {
            var gameTimer = dataContainer.levelManager.gameTimer;
            var minutes = Mathf.Floor(gameTimer / 60);
            var seconds = Mathf.RoundToInt(gameTimer % 60);
            var min = "";
            var sec = "";
 
            if(minutes < 10) {
                min = "0" + minutes;
            }
            if(seconds < 10) {
                sec = "0" + Mathf.RoundToInt(seconds);
            }
            else
            {
                sec = Mathf.RoundToInt(seconds).ToString();
            }

            timerText.text = min + ":" + sec;
        }

        private void NextLevelButton()
        {
            currLevelIndex++;
            PlayerPrefs.SetInt("Level", currLevelIndex);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void RetryButton()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void HomeButton()
        {
            SceneManager.LoadScene("Enterance");
        }

        private void SetCurrencyUis()
        {
            foreach (var cElement in currencyElements)
            {
                cElement.currencyImage.sprite =
                    dataContainer.walletManager.GetCurrency(cElement.currencyType).currencyImage;

                cElement.currencyText.text =
                    dataContainer.walletManager.GetStringFormatOfCurrency(cElement.currencyType);
            }
        }

        private async void SetWinCanvas()
        {
            int moneyAmount = 0;
            DOTween.To(() => moneyAmount, x => moneyAmount = x, inGameMoneyCount, 1f).OnUpdate(() =>
            {
                earnedMoneyCountText.text = moneyAmount.ToString();
            });

            int cupAmount = 0;
            DOTween.To(() => cupAmount, x => cupAmount = x, (dataContainer.levelManager.currentLevel.earnedStars * 6), 1f).OnUpdate(() =>
            {
                earnedCupCountText.text = cupAmount.ToString();
            });
            
            for (var i = 0; i < nonStarImages.Length; i++)
            {
                nonStarImages[i].transform.DOScale(Vector3.one, .25f).SetEase(dataContainer.generalVisualData.blopInCurve);
                starImages[i].gameObject.SetActive(dataContainer.levelManager.currentLevel.earnedStars >= (i + 1));
                await UniTask.Delay(TimeSpan.FromSeconds(.2f));
            }
        }

        private void GameStateChanged(GameStates state)
        {
            if (state.Equals(GameStates.Win))
            {
                winCanvas.gameObject.SetActive(true);
                SetWinCanvas();
                winCanvas.DOScale(Vector3.one, .5f).SetEase(dataContainer.generalVisualData.blopInCurve);
            }
            else if (state.Equals(GameStates.Lose))
            {
                loseCanvas.gameObject.SetActive(true);
                loseCanvas.DOScale(Vector3.one, .5f).SetEase(dataContainer.generalVisualData.blopInCurve);
            }
        }

        private async void OnMoneyUpdated(CurrencyType t, int c)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(.1f));
            SetCurrencyUis();
            if (t.Equals(CurrencyType.Gold)) {inGameMoneyCount++; }
        }

        private void OnEnable()
        {
            GameEvents.onStateChanged += GameStateChanged;
            WalletManager.onIncreaseCurrency += OnMoneyUpdated;
            WalletManager.onReduceCurrency += OnMoneyUpdated;

            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(NextLevelButton);

            homeButton.onClick.RemoveAllListeners();
            homeButton.onClick.AddListener(HomeButton);

            retryButton.onClick.RemoveAllListeners();
            retryButton.onClick.AddListener(RetryButton);
        }

        private void OnDisable()
        {
            GameEvents.onStateChanged -= GameStateChanged;
            WalletManager.onIncreaseCurrency -= OnMoneyUpdated;
            WalletManager.onReduceCurrency -= OnMoneyUpdated;
        }
    }
}