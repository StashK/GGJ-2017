using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public Duck[] duckArray;
	// Use this for initialization
	void Start () {
       duckArray = FindObjectsOfType<Duck>();
        Debug.Log(duckArray.Length);
	}
	
	// Update is called once per frame
	void Update () {
        foreach (Duck d in duckArray)
        {
            if (Vector3.Distance(d.transform.position, new Vector3(0, 0, 0)) >= DuckGameGlobalConfig.winDistance)
            {
                PlayerWon(d.playerName);
            }
        }
	}

    void PlayerWon(string name)
    {


    }
}
