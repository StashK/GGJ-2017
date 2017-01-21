using UnityEngine;
using System.Collections;

public class move : MonoBehaviour
{


	public float x = 1.0f;
	public float y = 0.0f;
	public float z = 0.0f;

	Vector3 tempPos;
	//var ymin=0.1f;
	//var ymax=1.5f;


	void Start()
	{

	}

	void Update()
	{
		

		//transform.Translate(dolphin.transform.position, new Vector3(x, y, z), 100 * Time.deltaTime*speed);

		tempPos = transform.position;

		tempPos.x = tempPos.x + x;
		tempPos.y = tempPos.y + y;
		tempPos.z = tempPos.z + z;

		transform.position = tempPos;

	}

}