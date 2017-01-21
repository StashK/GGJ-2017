using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Duck : MonoBehaviour, IComparable {

    private AirConsoleManager.Player airController;
    public int playerId;
    public string playerName;
    public float angle = 0;
    [Range(0,1)]
    public float distance = 1.0f;

    public bool isDeath = false;

    private float prevAngle;

    public int CompareTo(object obj)
    {
        if (obj == null) return 1;

        Duck otherDuck = obj as Duck;
        if (otherDuck != null)
        {
            return this.distance.CompareTo(otherDuck.distance);
        }

        return 0;
    }
	// Use this for initialization
	void Start () {
        airController = AirConsoleManager.Instance.GetPlayer(playerId);
        prevAngle = angle;
        PastelGenerator.Lightness = 0.3f;
        GetComponent<Renderer>().material.color = PastelGenerator.Generate();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!isDeath)
        {
            distance -= DuckGameGlobalConfig.distanceSpeed * Time.deltaTime;

            if (airController.GetButtonDown(InputAction.Gameplay.MoveLeft))
            {
                GoLeft();
            }

            if (airController.GetButtonDown(InputAction.Gameplay.MoveRight))
            {
                GoRight();
            }

            Vector2 toBePlacedVector = new Vector2(1.0f, 0.0f);
            toBePlacedVector = toBePlacedVector.Rotate(angle) * distance * DuckGameGlobalConfig.startDistance;
            transform.position = new Vector3(toBePlacedVector.x, 0, toBePlacedVector.y);
        }
	}

    private void Update()
    {
        prevAngle = angle;
    }

    private void GoLeft()
    {
        angle -= DuckGameGlobalConfig.moveSpeed * Time.deltaTime;
    }

    private void GoRight()
    {
        angle += DuckGameGlobalConfig.moveSpeed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter");
        if (collision.collider.tag == "Player")
        {
            Debug.Log(collision.contacts[0].point);
            //HackyHacky sorta working
            Vector3 right = Vector3.Cross(this.transform.position, Vector3.up);
            Vector3 from = this.transform.position;
            Vector3 to = collision.transform.position;
            Vector3 centerToSide = (to - from).normalized * (transform.localScale.x);
            from += centerToSide;
            to -= centerToSide;
            float diffAngle = Vector3.Angle(from, to);
            if (Vector3.Dot(right, (from - to)) < 0)
            {
                angle += diffAngle * 0.1f;
            }
            else
            {
                angle -= diffAngle * 0.1f;
            }
        }
    }
}
