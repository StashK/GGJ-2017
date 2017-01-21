using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGen : MonoBehaviour {

    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uv;
    public Quad[,] quads;

    public int sizeX = 5;
    public int sizeY = 5;
    public float size = 1f;

	// Use this for initialization
	void Start () {
        CreateMesh(sizeX, sizeY, size);
	}
	
	// Update is called once per frame
	void Update () {
        int vIndex = 0;
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeX; j++)
            {
                Quad q = quads[i,j];
                //q.Calculate();

                int triangleIndex = vIndex;
                // copy vertices
                for (int k = 0; k < q.vertices.Length; k++)
                {
                    vertices[vIndex] = q.vertices[k];
                    vIndex++;
                }
                
                // set triangles
                //vertices[triangleIndex] = triangleIndex;
            }
        }

        GetComponent<MeshFilter>().mesh.vertices = vertices;


    }

    void CreateMesh (int sizeX, int sizeY, float size = 1f)
    {
        Debug.Log("Create Mesh");

        int totalGrids = sizeX * sizeY;

        Mesh m = new Mesh();
        vertices = new Vector3[totalGrids * 3 * 2];
        triangles = new int[totalGrids * 3 * 2];
        uv = new Vector2[totalGrids * 3 * 2];
        quads = new Quad[sizeX, sizeY];

        //Vector3 center = new Vector3();

        int vIndex = 0;
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeX; j++)
            {
                Quad q = new Quad((new Vector3(i, 0, j) * size), new Vector3(i + 1, 0, j + 1) * size);

                int triangleIndex = vIndex;
                // copy vertices
                for (int k = 0; k < q.vertices.Length; k++)
                {
                    vertices[vIndex] = q.vertices[k];
                    uv[vIndex] = q.uv[k];
                    triangles[vIndex] =  vIndex;
                    vIndex++;
                }

                quads[i, j] = q;
                // set triangles
                //vertices[triangleIndex] = triangleIndex;
            }
        }

        for (int i = 0; i < m.vertices.Length -1; i++)
        {
            Debug.DrawLine(vertices[i], vertices[i + 1], Color.green, 1f);
        }

        m.vertices = vertices;
        m.triangles = triangles;
        m.uv = uv;
        m.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = m;
    }
    
    public class Quad
    {
        public bool active = true;
        public bool active1 { get { return Random.value > 0f; } }
        public bool active2 { get { return Random.value > 0f; } }

        public Vector3[] vertices = new Vector3[6];
        public Vector2[] uv = new Vector2[6];

        private Vector3 start;
        private Vector3 end;

        public Quad (Vector3 start, Vector3 end)
        {
            this.start = start;
            this.end = end;

            Calculate(start, end);

            for (int i = 0; i < 6; i++)
            {
                uv[i] = new Vector2(vertices[i].x, vertices[i].z);
            }
        }

        public void Calculate()
        {
            Calculate(start, end);
        }

        private void Calculate (Vector3 start, Vector3 end)
        {
            if (active1)
            {
                vertices[2] = new Vector3(start.x, 0f, start.z);
                vertices[1] = new Vector3(end.x, 0f, start.z);
                vertices[0] = new Vector3(start.x, 0f, end.z);
            }
            else
            {
                vertices[2] = Vector3.zero;
                vertices[1] = Vector3.zero;
                vertices[0] = Vector3.zero;
            }

            if (active2)
            {
                vertices[5] = new Vector3(start.x, 0f, end.z);
                vertices[4] = new Vector3(end.x, 0f, start.z);
                vertices[3] = new Vector3(end.x, 0f, end.z);
            }
            else
            {
                vertices[5] = Vector3.zero;
                vertices[4] = Vector3.zero;
                vertices[3] = Vector3.zero;
            }
        }
    }
}
