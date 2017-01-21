using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave
{
    public Vector3 position;
    public Vector3 direction;
    public float speed = 100.0f;
}

public class WavePlane : MonoBehaviour
{
    public static WavePlane Get
    {
        get { return local; }
    }
    private static WavePlane local;

    public float HeightMutliplier = 0.01f;
    public float HeightPower = 2;
    public List<Wave> Waves = new List<Wave>();

    // Use this for initialization
    void Start()
    {
        local = this;
        CreateWave(Vector3.zero, Vector3.forward);
        CreateWave(Vector3.zero, Vector3.forward);
        CreateWave(Vector3.zero, Vector3.forward);
        CreateWave(Vector3.zero, Vector3.forward);
        CreateWave(Vector3.zero, Vector3.forward);
        CreateWave(Vector3.zero, Vector3.forward);
        CreateWave(Vector3.zero, Vector3.forward);
        CreateWave(Vector3.zero, Vector3.forward);
        CreateWave(Vector3.zero, Vector3.forward);
        CreateWave(Vector3.zero, Vector3.forward);
    }

    void CreateWave(Vector3 position, Vector3 direction)
    {
        Wave newWave = new Wave
        {
            position = position,
            direction = direction
        };
        newWave.direction.x += Random.Range(-0.2f, 0.2f);
        newWave.direction.z += Random.Range(-0.2f, 0.2f);
        newWave.position.x += Random.Range(-5.0f, 5.0f);
        newWave.position.z += Random.Range(-5.0f, 5.0f);
        newWave.speed += Random.Range(-10.0f, 10.0f);
        Waves.Add(newWave);
    }

    // Update is called once per frame
    void Update()
    {

        foreach (Wave wave in Waves)
        {
            wave.position += wave.direction * Time.deltaTime * wave.speed;
            if (wave.position.magnitude > 150.0f)
            {
                Waves.Remove(wave);
                break;
            }
        }
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        int i = 0;

        while (i < vertices.Length)
        {
            float waveHeight = 0.0f;
            foreach (Wave wave in Waves)
            {
                Vector3 worldPt = transform.TransformPoint(vertices[i]);
                worldPt.y = 0.0f;

                Vector3 playerPos = wave.position;
                playerPos.y = 0.0f;

                float distance = Vector3.Distance(playerPos, worldPt);
                float localWaveHeight = (Mathf.Pow(distance, HeightPower)) * HeightMutliplier;
                localWaveHeight = Mathf.Clamp(localWaveHeight, 0.0f, 1.0f);
                localWaveHeight = 1.0f - localWaveHeight;

                Vector3 directional = worldPt - playerPos;
                float dirScale = Vector3.Dot(wave.direction, directional) * 0.1f;


                waveHeight += transform.position.y + (localWaveHeight * 3.0f * dirScale);
            }

            vertices[i].y = transform.position.y + waveHeight;
            i++;
        }
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        //mesh.RecalculateNormals();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (Wave wave in Waves)
        {
            Gizmos.DrawSphere(wave.position, 1.0f);
        }
    }
}
