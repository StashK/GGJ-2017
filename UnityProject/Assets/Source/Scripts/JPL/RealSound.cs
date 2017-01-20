using UnityEngine;
using System.Collections;

namespace JPL
{
    [RequireComponent(typeof(AudioLowPassFilter))]
    [RequireComponent(typeof(AudioSource))]
    public class RealSound : Mono
    {
        /// <summary>
        /// This should be the player transform that produced the sound, used for getting the correct path
        /// </summary>
        public Transform owner;
        /// <summary>
        /// The target (the actual player transform)
        /// </summary>
        public Transform target;
        /// <summary>
        /// The position the sound has to move to (used for tweening)
        /// </summary>
        public Vector3 targetPos;
        /// <summary>
        /// The original position of the sound
        /// </summary>
        public Vector3 originPos;
        /// <summary>
        /// Holds the connection to the AudoLowPassFilter
        /// </summary>
        public AudioLowPassFilter lowPass;
        /// <summary>
        /// Used for the amount of lowpass based on distance
        /// </summary>
        public AnimationCurve lowPassCurve = new AnimationCurve(new Keyframe[2] { new Keyframe(0f, 0f, 0f, 0f), new Keyframe(1f, 1f, 3f, 0f) });
        /// <summary>
        /// Holds the connection to the audio source
        /// </summary>
        public AudioSource audioSource;
        /// <summary>
        /// The curve used for volume, base on distance
        /// </summary>
        //public AnimationCurve volumeCurve = new AnimationCurve(new Keyframe[4] { new Keyframe(0f, 0f, 0f, 0f), new Keyframe(0.7147133f, 0.06254925f, 0.2f, 0.2f), new Keyframe(0.8888133f, 0.3303277f, 3f, 3f), new Keyframe(1f, 1f, 0f, 0f) });
        public AnimationCurve volumeCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        /// <summary>
        /// The target volume, used for tweening
        /// </summary>
        public float volumeTarget = 1f;
        /// <summary>
        /// The saved path that this sound has to use
        /// </summary>
        public Vector3[] path;
        /// <summary>
        /// The target lowPass, used for tweening
        /// </summary>
        public int lowPassTarget = 22000;
        public float audioDistance;
        /// <summary>
        /// The max audio distance
        /// </summary>
        public float maxAudioDistance = 100f;
        /// <summary>
        /// The max audio distance target, used for tweening
        /// </summary>
        private float maxAudioDistanceTarget;
        /// <summary>
        /// Is true when the audio can reach the player without obstacles
        /// </summary>
        //private bool directAudio;

        void Start()
        {
            // get connections
            lowPass = GetComponent<AudioLowPassFilter>();
            audioSource = GetComponent<AudioSource>();

            audioSource.spatialBlend = 1f;
            audioSource.dopplerLevel = 0f;
            audioSource.maxDistance = maxAudioDistance;

            // create a new owner if none are found
            if (owner == transform)
            {
                owner = new GameObject("_RealSoundSource").transform;
                owner.position = this.transform.position;

                StartSound(owner);
            }

            target = GameObject.FindGameObjectWithTag("AudioListener").transform;
            
        }

        public void StartSound(Transform _owner, AudioClip AC = null, float volume = 1f)
        {
            owner = _owner;

            if (Camera.main != null) {
                // get all the required connections
                target = Camera.main.transform;
            }

            if (owner != null && target != null) {
                //audioSource.volume = 1f;
                //volumeTarget = 1f;

                // Add this path to the path updater
                //Core.PathUpdater.AddPath(owner, target, true);

                audioSource.rolloffMode = AudioRolloffMode.Custom;

                audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, new AnimationCurve(new Keyframe[2] { new Keyframe(0f, 1f), new Keyframe(1f, 1f) }));

                // Save the original position of this sound
                originPos = owner.position;

                // Get the path of the sound
                path = Core.PathUpdater.GetPath(owner, target);

                // Update the lowpass filter
                UpdateLowPass();

                // Update the position
                UpdatePos();

                UpdateVolumeTarget();

                // Inital cutoffFrequency
                lowPass.cutoffFrequency = lowPassTarget;
                audioSource.volume = volumeTarget;

                transform.position = targetPos;

                audioSource.PlayOneShot(AC, volumeTarget);

                //Debug.Log("distance: "+ Core.PathUpdater.GetDistance(owner, target) + " volume:" + volumeTarget);
            }
            else
            {
                audioSource.volume = 1f;
                volumeTarget = 1f;

                audioSource.Play();
            }

        }

        void Update()
        {
            if (owner != null && target != null)
            {
                /*for (int i = 0; i < path.corners.Length - 1; i++)
                    Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);*/
            }
        }

        void FixedUpdate()
        {
            if (owner && target && audioSource.loop)
            {
                // Get the newest path
                path = Core.PathUpdater.GetPath(owner, target);
                // Update the position
                UpdatePos();
                // update the low pass filter
                UpdateLowPass();

                UpdateVolumeTarget();

                // update stuff
                audioDistance = Mathf.Lerp(maxAudioDistance, maxAudioDistanceTarget, Time.deltaTime * 3f);
                audioSource.volume = Mathf.Lerp(audioSource.volume, volumeTarget, Time.deltaTime * 5f);
            }
        }
        
        private void UpdateVolumeTarget ()
        {
            // check if the path can reach the player (otherwise there's a door, hopefully)
            if (Core.PathUpdater.GetStatus(owner, target) == UnityEngine.AI.NavMeshPathStatus.PathComplete)
            {
                //volumeTarget = volumeCurve.Evaluate(1f - (1f / maxAudioDistance * Core.PathUpdater.GetDistance(owner, target)));
                volumeTarget = 1f / Mathf.Exp(1f / maxAudioDistance * Core.PathUpdater.GetDistance(owner, target));
                //Debug.Log(""+ 1f / Mathf.Exp(1f / maxAudioDistance * Core.PathUpdater.GetDistance(owner, target)));
                maxAudioDistanceTarget = maxAudioDistance;
            }
            else
            {
                volumeTarget = 1f / Mathf.Exp(1f / maxAudioDistance * Core.PathUpdater.GetDistance(owner, target));
                maxAudioDistanceTarget = 20f;
            }

            volumeTarget *= volumeCurve.Evaluate(1f - (1f / maxAudioDistance * Core.PathUpdater.GetDistance(owner, target)));

            if (Core.PathUpdater.GetDistance(owner, target) > maxAudioDistance)
            {
                volumeTarget = 0f;
            }
        }

        private void UpdatePos()
        {
            if (path.Length > 2) // The listener can't be reached directly from the source
            {
                targetPos = path[path.Length - 2];
            }
            else // The listener can be reached from the source
            {
                targetPos = originPos;
            }

            // Update the position
            transform.position = Vector3.Slerp(transform.position, targetPos, Time.deltaTime * 5f);
        }

        private void UpdateLowPass()
        {
            if (path != null) {
                // Get the range for the raycast
                float maxRange = Vector3.Distance(originPos, target.position);

                RaycastHit hit;
                if (Physics.Raycast(originPos, (target.position - originPos), out hit, maxRange, Core.Sounds.layerMask))
                {
                    //directAudio = false;
                    //if (hit.transform != target) // hit something else then 
                    //{
                    Debug.DrawLine(originPos, target.position, Color.red);

                    switch (path.Length)
                    {
                        case 0:
                            lowPassTarget = 12000;
                            break;
                        case 1:
                            lowPassTarget = 10000;
                            break;
                        case 2:
                            lowPassTarget = 9000;
                            break;
                        case 3:
                            lowPassTarget = 8000;
                            break;
                        case 4:
                            lowPassTarget = 7000;
                            break;
                        case 5:
                            lowPassTarget = 6000;
                            break;
                        case 6:
                            lowPassTarget = 5000;
                            break;
                        case 7:
                            lowPassTarget = 4000;
                            break;
                        case 8:
                            lowPassTarget = 3000;
                            break;
                        case 9:
                            lowPassTarget = 2000;
                            break;
                        case 10:
                            lowPassTarget = 1500;
                            break;
                        case 11:
                            lowPassTarget = 1000;
                            break;
                        default:
                            lowPassTarget = 800;
                            break;
                    }
                    //}

                }
                else
                {
                    //directAudio = true;
                    lowPassTarget = 22000;
                    Debug.DrawLine(originPos, target.position, Color.green);
                }

                //Debug.Log(Mathf.Lerp(lowPass.cutoffFrequency, lowPassTarget, Time.deltaTime));
                lowPass.cutoffFrequency = Mathf.Lerp(lowPass.cutoffFrequency, CalcAudioTarget(), Time.deltaTime * 3f);
            }
        }

        public int CalcAudioTarget()
        {
            return (int)(lowPassTarget * (Core.PathUpdater.GetStatus(owner, target) == UnityEngine.AI.NavMeshPathStatus.PathComplete ? 1f : 0.1f));
            //return (int)Mathf.Max((lowPassTarget * (Core.PathUpdater.GetStatus(owner, target) == NavMeshPathStatus.PathComplete ? 1f : 0.1f) * lowPassCurve.Evaluate(1f - (1f / maxAudioDistance * Core.PathUpdater.GetDistance(owner, target)))), 800);
        }
    }
}