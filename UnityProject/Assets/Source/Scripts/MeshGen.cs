using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGen : MonoBehaviour {

	// Use this for initialization
	void Start () {
        CreateMesh(1, 1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void CreateMesh (int sizeX, int sizeY, float size = 1f)
    {
        int totalGrids = sizeX * sizeY;

        Mesh m = new Mesh();
        m.vertices = new Vector3[totalGrids * 3 * 2];
        m.triangles = new int[totalGrids * 3];

        for (int i = 0; i < totalGrids; i++)
        {
            // side one
            m.vertices[i] = new Vector3(size * (float)sizeX / 2f * i, 0, size * (float)sizeY / 2f * i);
            m.vertices[i + 1] = new Vector3(size * (float)sizeX / 2f * i, 0, size * (float)sizeY / 2f * i);
            m.vertices[i + 2] = new Vector3(size * (float)sizeX / 2f * i, 0, size * (float)sizeY / 2f * i);
        }
    }
    
}
