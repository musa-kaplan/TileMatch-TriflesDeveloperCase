using System;
using System.Collections.Generic;
using Data;
using MusaUtils.Pooling;
using UnityEngine;

namespace General
{
    public class DataContainer : MonoBehaviour
    {
        public static DataContainer dataContainer;

        public List<LevelData> levels;
        public MonoPools blockPool;
        public WalletManager walletManager;
        public LeaderboardManager leaderboardManager;

        private void Awake()
        {
            if (dataContainer == null)
            {
                dataContainer = this;
            }
            else
            {
                Destroy(this.gameObject);
                return;
            }

            levels[2].isUnlocked = true;
            levels[2].Save();
            
            DontDestroyOnLoad(this.gameObject);
        }

        private void OnEnable()
        {
            foreach (var level in levels) { level.Load(); }

            walletManager.LoadCurrencies();
        }

        private void OnDisable()
        {
            foreach (var level in levels) { level.Save(); }

            walletManager.SaveCurrencies();
        }
    }
}
