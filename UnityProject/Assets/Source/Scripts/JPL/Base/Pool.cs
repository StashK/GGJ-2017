using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JPL.Base
{
    public class Pool : Mono
    {
        #region variables
        /***** DATA *****/
        /// <summary>
        /// Contains the reference to the original transform
        /// </summary>
        public List<Transform> poolParent = new List<Transform>();
        /// <summary>
        /// Contains all the objects in the pool
        /// </summary>
        public List<List<Transform>> pool = new List<List<Transform>>();
        /// <summary>
        /// Contains all the settings for each pool
        /// </summary>
        [SerializeField]
        private Dictionary<string, PoolSettings> settings = new Dictionary<string, PoolSettings>();
        /// <summary>
        /// The parent object of all objects in the pool
        /// </summary>
        private Transform objectParent;

        /***** SETTINGS *****/
        /// <summary>
        /// The standard setting for the minimum required objects in the pool
        /// </summary>
        public int minPool = 0;
        /// <summary>
        /// The standard setting for the maximum required objects in the pool
        /// </summary>
        public int maxPool = 1;
        /// <summary>
        /// The setting for the spawn rate
        /// </summary>
        public int cacheRate = 3;	// how much the object spawns
        /// <summary>
        /// Setting for debugging the pool
        /// </summary>
        public bool debugPool = false;
        #endregion

        #region standard functions
        protected void Awake()
        {
            // create the parent object
            objectParent = new GameObject().transform;
            // give the parent object the correct name
            objectParent.name = "pool_objects";
        }

        protected void Update()
        {
            // loop through pool
            for (int i = 0; i < poolParent.Count; i++)
            {
                // if the amount of objects lower then the min in the pool
                if (pool[i] != null)
                {
                    if (pool[i].Count < settings[poolParent[i].name].min)
                    {
                        // add :)
                        for (int j = 0; j < cacheRate; j++)
                        {
                            Transform tObj = SpawnObject(poolParent[i]);
                            tObj.gameObject.SetActive(false);
                            pool[i].Add(tObj);
                        }
                    }
                    // if higher
                    else if (settings[poolParent[i].name].removeWhenHigh && pool[i].Count > settings[poolParent[i].name].max)
                    {
                        Destroy(pool[i][pool[i].Count - 1].gameObject);
                        pool[i].RemoveAt(pool[i].Count - 1);
                    }
                }
                else
                {
                    Debug.Log("Pool not found" + i);
                }
            }
        }
        #endregion

        #region Spawn functions
        /// <summary>
        /// This returns an object from the pool.
        /// </summary>
        /// <param name="tTransform">The required object</param>
        /// <returns></returns>
        public Transform Spawn(Transform tTransform, bool active = true)
        {
            bool foundParent = false;

            // loop through poolParent (to check which pool needs to be used)
            for (int i = 0; i < poolParent.Count; i++)
            {
                // found parent
                if (tTransform == poolParent[i])
                {
                    foundParent = true;
                    // get object
                    return active ? Activate(GetObject(tTransform, i)) : GetObject(tTransform, i);
                }
            }

            if (!foundParent)
            {
                // create a new pool
                CreatePool(tTransform);
                // spawn the to return object
                Transform obj = SpawnObject(tTransform);
                obj.parent = null;
                // disable if needed
                if (!active)
                {
                    obj.gameObject.SetActive(false);
                }

                return obj;
            }

            return null;
        }

        /// <summary>
        /// This returns an object from the pool with an delay
        /// </summary>
        /// <param name="tTransform"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public Transform Spawn(Transform tTransform, float time)
        {
            // spawn object
            tTransform = Spawn(tTransform);
            // turn it of
            tTransform.gameObject.SetActive(false);
            // restart it after x sec
            StartCoroutine(Activate(tTransform, time));
            // return it
            return tTransform;
        }

        /// <summary>
        /// This returns an object from the pool at a certain position
        /// </summary>
        /// <param name="tTransform"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public Transform Spawn(Transform tTransform, Vector3 pos)
        {
            // spawn deactivated object
            tTransform = Spawn(tTransform, false);
            // set the position
            tTransform.position = pos;
            // activate and return it
            return Activate(tTransform);
        }
        #endregion

        #region Remove functions
        /// <summary>
        /// This removes an object from the scene and puts it in the pool again
        /// </summary>
        /// <param name="tTransform"></param>
        public void Remove(Transform tTransform)
        {
            for (int i = 0; i < poolParent.Count; i++)
            {
                // found parent
                if (tTransform && tTransform.name == poolParent[i].name)
                {
                    // turn it off
                    tTransform.gameObject.SetActive(false);
                    // set to correct parent
                    tTransform.parent = objectParent.transform;
                    // reset object scale
                    tTransform.localScale = new Vector3(1, 1, 1);
                    // add object
                    pool[i].Add(tTransform);
                    break;
                }
                else
                {

                }
            }
        }

        /// <summary>
        /// This removes an obejct from the scene after a certain amount of time
        /// </summary>
        /// <param name="tReturn"></param>
        /// <param name="time"></param>
        public void Remove(Transform tReturn, float time)
        {
            StartCoroutine(RemoveObject(tReturn, time));
        }
        #endregion

        #region Spawn and remove helper functions
        /// <summary>
        /// This activates this object
        /// </summary>
        /// <param name="tTransform"></param>
        /// <returns></returns>
        private Transform Activate (Transform tTransform)
        {
            tTransform.gameObject.SetActive(true);
            return tTransform;
        }
        /// <summary>
        /// This activates an object after a specific time
        /// </summary>
        /// <param name="tTransform"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        private IEnumerator Activate(Transform tTransform, float time)
        {
            // yield
            yield return new WaitForSeconds(time);
            // activate
            tTransform.gameObject.SetActive(true);
            tTransform.gameObject.SendMessage("Activate");
        }

        /// <summary>
        /// This internal function handles the return of an object to the pool after a certain period of time
        /// </summary>
        /// <param name="target"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        IEnumerator RemoveObject(Transform target, float time)
        {
            // (; ･`д･´)
            //Debug.Log("Quiing removal: " + target);
            yield return new WaitForSeconds(time);

            Remove(target);
            //Debug.Log("Returned object: " + target);
        }
        #endregion

        #region internal functions
        /// <summary>
        /// This internal function gets an object from the required pool
        /// </summary>
        /// <param name="tTransform"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private Transform GetObject(Transform tTransform, int i)
        {
            Transform returnTransform = null;

            if (pool[i] != null)
            {
                // if there is a object, get it
                if (pool[i].Count > 0)
                {
                    // set return transform
                    returnTransform = pool[i][0];
                    // remove it from the pool
                    pool[i].Remove(returnTransform);
                    //Debug.Log ("loaded");
                    //returnTransform.gameObject.SetActive(true);
                }
                else
                {
                    returnTransform = SpawnObject(tTransform);
                }
            }

            returnTransform.parent = null;
            return returnTransform;
        }

        /// <summary>
        /// This internal function instantiates a new instance of an object
        /// </summary>
        /// <param name="tTransform"></param>
        /// <returns></returns>
        private Transform SpawnObject(Transform tTransform)
        {
            //Debug.Log ("instantiate");
            Transform tReturn = (Transform)Instantiate(tTransform, transform.position, Quaternion.identity) as Transform;
            tReturn.name = tTransform.name;
            tReturn.parent = objectParent;

            return tReturn;
        }

        /// <summary>
        /// This internal function creates a new pool
        /// </summary>
        /// <param name="tTransform"></param>
        private void CreatePool(Transform tTransform)
        {
            // add the prefab
            poolParent.Add(tTransform);
            // create new list
            pool.Add(new List<Transform>());
            // add settings if not there
            if (!settings.ContainsKey(tTransform.name))
            {
                AddSettings(tTransform.name, minPool, maxPool);
            }

        }
        #endregion

        #region public helper functions
        /// <summary>
        /// This clears the pool
        /// </summary>
        public void Clear()
        {
            poolParent = new List<Transform>();
            pool = new List<List<Transform>>();
        }

        /// <summary>
        /// This adds a new setting for a pool
        /// </summary>
        /// <param name="name"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="removeWhenHigh"></param>
        public void AddSettings(string name, int min, int max, bool removeWhenHigh = false)
        {
            // add new settings if not there
            if (!settings.ContainsKey(name))
            {
                settings.Add(name, new PoolSettings(min, max, removeWhenHigh));
                //                Debug.Log("Pool: Added new settings for "+name+" ["+min+"/"+max+"]");
            }
            // update the settings
            else
            {
                settings[name].Set(min, max);
                //              Debug.Log("Pool: Updated new settings for " + name + " [" + min + "/" + max + "]");
            }
        }
        #endregion
        
    }

    [System.Serializable]
    class PoolSettings
    {
        private int _min = 0;
        private int _max = 1;
        public bool removeWhenHigh = false;

        public int min
        {
            get { return _min; }
        }

        public int max
        {
            get { return _max; }
        }

        public PoolSettings(int i_min, int i_max, bool b_removeWhenHigh)
        {
            _min = i_min;
            _max = i_max;
            removeWhenHigh = b_removeWhenHigh;
        }

        public void Set(int i_min, int i_max)
        {
            _min = i_min;
            _max = i_max;
        }
    }
}