using NDream.AirConsole;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ConnectMenu : MonoBehaviour {

	public List<GameObject> playerConnectLines;
	public GameObject duckOff;

	// Use this for initialization
	void Start() {
	}

	// Update is called once per frame
	void Update() {

	}
	void OnEnable()
	{
		AirConsole.instance.onConnect += ConnectPlayer;
		AirConsole.instance.onDisconnect += DisconnectPlayer;
		ActivateLines();
	}
	void OnDisable()
	{
		AirConsole.instance.onConnect -= ConnectPlayer;
		AirConsole.instance.onDisconnect -= DisconnectPlayer;
	}
	void ConnectPlayer(int id)
	{
		ActivateLines();
	}
	void DisconnectPlayer(int id)
	{
		ActivateLines();
	}

	void ActivateLines()
	{
		int activePlayers = AirConsoleManager.Instance.ActivePlayers().Count;

		for (int i = 0; i < playerConnectLines.Count; i++)
		{
			if (i < activePlayers)
			{
				playerConnectLines[i].SetActive(true);
				continue;
			}

			playerConnectLines[i].SetActive(false);
		}
	}
	public void PlayGame()
	{
		duckOff.SetActive(true);
		SceneManager.LoadScene("Gameplay");
	}
}
