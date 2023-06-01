using System;
using Cysharp.Threading.Tasks;
using Data;
using DG.Tweening;
using General;
using MusaUtils.Pooling;
using UnityEngine;

namespace InGame
{
    public class CurrencyInteraction : MonoBehaviour
    {
        private static event Action<CurrencyType, int, Vector3> onCurrencyGained;
        public static void CurrencyGained(CurrencyType cType, int count, Vector3 center) => onCurrencyGained?.Invoke(cType, count, center);

        [SerializeField] private RectTransform coinPosition;
        [SerializeField] private RectTransform cupPosition;

        private RectTransform canvasRect;
        private DataContainer dataContainer;
        private Camera mainCamera;

        private void Start()
        {
            dataContainer = DataContainer.dataContainer;
            canvasRect = GetComponent<RectTransform>();
            mainCamera = Camera.main;
        }

        private void InstantiateCurrencies(CurrencyType t, int c, Vector3 pos)
        {
            var count = c > 10 ? 10 : c;
            for (var i = 0; i < count; i++)
            {
                var currency = AquaPoolManager.PoolInit()
                    .GetObject(dataContainer.walletManager.GetCurrency(t).currencyPool);
                
                currency.transform.SetParent(transform);
                currency.transform.localScale = Vector3.zero;

                {
                    currency.TryGetComponent(out RectTransform rt);

                    canvasRect = GetComponent<RectTransform>();

                    var viewportPosition = mainCamera.WorldToViewportPoint(pos);
                    viewportPosition.z = 0;
                    var worldObject_ScreenPosition = new Vector2(
                        ((viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
                        ((viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));

                    rt.anchoredPosition = worldObject_ScreenPosition;
                    
                    SlideCurrency(rt, t);
                }
            }
        }

        private async void SlideCurrency(RectTransform currency, CurrencyType cType)
        {
            currency.sizeDelta = coinPosition.sizeDelta;
            currency.DOScale(Vector3.one, .35f).SetEase(dataContainer.generalVisualData.blopInCurve);
            
            await UniTask.Delay(TimeSpan.FromSeconds(.4f));
            
            currency.DOMove(coinPosition.position, 1f).SetEase(Ease.InCirc);

            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            
            WalletManager.IncreaseCurrency(cType, 1);
            AquaPoolManager.PoolInit().ReturnObjectToPool(dataContainer.walletManager.GetCurrency(cType).currencyPool, currency.gameObject, 0);
        }
        
        private void OnEnable()
        {
            onCurrencyGained += InstantiateCurrencies;
        }

        private void OnDisable()
        {
            onCurrencyGained -= InstantiateCurrencies;
        }
    }
}