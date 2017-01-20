using UnityEngine;
using System.Collections;

//[assembly: System.Reflection.AssemblyVersion("0.2.*.*")]
namespace JPL
{
    public class Core
    {
        #region /***** VARIABLES *****/
        /// Encryption key
        public static string Key = "12345678901234567890123456789012";
        /// Is it initialized
        private static bool _initialized;
        private static string _buildNumber;
        /// Reference to the gamehelper class
        private static CoreHelper _coreHelper;
        /// Reference to the pool class
        private static Pool _pool;
        /// Reference tot the game class
        private static Base.Game _game;
        /// Reference to errors handler
        private static Errors _errors;
        /// Reference to prefabs class
        private static Prefabs _prefabs;
        /// Reference to sound
        private static Sounds _sounds;
        private static PathUpdater _pathUpdater;

        #endregion

        #region /***** GETTERS/SETTERS ******/
        public static CoreHelper CoreHelper
        {
            get
            {
                // return if excists
                if (_coreHelper != null)
                    return _coreHelper;
                // create new, then return
                else
                {
                    _coreHelper = new GameObject("_GameHelper").AddComponent<CoreHelper>();
                    return _coreHelper;
                }
            }
            set { _coreHelper = value; }
        }

        public static PathUpdater PathUpdater
        {
            get
            {
                // return if excists
                if (_pathUpdater != null)
                    return _pathUpdater;
                // create new, then return
                else
                {
                    _pathUpdater = new GameObject("_PathUpdater").AddComponent<PathUpdater>();
                    return _pathUpdater;
                }
            }
            set { _pathUpdater = value; }
        }

        public static Base.Game Game
        {
            get
            {
                // return if excists
                if (_game != null)
                    return _game;
                // create new, then return
                else
                {
                    _game = new GameObject("_Game").AddComponent<Base.Game>();
                    return _game;
                }
            }
            set { _game = value; }
        }

        public static Pool Pool
        {
            get
            {
                // return if excists
                if (_pool != null)
                    return _pool;
                // create new, then return
                else
                {
                    Pool[] pools = CoreHelper.FindObjectsOfType(typeof(Pool)) as Pool[];

                    if (pools.Length >= 1)
                    {
                        _pool = pools[0];
                    }
                    else
                    {
                        _pool = new GameObject("_pool").AddComponent<Pool>();
                    }

                    return _pool;
                }
            }
            set { _pool = value; }
        }

        public static Prefabs Prefabs
        {
            get
            {
                // return if excists
                if (_prefabs != null)
                    return _prefabs;
                // create new, then return
                else
                {
                    Prefabs[] prefabs = CoreHelper.FindObjectsOfType(typeof(Prefabs)) as Prefabs[];

                    if (prefabs.Length >= 1)
                    {
                        _prefabs = prefabs[0];
                    }
                    else
                    {
                        _prefabs = new GameObject("_prefabs").AddComponent<Prefabs>();
                    }

                    return _prefabs;
                }
            }
            set { _prefabs = value; }
        }

        public static Sounds Sounds
        {
            get
            {
                // return if excists
                if (_sounds != null)
                    return _sounds;
                // create new, then return
                else
                {
                    Sounds[] sounds = CoreHelper.FindObjectsOfType(typeof(Sounds)) as Sounds[];

                    if (sounds.Length >= 1)
                    {
                        _sounds = sounds[0];
                    }
                    else
                    {
                        _sounds = new GameObject("_Sounds").AddComponent<Sounds>();
                    }

                    return _sounds;
                }
            }
            set { _sounds = value; }
        }

        public static Errors Errors
        {
            get
            {
                // return if excists
                if (_errors != null)
                    return _errors;
                // create new, then return
                else
                {
                    _errors = new GameObject("_Errors").AddComponent<Errors>();
                    return _errors;
                }
            }
            set { _errors = value; }
        }

        public static string buildnumber
        {
            get {
                if (_buildNumber != "" && _buildNumber != null)
                {
                    return _buildNumber;
                }
                else
                {
                    _buildNumber = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                    return _buildNumber;
                }
            }
        }
        #endregion

        static Core ()
        {
            Initialize();
        }

        public static void Initialize ()
        {
            if (!_initialized)
            {

                // start errors
                _errors = Errors;
                // get the build
                Debug.Log("build: "+buildnumber);

                SaveData.Initialize(true);
                
                _initialized = true;
            }
        }

        public static void LoadScene (int i, float delay = 0f)
        {
            CoreHelper.LoadScene(i, delay);
        }
    }
}
