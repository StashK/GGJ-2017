using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleScript : MonoBehaviour
{
    private Vector3 startPos;
    public MeshRenderer render;

    // Use this for initialization
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        render.enabled = GameManager.Get.vaporTrapMode;
        transform.position = transform.position + transform.forward * 0.33f;

        if (transform.position.z > 90)
            transform.position = startPos;
        // transform.eulerAngles = new Vector3(transform.position.x + 1.0f, transform.position.y, transform.position.z);
    }
}
