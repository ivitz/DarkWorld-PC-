using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour {
	//private Button [] buttons;
	private GameObject savePlayerStats;
	private GameObject dialogueManager;
	public static string sceneToLoad;
	public static bool useSaveData;							//set this so that when the scene is loaded use the save file

	public static bool useSaveDataAtSavePoint;				//set this so that when the scene is loaded use the save file AT SAVE POINT

	public GameObject loadSavePointButton;
	public GameObject loadCahapterButton;

    private bool loaded;            //if the scene is loading - make it true so wont load again

	// Use this for initialization
	void Start () 
    {

		Time.timeScale = 1;
		savePlayerStats = GameObject.Find ("SavePlayerStats");
		dialogueManager = GameObject.Find ("Dialogue Manager");
		InventoryScript.ClearInventory ();
		if (savePlayerStats != null) {
			Destroy (savePlayerStats);
		}
		if (dialogueManager != null) {
			Destroy (dialogueManager);
		}
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;

		if (File.Exists (Application.dataPath + "/SaveGame.dat")) {
			loadCahapterButton.GetComponent<Button> ().interactable = true;
		} else 
			loadCahapterButton.GetComponent<Button> ().interactable = false;
		if (File.Exists (Application.dataPath + "/SaveGameAtSavePoint.dat")) {
			loadSavePointButton.GetComponent<Button> ().interactable = true;
		} else 
			loadSavePointButton.GetComponent<Button> ().interactable = false;
        StartCoroutine(ChangeEventSystem());
	}

	void Awake () {
		savePlayerStats = GameObject.Find ("SavePlayerStats");
		if (savePlayerStats != null) {
			Destroy (savePlayerStats);

		}
      
	}

    private IEnumerator ChangeEventSystem() {
        yield return new WaitForSeconds(0.5f);
        while (EventSystem.current == null)
        {     
            EventSystem.current = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        }

    }
	
//load saved game
	public void LoadGame () {
		if (File.Exists (Application.dataPath + "/SaveGame.dat")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.dataPath + "/SaveGame.dat", FileMode.Open);
			SaveDataFromPlayerStats data = (SaveDataFromPlayerStats)bf.Deserialize (file);
			file.Close ();


			sceneToLoad = data.currentLevel; //get the saved scene;
            LoadingScreenLogic.sceneName = sceneToLoad;
			useSaveData = true; //set this so that when the scene is loaded use the save file
			ChangeLevel ("LoadingScreen");
		}
	}

	public void LoadGameAtSavePoint () {
		if (File.Exists (Application.dataPath + "/SaveGameAtSavePoint.dat")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.dataPath + "/SaveGameAtSavePoint.dat", FileMode.Open);
			SaveDataFromPlayerStats data = (SaveDataFromPlayerStats)bf.Deserialize (file);
			file.Close ();

			sceneToLoad = data.currentLevel; //get the saved scene;
            LoadingScreenLogic.sceneName = sceneToLoad;
			useSaveDataAtSavePoint = true; //set this so that when the scene is loaded use the save file
            ChangeLevel ("LoadingScreen");
		}
	}

	public void ChangeLevel (string sceneName) {
       
        if (loaded == false)
        {
            if (sceneName == "Intro01(Nature)")
            {
                StartCoroutine(NewGame(sceneName)); 
                loaded = true;
            }
            else
            {
                SceneManager.LoadScene(sceneName);
                loaded = true;
            }
        }
	}

	public void QuitGame () {
		Debug.Log ("quit");
		Application.Quit ();
	}

    public static IEnumerator NewGame (string sceneName) {
       
        yield return new WaitForSeconds (2);
        Cursor.visible = true;
        SceneManager.LoadScene(sceneName);
    }


}

[Serializable]
class SaveData {
	public string currentLevel;
}

