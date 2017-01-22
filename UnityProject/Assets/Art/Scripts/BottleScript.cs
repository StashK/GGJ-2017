using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleScript : MonoBehaviour
{
    private Vector3 startPos;
    public MeshRenderer render;
    private bool prev = false;

    // Use this for initialization
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (prev != GameManager.Get.vaporTrapMode)
        {
            transform.position = startPos;
            render.enabled = GameManager.Get.vaporTrapMode;
            prev = GameManager.Get.vaporTrapMode;
        }
        transform.position = transform.position + transform.forward * 0.33f;

        if (transform.position.z > 90)
            transform.position = startPos;
    }
}
