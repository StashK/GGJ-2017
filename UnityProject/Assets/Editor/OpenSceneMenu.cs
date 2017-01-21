using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class OpenSceneMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	[MenuItem("OpenScene/AirConsoleInit")]
	static void OpenAirConsoleInit()
	{
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Source/Scenes/AirConsoleInit.unity");
	}

	[MenuItem("OpenScene/GameplayScene")]
	static void OpenGameplayScene()
	{
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Source/Scenes/Gameplay.unity");
	}
}
