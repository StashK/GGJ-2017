using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DuckTitles
{
    public Color Colour;
	public Vector3 Position;
    public int Size;
    public string Text;
    public float Alive;
}

public class SubtitleRenderer : MonoBehaviour
{
    public static SubtitleRenderer Get
    {
        get
        {
            return instance;
        }
    }
    private static SubtitleRenderer instance;
	public GameObject quackObject;
	public Canvas quackCanvas;

	private static List<DuckTitles> queue;
    public List<AudioClip> Audio = new List<AudioClip>();

    public void AddSubtitle(DuckTitles title, Color color)
    {
        title.Alive = 1.0f / title.Size * 50.0f;
        queue.Add(title);
		JPL.Core.Sounds.PlaySound( SubtitleRenderer.Get.Audio[Random.Range(0, SubtitleRenderer.Get.Audio.Count -1)], JPL.SOUNDSETTING.SFX);
		Text quack = Instantiate(quackObject, title.Position, Quaternion.identity).GetComponent<Text>();
		quack.transform.SetParent(quackCanvas.transform);
		quack.transform.up = Vector3.up;
		quack.color = color;
	}

	// Use this for initialization
	void Start()
    {
        instance = this;

        queue = new List<DuckTitles>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (DuckTitles title in queue)
        {
            title.Alive -= Time.deltaTime;
            if (title.Alive <= 0.0f)
            {
                queue.Remove(title);
                break;
            }
        }
    }
}
