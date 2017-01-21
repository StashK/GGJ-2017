using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadDropper : MonoBehaviour {


	public float intervalMin;
	public float intervalMax;

	public float breadSpeedMin;
	public float breadSpeedMax;

	public GameObject breadObject;

	private float lastDropTime;
	private float dropTimer;

	// Use this for initialization
	void Start () {
		lastDropTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if(dropTimer > 0f)
			dropTimer -= Time.deltaTime;
		else
		{
			GameObject spawnedBread = Instantiate(breadObject, transform.position, Quaternion.Euler( Random.insideUnitSphere));
			dropTimer = Random.Range(intervalMin, intervalMax);

			Bread bread = spawnedBread.GetComponent<Bread>();
			if (bread)
				bread.moveSpeed = Random.Range(breadSpeedMin, breadSpeedMax);
		}
	}
}
