using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiManController : MonoBehaviour
{

    public Vector2 inputVector;
    public float inputLength;

    public Vector3 targetLineForward;
    public float targetLineDistance;

    [Range(0, 1)]
    public float targetLineLerp = 0.5f;
    [Range(0, 0.2f)]
    public float pushbackPlayerDistance = 0.01f;

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
    }

    void UpdateWaving()
    {
        targetLineForward = Vector3.Slerp(targetLineForward, new Vector3(inputVector.x, 0, inputVector.y), targetLineLerp);
        inputLength = Mathf.Min(1, targetLineForward.magnitude);

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
            mainModule.startSpeed = 30f * inputLength;

            WavePlane.Get.CreateWave(transform.position, targetLineForward.normalized);
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
            GameObject gameObject = collider.gameObject;
            Duck duck = gameObject.GetComponent<Duck>();

            if (duck)
            {
                duck.distance += 0.1f * inputLength;
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
