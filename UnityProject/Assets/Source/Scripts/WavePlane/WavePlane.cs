using CielaSpike;
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

    public Vector3[] vertices;

    public static float[] HeightMap;

    // Use this for initialization
    void Start()
    {
        local = this;

        //this.StartCoroutineAsync(Calc());
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

        //if (vertices != null && vertices.Length > 0)
        //    GetComponent<MeshFilter>().mesh.vertices = vertices;
        Calc2();
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

        List<float> tempMap = new List<float>();

        float prevHeight = 0.0f;

        while (i < vertices.Length)
        {
            Vector3 worldPt = vertices[i];
            worldPt.y = 0.0f;
            float waveHeight = 0.0f;

            foreach (Wave wave in Waves)
            {

                Vector3 playerPos = wave.position;
                playerPos.y = 0.0f;

                float distance = Vector3.Distance(playerPos, worldPt);
                if (distance > 4.0f)
                    continue;

                //waveHeight += 5.0f - distance;
                waveHeight += Mathf.Clamp(4.0f - distance, 0.0f, 2.0f) * 0.25f;
            }

            tempMap.Add(Mathf.Lerp(prevHeight, waveHeight, 0.25f));
            tempMap.Add(Mathf.Lerp(prevHeight, waveHeight, 0.5f));
            tempMap.Add(Mathf.Lerp(prevHeight, waveHeight, 0.75f));
            tempMap.Add(waveHeight);
            prevHeight = waveHeight;
            i += 4;
        }


        //yield return Ninja.JumpToUnity;

        HeightMap = tempMap.ToArray();
    }

    IEnumerator Calc()
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
        Vector3[] vertices = (Vector3[])mesh.vertices.Clone();

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

                float distance = Vector3.Distance(playerPos, worldPt);
                if (distance > 5.0f)
                    break;

                waveHeight += 1.0f / distance;
                waveHeight = Mathf.Clamp(waveHeight, 0.0f, 1.0f) * 1.5f;
            }

            tempMap.Add(waveHeight);
            i++;
        }


        //yield return Ninja.JumpToUnity;

        HeightMap = tempMap.ToArray();

        //this.StartCoroutineAsync(Calc());
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
