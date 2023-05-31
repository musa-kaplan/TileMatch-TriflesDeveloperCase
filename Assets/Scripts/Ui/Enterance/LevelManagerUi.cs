using Cysharp.Threading.Tasks;
using General;
using UnityEngine;

namespace Ui.Enterance
{
    public class LevelManagerUi : MonoBehaviour
    {
        [SerializeField] private RectTransform contentRect;
        [SerializeField] private GameObject levelButton;
        
        private DataContainer dataContainer;
        private Vector2 contentRectPosition;
        private int buttonCount;
        private int unlockedButtonCount;

        private void Start()
        {
            dataContainer = DataContainer.dataContainer;

            for (var i = 0; i < dataContainer.levels.Count; i++)
            {
                buttonCount++;
                var level = dataContainer.levels[i];
                var button = Instantiate(levelButton, contentRect);
                
                if (button.TryGetComponent(out LevelButton lb))
                {
                    lb.SetMe(level);
                }

                if (level.isUnlocked)
                {
                    unlockedButtonCount++;
                    SetContentRect(button);
                }
            }

            contentRect.anchorMin = new Vector2(.5f - (buttonCount * .15f), 0);
            contentRect.anchorMax = new Vector2(.5f + (buttonCount * .15f), 1f);
        }

        private async void SetContentRect(GameObject button)
        {
            await UniTask.DelayFrame(5);
            if (button.TryGetComponent(out RectTransform rect))
            {
                contentRectPosition.x = (buttonCount * rect.sizeDelta.x) - (unlockedButtonCount *
                                                                            (rect.sizeDelta.x * 2f));
            }
            
            contentRect.anchoredPosition = contentRectPosition;
        }
    }
}
