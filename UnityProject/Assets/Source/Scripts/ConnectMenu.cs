using NDream.AirConsole;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ConnectMenu : MonoBehaviour {

	public List<GameObject> playerConnectLines;
	public GameObject duckOff;

	// Use this for initialization
	void Start () {
		AirConsole.instance.onConnect += ConnectPlayer;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void ConnectPlayer(int id)
	{
		int activePlayers = AirConsoleManager.Instance.ActivePlayers().Count;

		for (int i = 0; i < playerConnectLines.Count; i++)
		{
			playerConnectLines[i].SetActive(false);

			if(i < activePlayers)
				playerConnectLines[i].SetActive(true);
		}
	}

	public void PlayGame()
	{
		duckOff.SetActive(true);
		SceneManager.LoadScene("Gameplay");
	}
}
