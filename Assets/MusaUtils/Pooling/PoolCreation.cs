using System.Collections.Generic;
using UnityEngine;

namespace MusaUtils.Pooling
{
    public class PoolCreation : MonoBehaviour
    {
        private static GameObject _currObj;
        public static void LetsCreate(List<GameObject> l, GameObject b, int c)
        {
            for (int i = 0; i < c; i++)
            {
                _currObj = Instantiate(b, FindObjectOfType<AquaPoolMono>()?.transform, false);
                l.Add(_currObj);
                _currObj.SetActive(false);
            }
        }
    }
}
