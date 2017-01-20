using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColTester : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0, 15) == 0)
        {
            GetComponent<Renderer>().material.color = PastelGenerator.Generate();
        }
    }
}
