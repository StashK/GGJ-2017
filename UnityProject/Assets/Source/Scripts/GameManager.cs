using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public List<Duck> duckList;
    public bool isGameFinished = false;
    public float startTime;
    private bool isPreFallOffFinished = false;
    public float lastDropOffTime;
    public HexGrid hexGrid;
    private float maxDistance;
	// Use this for initialization
	void Start () {
        foreach (AirConsoleManager.Player p in AirConsoleManager.Instance.ActivePlayers())
        {
            Transform instantiatedDuckTransform = Instantiate(JPL.Core.Prefabs.duck, new Vector3(10.0f, 0, 0), Quaternion.identity);
            instantiatedDuckTransform.GetComponent<Duck>().playerId = p.PlayerId;
            instantiatedDuckTransform.GetComponent<Duck>().playerName = p.playerName;
            instantiatedDuckTransform.GetComponent<Duck>().angle = Random.Range(0.0f, 360.0f);
            duckList.Add(instantiatedDuckTransform.GetComponent<Duck>());
        }
        startTime = Time.time;
        maxDistance = DuckGameGlobalConfig.startDistance;
	}
	
	// Update is called once per frame
	void Update () {

        if (!isGameFinished)
        {
            if(!isPreFallOffFinished && Time.time >= startTime + DuckGameGlobalConfig.preDropOffTime) //Duck dieing starts
            {
                isPreFallOffFinished = true;
                lastDropOffTime = Time.time;
                Debug.Log("dropoff starting");
            }

            if (isPreFallOffFinished)
            {
                if (Time.time >= lastDropOffTime + DuckGameGlobalConfig.dropOffTime) //Furthers duck dies 
                {
                    lastDropOffTime = Time.time;
                    Duck furtherstDuck = GetFurtherstDuck();
                    furtherstDuck.Kill();
                    maxDistance = Vector3.Distance(furtherstDuck.transform.position, Vector3.zero);
                    hexGrid.SetFalloff(Vector3.Distance(furtherstDuck.transform.position, Vector3.zero));
                    Debug.Log("someone lost");
                }
            }

            foreach (Duck d in duckList)
            {
                float duckDistance = Vector3.Distance(d.transform.position, new Vector3(0, 0, 0));
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

    void PlayerWon(string name)
    {
        Debug.Log(name + " won");
        duckList.Sort();
        isGameFinished = true;
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
}
