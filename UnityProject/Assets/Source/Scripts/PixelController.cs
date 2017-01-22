using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PixelController : MonoBehaviour {

    public RawImage image;
    public static PixelController Instance;
    public float targetAlpha = 1f;

	// Use this for initialization
	void Awake () {
        Instance = this;
        StartCoroutine(Go());
	}

    IEnumerator Go ()
    {
        yield return new WaitForSeconds(2f);
        SetTargetAlpha(0f);
    }


    public static void SetTargetAlpha (float f)
    {
        Instance.targetAlpha = f;
        LeanTween.alpha(Instance.image.rectTransform, f, 1f);
    }

	// Update is called once per frame
	void Update () {
        //image.color = new Vector4(1f, 1f, 1f, Mathf.Lerp(image.color.a, targetAlpha, Time.deltaTime));
	}
}
