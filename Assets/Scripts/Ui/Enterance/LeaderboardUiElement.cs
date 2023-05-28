using System;
using TMPro;
using UnityEngine;

namespace Ui.Enterance
{
    public class LeaderboardUiElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI myIndex;
        [SerializeField] private TextMeshProUGUI myName;
        [SerializeField] private TextMeshProUGUI myCupAmount;

        private Color indexTextStartColor;
        private Color nameTextStartColor;
        private Color cupAmountTextStartColor;
        
        private void Start()
        {
            indexTextStartColor = myIndex.color;
            nameTextStartColor = myName.color;
            cupAmountTextStartColor = myCupAmount.color;
        }

        public void SetMe(int index, string pName, int cupAmount)
        {
            myIndex.color = indexTextStartColor;
            myName.color = nameTextStartColor;
            myCupAmount.color = cupAmountTextStartColor;
            
            myIndex.text = (index + 1).ToString();
            myName.text = pName;
            myCupAmount.text = cupAmount.ToString();
        }

        public void ClearMe()
        {
            myIndex.color = myName.color = myCupAmount.color = Color.clear;
        }
    }
}
