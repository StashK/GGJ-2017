using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavePlane : MonoBehaviour
{
    public Transform player;
    public float HeightMutliplier = 0.01f;
    public float HeightPower = 2;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        int i = 0;

        while (i < vertices.Length)
        {
            Vector3 worldPt = transform.TransformPoint(vertices[i]);
            worldPt.y = 0.0f;

            Vector3 playerPos = player.position;
            playerPos.y = 0.0f;

            float distance = Vector3.Distance(playerPos, worldPt);
            float waveHeight = (Mathf.Pow(distance, HeightPower)) * HeightMutliplier;
            waveHeight = Mathf.Clamp(waveHeight, 0.0f, 1.0f);
            waveHeight = 1.0f - waveHeight;

            vertices[i].y = waveHeight;
            i++;
        }
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        //mesh.RecalculateNormals();
    }
}
