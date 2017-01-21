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
    public float fatness = 1.0f;

    private bool isDeath = false;
    private Rigidbody rb;
    private bool isBashActivated = false;
    private int bashCounter = 0;
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
            return Mathf.RoundToInt(Vector3.Distance(this.transform.position, Vector3.zero) - Vector3.Distance(otherDuck.transform.position, Vector3.zero));
        }

        return 0;
    }
    // Use this for initialization
    void Start()
    {
        airController = AirConsoleManager.Instance.GetPlayer(playerId);
        PastelGenerator.Lightness = 0.8f;
        transform.Find("Ducky_Body").GetComponent<Renderer>().material.color = PastelGenerator.Generate();
        rb = GetComponent<Rigidbody>();
        transform.LookAt(FindObjectOfType<AntiManController>().transform);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isDeath)
            return;

        if (isBashActivated)
            bashCounter++;

        if (bashCounter >= 100)
        {
            isBashActivated = false;
            bashCounter = 0;
            Debug.Log("Bash deactivated " + Time.frameCount);
        }

        //Movement
        transform.LookAt(FindObjectOfType<AntiManController>().transform);
        rb.velocity = transform.forward * DuckGameGlobalConfig.moveSpeed;

        if (airController.GetButton(InputAction.Gameplay.MoveLeft))
        {
            GoLeft();
        }

        if (airController.GetButton(InputAction.Gameplay.MoveRight))
        {
            GoRight();
        }
    }

    void Update()
    {
        if (airController.GetButtonDown(InputAction.Gameplay.WeaponLeft))
        {
            Debug.Log("FSDFSDF");
            SubtitleRenderer.AddSubtitle(new DuckTitles
            {
                Text = "Quack !",
                Colour = PastelGenerator.Generate(),
                Size = 32
            });
        }

        if (airController.GetButtonDown(InputAction.Gameplay.WeaponRight))
        {
            isBashActivated = true;
            bashCounter = 0;
            Debug.Log("Bash Activated" + Time.frameCount);
        }

        RaycastHit hit;
        if (Physics.Raycast(new Vector3(transform.position.x, 1000, transform.position.z), -Vector3.up, out hit, 1000.0f))
        {
            Debug.Log(hit.point);
            transform.position = new Vector3(transform.position.x, hit.point.y + 0.8f, transform.position.z);
        }
    }

    private void GoLeft()
    {
        rb.velocity += transform.right * DuckGameGlobalConfig.turnSpeed;
    }

    private void GoRight()
    {
        rb.velocity -= transform.right * DuckGameGlobalConfig.turnSpeed;
    }

    public void Kill()
    {
        isDeath = true;

    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            if (isBashActivated)
            {
                Vector3 bashVector = collision.transform.position - transform.position;
                bashVector.Normalize();

                collision.transform.GetComponent<Rigidbody>().AddForce(bashVector * DuckGameGlobalConfig.bashForce);
            }
        }
    }
}
