using System;
using System.Collections.Generic;
using Data;
using General;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Enterance
{
    public class EnteranceGeneralUi : MonoBehaviour
    {
        private DataContainer dataContainer;

        [Header("Top Bar Ui Elements")] 
        [SerializeField] private List<CurrencyUiElements> currencyElements;
        
        private void Start()
        {
            dataContainer = DataContainer.dataContainer;
            SetCurrencyUis();
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

    [Serializable]
    public class CurrencyUiElements
    {
        public CurrencyType currencyType;
        public TextMeshProUGUI currencyText;
        public Image currencyImage;
    }
}
