using System;
using UnityEngine;

namespace Data
{
    public enum CurrencyType
    {
        Gold,
        Cup
    }

    [Serializable]
    public class CurrencyInfo
    {
        public CurrencyType currencyType;
        public int amount;
        
        [Header("Visuals")]
        public Sprite currencyImage;
    }

    public class WalletManager : MonoBehaviour
    {
        public static event Action<CurrencyType, int> onIncreaseCurrency;
        public static void IncreaseCurrency(CurrencyType ct, int a) => onIncreaseCurrency?.Invoke(ct, a);


        public static event Action<CurrencyType, int> onReduceCurrency;
        public static void ReduceCurrency(CurrencyType ct, int a) => onReduceCurrency?.Invoke(ct, a);


        [SerializeField] private WalletData walletData;

        public string GetStringFormatOfCurrency(CurrencyType cType)
        {
            var amount = GetCurrency(cType).amount;
            return amount > 999 ? (amount / 1000f).ToString("F") + "K" : amount.ToString();
        }
        
        private void CurrencyIncreasing(CurrencyType currencyType, int amount)
        {
            GetCurrency(currencyType).amount += amount;
        }

        private void CurrencyReducing(CurrencyType currencyType, int amount)
        {
            GetCurrency(currencyType).amount -= amount;
        }

        public CurrencyInfo GetCurrency(CurrencyType cType)
        {
            foreach (var currency in walletData.currencies)
            {
                if (currency.currencyType.Equals(cType))
                {
                    return currency;
                }
            }

            Debug.LogWarning("There is no currency in wallet named : " + cType.ToString());
            return null;
        }

        public void SaveCurrencies()
        {
            walletData.Save();
        }

        public void LoadCurrencies()
        {
            walletData.Load();
        }

        private void OnEnable()
        {
            onIncreaseCurrency += CurrencyIncreasing;
            onReduceCurrency += CurrencyReducing;
        }

        private void OnDisable()
        {
            onIncreaseCurrency -= CurrencyIncreasing;
            onReduceCurrency -= CurrencyReducing;
        }
    }
}