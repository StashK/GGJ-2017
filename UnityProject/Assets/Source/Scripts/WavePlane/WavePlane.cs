using CielaSpike;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public float HeightMutliplier = 1.0f;
    public float RadiusMutliplier = 5.0f;
    public float HeightPower = 2;
    public List<Wave> Waves = new List<Wave>();
    public float randomDirecton = 0.1f;
    public float randomPosition = 5.0f;
    public float randomSpeed = 10.0f;

    private float DeWaveTimer = 0.0f;

    public Vector3[] vertices;
    Vector3[] avgPoints;
    public static float[] HeightMap;

    // Use this for initialization
    void Start()
    {
        local = this;

        //this.StartCoroutineAsync(Calc());

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

        HeightMap = new float[vertices.Length];
    }

    public void CreateWave(Vector3 position, Vector3 direction)
    {
        if (DeWaveTimer < 0.15f)
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

        RadiusMutliplier = 3.0f;
        HeightMutliplier = 1.0f;

        if (AntiManController.isBoosting)
        {
            RadiusMutliplier = 5.0f;
            HeightMutliplier = 1.3f;
        }

        //if (vertices != null && vertices.Length > 0)
        //    GetComponent<MeshFilter>().mesh.vertices = vertices;
        Calc2();
    }

    public Vector3 CenterOfVectors(Vector3[] vectors)
    {
        Vector3 sum = Vector3.zero;
        if (vectors == null || vectors.Length == 0)
        {
            return sum;
        }

        foreach (Vector3 vec in vectors)
        {
            sum += vec;
        }
        return sum / vectors.Length;
    }

    void Calc2()
    {
        foreach (Wave wave in this.Waves)
        {
            wave.position += wave.direction * Time.deltaTime * wave.speed;
            if (wave.position.magnitude > 50.0f)
            {
                this.Waves.Remove(wave);
                break;
            }
        }

        Wave[] Waves = new Wave[this.Waves.Count];
        this.Waves.CopyTo(Waves);

        //Debug.Log(Waves.Length);

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = (Vector3[])mesh.vertices.Clone();

        float curY = transform.position.y;

        //yield return Ninja.JumpBack;
        int i = 0;

        while (i < vertices.Length / 3)
        {
            Vector3 worldPt = CenterOfVectors(new Vector3[] { vertices[i * 3 + 0], vertices[i * 3 + 1], vertices[i * 3 + 2] });
            worldPt.y = 0.0f;
            float waveHeight = 0.0f;

            foreach (Wave wave in Waves)
            {
                Vector3 playerPos = wave.position;
                playerPos.y = 0.0f;

                float distance = Vector3.Distance(playerPos, worldPt);
                if (distance > RadiusMutliplier)
                    continue;

                float dot = (Vector3.Dot(playerPos.normalized, worldPt.normalized) + 1.0f) * 0.5f;

                waveHeight += Mathf.Clamp(RadiusMutliplier - distance, 0.0f, 2.0f) * HeightMutliplier * dot;
            }

            HeightMap[i * 3 + 0] = waveHeight;
            HeightMap[i * 3 + 1] = waveHeight;
            HeightMap[i * 3 + 2] = waveHeight;

            i++;
        }
        //yield return Ninja.JumpToUnity;
    }

    /*IEnumerator Calc()
    {
        yield return Ninja.JumpToUnity;

        foreach (Wave wave in this.Waves)
        {
            wave.position += wave.direction * Time.deltaTime * wave.speed;
            if (wave.position.magnitude > 25.0f)
            {
                this.Waves.Remove(wave);
                break;
            }
        }

        Wave[] Waves = new Wave[this.Waves.Count];
        this.Waves.CopyTo(Waves);


        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

        float curY = transform.position.y;

        //yield return Ninja.JumpBack;
        int i = 0;

        List<float> tempMap = new List<float>();

        while (i < vertices.Length)
        {
            Vector3 worldPt = vertices[i];
            worldPt.y = 0.0f;
            float waveHeight = 0.0f;

            foreach (Wave wave in Waves)
            {

                Vector3 playerPos = wave.position;
                playerPos.y = 0.0f;

                float distance = Vector3.SqrMagnitude(playerPos - worldPt);
                if (distance > 25.0f)
                    break;

                waveHeight += 1.0f / distance;
                waveHeight = Mathf.Clamp(waveHeight, 0.0f, 1.0f) * 5.0f;
            }

            tempMap.Add(waveHeight);
            i++;
        }


        //yield return Ninja.JumpToUnity;

        HeightMap = tempMap.ToArray();

        //this.StartCoroutineAsync(Calc());
    }*/

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (Wave wave in Waves)
        {
            Gizmos.DrawSphere(wave.position, 1.0f);
        }
    }
}
