using UnityEngine;

namespace MusaUtils.SaveSystem
{
    [CreateAssetMenu(menuName = "PlayerPreference", fileName = "NewPreference")]
    public class PrefData : ScriptableObject
    {
        public enum DataType
        {
            Integer,
            Float,
            String
        }

        public DataType _dataType;
    }
}
