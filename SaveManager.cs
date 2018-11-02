using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using System.Linq;
//using PixelCrushers.DialogueSystem;

[Serializable] public class SaveManager: MonoBehaviour  {

	public string sceneName;
	public List<float> iDList;

	public GameObject[] monsterList;
	public GameObject[] ammoList;
	public GameObject[] armorList;
	public GameObject[] upgradeList;
	public SecretPlace[] secretsList;
	public GameObject[] weaponList;
	public GameObject[] soulsList;
    public StartConvo[] conversationsList;


    public bool noSaveCude;

	public LightCapsuleFade[] lightCapsuleList;

	private SaveCube saveCube;
	private SavePlayerStats playerStats;

	[Header("Special For Different Worlds")]

	public bool w01OldKnightTalk;

	void Start () 
	{
		
		playerStats = GameObject.Find ("SavePlayerStats").GetComponent<SavePlayerStats>();
	}

	void OnLevelWasLoaded () {
		playerStats = GameObject.Find ("SavePlayerStats").GetComponent<SavePlayerStats>();
		//clear the list of IDs on every change level (dont clear if loadong from menu)
		if (!MainMenu.useSaveDataAtSavePoint) {
			iDList.Clear ();
		}

	}

	/// <summary>
	/// Call this method when loading the game. It will loop through every object in scene and check it's id and compare it to the List of IDs that was saved.
	/// Will disable every object with the same ID as in List<int>
	/// </summary>
	public void LoadGameAndDisableIDs () {
		playerStats = GameObject.Find ("SavePlayerStats").GetComponent<SavePlayerStats>();
		//Debug.Log ("Start Destroying");
        try 
        {
		saveCube = GameObject.Find ("SaveCube").GetComponent<SaveCube>();
        } catch 
        {            
            noSaveCude = true;
        }

		//check monsters	
		monsterList = GameObject.FindGameObjectsWithTag ("Enemy");
		foreach (GameObject obj in monsterList) {
			foreach (float f in iDList) {
				if (obj.GetComponent<EnemyHP> ().iDForSaveGame == f) {
					//Debug.Log (obj.name);
                    if (!noSaveCude)
                    {
                        if (!obj.GetComponent<EnemyHP>().dontRespawn && !obj.GetComponent<EnemyHP>().isBoss)
                        {
                            saveCube.AddDeadEnemy(obj.name, obj.transform);
                        }
                    }
					Destroy (obj);
				}
			}
		}

		//check ammo
		ammoList = GameObject.FindGameObjectsWithTag ("Ammo");
		foreach (GameObject obj in ammoList) {
			//Debug.Log (obj.GetComponent<Ammo> ().iD);
			foreach (float f in iDList) {
				//Debug.Log (f);
				if (obj.GetComponent<Ammo> ().iD == f) {
					//Debug.Log (obj.name + "    " + f);
					Destroy (obj);
				}
			}
		}

		//check armor
		armorList =  GameObject.FindGameObjectsWithTag ("Armor");
		foreach (GameObject obj in armorList) {
			foreach (float f in iDList) {
				if (obj.GetComponent<Armor> ().iD == f) {					
					Destroy (obj);
				}
			}
		}

		//check weapon Upgrade
		upgradeList =  GameObject.FindGameObjectsWithTag ("WeaponUpgrade");
		foreach (GameObject obj in upgradeList) {
			foreach (float f in iDList) {
				//Debug.Log (obj.name + "   " + f);
				if (obj.GetComponent<WeaponUpgradeItem> ().iD == f) {					
					Destroy (obj);
				}
			}
		}

		//check weapons 
		weaponList =  GameObject.FindGameObjectsWithTag ("PickUpObjects");
		foreach (GameObject obj in weaponList) {
			//Debug.Log (obj.name);
			foreach (float f in iDList) {
				if (obj.GetComponent<WeaponScript> ().iD == f) {					
					Destroy (obj);
				}
			}
		}

		//secrets
		secretsList = GameObject.FindObjectsOfType<SecretPlace> ();
		foreach (SecretPlace obj in secretsList) {
			playerStats.secretsTotal++;
			foreach (float f in iDList) {
				if (obj.iD == f) {
					//Debug.Log (obj.name + " LOADED AND DESTROYED");
					playerStats.secretsFound++;
					//playerStats.secretsTotal++;
					Destroy (obj.gameObject);
				}
			}
		}
		//monster souls
		soulsList =  GameObject.FindGameObjectsWithTag ("MonsterSoul");
		foreach (GameObject obj in soulsList) {
			foreach (float f in iDList) {
				if (obj.GetComponent<MonsterSoul> ().iD == f) {
					
					Destroy (obj);
				}
			}
		}

		//light capsules
		lightCapsuleList = GameObject.FindObjectsOfType<LightCapsuleFade> ();
		if (lightCapsuleList.Length >= 1)
		{
			foreach (LightCapsuleFade obj in lightCapsuleList)
			{
				foreach (float f in iDList)
				{
					if (obj.iD == f)
					{

						Destroy(obj.transform.parent.gameObject);
					}
				}
			}
		}

        //conversations
        conversationsList = GameObject.FindObjectsOfType<StartConvo>();

        if (conversationsList.Length >= 1)
        {
            foreach (StartConvo convo in conversationsList)
            {
                foreach (float f in iDList)
                {
                  
                    if (convo.iD == f)
                    {
                       
                        Destroy(convo);
                    }
                }
            }
        }
	}

	/// <summary>
	/// Call this method and pass the Id of the object here (eg picking up objects, killing monsters etc). It will save it to the array and will disable this object on load game
	/// </summary>
	public void AddID (float iD) {
		iDList.Add (iD);
	}
}

	/*
	//save the game
	public void SaveGame() {

		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.dataPath + "/" + sceneName + "SaveGame.dat");

		SceneData data = new SceneData ();	

		//here comes the data to save

		bf.Serialize (file, data);
		file.Close ();

		//Debug.Log ("Game Saved " + data.playerHP);
	}
}

[Serializable] class SceneData {
	
}
*/