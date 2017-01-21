using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JPL
{
    public class Prefabs : Mono
    {
        public Transform duck;

        [Header("Global Hit Particles")]
        public List<GameObject> impactParticles;
    }
}