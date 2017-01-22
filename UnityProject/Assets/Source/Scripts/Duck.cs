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
	public int fatness = 1;
    private float fatnessTimer = 0f;

	public Vector3 displacementVector;
	public float displacementRechargeLerp;

	private bool isDeath = false;
	private Rigidbody rb;
	private bool tapped = false;
	private bool isDoubleTapped = false;
	private int doubleTapCounter = 0;

	private float lastQuackTime;

	private Color duckColour;

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
		duckColour = PastelGenerator.Generate();
		airController = AirConsoleManager.Instance.GetPlayer(playerId);
		PastelGenerator.Lightness = 0.8f;
        transform.Find("Ducky_Body").GetComponent<Renderer>().material.color = duckColour;
		rb = GetComponent<Rigidbody>();
		transform.LookAt(FindObjectOfType<AntiManController>().transform);

        fatnessTimer = DuckGameGlobalConfig.removeDuckFatnessInterval;
    }

    // Update is called once per frame
    void FixedUpdate()
	{
        if (isDeath || GameManager.GameIntroTime > 0.0f)
			return;

		transform.LookAt(FindObjectOfType<AntiManController>().transform);

        float vectorLength = Mathf.Clamp(displacementVector.magnitude, 0, DuckGameGlobalConfig.moveSpeed * 15.0f);
        displacementVector.Normalize();
        displacementVector *= vectorLength;
		rb.velocity = transform.forward * DuckGameGlobalConfig.moveSpeed + displacementVector;
		
		/*
		if (GameManager.Get.vaporTrapMode && transform.position.magnitude < DuckGameGlobalConfig.startDistance - 1f)
		{
			transform.position = transform.position.normalized * (DuckGameGlobalConfig.startDistance - 1f);
			rb.velocity = Vector3.zero;
		}
		*/

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
        if (GameManager.GameIntroTime > 0.0f)
            return;
		// transform.LookAt(GetComponent<AntiManController>().transform);
		transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(fatness, fatness, fatness), 0.2f);

		displacementVector = Vector3.Lerp(displacementVector, Vector3.zero, displacementRechargeLerp);

        if (fatnessTimer < 0f)
        {
            fatnessTimer = DuckGameGlobalConfig.removeDuckFatnessInterval;
            if (fatness > 1)
                fatness--;
        }
        fatnessTimer -= Time.deltaTime;

		if (DuckGameGlobalConfig.drawDebugLines)
			Debug.DrawRay(transform.position, displacementVector, Color.green);

		if (airController.GetButtonDown(InputAction.Gameplay.WeaponLeft) && !isDeath && (Time.time - lastQuackTime > DuckGameGlobalConfig.quackSpamInterval))
		{
			SubtitleRenderer.Get.AddSubtitle(new DuckTitles
			{
				Text = "Quack !",
				Colour = new Color(duckColour.r + 0.3f, duckColour.g + 0.3f, duckColour.b + 0.3f),
				Position = transform.position + Vector3.up,
				Size = 32
			}, duckColour);

			GameManager.Get.quakCounter++;
			lastQuackTime = Time.time;
		}
	}

    void LateUpdate()
    {
        Debug.Log(GetTerrainHeight(transform.position.x, transform.position.z));
        transform.position = new Vector3(transform.position.x, GetTerrainHeight(transform.position.x, transform.position.z) + 0.8f, transform.position.z);

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
		transform.LookAt(Vector3.zero);
		displacementVector -= transform.right * DuckGameGlobalConfig.sideMoveSpeed;
	}

	private void GoRight()
	{
		transform.LookAt(Vector3.zero);
        displacementVector += transform.right * DuckGameGlobalConfig.sideMoveSpeed;
    }

	public void Kill()
	{
		isDeath = true;
	}

	public void OnCollisionEnter(Collision other)
	{
		if (other.collider.tag == "BreadPickup")
		{
            if (fatness < 3)
            {
                fatness++;
                Destroy(other.gameObject);
            }
		}
	}
}
