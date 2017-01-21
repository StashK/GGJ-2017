﻿using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Duck : MonoBehaviour, IComparable
{

    private AirConsoleManager.Player airController;
    public int playerId;
    public string playerName;
    public float angle = 0;
    [Range(0, 1)]
    public float distance = 1.0f;
	public float fatness = 1.0f;

    private bool isDeath = false;
    private Rigidbody rb;
    private bool tapped = false;
    private bool isDoubleTapped = false;
    private int doubleTapCounter = 0;
    public bool IsDeath()
    {
        return isDeath;

    }

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
    void Start()
    {
        airController = AirConsoleManager.Instance.GetPlayer(playerId);
        PastelGenerator.Lightness = 0.3f;
        GetComponent<Renderer>().material.color = PastelGenerator.Generate();
        rb = GetComponent<Rigidbody>();
        transform.LookAt(FindObjectOfType<AntiManController>().transform);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(isDeath);
        if(isDeath)
            return;


        Debug.Log(transform.forward);
        transform.LookAt(FindObjectOfType<AntiManController>().transform);
        rb.velocity = transform.forward * DuckGameGlobalConfig.moveSpeed;

        if (tapped)
            doubleTapCounter++;

        if (doubleTapCounter >= 10)
        {
            doubleTapCounter = 0;
            tapped = false;
            Debug.Log("tapped is false");
        }
        
        
        if (airController.GetButtonDown(InputAction.Gameplay.MoveLeft))
        {
            GoLeft();

            if (tapped)
            {
                isDoubleTapped = true;
                Debug.Log("Double tapped");
            }
        }
        
        if (airController.GetButtonDown(InputAction.Gameplay.MoveRight))
        {
            GoRight();

            if (tapped)
            {
                isDoubleTapped = true;
                Debug.Log("Double tapped");
            }
        }

        if (airController.GetButtonUp(InputAction.Gameplay.MoveLeft) || airController.GetButtonUp(InputAction.Gameplay.MoveRight))
        {
            tapped = true;
            Debug.Log("tapped is true");
        }
    }

    void Update()
    {
       // transform.LookAt(GetComponent<AntiManController>().transform);

    }

    private void GoLeft()
    {
        rb.velocity += transform.right;
    }

    private void GoRight()
    {
        rb.velocity -= transform.right;
    }

    public void Kill()
    {
        isDeath = true;

    }

	void OnCollisionEnter(Collision collision)
    {
		
        /*if (collision.collider.tag == "Player")
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
            if (!isDoubleTapped)
            {
                if (Vector3.Dot(right, (from - to)) < 0)
                {
                    angle += diffAngle * 0.1f;
                }
                else
                {
                    angle -= diffAngle * 0.1f;
                }
            }
            else
            {
                if (Vector3.Dot(right, (from - to)) < 0)
                {
                    collision.transform.GetComponent<Duck>().angle -= diffAngle * 0.8f;
                }
                else
                {
                    collision.transform.GetComponent<Duck>().angle += diffAngle * 0.8f;
                }
            }
        }

		if(collision.collider.tag == "BreadPickup")
		{
			Debug.Log("BreadPickup");
			fatness++;
			transform.localScale = Vector3.one * fatness;
			Destroy(collision.collider.gameObject);
		}
		*/
    }
}
