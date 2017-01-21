using CielaSpike;
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

    public float distance = 25f;
    public static float curDistance = 25f;

	// Use this for initialization
	void Start () {
        CreateMesh(sizeX, sizeY, size);

        this.StartCoroutineAsync(Test());
    }
	
    public void SetCurDistance (float newDistance)
    {
        curDistance = newDistance;
    }

    IEnumerator Test ()
    {
        yield return new WaitForEndOfFrame();
        //yield return Ninja.JumpToUnity;
        //vertices = GetComponent<MeshFilter>().mesh.vertices;
        //yield return Ninja.JumpBack;
        curDistance = distance;

        int vIndex = 0;
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeX; j++)
            {
                Quad q = quads[i, j];
                q.Calculate();

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

        yield return Ninja.JumpToUnity;

        //GetComponent<MeshFilter>().mesh.Clear();
        GetComponent<MeshFilter>().sharedMesh.vertices = vertices;

        this.StartCoroutineAsync(Test());
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

        Vector3 center = new Vector3(sizeX / 2f * size, 0f, sizeY / 2f * size);

        int vIndex = 0;
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeX; j++)
            {
                Quad q = new Quad((new Vector3(i, 0, j) * size) - center, (new Vector3(i + 1, 0, j + 1) * size) - center);

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

        m.vertices = vertices;
        m.triangles = triangles;
        m.uv = uv;
        m.RecalculateNormals();

        GetComponent<MeshFilter>().sharedMesh = m;
    }
    
    public class Quad
    {
        public bool active = true;
        public bool active1 { get { return center1Magnitude < MeshGen.curDistance; } }
        public bool active2 { get { return center2Magnitude < MeshGen.curDistance; } }
        public Vector3 center1;
        public float center1Magnitude;
        public Vector3 center2;
        public float center2Magnitude;
        public float random;

        private Vector3[] originalVertices = new Vector3[6];
        public Vector3[] vertices = new Vector3[6];
        public Vector2[] uv = new Vector2[6];

        private Vector3 start;
        private Vector3 end;

        public Quad (Vector3 start, Vector3 end)
        {
            this.start = start;
            this.end = end;

            Calculate(start, end);

            originalVertices = (Vector3[])vertices.Clone();
            center1 = (originalVertices[0] + originalVertices[1] + originalVertices[2]) / 3f;
            center2 = (originalVertices[3] + originalVertices[4] + originalVertices[5]) / 3f;
            center1Magnitude = center1.magnitude;
            center2Magnitude = center2.magnitude;

            random = Random.Range(0.8f, 0.95f);

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
                vertices[2] = (center1 + ((vertices[2] - center1)) * random);
                vertices[1] = (center1 + ((vertices[1] - center1)) * random);
                vertices[0] = (center1 + ((vertices[0] - center1)) * random);
            }

            if (active2)
            {
                vertices[5] = new Vector3(start.x, 0f, end.z);
                vertices[4] = new Vector3(end.x, 0f, start.z);
                vertices[3] = new Vector3(end.x, 0f, end.z);
            }
            else
            {
                vertices[5] = (center2 + ((vertices[5] - center2)) * random);
                vertices[4] = (center2 + ((vertices[4] - center2)) * random);
                vertices[3] = (center2 + ((vertices[3] - center2)) * random);
            }
        }
    }
}
