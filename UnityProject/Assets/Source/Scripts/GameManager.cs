using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public List<Duck> duckList;
    public bool isGameFinished = false;
	// Use this for initialization
	void Start () {
       duckList = FindObjectsOfType<Duck>().ToList<Duck>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!isGameFinished)
        {
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
}
