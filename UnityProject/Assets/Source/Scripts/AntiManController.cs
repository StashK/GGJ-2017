using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiManController : MonoBehaviour {

	public Vector2 inputVector;
	public float inputLength;

	public Vector3 targetLineForward;

	public ParticleSystem waveParticles;

	public bool isWaving;

	// Use this for initialization
	void Start () {
		waveParticles = GetComponentInChildren<ParticleSystem>();
		if (!waveParticles)
			Debug.LogWarning("Particles on player not found");
	}

	void CheckInput()
	{
		inputVector.x = Input.GetAxisRaw("Horizontal");
		inputVector.y = Input.GetAxisRaw("Vertical");
	}

	void UpdateWaving()
	{
		targetLineForward = new Vector3(inputVector.x, 0, -inputVector.y);
		inputLength = targetLineForward.magnitude;

		//Debug.Log("InputLenght: " + inputLength);

		// Toggle isWaving and particles
		if (inputLength > 0.05f)
		{
			waveParticles.transform.rotation = Quaternion.LookRotation(targetLineForward, transform.up);
			isWaving = true;
			if(!waveParticles.isEmitting)
				waveParticles.Play();
		}
		else if(isWaving)
		{
			isWaving = false;
			if (waveParticles.isEmitting)
				waveParticles.Stop();
		}
	}

	void PushBackDucks()
	{
		Collider[] gameObjectsInRange = Physics.OverlapCapsule(transform.position, transform.position + targetLineForward.normalized, 1.5f);

		foreach (Collider collider in gameObjectsInRange)
		{
			GameObject gameObject = collider.gameObject;
			Duck duck = gameObject.GetComponent<Duck>();

			if(duck)
			{
				duck.distance += 0.1f;
			}
			
		}

	}

	// Update is called once per frame
	void Update () {
		CheckInput();

		UpdateWaving();

		if(isWaving)
		{
			PushBackDucks();
		}

	}
}
