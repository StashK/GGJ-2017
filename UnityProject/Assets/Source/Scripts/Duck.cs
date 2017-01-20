using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : MonoBehaviour {

    private AirConsoleManager.Player airController;
    private float angle = 0;
    public float speed = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    private void GoLeft()
    {
        angle -= speed * Time.deltaTime;
    }

    private void GoRight()
    {
        angle += speed * Time.deltaTime;
    }
}
