using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<Duck> duckList;
    public bool isGameFinished = false;
    public float startTime;
    private bool isPreFallOffFinished = false;
    public float lastDropOffTime;
    public HexGrid hexGrid;
    private float maxDistance;
    public float duckStartY;

    public int maxQuaks;
    public int quakCounter;
    public bool vaporTrapMode;
    private float vaporTrapTimer;
    public Text quakCounterText;

	public bool killOffSoundPlayed;

    public AudioSource backgroundAudioSource;
	public AudioClip vaporTrapClip;
    public AudioClip vaporDefaultClip;
    public AudioSource killoffAudioSource;

    float moveSpeedCache;
    float sideMoveSpeedCache;
    float duckPushDistanceCache;
	float quackSpamIntervalCache;

	private static GameManager instance;
    public static GameManager Get { get { return instance; } }

    public static float GameIntroTime;

	// Use this for initialization
	void Start()
    {
        instance = this;
        int activePlayers = AirConsoleManager.Instance.ActivePlayers().Count;
        float steps = 360.0f / activePlayers;
        int steppers = 0;
        foreach (AirConsoleManager.Player p in AirConsoleManager.Instance.ActivePlayers())
        {
            Vector2 spawnPosition = new Vector2(1, 0);
            spawnPosition = spawnPosition.Rotate(steppers * steps);
            Transform instantiatedDuckTransform = Instantiate(JPL.Core.Prefabs.duck, new Vector3(spawnPosition.x * DuckGameGlobalConfig.startDistance, duckStartY, spawnPosition.y * DuckGameGlobalConfig.startDistance), Quaternion.identity);
            Duck neededDuck = instantiatedDuckTransform.GetComponent<Duck>();
            neededDuck.playerId = p.PlayerId;
            neededDuck.playerName = p.playerName;
            duckList.Add(neededDuck);
            steppers++;
        }
        startTime = Time.time;
        maxDistance = DuckGameGlobalConfig.startDistance + 3;
        GameIntroTime = 3.0f;


        maxQuaks = activePlayers * 20;
    }

    // Update is called once per frame
    void Update()
    {
        GameIntroTime -= Time.deltaTime;

        if (!isGameFinished)
        {
            if (!isPreFallOffFinished && Time.time >= startTime + DuckGameGlobalConfig.preDropOffTime) //Duck dieing starts
            {
                isPreFallOffFinished = true;
				killOffSoundPlayed = false;
				lastDropOffTime = Time.time;
                Debug.Log("dropoff starting");
            }

            if (isPreFallOffFinished && !vaporTrapMode)
            {
                if (Time.time >= lastDropOffTime + DuckGameGlobalConfig.dropOffTime) //Furthers duck dies 
                {
                    lastDropOffTime = Time.time;
                    Duck furtherstDuck = GetFurtherstDuck();
                    furtherstDuck.Kill();
                    maxDistance = Vector3.Distance(furtherstDuck.transform.position, Vector3.zero);
                    //hexGrid.SetFalloff(Vector3.Distance(furtherstDuck.transform.position, Vector3.zero));
                    MeshGen.curDistance = Vector3.Distance(furtherstDuck.transform.position, Vector3.zero);
                    Debug.Log("someone lost");
                }
				if(!killOffSoundPlayed && Time.time >= lastDropOffTime + DuckGameGlobalConfig.dropOffTime - killoffAudioSource.clip.length )
				{
					killoffAudioSource.PlayOneShot(killoffAudioSource.clip);
					killOffSoundPlayed = true;
				}
            }

			if(quakCounter >= maxQuaks && !vaporTrapMode)
			{
				quakCounter = 0;
				vaporTrapMode = true;
				vaporTrapTimer = vaporTrapClip.length;
				backgroundAudioSource.clip = vaporTrapClip;
				backgroundAudioSource.volume = 1.5f;
				backgroundAudioSource.Play();
				moveSpeedCache = DuckGameGlobalConfig.moveSpeed;
				sideMoveSpeedCache = DuckGameGlobalConfig.sideMoveSpeed;
				duckPushDistanceCache = DuckGameGlobalConfig.duckPushDistance;
				quackSpamIntervalCache = DuckGameGlobalConfig.quackSpamInterval;
				DuckGameGlobalConfig.moveSpeed = 10f;
				DuckGameGlobalConfig.sideMoveSpeed = 15f;
				DuckGameGlobalConfig.duckPushDistance = 20f;
				DuckGameGlobalConfig.quackSpamInterval = 0f;
				quakCounterText.gameObject.SetActive(false);
			}

            if (vaporTrapMode)
            {
                vaporTrapTimer -= Time.deltaTime;

				if (vaporTrapTimer <= 0f)
				{
					vaporTrapMode = false;
					quakCounter = 0;
					backgroundAudioSource.clip = vaporDefaultClip;
					backgroundAudioSource.volume = 0.5f;
					backgroundAudioSource.Play();
					DuckGameGlobalConfig.moveSpeed = moveSpeedCache;
					DuckGameGlobalConfig.sideMoveSpeed = sideMoveSpeedCache;
					DuckGameGlobalConfig.duckPushDistance = duckPushDistanceCache;
					DuckGameGlobalConfig.quackSpamInterval = quackSpamIntervalCache;
					vaporTrapTimer = vaporTrapClip.length;
					quakCounterText.gameObject.SetActive(true);
				}
			}
			else
			{
				quakCounterText.text = (maxQuaks - quakCounter).ToString();
				foreach (Duck d in duckList)
				{
					float duckDistance = Vector3.Distance(d.transform.position, new Vector3(0, duckStartY, 0));
					if (duckDistance >= maxDistance) //if duck distance is higher than max distance, kill the duck
						d.Kill();

                    if (duckDistance <= DuckGameGlobalConfig.winDistance)
                    {
                        PlayerWon(d.playerName);
                        return;
                    }
                }
            }
        }
    }

    void PlayerWon(string name)
    {
        Debug.Log(name + " won");
        duckList.Sort();
        isGameFinished = true;

        foreach (Duck d in duckList)
        {
            Debug.Log("killing duck");
            d.Kill();
        }
    }

    Duck GetFurtherstDuck()
    {
        Duck returnDuck;
        float furtherstDistance = 0;
        returnDuck = duckList[0];
        foreach (Duck d in duckList)
        {
            if (!d.IsDeath())
            {
                if (Vector3.Distance(d.transform.position, new Vector3(0, 0, 0)) > furtherstDistance)
                {
                    returnDuck = d;
                    furtherstDistance = Vector3.Distance(d.transform.position, new Vector3(0, 0, 0));
                }
            }
        }
        return returnDuck;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("Gameplay");
    }
}
