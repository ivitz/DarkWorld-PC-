using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using PixelCrushers.DialogueSystem;


public class MonsterLoreLogic : MonoBehaviour 
{
	private ArrayList loreListFromSave;					//the list of ID's from save file
	private int i;
	private GameObject[] allMonsterList;				//list all gameobjects of monsters
	private GameObject[] NewMonsterList;				//get only gameobjects with the ID's from save file
	public Transform placeOfTransform;
	private GameObject instantiatedMonster;
	private Camera cam;
	private Vector3 dir;
	public Text loreText;								//descriptions of monsters
	public Text unlockedText;							//the text of unlocked monsters
	private GameObject currentMonsterToSpawn;			//find the closest monster to spawn

	private GameObject instantiatedMonsterToRotate;
	private Collider colliderToLookAt;
	// Use this for initialization
	void Start () 
    {
		
		cam = Camera.main;
		allMonsterList = Resources.LoadAll (("My/Prefabs/Enemies/"), typeof(GameObject)).Cast<GameObject>().ToArray();
		/*
		foreach (GameObject monster in allMonsterList) {
			if (monster.GetComponentInChildren<EnemyProjectile> () != null) {
				monster.GetComponentInChildren<EnemyProjectile> ().enabled = true;
			}
		}
		*/

		if (File.Exists (Application.dataPath + "/SaveGameAtSavePoint.dat")) {
			LoadSavePoint ();
		} else if (File.Exists (Application.dataPath + "/SaveGame.dat")) {
			LoadChapter();
		}

		if (NewMonsterList != null) {
			unlockedText.text = string.Format ("Unlocked {0}/{1}", loreListFromSave.Count, allMonsterList.Length - 35);
		
			/*
		foreach (var id in loreListFromSave) {
			Debug.Log (id + " ID of monster");
		}
		*/
			for (int j = 0; j < loreListFromSave.Count; j++) {
				for (int k = 0; k < allMonsterList.Length; k++) {
					if (allMonsterList [k].GetComponent<EnemyHP> () != null && allMonsterList [k].GetComponent<EnemyHP> ().monsterID == (int)loreListFromSave [j]) {
						
						NewMonsterList [j] = allMonsterList [k];
						//Debug.Log ("Monster found. Name:::::: " + allMonsterList [k].name);
						break;
					}
				}
			}
		} else unlockedText.text = string.Format ("No monsters to show.");


		/*
		foreach (GameObject id in NewMonsterList) {
			Debug.Log (id.name + " monster name");
		}
		*/
		/*
		for (int j; j < loreList.Count; j++) {
			if (allMonsters[j].GetComponent<EnemyHP>().monsterID == 
		}
		*/
	}



	// Update is called once per frame
	void Update () {
		if (Input.GetAxis ("Mouse ScrollWheel") < 0) {   //back
			if (cam.fieldOfView < 75) cam.fieldOfView += 2f;
		}
		if (Input.GetAxis ("Mouse ScrollWheel") > 0) {   //forward
			if (cam.fieldOfView > 10) cam.fieldOfView -= 2f;
		}

		if (instantiatedMonsterToRotate != null) {
			instantiatedMonsterToRotate.transform.localRotation *= Quaternion.Euler (0, 0.5f, 0);
		}
	}

	public void SpawnNextMonster () {
					//check if array was filled with monsters
		if (NewMonsterList != null) {
						
			if (NewMonsterList.Length > 0) {
						
				//destroy previously instantiated monster
				if (instantiatedMonster != null) {
					Destroy (instantiatedMonster);
				}

				//check if gameobject has EnemyHP (which indicates that it is a monster
				if (NewMonsterList [i].GetComponent<EnemyHP> () == null) {
					i++;
				}

				//instantiate monster
				instantiatedMonster = Instantiate (NewMonsterList [i], placeOfTransform.position, placeOfTransform.rotation);
				
				try {
					loreText.text = instantiatedMonster.GetComponent<MonsterLoreAdd> ().aboutMonster;

					if (instantiatedMonster.GetComponentInChildren<EnemyProjectile> () != null) {
						instantiatedMonster.GetComponentInChildren<EnemyProjectile> ().enabled = false;
					}
			
				} catch (Exception e) {
					Debug.Log (e);
				}
				dir = (cam.transform.position - instantiatedMonster.transform.position);

				//if enemy has projectile = destroy it!

			

				//rotate the monster to the camera (if sprite monster - rotate -1, else +1
				if (instantiatedMonster.GetComponent<SpriteRenderer> ().sprite != null) {
					instantiatedMonster.transform.rotation = Quaternion.LookRotation (-dir);
					instantiatedMonster.transform.position += Vector3.up * 10;
				} else {
					instantiatedMonsterToRotate = instantiatedMonster;
					instantiatedMonster.transform.rotation = Quaternion.LookRotation (dir);
				}

				//find the collider for camera to look at. ignore triggers. this will help to focus camera on center of the monster
				foreach (Collider coll in instantiatedMonster.GetComponents<Collider>()) {
					if (!coll.isTrigger) {
						colliderToLookAt = coll;
						break;
					}									
				}
				//point camera at collider center
				cam.transform.LookAt (colliderToLookAt.bounds.center);

				MonoBehaviour[] allScripts = instantiatedMonster.GetComponentsInChildren<MonoBehaviour> ();
				foreach (MonoBehaviour script in allScripts) {
					script.enabled = false;
				}
				try {			
					Rigidbody[] rb = instantiatedMonster.GetComponentsInChildren<Rigidbody> ();
					foreach (Rigidbody rig in rb) {
						rig.isKinematic = true;			
					}
				} catch (Exception e) {
					Debug.Log (e);
				}

				if (i < NewMonsterList.Length - 1) {
					i++;
				} else
					i = 0;
			}	
		}
	}


	public void Quit() {
		SceneManager.LoadScene ("MainMenu");		
	}


                void LoadChapter () {
					BinaryFormatter bf = new BinaryFormatter ();
					FileStream file = File.Open (Application.dataPath + "/SaveGame.dat", FileMode.Open);
					SaveDataFromPlayerStats data = (SaveDataFromPlayerStats)bf.Deserialize (file);
					file.Close ();
					loreListFromSave = data.loreList;
					NewMonsterList = new GameObject[loreListFromSave.Count];

				}

				void LoadSavePoint () {
					BinaryFormatter bf = new BinaryFormatter ();
					FileStream file = File.Open (Application.dataPath + "/SaveGameAtSavePoint.dat", FileMode.Open);
					SaveDataFromPlayerStats data = (SaveDataFromPlayerStats)bf.Deserialize (file);
					file.Close ();
					loreListFromSave = data.loreList;
					NewMonsterList = new GameObject[loreListFromSave.Count];
				}
}
