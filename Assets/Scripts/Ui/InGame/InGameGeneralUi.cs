using System.Collections.Generic;
using General;
using TMPro;
using Ui.Enterance;
using UnityEngine;

namespace Ui.InGame
{
    public class InGameGeneralUi : MonoBehaviour
    {
        private DataContainer dataContainer;

        [SerializeField] private RectTransform winCanvas;
        [SerializeField] private RectTransform loseCanvas;
        [SerializeField] private TextMeshProUGUI levelText;
        
        [Header("Top Bar Ui Elements")] 
        [SerializeField] private List<CurrencyUiElements> currencyElements;
        
        private void Start()
        {
            dataContainer = DataContainer.dataContainer;
            SetCurrencyUis();
            levelText.text = "Level " + (PlayerPrefs.GetInt("Level") + 1);
        }

        private void SetCurrencyUis()
        {
            foreach (var cElement in currencyElements)
            {
                cElement.currencyImage.sprite =
                    dataContainer.walletManager.GetCurrency(cElement.currencyType).currencyImage;

                cElement.currencyText.text = dataContainer.walletManager.GetStringFormatOfCurrency(cElement.currencyType);
            }
        }
    }
}
