using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : MonoBehaviour {

    private AirConsoleManager.Player airController;
    public int playerId;
    private float angle = 0;
    [Range(0,1)]
    public float distance = 1.0f;
	// Use this for initialization
	void Start () {
        airController = AirConsoleManager.Instance.GetPlayer(playerId);
	}
	
	// Update is called once per frame
	void Update () {
        distance -= DuckGameGlobalConfig.distanceSpeed * Time.deltaTime;

        if (airController.GetButtonDown(InputAction.Gameplay.MoveLeft))
        {
            GoLeft();
        }

        if (airController.GetButtonDown(InputAction.Gameplay.MoveRight))
        {
            GoRight();
        }
	}


    private void GoLeft()
    {
        angle -= DuckGameGlobalConfig.moveSpeed * Time.deltaTime;
    }

    private void GoRight()
    {
        angle += DuckGameGlobalConfig.moveSpeed * Time.deltaTime;
    }
}
