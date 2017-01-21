using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave
{
    public Vector3 position;
    public Vector3 direction;
}

public class WavePlane : MonoBehaviour
{
    public float HeightMutliplier = 0.01f;
    public float HeightPower = 2;
    public List<Vector3> Waves = new List<Vector3>();

    // Use this for initialization
    void Start()
    {

    }

    void CreateWave(Vector3 position, Vector3 direction)
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

            Vector3 directional = worldPt - playerPos;
            float dirScale = Vector3.Dot(Vector3.forward, directional) * 0.1f;


            vertices[i].y = transform.position.y + (waveHeight * 3.0f * dirScale);
            i++;
        }
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        //mesh.RecalculateNormals();
    }
}
