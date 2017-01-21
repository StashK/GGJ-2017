using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	bool connectMenuOpen;
	public GameObject connectMenu;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadGameplay ()
    {
        SceneManager.LoadScene("Gameplay");
    }

	public void OpenConnectMenu()
	{
		if(!connectMenuOpen && connectMenu)
		{
			connectMenu.SetActive(true);
		}
	}
}
