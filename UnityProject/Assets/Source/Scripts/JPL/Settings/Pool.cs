using UnityEngine;
using System.Collections;

namespace JPL
{
    public class Pool : Base.Pool
    {
        new void Awake ()
        {
            base.Awake();

            AddSettings("AudioTransform", 3, 10);
            AddSettings("BulletHole", 10, 200);
            AddSettings("BulletTrail", 10, 200);
            AddSettings("ThirdPersonCamera", 0, 4);
        }
        
    }
}