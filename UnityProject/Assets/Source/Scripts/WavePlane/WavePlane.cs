using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave
{
    public Vector3 position;
    public Vector3 direction;
    public float speed = 50.0f;
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
    public float randomDirecton = 0.1f;
    public float randomPosition = 5.0f;
    public float randomSpeed = 10.0f;

    private float DeWaveTimer = 0.0f;

    // Use this for initialization
    void Start()
    {
        local = this;
    }

    public void CreateWave(Vector3 position, Vector3 direction)
    {
        if (DeWaveTimer < 0.2f)
            return;

        DeWaveTimer = 0.0f;
        Wave newWave = new Wave
        {
            position = position,
            direction = direction
        };
        newWave.direction.x += Random.Range(-randomDirecton, randomDirecton);
        newWave.direction.z += Random.Range(-randomDirecton, randomDirecton);
        newWave.position.x += Random.Range(-randomPosition, randomPosition);
        newWave.position.z += Random.Range(-randomPosition, randomPosition);
        newWave.speed += Random.Range(-randomSpeed, randomSpeed);
        Waves.Add(newWave);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        DeWaveTimer += Time.deltaTime;

        foreach (Wave wave in Waves)
        {
            wave.position += wave.direction * Time.deltaTime * wave.speed;
            if (wave.position.magnitude > 25.0f)
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

                waveHeight += 1.0f / distance;
                waveHeight = Mathf.Clamp(waveHeight, 0.0f, 1.0f) * 1.5f;

            }

            vertices[i].y += transform.position.y + waveHeight;
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
