using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace General
{
    public class PlayerInfo
    {
        public int rank;
        public string playerName;
        public int rankScore;

        public PlayerInfo(int r, string n, int s)
        {
            rank = r;
            playerName = n;
            rankScore = s;
        }
    }
    
    public class LeaderboardManager : MonoBehaviour
    {
        private static event Action<int> onSendPlayfabLeaderboard;
        public static void SendPlayfabLeaderboard(int amount) => onSendPlayfabLeaderboard?.Invoke(amount);

        private List<PlayerInfo> leaderboardItems = new List<PlayerInfo>();

        public List<PlayerInfo> GiveLeaderboardPlayers()
        {
            GetLeaderboard();
            return leaderboardItems;
        }

        private void GetLeaderboard()
        {
            var request = new GetLeaderboardRequest
                { StatisticName = "CupAmount", StartPosition = 0, MaxResultsCount = 10 };
            PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
        }

        private void OnLeaderboardGet(GetLeaderboardResult result)
        {
            leaderboardItems.Clear();
            foreach (var leaderBoard in result.Leaderboard)
            {
                var item = new PlayerInfo(leaderBoard.Position, leaderBoard.PlayFabId, leaderBoard.StatValue);
                leaderboardItems.Add(item);
            }
        }

        private void LetsSendLeaderboardData(int amount)
        {
            var request = new UpdatePlayerStatisticsRequest { Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate{StatisticName = "CupAmount", Value = amount}
            }};
            
            PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
        }

        private void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
        {
            Debug.Log("Leaderboard Data Sent");
        }

        private void OnError(PlayFabError error)
        {
            Debug.Log("Error on Sending Leaderboard Data : " + error);
        }
        
        private void OnEnable()
        {
            onSendPlayfabLeaderboard += LetsSendLeaderboardData;
        }

        private void OnDisable()
        {
            onSendPlayfabLeaderboard -= LetsSendLeaderboardData;
        }
    }
}
