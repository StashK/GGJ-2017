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

	public Vector3 displacementVector;
	public float displacementRechargeLerp;

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
        rb.velocity = transform.forward * DuckGameGlobalConfig.moveSpeed + displacementVector;

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
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * Mathf.Pow(fatness, -3), 0.1f);

		displacementVector = Vector3.Lerp(displacementVector, Vector3.zero, displacementRechargeLerp);

		if(DuckGameGlobalConfig.drawDebugLines)
			Debug.DrawRay(transform.position, displacementVector, Color.green);

		if (airController.GetButtonDown(InputAction.Gameplay.WeaponLeft))
        {
            SubtitleRenderer.AddSubtitle(new DuckTitles
            {
                Text = "Quack !",
                Colour = transform.Find("Ducky_Body").GetComponent<Renderer>().material.color,
                Size = 32
            });
        }
    }

    public float GetTerrainHeight(float xPos, float zPos)
    {
        int x = (int)(xPos) + 25;
        int z = (int)(zPos) + 25;

        int i = x + z * 25;

        float height = WavePlane.HeightMap[i];

        return height;
    }

    private void GoLeft()
    {
		displacementVector -= transform.right * DuckGameGlobalConfig.sideMoveSpeed;
    }

    private void GoRight()
    {
		displacementVector += transform.right * DuckGameGlobalConfig.sideMoveSpeed;
    }

    public void Kill()
    {
        isDeath = true;

    }

	public void OnCollisionEnter(Collision other)
	{
		if(other.collider.tag == "BreadPickup")
		{
			fatness++;
			Destroy(other.gameObject);
		}
	}
}
