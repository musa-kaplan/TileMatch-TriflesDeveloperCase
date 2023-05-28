using System;
using Data;
using UnityEngine;

namespace General
{
    public class DataContainer : MonoBehaviour
    {
        public static DataContainer dataContainer;

        public WalletManager walletManager;

        private void Awake() => dataContainer = this;

        private void OnEnable()
        {
            walletManager.LoadCurrencies();
        }

        private void OnDisable()
        {
            walletManager.SaveCurrencies();
        }
    }
}
