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
    }

    /// <summary>
    /// Get the height of the terrain at given horizontal coordinates.
    /// </summary>
    /// <param name="xPos">X coordinate</param>
    /// <param name="zPos">Z coordinate</param>
    /// <returns>Height at given coordinates</returns>
    public float GetTerrainHeight(float xPos, float zPos)
    {

        Mesh mesh = FindObjectOfType<WavePlane>().GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

        // we first get the height of four points of the quad underneath the point
        // Check to make sure this point is not off the map at all
        int x = (int)(xPos);
        int z = (int)(zPos);

        int xPlusOne = x + 1;
        int zPlusOne = z + 1;

        float height = (vertices[x + z * (int)Mathf.Sqrt(vertices.Length)]).y;

        return height;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position + Vector3.up * GetTerrainHeight(transform.position.x, transform.position.y), 1.0f);
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
