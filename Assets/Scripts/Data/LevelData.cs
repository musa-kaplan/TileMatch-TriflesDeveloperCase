using System;
using System.Collections.Generic;
using General;
using InGame;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "NewLevel", menuName = "Trifles/Level Data")]
    public class LevelData : ScriptableObject
    {
        public int index;
        [HideInInspector] public bool isUnlocked;
        [HideInInspector] public bool isCompleted;
        [HideInInspector] public int earnedStars;
        public List<LayerInfo> layers;
        public List<BlockInfo> blockTypes;
        public float threeStarTimeLimit;
        public float twoStarTimeLimit;

        [Header("Visuals")] 
        public Sprite lockedSprite;
        public Sprite nonStarSprite;
        public Sprite starSprite;

        public void Save()
        {
            PlayerPrefs.SetInt("Level" + index + "Unlocked", isUnlocked ? 1 : 0);
            PlayerPrefs.SetInt("Level" + index + "Completed", isCompleted ? 1 : 0);
            PlayerPrefs.SetInt("Level" + index + "EarnedStars", earnedStars);
        }

        public void Load()
        {
            isUnlocked = PlayerPrefs.HasKey("Level" + index + "Unlocked") && 
                         PlayerPrefs.GetInt("Level" + index + "Unlocked").Equals(1);
            
            isCompleted = PlayerPrefs.HasKey("Level" + index + "Completed") && 
                          PlayerPrefs.GetInt("Level" + index + "Completed").Equals(1);
            
            earnedStars = PlayerPrefs.HasKey("Level" + index + "EarnedStars") ? 
                PlayerPrefs.GetInt("Level" + index + "EarnedStars") : 0;
        }
    }
}