using System;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace General
{
    public class PlayfabLogin : MonoBehaviour
    {
        [SerializeField] private Button retryButton;
        
        private void Start()
        {
            Login();
        }

        private void Login()
        {
            var request = new LoginWithCustomIDRequest{CustomId = SystemInfo.deviceUniqueIdentifier, CreateAccount = true};
            PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
        }

        private void OnSuccess(LoginResult result)
        {
            Debug.Log("Login/Create is Successful");
            SceneManager.LoadScene("Enterance");
        }

        private void OnError(PlayFabError error)
        {
            Debug.Log("We got error on Login/Create account");
            Debug.LogWarning(error.GenerateErrorReport());
        }

        private void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void OnEnable()
        {
            retryButton.onClick.RemoveAllListeners();
            retryButton.onClick.AddListener(ReloadScene);
        }
    }
}
