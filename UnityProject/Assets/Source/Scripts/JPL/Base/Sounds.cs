using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JPL.Base
{
    public class Sounds : Mono
    {

        public LayerMask layerMask;

        private AudioSource _audioSource;
        private Transform _audioTransform;
        private Transform _realSoundTransform;

        public Dictionary<Transform, Dictionary<string, AudioSource>> _loops = new Dictionary<Transform, Dictionary<string, AudioSource>>();

        private Transform _backgroundMusic;

        private Loader loader = new Loader();

        public void Awake()
        {
            Debug.Log("awake");
            
            // Get main audio components
            if (Camera.main.GetComponent<AudioSource>())
            {
                _audioSource = Camera.main.GetComponent<AudioSource>();
            }
            // Add audio component
            else
            {
                _audioSource = Camera.main.gameObject.AddComponent<AudioSource>();
                //Camera.main.gameObject.AddComponent<AudioRecorder>();
            }


            // create the audio transform
            _audioTransform = new GameObject().transform;
            _audioTransform.name = "AudioTransform";
            AudioSource AS = _audioTransform.gameObject.AddComponent<AudioSource>();
            AS.playOnAwake = false;
            AS.maxDistance = 10f;
            AS.spatialBlend = 0f;

            /*_realSoundTransform = new GameObject().transform;
            _realSoundTransform.name = "RealSoundTransform";
            AS = _realSoundTransform.gameObject.AddComponent<AudioSource>();
            _realSoundTransform.gameObject.AddComponent<AudioLowPassFilter>();
            _realSoundTransform.gameObject.AddComponent<RealSound>();
            AS.playOnAwake = false;*/

        }

        void Start ()
        {

            //new Loader().Init();
        }

        public AudioClip GetRandomAudioClip(List<AudioClip> _list)
        {
            if (_list == null || _list.Count == 0) return null;

            return _list[Random.Range(0, _list.Count)];
        }

        public AudioClip PlaySound(List<AudioClip> ACList, SOUNDSETTING soundSetting)
        {
            return PlaySound(GetRandomAudioClip(ACList), soundSetting);
        }

        public AudioClip PlaySound(AudioClip AC, SOUNDSETTING soundSetting)
        {
            if (AC == null) return null;
            // play sound on main camera object

            //GameData.GetCamera(0).cameraComponent.GetComponent<AudioSource>().PlayOneShot(AC, GetVolume(soundSetting));
            //Debug.Log("Source: " + _audioSource + " clip: " + AC);
            _audioSource.PlayOneShot(AC, GetVolume(soundSetting));
            return AC;
        }

        public void PlaySound(AudioClip AC, Transform t, SOUNDSETTING soundSetting, bool soundOcclusion = false)
        {
            if (AC == null) return;
            // create object
            Transform tObj = Core.Pool.Spawn(GetTransform(soundOcclusion));
            // set parent
            tObj.position = t.position;

            if (soundOcclusion)
            {
                tObj.GetComponent<RealSound>().StartSound(t, AC, GetVolume(soundSetting));
            }
            else
            {
                tObj.parent = t;

                // play sound
                if (!tObj.GetComponent<AudioSource>().isPlaying)
                {
                    tObj.GetComponent<AudioSource>().PlayOneShot(AC, GetVolume(soundSetting));
                }
            }

            // remove obejct
            Core.Pool.Remove(tObj, AC.length);
        }

        public void PlaySound(AudioClip AC, Vector3 pos, SOUNDSETTING soundSetting)
        {
            if (AC == null) return;
            // create object
            Transform tObj = Core.Pool.Spawn(_audioTransform);
            // set position
            tObj.position = pos;
            // play sound
            tObj.GetComponent<AudioSource>().PlayOneShot(AC, GetVolume(soundSetting));
            // remove obejct
            Core.Pool.Remove(tObj, AC.length);
        }

        public AudioSource StartLoop(AudioClip AC, Transform t, string key, SOUNDSETTING soundSetting)
        {

            if (_loops.ContainsKey(t))
            {
                if (_loops[t].ContainsKey(key))
                    return _loops[t][key];
            }
            else
            {
                _loops.Add(t, new Dictionary<string, AudioSource>());
            }
            // create object
            Transform tObj = Core.Pool.Spawn(_audioTransform);
            // set parent
            tObj.position = t.position;
            tObj.parent = t;
            // get the source
            AudioSource AS = tObj.GetComponent<AudioSource>();
            // play the sound
            AS.clip = AC;
            AS.loop = true;
            AS.volume = GetVolume(soundSetting);
            AS.Play();
            _loops[t].Add(key, AS);
            return AS;

            //   return null;
        }

        public void StartBackgroundLoop(AudioClip AC1, AudioClip AC2, bool overwrite = true)
        {
            if (AC1 == null || AC2 == null) return;

            GetBackgroundTransform();
            if (_backgroundMusic != null)
            {
                AudioSource AS = _backgroundMusic.GetComponent<AudioSource>();

                if ((!overwrite && AS.isPlaying) || AS.clip == AC2) return;

                PlaySound(AC1, SOUNDSETTING.MUSIC);

                // play the sound
                AS.clip = AC2;
                AS.loop = true;
                AS.volume = GetVolume(SOUNDSETTING.MUSIC);
                AS.PlayScheduled(AudioSettings.dspTime + AC1.length);

                DontDestroyOnLoad(_backgroundMusic.gameObject);
            }
        }

        public void StartBackgroundLoop (AudioClip AC, bool overwrite = true)
        {
            if (AC == null) return;

            GetBackgroundTransform();
            if (_backgroundMusic != null)
            {
                AudioSource AS = _backgroundMusic.GetComponent<AudioSource>();

                if ((!overwrite && AS.isPlaying) || AS.clip == AC) return;

                // play the sound
                AS.clip = AC;
                AS.loop = true;
                AS.volume = GetVolume(SOUNDSETTING.MUSIC);
                AS.Play();

                DontDestroyOnLoad(_backgroundMusic.gameObject);
            }
            
        }

        public void GetBackgroundTransform ()
        {
            if (_backgroundMusic == null)
            {
                if (GameObject.Find("_BackgroundLoop"))
                {
                    _backgroundMusic = GameObject.Find("_BackgroundLoop").transform;
                }
                else
                {
                    _backgroundMusic = new GameObject("_BackgroundLoop").GetComponent<Transform>();
                    AudioSource AS = _backgroundMusic.gameObject.AddComponent<AudioSource>();
                }
            }
        }

        public void FadeBackgroundVolume (float volume, float time = 1f)
        {
            GetBackgroundTransform();

            if (_backgroundMusic != null)
            {
                LeanTween.value(gameObject, UpdateBackgroundVolume, GetVolume(_backgroundMusic) * GetVolume(SOUNDSETTING.MUSIC), volume, time).setEase(LeanTweenType.easeInOutQuad);
            }
        }

        public AudioSource GetLoop(Transform t, string key)
        {
            return _loops[t][key];
        }

        public void SetLoopSound(Transform t, string key, SOUNDSETTING soundSetting)
        {
            // get the AS
            AudioSource AS = GetLoop(t, key);

            if (GetVolume(soundSetting) != 0)
            {
                AS.gameObject.SetActive(true);

                // set volume
                AS.volume = GetVolume(soundSetting);
            }
            else
            {
                GetLoop(t, key).gameObject.SetActive(false);
            }
        }

        public void SetVolume(AudioSource AS, float volume, float time, SOUNDSETTING setting, System.Action<float> update)
        {
            LeanTween.value(gameObject, update, AS.volume * GetVolume(SOUNDSETTING.MUSIC), volume, time).setEase(LeanTweenType.easeInOutQuad);
        }

        public void SetVolume (AudioSource AS, float volume, SOUNDSETTING setting)
        {
            AS.volume = volume * GetVolume(setting);
        }

        private float GetVolume (Transform t)
        {
            return t.GetComponent<AudioSource>().volume;
        }

        private float GetVolume (SOUNDSETTING soundSetting)
        {
            switch (soundSetting)
            {
                case SOUNDSETTING.MUSIC:
                    return Settings.soundMusic;
                case SOUNDSETTING.SFX:
                    return Settings.soundFX;
                case SOUNDSETTING.SPEAK:
                    return Settings.soundSpeak;
                default:
                    return 1f;
            }
        }

        private Transform GetTransform (bool soundOcclusion)
        {
            return soundOcclusion ? _realSoundTransform : _audioTransform;
        }

        public void UpdateBackgroundVolume (float volume)
        {
            if (volume <= 0.1f)
            {
                volume = 0f;
            }

            _backgroundMusic.GetComponent<AudioSource>().volume = volume;
        }

        public void LoadSounds (System.Action<AudioClip> doneAction)
        {
            loader.Init(doneAction);
        }

        public List<AudioClip> GetLoadedClips ()
        {
            return loader.clips;
        }

        class Loader {
            public enum SeekDirection { Forward, Backward }
            
            public List<AudioClip> clips = new List<AudioClip>();

            [SerializeField]
            [HideInInspector]
            private int currentIndex = 0;

            private FileInfo[] soundFiles;
            private List<string> validExtensions = new List<string> { ".ogg", ".wav" }; // Don't forget the "." i.e. "ogg" won't work - cause Path.GetExtension(filePath) will return .ext, not just ext.
            private string absolutePath = "./ImportSounds"; // relative path to where the app is running - change this to "./music" in your case

            private System.Action<AudioClip> actionCallback;

            public void Init(System.Action<AudioClip> doneAction)
            {
                //being able to test in unity
                if (Application.isEditor) absolutePath = "ImportSounds";

                Debug.Log(absolutePath);

                actionCallback = doneAction;

                ReloadSounds();
            }

            void ReloadSounds()
            {
                clips.Clear();
                // get all valid files
                var info = new DirectoryInfo(absolutePath);
                soundFiles = info.GetFiles()
                    .Where(f => IsValidFileType(f.Name))
                    .ToArray();

                // and load them
                foreach (var s in soundFiles)
                    JPL.Core.CoreHelper.StartCoroutine(LoadFile(s.FullName));

                
            }

            bool IsValidFileType(string fileName)
            {
                return validExtensions.Contains(System.IO.Path.GetExtension(fileName));
                // Alternatively, you could go fileName.SubString(fileName.LastIndexOf('.') + 1); that way you don't need the '.' when you add your extensions
            }

            IEnumerator LoadFile(string path)
            {
                WWW www = new WWW("file://" + path);
                //Debug.Log("loading " + path);

                AudioClip clip = www.GetAudioClip(false);
                while (clip.loadState != AudioDataLoadState.Loaded)
                    yield return www;

                //print("done loading");
                clip.name = System.IO.Path.GetFileName(path);
                clips.Add(clip);

                //Debug.Log("loaded clip: " + clip.name);
                actionCallback(clip);
            }
        }
    }
}
