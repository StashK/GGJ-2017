using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JPL
{
    public class PathUpdater : Mono
    {
        /// <summary>
        /// Contains all the paths
        /// </summary>
        [SerializeField]
        private Dictionary<string, Path> _paths = new Dictionary<string, Path>();
        /// <summary>
        /// The current index of the updater
        /// </summary>
        private int updateIndex = 0;
        /// <summary>
        /// The amount of paths that need updating each frame
        /// </summary>
        int updateCount = 2;

        void Awake ()
        {
            _debug = true;
        }

        void Update ()
        {
            // create local copy of the updateCound
            int updateCount = this.updateCount;
            // only update if there are actually any paths
            if (_paths.Count > 0) {
                // keep updating paths untill it's finished
                while (updateCount > 0)
                {
                    // at the end of the amount of paths, reset updateIndex
                    if (updateIndex > _paths.Count - 1) {
                        updateIndex = 0;
                    }
                    // recalculate the path
                    _paths[_paths.Keys.ElementAt(updateIndex)].Calculate();
                    updateIndex++;
                    updateCount--;
                }
            }

            if (_debug)
            {
                foreach (Path p in _paths.Values)
                {
                    p.DrawDebug();
                }
            }
        }

        /// <summary>
        /// Adds a path to the updater
        /// </summary>
        /// <param name="owner">The origin transform</param>
        /// <param name="target">The target transform</param>
        /// <param name="calcFromHighest">Setting this to true ensures the shortest path when using fall height in navmesh baking, used for sound</param>
        public void AddPath (Transform owner, Transform target, bool calcFromHighest = false)
        {
            if (owner != null && target != null && !_paths.ContainsKey(GetId(owner, target))) {
                _paths.Add(GetId(owner, target), new Path(owner, target, calcFromHighest));
                Debug.Log("Added path: "+owner+" "+target);
            }
            else
            {
                //Debug.LogWarning("Unable to add path with owner: "+owner+" "+target);
            }
        }

        /// <summary>
        /// Remove a path
        /// </summary>
        /// <param name="owner">The origin transform</param>
        /// <param name="target">The target transform</param>
        public void RemovePath (Transform owner, Transform target)
        {
            _paths.Remove(GetId(owner, target));
        }

        /// <summary>
        /// Returns a path from the updater
        /// </summary>
        /// <param name="owner">The origina transform </param>
        /// <param name="target">The target transform</param>
        /// <returns></returns>
        public Vector3[] GetPath (Transform owner, Transform target)
        {
            if (_paths.ContainsKey(GetId(owner, target)))
            {
                return _paths[GetId(owner, target)].GetPath(owner, target);
            }
            else
            {
                AddPath(owner, target);
                return GetPath(owner, target);
            }
        }

        /// <summary>
        /// Returns a navMeshPath from the updater
        /// </summary>
        /// <param name="owner">The origin transform</param>
        /// <param name="target">The target transform</param>
        /// <returns></returns>
        public UnityEngine.AI.NavMeshPath GetNavmeshPath (Transform owner, Transform target)
        {
            if (_paths.ContainsKey(GetId(owner, target)))
            {
                return _paths[GetId(owner, target)].navMeshPath;
            }
            else
            {
                AddPath(owner, target);
                return _paths[GetId(owner, target)].navMeshPath;
            }
        }

        /// <summary>
        /// Returns the total distance of a path
        /// </summary>
        /// <param name="owner">The origin transform</param>
        /// <param name="target">The target transform</param>
        /// <returns></returns>
        public float GetDistance (Transform owner, Transform target)
        {
            return _paths[GetId(owner, target)].distance;
        }

        /// <summary>
        /// Return the NavMeshPathStatus of a path
        /// </summary>
        /// <param name="owner">The origin transform</param>
        /// <param name="target">The target transform</param>
        /// <returns></returns>
        public UnityEngine.AI.NavMeshPathStatus GetStatus (Transform owner, Transform target)
        {
            return _paths[GetId(owner, target)].navMeshPath.status;
        }

        /// <summary>
        /// Sets the areaCosts for a specific path
        /// </summary>
        /// <param name="owner">The origin transform</param>
        /// <param name="target">The target transform</param>
        /// <param name="areaCosts">The area costs</param>
        public void SetAreaCosts (Transform owner, Transform target, float[] areaCosts)
        {
            _paths[GetId(owner, target)].areaCosts = areaCosts;
        }

        /// <summary>
        /// Returns the path id
        /// </summary>
        /// <param name="owner">The origin transform</param>
        /// <param name="target">The target transform</param>
        /// <returns></returns>
        private string GetId (Transform owner, Transform target)
        {
            return "" + Mathf.Min(owner.GetInstanceID(), target.GetInstanceID()) + "-" + Mathf.Max(owner.GetInstanceID(), target.GetInstanceID());
        }
    }

    [System.Serializable]
    public class Path
    {
        public float[] areaCosts;
        private Transform origin;
        private Transform target;
        public UnityEngine.AI.NavMeshPath navMeshPath = new UnityEngine.AI.NavMeshPath();

        public Transform highest;
        public Transform lowest;

        public float distance;

        private float lastActive = 0f;
        private bool hasTransforms
        {
            get { return origin && target; }
        }

        private bool calcFromHighest;

        public Path (Transform _origin, Transform _target, bool _calcFromHighest = false)
        {
            origin = _origin;
            target = _target;
            calcFromHighest = _calcFromHighest;
            Calculate();
        }

        public Vector3[] GetPath (Transform _origin, Transform _target)
        {
            return CorrectPath(_origin, _target);
        }

        public void Calculate ()
        {
            if (hasTransforms) {
                UnityEngine.AI.NavMeshPath _navMeshPath = new UnityEngine.AI.NavMeshPath();
                float[] originalCost = StandardAreaCosts();
                if (areaCosts != null)
                {
                    SetAreaCosts(areaCosts);
                }

                if (calcFromHighest)
                {
                    RearangeHighest();

                    UnityEngine.AI.NavMesh.CalculatePath(highest.position, lowest.position, UnityEngine.AI.NavMesh.AllAreas, _navMeshPath);

                    if (_navMeshPath.status == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
                    {
                        UnityEngine.AI.NavMesh.CalculatePath(highest.position, navMeshPath.corners[navMeshPath.corners.Length -1], UnityEngine.AI.NavMesh.AllAreas, _navMeshPath);
                    }
                }
                else
                {
                    UnityEngine.AI.NavMesh.CalculatePath(origin.position, target.position, UnityEngine.AI.NavMesh.AllAreas, _navMeshPath);

                    if (_navMeshPath.status == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
                    {
                        UnityEngine.AI.NavMesh.CalculatePath(origin.position, navMeshPath.corners[navMeshPath.corners.Length - 1], UnityEngine.AI.NavMesh.AllAreas, _navMeshPath);
                    }
                }

                if (_navMeshPath.status != UnityEngine.AI.NavMeshPathStatus.PathInvalid)
                {
                    CalcDistance();
                    navMeshPath = _navMeshPath;
                }

                if (areaCosts != null)
                {
                    SetAreaCosts(originalCost);
                }
            }
            else
            {
                lastActive += Time.deltaTime;

                if (lastActive >= 10f)
                {
                    Core.PathUpdater.RemovePath(origin, target);
                }
            }
        }

        private void CalcDistance ()
        {
            distance = 0f;
            for (int i = 0; i < navMeshPath.corners.Length -1; i++)
            {
                distance += Vector3.Distance(navMeshPath.corners[i], navMeshPath.corners[i + 1]);
            }
        }

        public void DrawDebug ()
        {
            for (int i = 0; i < navMeshPath.corners.Length - 1; i++)
                Debug.DrawLine(navMeshPath.corners[i], navMeshPath.corners[i + 1], Color.red);
        }

        private void RearangeHighest ()
        {
            if (hasTransforms) {
                if (origin.position.y > target.position.y)
                {
                    highest = origin;
                    lowest = target;
                }
                else
                {
                    highest = target;
                    lowest = origin;
                }
            }
        }

        private Vector3[] CorrectPath (Transform _origin, Transform _target)
        {
            if (hasTransforms) {
                if (_origin == highest || !calcFromHighest)
                {
                    return navMeshPath.corners;
                }
                else
                {
                    Vector3[] v3 = new Vector3[navMeshPath.corners.Length];

                    for (int i = 0; i < navMeshPath.corners.Length; i++)
                    {
                        v3[navMeshPath.corners.Length - 1 - i] = navMeshPath.corners[i];
                    }

                    return v3;
                }
            }
            else
            {
                return navMeshPath.corners;
            }
        }

        private void SetAreaCosts (float[] costs)
        {
            for (int i = 0; i < costs.Length; i++)
            {
                UnityEngine.AI.NavMesh.SetAreaCost(i, costs[i]);
            }
        }

        private float[] StandardAreaCosts ()
        {
            float[] f = new float[32];

            for (int i = 0; i < 32; i++)
            {
                f[i] = UnityEngine.AI.NavMesh.GetAreaCost(i);
            }

            return f; 
        }
    }
}