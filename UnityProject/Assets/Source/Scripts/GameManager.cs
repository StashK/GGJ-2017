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
	// Use this for initialization
	void Start () {
       duckList = FindObjectsOfType<Duck>().ToList<Duck>();
        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        if (!isGameFinished)
        {
            if(!isPreFallOffFinished && Time.time >= startTime + DuckGameGlobalConfig.preDropOffTime)
            {
                isPreFallOffFinished = true;
                lastDropOffTime = Time.time;
                Debug.Log("Fall off starting");
            }

            if (isPreFallOffFinished)
            {
                if (Time.time >= lastDropOffTime + DuckGameGlobalConfig.DropOfftime)
                {
                    lastDropOffTime = Time.time;
                    GetFurtherstDuck().isDeath = true;
                    Debug.Log("someone lost");
                }
            }

            foreach (Duck d in duckList)
            {
                if (Vector3.Distance(d.transform.position, new Vector3(0, 0, 0)) <= DuckGameGlobalConfig.winDistance)
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
            if (Vector3.Distance(d.transform.position, new Vector3(0, 0, 0)) > furtherstDistance)
            {
                returnDuck = d;
                furtherstDistance = Vector3.Distance(d.transform.position, new Vector3(0, 0, 0));
            }

        }
        return returnDuck;
    }
}
