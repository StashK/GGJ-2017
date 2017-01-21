using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	bool connectMenuOpen;
	public GameObject connectMenu;
	public Button DuckOffIcon;

	float lastSelectTime = 0f;
	bool iconSelected = false;

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
			connectMenuOpen = true;
		}
	}
	public void CloseConnectMenu()
	{
		if (connectMenuOpen && connectMenu)
		{
			connectMenu.SetActive(false);
			connectMenuOpen = false;
		}
	}

	public void SelectIcon()
	{
		if(iconSelected)
		{
			if (Time.time - lastSelectTime < 2f)
			{
				OpenConnectMenu();
			}
			iconSelected = false;
			EventSystem.current.SetSelectedGameObject(null);

		}
		else
		{
			lastSelectTime = Time.time;
			iconSelected = true;
			EventSystem.current.SetSelectedGameObject(DuckOffIcon.gameObject);
		}
	}
}
