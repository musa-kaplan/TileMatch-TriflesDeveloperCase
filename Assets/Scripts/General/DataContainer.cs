using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Data;
using InGame;
using UnityEngine;

namespace General
{
    public class DataContainer : MonoBehaviour
    {
        public static DataContainer dataContainer;

        public List<LevelData> levels;
        public LevelManager levelManager;
        public BoardManager boardManager;
        public ParticleManager particleManager;
        public WalletManager walletManager;
        public GeneralVisualData generalVisualData;
        public LeaderboardManager leaderboardManager;

        [HideInInspector] public bool isBlockClicked;

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

            levels[0].isUnlocked = true;
            levels[0].Save();
        }

        public async void BlockClicked()
        {
            isBlockClicked = true;
            await UniTask.Delay(TimeSpan.FromSeconds(.15f));
            isBlockClicked = false;
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
