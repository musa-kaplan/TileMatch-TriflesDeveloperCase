using System;
using UnityEngine;

namespace Ui
{
    public class LoadingScreenManager : MonoBehaviour
    {
        private static event Action<bool> onLoadingScreenStateChanged;
        public static void ChangeLoadingScreenState(bool state) => onLoadingScreenStateChanged?.Invoke(state);

        [SerializeField] private Animator myAnimator;
        private static readonly int State = Animator.StringToHash("State");

        private void SetLoadingScreenAnimator(bool s)
        {
            myAnimator.SetBool(State, s);
        }
        
        private void OnEnable()
        {
            onLoadingScreenStateChanged += SetLoadingScreenAnimator;
        }

        private void OnDisable()
        {
            onLoadingScreenStateChanged -= SetLoadingScreenAnimator;
        }
    }
}