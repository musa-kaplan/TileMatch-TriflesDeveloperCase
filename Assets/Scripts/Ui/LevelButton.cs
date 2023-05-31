using System.Linq;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Ui
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private Button myButton;
        [SerializeField] private TextMeshProUGUI myLevelText;
        [SerializeField] private Image[] nonStarImages;
        [SerializeField] private Image[] starImages;
        [SerializeField] private Image lockImage;
        
        public void SetMe(LevelData data)
        {
            foreach (var starImage in starImages)
            { starImage.sprite = data.starSprite; starImage.gameObject.SetActive(false);
                starImage.preserveAspect = true;
            }
            
            foreach (var nonStarImage in nonStarImages)
            { nonStarImage.sprite = data.nonStarSprite; nonStarImage.gameObject.SetActive(false);
                nonStarImage.preserveAspect = true;
            }

            lockImage.sprite = data.lockedSprite;
            lockImage.preserveAspect = true;
            
            if (data.isUnlocked)
            {
                lockImage.gameObject.SetActive(false);
                myButton.interactable = true;
                myLevelText.gameObject.SetActive(true);
                
                foreach (var nonStarImage in nonStarImages)
                { nonStarImage.gameObject.SetActive(true); }
                
                if (data.isCompleted)
                {
                    for (var j = 0; j < data.earnedStars; j++) { starImages[j].gameObject.SetActive(true); }
                }
            }
            else
            {
                myLevelText.gameObject.SetActive(false);
                myButton.interactable = false;
                lockImage.gameObject.SetActive(true);
            }
            myLevelText.text = (data.index + 1).ToString();

            myButton.onClick.RemoveAllListeners();
            var i = data.index;
            myButton.onClick.AddListener(()=>{OnButtonClick(i);});
        }

        private void OnButtonClick(int index)
        {
            PlayerPrefs.SetInt("Level", index);
            SceneManager.LoadScene("GameScene");
        }
    }
}