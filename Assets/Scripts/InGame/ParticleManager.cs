using System;
using System.Collections.Generic;
using UnityEngine;

namespace InGame
{
    public enum ParticleTypes{BlockBlast}

    [Serializable]
    public class ParticlesByTypes
    {
        public ParticleTypes particleType;
        public ParticleSystem particle;
    }
    public class ParticleManager : MonoBehaviour
    {
        [SerializeField] private List<ParticlesByTypes> particles;

        public void PlayParticle(ParticleTypes pType, Vector3 position)
        {
            for (var i = 0; i < particles.Count; i++)
            {
                if (particles[i].particleType.Equals(pType))
                {
                    var part = particles[i].particle;
                    position.z = 4f;
                    part.transform.position = position;
                    part.Play();
                }
            }
        }
    }
}
