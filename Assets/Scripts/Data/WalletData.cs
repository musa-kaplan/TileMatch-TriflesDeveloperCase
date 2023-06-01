using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "NewWallet", menuName = "Trifles/Wallet Data")]
    public class WalletData : ScriptableObject
    {
        public List<CurrencyInfo> currencies;
        
        public void Save()
        {
            foreach (var c in currencies)
            {
                PlayerPrefs.SetInt("Currency" + c.currencyType.ToString(), c.amount);
            }
        }

        public void Load()
        {
            foreach (var c in currencies)
            {
                if (PlayerPrefs.HasKey("Currency" + c.currencyType.ToString()))
                {
                    c.amount = PlayerPrefs.GetInt("Currency" + c.currencyType);
                }
                else
                {
                    c.amount = 0;
                }
            }
        }
    }
}
