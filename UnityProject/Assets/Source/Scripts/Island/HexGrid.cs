using UnityEngine;
using System.Collections;

public class HexGrid : MonoBehaviour
{
    public Transform spawnThis;

    private int x = 50;
    private int y = 50;

    public float radius = 0.5f;
    private bool useAsInnerCircleRadius = false;

    private float offsetX, offsetY;

    private Transform[,] gridTransforms;

    private float activeRadius = 25f;

    void Awake ()
    {
        LeanTween.init(2048);
    }

    void Start()
    {
        InitGrid();

        SpawnGrid();

        RemoveOuterRing(20f);
        //StartCoroutine(Test());
    }

    IEnumerator Test ()
    {
        yield return new WaitForSeconds(5f);
        SetFalloff(15f);
    }

    private void InitGrid ()
    {
        x = (int)((int)DuckGameGlobalConfig.startDistance * 2 / radius) + 1;
        y = (int)((int)DuckGameGlobalConfig.startDistance * 2 / radius) + 1;

        activeRadius = DuckGameGlobalConfig.startDistance;

        float unitLength = (useAsInnerCircleRadius) ? (radius / (Mathf.Sqrt(3) / 2)) : radius;

        offsetX = unitLength * Mathf.Sqrt(3);
        offsetY = unitLength * 1.5f;

        gridTransforms = new Transform[x,y];
    }

    private void SpawnGrid ()
    {
        Transform parent = new GameObject("level").transform;

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                Vector2 hexpos = HexOffset(i, j);
                Vector3 pos = new Vector3(hexpos.x, 0, hexpos.y);

                if(Vector3.Distance(pos, Vector3.zero) < activeRadius)
                {
                    gridTransforms[i, j] = Instantiate(spawnThis, pos, Quaternion.identity);
                    gridTransforms[i, j].parent = parent;
                }
            }
        }
    }

    public void SetFalloff (float radius)
    {
        RemoveOuterRing(radius);
    }

    private void RemoveOuterRing (float newRadius)
    {
        Debug.Log("setting grid radius from " + activeRadius + " to " + newRadius);
        
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                Vector2 hexpos = HexOffset(i, j);
                Vector3 pos = new Vector3(hexpos.x, 0, hexpos.y);

                if (Vector3.Distance(pos, Vector3.zero) > newRadius)
                {
                    RemoveHex(i,j, Random.Range(0.1f, 0.5f) + (int)(newRadius - Vector3.Distance(pos, Vector3.zero)) / 2f);
                }
            }
        }

        activeRadius = newRadius;
    }

    private void RemoveHex (int x, int y, float delay)
    {
        if (gridTransforms[x,y] != null)
        {
            LeanTween.scale(gridTransforms[x, y].gameObject, Vector3.zero, 1.5f).setDelay(2f + delay + Random.Range(0f, 0.1f)).setEase(LeanTweenType.easeInCubic);
        }
    }

    Vector3 HexOffset(int x, int y)
    {
        Vector3 position = Vector3.zero;

        if (y % 2 == 0)
        {
            position.x = x * offsetX;
            position.y = y * offsetY;
        }
        else {
            position.x = (x + 0.5f) * offsetX;
            position.y = y * offsetY;
        }

        position.x -= offsetX * this.x / 2;
        position.y -= offsetY * this.y / 2;

        return position;
    }
}