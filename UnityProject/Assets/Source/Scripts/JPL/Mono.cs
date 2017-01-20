using UnityEngine;
using System.Collections;

namespace JPL
{
    public class Mono : MonoBehaviour
    {

        protected bool _debug = false;
            
        protected void Log(object message)
        {

            #if UNITY_EDITOR
            if (_debug)
                Debug.Log(message);

            #elif UNITY_DEBUG
            if (_debug)
                Debug.Log(s);
            #endif
        }
    }
}