using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PastelGenerator
{
    public static float Lightness = 1.0f;

    public static Color Generate()
    {
        float r = Random.Range(0.0f, 1.0f);
        float g = Random.Range(0.0f, 1.0f);
        float b = Random.Range(0.0f, 1.0f);

        r = (r + Lightness) / 2.0f;
        g = (g + Lightness) / 2.0f;
        b = (b + Lightness) / 2.0f;

        return new Color(r, g, b);
    }
}
