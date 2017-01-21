using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AntiManController : MonoBehaviour
{

    public Vector2 inputVector;
    public float inputLength;
	public bool inputBoostDown;
	public bool inputBoostUp;

    public Vector3 targetLineForward;
    public float targetLineDistance;

    [Range(0, 1)]
    public float targetLineLerp = 0.5f;
    [Range(100f, 10000f)]
    public float duckPushForce = 0.01f;

	public bool isBoosting = false;
	public float boostTime = 2f;
	private float boostTimer = 2f;
	public float boostRechargeRate = 1f;
	public Image boostSprite;

	public ParticleSystem waveParticles;

    public bool drawDebugLines;
    public bool isWaving;

    // Use this for initialization
    void Start()
    {
        waveParticles = GetComponentInChildren<ParticleSystem>();
        if (!waveParticles)
            Debug.LogWarning("Particles on player not found");
    }

    void CheckInput()
    {
        inputVector.x = Input.GetAxisRaw("Horizontal");
        inputVector.y = Input.GetAxisRaw("Vertical");

		inputBoostDown = Input.GetButtonDown("Fire1");
		inputBoostUp = Input.GetButtonUp("Fire1");
	}

    void UpdateWaving()
    {
        targetLineForward = Vector3.Slerp(targetLineForward, new Vector3(inputVector.x, 0, inputVector.y), targetLineLerp);
        inputLength = Mathf.Min(1, targetLineForward.magnitude);

		if (isBoosting)
		{
			if (inputBoostUp)
				isBoosting = false;

			// Set boost
			boostTimer -= Time.deltaTime;
			if (boostTimer > 0f)
				inputLength *= 2f;
			else
				isBoosting = false;
		}
		else
		{
			// Start boost
			if (inputBoostDown)
				isBoosting = true;
			// Recharge boost
			if (boostTimer < boostTime && !Input.GetButton("Fire1"))
				boostTimer += boostRechargeRate * 0.1f;
		}
		if (boostSprite)
			boostSprite.fillAmount = (boostTimer / boostTime);

		//Debug.Log("InputLenght: " + inputLength);

		// Toggle isWaving and particles
		if (inputLength > 0.05f)
        {
            waveParticles.transform.rotation = Quaternion.LookRotation(targetLineForward, transform.up);
            isWaving = true;
            if (!waveParticles.isEmitting)
                waveParticles.Play();
            ParticleSystem.EmissionModule emissionModule = waveParticles.emission;
            emissionModule.rateOverTimeMultiplier = inputLength * 50f;
            ParticleSystem.MainModule mainModule = waveParticles.main;
            mainModule.startSpeed = 10f * inputLength;

            //WavePlane.Get.CreateWave(transform.position, targetLineForward.normalized);
        }
        else if (isWaving)
        {
            isWaving = false;
            if (waveParticles.isEmitting)
                waveParticles.Stop();
        }
    }

    void PushBackDucks()
    {
        Collider[] gameObjectsInRange = Physics.OverlapCapsule(transform.position, transform.position + targetLineForward.normalized * targetLineDistance * inputLength, 1.5f);

        if (drawDebugLines)
            Debug.DrawRay(transform.position, targetLineForward.normalized * targetLineDistance * inputLength, Color.red);

        foreach (Collider collider in gameObjectsInRange)
        {
            Rigidbody duck = collider.GetComponent<Rigidbody>();

            if (duck)
            {
                duck.AddForce(targetLineForward * duckPushForce * inputLength, ForceMode.Force);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();

        UpdateWaving();

        if (isWaving)
        {
            PushBackDucks();
        }
    }
}
