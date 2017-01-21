using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bread : MonoBehaviour {

	Vector3 direction = Vector3.forward;
	public float moveSpeed = 1f;

	// Use this for initialization
	void Start () {
		direction = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += direction * moveSpeed * Time.deltaTime;
	}
}
