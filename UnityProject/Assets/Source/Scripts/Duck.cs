using System.Collections;
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
        transform.Find("Ducky_Body").GetComponent<Renderer>().material.color = PastelGenerator.Generate();
        rb = GetComponent<Rigidbody>();
        transform.LookAt(FindObjectOfType<AntiManController>().transform);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isDeath)
            return;

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


        if (airController.GetButton(InputAction.Gameplay.MoveLeft))
        {
            GoLeft();

            if (tapped)
            {
                isDoubleTapped = true;
                Debug.Log("Double tapped");
            }
        }

        if (airController.GetButton(InputAction.Gameplay.WeaponLeft))
        {
            SubtitleRenderer.AddSubtitle(new DuckTitles
            {
                Text = "Quack !",
                Colour = PastelGenerator.Generate(),
                Size = 16
            });
        }

        if (airController.GetButton(InputAction.Gameplay.MoveRight))
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
}
