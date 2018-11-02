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
using UnityEngine.EventSystems;


using UnityStandardAssets.Characters.FirstPerson;

public class SavePlayerStats : MonoBehaviour {
	[HideInInspector]
	public ArrayList loreList = new ArrayList();			//use this to add monsters ID here to use later in monster lore
	[HideInInspector]
	public ArrayList weaponList = new ArrayList (); //list of weapons player has (just the main info like Id, name, upgrade amount
	[HideInInspector]
	public ArrayList achievementsID = new ArrayList(); //list of acievement IDs
	[HideInInspector]
	public GameObject[] achievementArray; //list of acievement objects
	[HideInInspector]
	public ArrayList weaponsID = new ArrayList();
	[HideInInspector]
	public string[] weaponsUpgrades = new string[0];
	[HideInInspector]
	public int weaponsAmount; //will increase with each weapon added to the inventory
	private GameObject[] weaponsPrefabsArray;				//load all weapon prefabs
	public static SavePlayerStats instance = null;
	[HideInInspector]
	public int notesCollected;

	public float corePlayerHP;
	public float corePlayerDamage;
	public float corePlayerDamageImpule;	//how much force applied on hit
	public float corePlayerHitRate;	//the rate of hitting
	//public float corePlayerHealingRate;
	//melee
	[Header("Melee Weapon")]
	public string weaponName;
	public float weaponDamage;
	public float weaponAttackSpeed = 2f;
	public float weaponImpulse;
	//ranged
	[Header("Ranged Weapon")]
	public float weaponUpgradeBonus;				//from pick up upgrades
	public float WeaponRangedDamage;				//the damage with EXP + weapon
	public string rangedWeaponName;
	public string weaponDescription;
	public float rangedDamage;
	public float rangedAttackSpeed = 2f;
	public float rangedImpulse;
	public float rangedWeaponRange;
	public float currAmmo;
	public float recoil;
	public float totalWeaponAmmo;
	public float totalAmmo;						//total ammo player has
	public float maxAmmo;						//maximum ammo player is allowed to carry
	public float reloadTime;
	public float ammoConsume;
    public float weaponShakeCameraAmount;

	public float weaponUpgradeAmount; //the amount of bonus upgrade will give
	public float weaponUpgradeCost; //the cost to upgrade weapon
	public float weaponUpgradeEXPCap; //the amount of exp needed to upgrade weapon!
	public AudioClip rangedFireSound;
	public WeaponScript changeWeapon;
	//public GameObject fpsWeaponModel;		//this is the gameobject of the Gun used to store weapon
	public FPSWeapon fpsWeapon;				//this is the script used to store weapon behaivor like rotation on camera
	private GameObject fpsWeaponScale;			//gameobject of the gun
	public Image weaponImage;
	public Image rangedWeaponImage;

	[Header("Stats")]
	public float playerHP;
	private float playerHPOnGUI;			//the amount of hp player had when he started reading note or something else
	public float playerArmor;
	public float playerDamage;
	public float playerDamageImpule;	//how much force applied on hit
	public float playerHitRate;	//the rate of hitting
	public int secretsTotal;
	public int secretsFound;

	[Header("Leveling Stats")]
	public float playerEXP;
	[HideInInspector]
	public float playerCurrentEXP;
	[HideInInspector]
	public float mostersKilled;
	public float playerGOLD; //the gold of the player
	public float monsterSoulsAmount;
	private float playerGOLDSpent; //stores the amount of gold that was spent in shop to calculate EXP - stentGOLD
	[HideInInspector]
	public Vector3 startingLevelPosition;


	[Header ("Bonus Stats")]
	public float ammoBonus = 1;		//added
	public float soulBonus = 1;		//added
	public float dropChance = 1;	//added
	public float armorBonus = 1;	//added
	public float healBonus = 1;		//added
	public float maxHP;				//checked
	public float level;				//not saved

	[Header("GUI stuff")]
    [Tooltip("set this to make it first selected on gui for joystick control")]
    public GameObject loadGameButton;       //set this to make it first selected on gui for joystick control
    public GameObject pickupSmoke;
	public GameObject levelUpPanel;
	private GameObject rewardText;		//the reward text to show when you pick up stuff
	public Shop currentShop;		//the shop script of current shop you are using
	private GameObject pauseMenu; //pause menu GUI
	public GameObject shopMenu; //shop pause menu
	private Text pauseInfoText; //GUI text to show monsters killed
	//private Text pauseEXP; //GUI text to current EXP
	//private Text notesCollectedGUI;
	public Text playerGOLDText;//GUI text to current GOLD
	public Text playerSoulsText;//GUI text to current GOLD
	public Text WeaponInfoText;
	public GameObject upgradeButton; //button used to upgrade current weapon in shop
	public GameObject buyAmmoButton; //button used to buy ammo
	public GameObject buyHpButton; //button used to buy hp
	public GameObject buyMoreAmmoButton; //button used to buy ammo
	public GameObject buyMoreHpButton; //button used to buy hp
	public GameObject Sell1Soul; 
	public GameObject Sell10Soul; 
	public GameObject Sell100Soul; 
	public GameObject notes; //add here the note GUI
    public EventSystem eventSystem;            //search for event system to later add First Selected to it (for joystick)

    public GameObject pauseMenuMainPanel;

    //public GameObject difficultyButton;
	public bool isPause; //check if the game was paused (F1)
	public bool readingNote; //check if the player picked up a note
	public bool isShop; //if player interrats with shop;
    private bool difficultyIsReduced;

	private GameObject player;

	//public GameObject[] monsterLoreArray;
	public string currentLevel; //get the scene name
	public ArrayList weapons = new ArrayList();
	public ArrayList rangedWeapons = new ArrayList();
	public GameObject[] weaponPrefabs; //array of weapon prefabs in My/Prefabs/Weapons


	private SaveManager saveManager;


    public static bool disableControlls;             //use this to disable controls for player;



	void Start () 

    {
        pauseMenuMainPanel.SetActive(true);
        //eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        disableControlls = false;
        if (InventoryScript.rangedWeapons.Count != 0) {
            InventoryScript.SelectBestWeapon ();
        }
		//SAVE MANAGER HERE
		try 
		{
			saveManager = GetComponent<SaveManager> ();
		} 
		catch 
		{
			Debug.Log ("No Save Manager! Shit");
		}
		//SAVE MANAGER HERE
		
		rewardText = Resources.Load ("My/Prefabs/RewardText") as GameObject;

		pauseInfoText = GameObject.Find ("PauseInfoText").GetComponent<Text>();

		pauseMenu = GameObject.Find 		  ("PauseMenu");
       


		pauseMenu.SetActive (false);
		//find player and get some components
		player = GameObject.FindGameObjectWithTag ("Player");
		if (player != null) {			

			fpsWeapon = GameObject.Find ("Gun").GetComponent<FPSWeapon> (); //get the gun (used to draw fps gun on screen)
			fpsWeaponScale = GameObject.Find ("Gun");
			startingLevelPosition = player.transform.position;				//get the starting position of the player. start of level
		}

		//load the game from start of level
		if (MainMenu.useSaveData) {

			LoadGame ();
			MainMenu.useSaveData = false;

		}

		//load the game from SAVE POINT
		if (MainMenu.useSaveDataAtSavePoint) {

			Debug.Log ("Load Game save point in Start()");
			LoadGameAtSavePoint ();
			MainMenu.useSaveDataAtSavePoint = false;

		}
		StatsUpdate ();	
        Debug.Log("Start on SavePlayer");
	}

	void Awake() {
		weaponsPrefabsArray = Resources.LoadAll (("My/Prefabs/Weapons"), typeof(GameObject)).Cast<GameObject> ().ToArray();
		currentLevel = SceneManager.GetActiveScene ().name;
		//monsterLoreArray = GameObject.FindGameObjectsWithTag ("MonsterLore"); //find all references in monster lore journal.
		achievementArray = GameObject.FindGameObjectsWithTag ("Achievement"); //find all references in achievement panel

		if (instance == null)
			instance = this;
		else if (instance != this)
		Destroy (gameObject);
		
		DontDestroyOnLoad (gameObject);
		playerHP = corePlayerHP;
		playerHitRate = corePlayerHitRate;
		playerDamage = corePlayerDamage;
		playerDamageImpule = corePlayerDamageImpule;
		playerCurrentEXP = 0;
		maxHP = 100;
		//load the game if from main menu load game was chosen

        Debug.Log("AWAKE on SavePlayer");

	}

	void OnLevelWasLoaded () {
        //eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        disableControlls = false;
        currentLevel = SceneManager.GetActiveScene ().name;
		if (!MainMenu.useSaveDataAtSavePoint && !MainMenu.useSaveData) {
			secretsFound = 0;
			secretsTotal = 0;
			//SAVE MANAGER HERE
			try {
				saveManager = GetComponent<SaveManager> ();

			} catch {
				Debug.Log ("No Save Manager! Shit");
			}
			//SAVE MANAGER HERE


			try {
				//one instance of savePlayerStats
                if (instance == this && currentLevel != "MainMenu" && currentLevel != "LoadingScreen") {			
					player = GameObject.FindGameObjectWithTag ("Player");				//find player
					//check if the player is dead on level transition. if it is - ressurect with 15hp		

					if (player != null) {				
						fpsWeapon = GameObject.Find ("Gun").GetComponent<FPSWeapon> ();
						fpsWeaponScale = GameObject.Find ("Gun");							//the object of the gun (weapons are attached to it). it draws the weapons in FPS
						startingLevelPosition = player.transform.position;
					}
			
					

					//when the scene is changed - save the game! dont save on training or main menu
                    if (currentLevel != "MainMenu" && currentLevel != "Training" && currentLevel != "LoadingScreen") {
						SaveGame ();
						Debug.Log ("Save the Game on level loaded!");
					}

			

					StatsUpdate ();	
					if (playerHP <= 0 || player.GetComponent<RayCastScript> ().isDead) {

						player.GetComponent<RayCastScript> ().isDead = false;
						playerHP = 70;
					}

					if (!MainMenu.useSaveDataAtSavePoint) {
						Debug.Log ("Clearing SaveManager list");
						saveManager.iDList.Clear ();
					}
                    if (InventoryScript.rangedWeapons.Count != 0) {
                        InventoryScript.SelectBestWeapon ();
                    }
	
				}
			} catch (Exception e) {
				Debug.Log (e);
			}
		}

        Debug.Log("ONLEVELLOADED on " + this.gameObject.name);
	}

	void Update () {		
		//pause menu
		//if (Input.GetKeyDown(KeyCode.Escape)) {
        if (Input.GetButtonDown("Escape")) {
			//Cursor.visible = !Cursor.visible;
            if (PlayerConversations.conversationActive == false && !isShop && !levelUpPanel.activeInHierarchy && !readingNote) {
				isPause = !isPause;
				if (isPause) {
					Pause (true);
                    //select the first menu button to eneble control with joystick)
                    eventSystem.SetSelectedGameObject(loadGameButton);
                    //eventSystem.
				} else {				
					Pause (false);
				}
			}
		}

        if (Input.GetKeyDown(KeyCode.F5)) {
            if (player.GetComponent<RayCastScript>().isDead == true) 
            {
                LoadGameWhenDead();
            }
        }
}

	public void Pause(bool yes) {
		if (yes) {
            disableControlls = true;
			player.GetComponent<FirstPersonController> ().enabled = false;
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			Time.timeScale = 0;
			isPause = true;

		}
		if (!yes) {
            disableControlls = false;
			Time.timeScale = 1;
            if (player.GetComponent<RayCastScript>().isDead == false)
            {
                player.GetComponent<FirstPersonController>().enabled = true;
            }
			isPause = false;
		}
	}
	//when monster attack - change player hp
	public void PlayerHPChange (float damage) { 
		//if the damage is greater than current armor: damage - armor, hp - damageLeft to 0
		//if damage is less than current armor: armor-damage, hp - damage/100
		float damageLeft = 0; //calculates the damage - armor (ex. damage100 - armor50= 50 damage is done to player hp, armor = 0
		if (playerArmor <= damage) {
			damageLeft = damage - playerArmor;
			playerArmor = 0;
			playerHP -= damageLeft;
		}
		if (playerArmor >= damage) {	
			playerArmor -= damage;
			playerHP -= damage / playerArmor;
		}
	}

	//pick up the note and add its text to the GUI to read. disable camera moves and enable cursor. if the note is closed, enable everything back
	public void NotePickUp (string noteText, GameObject note) {
        print("convos on");
        disableControlls = true;
		playerHPOnGUI = playerHP;
		notes.SetActive (true);
		player.GetComponent<FirstPersonController> ().enabled = false;
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		readingNote = true;
		notes.GetComponentInChildren<Text> ().text = noteText;
		//Destroy (note);
		//check if note was read already. if not add +1 to read notes.
		if (!note.GetComponent<Notes> ().readOnce) {
			notesCollected += 1;
			note.GetComponent<Notes> ().readOnce = true;
		}
	}

	public void WeaponPickUp (WeaponScript weapon){//WeaponScript weapon) {//string weaponName, float weaponDamage, float weaponSpeed, float weaponImpulse, Sprite weaponSprite, bool isRanged, float weaponRange, AudioClip rangedFireSound, float currAmmo, float totalAmmo) { //when weapon is picked up
		//WeaponScript weapon = weaponObject.GetComponent<WeaponScript>();
		if (weapon.isRanged == true) {
			foreach (Transform child in fpsWeaponScale.transform) {
				Destroy (child.gameObject);
			}
			fpsWeaponScale.transform.localScale = new Vector3 (0.25f, 0.25f, 0.25f);
			//saveManager.iDList.Add (weapon.iD);
			this.rangedWeaponName = weapon.weaponName;
			this.weaponDescription = weapon.weaponDescription;
			this.WeaponRangedDamage = weapon.weaponDamage;
			this.rangedAttackSpeed =weapon.weaponAttackSpeed;
			this.rangedImpulse = weapon.weaponImpulse;
			this.rangedWeaponRange = weapon.weaponRange;
//			this.rangedWeaponImage.sprite = weapon.weaponSprite;
			this.rangedFireSound = weapon.rangedFireSound;
			this.currAmmo = weapon.currAmmo;
			this.totalWeaponAmmo = weapon.totalAmmo;
			this.reloadTime = weapon.reloadTime;
			this.ammoConsume = weapon.ammoConsume;
            this.weaponShakeCameraAmount = weapon.cameraShakeAmount;

			this.weaponUpgradeAmount = weapon.weaponUpgradeAmount;
			this.weaponUpgradeCost = weapon.weaponUpgradeCost;
			//Debug.Log (weapon.weaponUpgradeCost);
			this.weaponUpgradeEXPCap = weapon.weaponUpgradeEXPCap;
			this.recoil = weapon.recoil;
			//fpsWeapon.weaponSpriteRendered.sprite = weapon.weaponSprite;				//add weapon sprite
			//fpsWeaponScale.transform.localScale *= weapon.weaponSize;

			//with models. doesnt work for now.
			//load all resourses and find the current weapon to get its model and add it to the gun
			foreach (GameObject weaponPrefab in weaponsPrefabsArray) {
				if (weaponPrefab.GetComponent<WeaponScript>() != null) {
					//check if prefab is a weapon
					if (weaponPrefab.GetComponent<WeaponScript>().weaponName == weapon.weaponName) {
						//Debug.Log(weaponPrefab.GetComponent<WeaponScript>().weaponName); 
						GameObject weaponModel = Instantiate (weaponPrefab.GetComponent<WeaponScript>().weaponModel, fpsWeaponScale.transform) as GameObject;
						Destroy(weaponModel.GetComponent<Animator>());
						weaponModel.name = weapon.weaponName;
						weaponModel.transform.localRotation = new Quaternion (0,0,0,0);				//the rotation of the weapon
						weaponModel.transform.localPosition = new Vector3 (0, 0, 0);				//the position of the weapon
						weaponModel.transform.localScale /= weapon.weaponSize;
						//Debug.Log (weaponModel.name);
						//Debug.Log (weaponModel.transform.position);
					}
				}
			}

			foreach (WeaponToInventory inventoryWeapon in weaponList) {
				if (inventoryWeapon.weaponName == weapon.weaponName) {
					inventoryWeapon.weaponUpgradeMultiplier = weapon.weaponUpgradeMultiplier;
					inventoryWeapon.upgradeEXPCap = weapon.weaponUpgradeEXPCap;
					inventoryWeapon.upgradeCost = weapon.weaponUpgradeCost;
				}
			}

		}
		/*
		if (weapon.isRanged == false) {
			this.weaponDamage = weapon.weaponDamage;
			this.weaponAttackSpeed = weapon.weaponAttackSpeed;
			this.weaponImpulse = weapon.weaponImpulse;
			this.weaponImage.sprite = weapon.weaponSprite; 
			this.weaponName = weapon.weaponName;
		}
		*/
		StatsUpdate ();
	}

	//update player stats
	public void StatsUpdate () { 
		
		if (playerHP >= maxHP) {
			playerHP = maxHP;
		}

        if (playerArmor >= 350)
        {
            playerArmor = 350;
        }
		//playerHitRate = weaponAttackSpeed;//corePlayerHitRate - (playerEXP / 500);
		playerDamage = corePlayerDamage + (playerEXP / 100);
		rangedDamage = (WeaponRangedDamage * ((100 + weaponUpgradeBonus) / 100)) + (Mathf.Round (WeaponRangedDamage * playerEXP * 0.00006f * 100)) / 100;
		playerDamageImpule = corePlayerDamageImpule + Mathf.Round(playerEXP / 150000) + weaponImpulse;
		//rangedAttackSpeed = rangedAttackSpeed * ((100 + weaponUpgradeBonus/3) / 100);
		//rangedImpulse = rangedImpulse * ((100 + weaponUpgradeBonus/2) / 100);
		//rangedWeaponRange = rangedWeaponRange * ((100 + weaponUpgradeBonus/3) / 100);
		maxAmmo = 150 + (Mathf.Round(playerEXP/40));				//each 1000 EXP will give 10 more maximum ammo

		playerCurrentEXP = playerEXP;

	}

	public void SoulPickUp (float soulAmount, float soulEXP, GameObject soul) {
		soulAmount = Mathf.Round (soulAmount * 10) / 10;											//round the souls to 2 decimal
		float bonus = Mathf.Round (((soulAmount * soulBonus) - soulAmount) * 10) / 10;				//calculate the bonus from level ups
		float total = soulAmount + bonus;															//the total souls that is gained
		GameObject reward = Instantiate (rewardText, transform) as GameObject;						//instantiate reward text
		reward.GetComponent<RewardText> ().ShowRewardText (soulAmount, soulEXP, bonus);				//send to reward text
		playerEXP += soulEXP;																		//add exp
		monsterSoulsAmount += total;
		if (!soul.GetComponent<MonsterSoul> ().dropped) {
			saveManager.AddID (soul.GetComponent<MonsterSoul> ().iD);
		}
        //instantiate pickup smoke
        GameObject pickupSmokeclone = Instantiate(pickupSmoke, soul.transform.position, pickupSmoke.transform.rotation) as GameObject;           
        Destroy (pickupSmokeclone, 5);

        Destroy (pickupSmokeclone, 5);

		Destroy (soul);
	}

	public void AmmoPickUp (float amount, GameObject ammo) {
		
		amount = Mathf.Round (amount * 10) / 10;

		if (totalAmmo < maxAmmo) 
		{
			float bonus = Mathf.Round ((amount * ammoBonus) * 10) / 10 - amount;	
			float total = amount + bonus;
			GameObject reward = Instantiate (rewardText, transform) as GameObject;
			reward.GetComponent<RewardText> ().AmmoRewardText (amount, bonus);
			totalAmmo += total;
			if (totalAmmo > maxAmmo)
				totalAmmo = maxAmmo;
			saveManager.AddID (ammo.GetComponent<Ammo> ().iD);
			
            //instantiate pickup smoke
            GameObject pickupSmokeclone = Instantiate(pickupSmoke, ammo.transform.position, pickupSmoke.transform.rotation) as GameObject;           
            Destroy (pickupSmokeclone, 5);

			Destroy (ammo);
		} 
		else 
		{
			GameObject reward = Instantiate (rewardText, transform) as GameObject;
			reward.GetComponent<RewardText> ().ReadSign ("You can't carry more ammo!");
		}
	}

	public void ArmorPickUp (float amount, GameObject armor) {
		amount = Mathf.Round (amount * 10) / 10;
		float bonus = Mathf.Round ((amount * armorBonus) * 10) / 10 - amount;	
		float total = amount + bonus;
		GameObject reward = Instantiate (rewardText, transform) as GameObject;
		reward.GetComponent<RewardText> ().ArmorRewardText (amount, bonus);
		playerArmor += total;
		saveManager.AddID (armor.GetComponent<Armor>().iD);
		
        //instantiate pickup smoke
        GameObject pickupSmokeclone = Instantiate(pickupSmoke, armor.transform.position, pickupSmoke.transform.rotation) as GameObject;           
        Destroy (pickupSmokeclone, 5);

		Destroy (armor);
	}

	public void UpgradePickup (float upgradeAmount, GameObject upgrade) {
		GameObject reward = Instantiate (rewardText, transform) as GameObject;
		reward.GetComponent<RewardText> ().UpgradeReward (upgradeAmount);
		weaponUpgradeBonus += upgradeAmount;
		StatsUpdate ();
		//Debug.Log (upgrade.GetComponent<WeaponUpgradeItem>().iD);
		saveManager.AddID (upgrade.GetComponent<WeaponUpgradeItem>().iD);

        //instantiate pickup smoke
        GameObject pickupSmokeclone = Instantiate(pickupSmoke, upgrade.transform.position, pickupSmoke.transform.rotation) as GameObject;           
        Destroy (pickupSmokeclone, 5);

		Destroy (upgrade);
	}
	//
	public void MonsterHealOnDeath (float amount) {
		amount = Mathf.Round (amount * 10) / 10;
		float bonus = Mathf.Round ((amount * healBonus) * 10) / 10 - amount;	
		float total = amount + bonus;
		GameObject reward = Instantiate (rewardText, transform) as GameObject;
		reward.GetComponent<RewardText> ().HealReward (amount, bonus);
		playerHP += total;
		StatsUpdate ();
	}

	public void ReadSign (string text) {
		GameObject reward = Instantiate (rewardText, transform) as GameObject;
		reward.GetComponent<RewardText> ().ReadSign (text);
	}

	void OnGUI () 
    {
		if (readingNote) {

            if (!notes.activeInHierarchy || playerHP < playerHPOnGUI || Input.GetButton("Escape")) {                
				notes.SetActive (false);
				player.GetComponent<FirstPersonController> ().enabled = true;
				readingNote = false;
                disableControlls = false;
                print("convos off");
			}	
		} 

		if (isPause) {
		
			if (isShop) {
				shopMenu.SetActive (true);
                pauseMenuMainPanel.SetActive(false);
				//weaponUpgradeCost = weaponUpgradeCost + currentShop.weaponUpdatePrice;
				//weaponUpgradeEXPCap = weaponUpgradeEXPCap + currentShop.weaponUpdatePrice;
				//small ammo
				buyAmmoButton.GetComponentInChildren<Text> ().text = string.Format ("Buy 10 Ammo ({0}GOLD)", currentShop.smallAmmoPrice);
				//big ammo
				buyMoreAmmoButton.GetComponentInChildren<Text> ().text = string.Format ("Buy 100 Ammo ({0}GOLD)", currentShop.bigAmmoPrice);
				//small ARMOR
				buyHpButton.GetComponentInChildren<Text> ().text = string.Format ("Buy 10 ARMOR ({0}GOLD)", currentShop.smallArmorPrice);
				//big ARMOR
				buyMoreHpButton.GetComponentInChildren<Text> ().text = string.Format ("Buy 100 ARMOR ({0}GOLD)", currentShop.bigArmorPrice);

				Sell1Soul.GetComponentInChildren<Text> ().text = string.Format ("Sell 1 Soul ({0}GOLD)", currentShop.soulPrice);
				Sell10Soul.GetComponentInChildren<Text> ().text = string.Format ("Sell 10 Souls ({0}GOLD)", currentShop.soulPrice * 10);
				Sell100Soul.GetComponentInChildren<Text> ().text = string.Format ("Sell 100 Souls ({0}GOLD)", currentShop.soulPrice * 100);
				//upgrade weapon
				upgradeButton.GetComponentInChildren<Text> ().text = "Upgrade " + rangedWeaponName + " " + weaponUpgradeCost + "G" + " Damage bonus: " + weaponUpgradeAmount + " and small bonus to all stats" + "(Need " + weaponUpgradeEXPCap + "EXP to upgrade)";
				if (weaponUpgradeCost != 0 && playerGOLD >= weaponUpgradeCost && playerEXP >= weaponUpgradeEXPCap)
					upgradeButton.GetComponentInChildren<Text> ().color = Color.black;
				else
					upgradeButton.GetComponentInChildren<Text> ().color = Color.red;
				if (playerGOLD >= currentShop.smallAmmoPrice)
					buyAmmoButton.GetComponentInChildren<Text> ().color = Color.black;
				else
					buyAmmoButton.GetComponentInChildren<Text> ().color = Color.red;
				if (playerGOLD >= currentShop.smallArmorPrice)
					buyHpButton.GetComponentInChildren<Text> ().color = Color.black;
				else
					buyHpButton.GetComponentInChildren<Text> ().color = Color.red;
				if (playerGOLD >= currentShop.bigAmmoPrice)
					buyMoreAmmoButton.GetComponentInChildren<Text> ().color = Color.black;
				else
					buyMoreAmmoButton.GetComponentInChildren<Text> ().color = Color.red;
				if (playerGOLD >= currentShop.bigArmorPrice)
					buyMoreHpButton.GetComponentInChildren<Text> ().color = Color.black;
				else
					buyMoreHpButton.GetComponentInChildren<Text> ().color = Color.red;

				if (monsterSoulsAmount >= 1)
					Sell1Soul.GetComponentInChildren<Text> ().color = Color.yellow;
				else
					Sell1Soul.GetComponentInChildren<Text> ().color = Color.red;
				if (monsterSoulsAmount >= 10)
					Sell10Soul.GetComponentInChildren<Text> ().color = Color.yellow;
				else
					Sell10Soul.GetComponentInChildren<Text> ().color = Color.red;
				if (monsterSoulsAmount >= 100)
					Sell100Soul.GetComponentInChildren<Text> ().color = Color.yellow;
				else
					Sell100Soul.GetComponentInChildren<Text> ().color = Color.red;

				playerGOLDText.text = "Gold: " + playerGOLD;
                playerSoulsText.text = "Monster Souls: " + Mathf.Round(monsterSoulsAmount * 100) / 100 + "\nAmmo: " + currAmmo +"/" + totalAmmo + "\nArmor: " + playerArmor + "\nWeapon Damage: " + rangedDamage;

			}
			//info text in the F1 menu
			
            if (difficultyIsReduced)
            {
                pauseInfoText.text = string.Format ("Enemies Killed: {0}\nEXP: {1}\nGold: {2}\nMonster Souls: {3}\nNotes Collected: {4}\nSecrets: {5}/{6}\nMonsters are weak, less EXP and Souls (will return to normal after game load)",
                    mostersKilled, playerCurrentEXP, playerGOLD, Mathf.Round(monsterSoulsAmount*100)/100, notesCollected, secretsFound, secretsTotal);
            } else 
                pauseInfoText.text = string.Format ("Enemies Killed: {0}\nEXP: {1}\nGold: {2}\nMonster Souls: {3}\nNotes Collected: {4}\nSecrets: {5}/{6}",
                    mostersKilled, playerCurrentEXP, playerGOLD, Mathf.Round(monsterSoulsAmount*100)/100, notesCollected, secretsFound, secretsTotal);
			//weapon text in the F1 menu weapon info
			WeaponInfoText.text = string.Format ("Upgrade Bonus(dropped by monsters or found): {8}%\nName: {0}\nDamage: {1}\nRange: {2}\nFire Rate: {3}\nAmmo Consume per Shot: {5}\nImpulse: {4}\n--{7}--\nMelee Damage: {6}",
				rangedWeaponName, rangedDamage, rangedWeaponRange, rangedAttackSpeed, rangedImpulse, ammoConsume, playerDamage, weaponDescription, weaponUpgradeBonus);
			pauseMenu.SetActive (true);
		} else {
			//GameObject.Find ("LevelUpPanel").SetActive (false);
			pauseMenu.SetActive (false);
		}
	}

	public void GoToMainMenu () {
		InventoryScript.ClearInventory ();

		MainMenu.useSaveData = false;
		Time.timeScale = 1;
		if (DialogueManager.IsConversationActive) DialogueManager.StopConversation ();
		SceneManager.LoadScene ("MainMenu");
	}

    //will decrease all enemy's hp by 2
    public void ChangeDifficuly () 
    {
        difficultyIsReduced = true;
        //find all enemies on level
        EnemyHP[] allEnemyHP = GameObject.FindObjectsOfType<EnemyHP>() as EnemyHP[];
        //loop through enemies, if not boss - reduce hp
        foreach (EnemyHP hp in allEnemyHP)
        {
            if (!hp.isBoss)
            {
                hp.hpMonster /= 2;
                hp.expFromMonster *= 0.6f;
                hp.damageMonster *= 0.7f;
            }
        }
    }

	//shop menu
	//1 souls
	public void SellOneSoul () {
		if (monsterSoulsAmount >= 1) {
			monsterSoulsAmount -= 1;
			playerGOLD += currentShop.soulPrice;
			StatsUpdate ();
		}
	}
	//10 souls
	public void SellTenSoul () {
		if (monsterSoulsAmount >= 10) {
			monsterSoulsAmount -= 10;
			playerGOLD += currentShop.soulPrice * 10;
			StatsUpdate ();
		}
	}
	//100 souls
	public void SellHundredSoul () {
		if (monsterSoulsAmount >= 100) {
			monsterSoulsAmount -= 100;
			playerGOLD += currentShop.soulPrice * 100;
			StatsUpdate ();
		}
	}
	//10 ammo
	public void BuyAmmo () {
		if (playerGOLD >= currentShop.smallAmmoPrice) {
			if (totalAmmo < maxAmmo) {
				totalAmmo += 10;
				if (totalAmmo > maxAmmo) {
					totalAmmo = maxAmmo;
				}
				GameObject reward = Instantiate (rewardText, transform) as GameObject;
				reward.GetComponent<RewardText> ().ReadSign ("+10 AMMO. -" + currentShop.smallAmmoPrice + " gold");
				playerGOLD -= currentShop.smallAmmoPrice;
				StatsUpdate ();
			} else {
				GameObject reward = Instantiate (rewardText, transform) as GameObject;
				reward.GetComponent<RewardText> ().ReadSign ("You can't carry more ammo!");
			}
		}
	}
	public void BuyMoreAmmo () {
		if (playerGOLD >= currentShop.bigAmmoPrice) {
			if (totalAmmo + 100 < maxAmmo) {
				totalAmmo += 100;
				if (totalAmmo > maxAmmo) {
					totalAmmo = maxAmmo;
				}
				GameObject reward = Instantiate (rewardText, transform) as GameObject;
				reward.GetComponent<RewardText> ().ReadSign ("+10 AMMO. -" + currentShop.bigAmmoPrice + " gold");
				playerGOLD -= currentShop.bigAmmoPrice;
				StatsUpdate ();
			} else {
				GameObject reward = Instantiate (rewardText, transform) as GameObject;
				reward.GetComponent<RewardText> ().ReadSign ("You can't carry more ammo!");
			}
		}
	}
	public void BuyHeal () {
		if (playerGOLD >= currentShop.smallArmorPrice) {
			playerArmor += 10;
			playerGOLD -= currentShop.smallArmorPrice;
			StatsUpdate ();
		}
	}
	public void BuyMoreHeal () {
		if (playerGOLD >= currentShop.bigArmorPrice) {
			playerArmor += 100;
			playerGOLD -= currentShop.bigArmorPrice;
			StatsUpdate ();
		}
	}

	public void UpgradeWeapon () {
		//upgrade the weapon in shop.
		if (playerGOLD >= weaponUpgradeCost && playerEXP >= weaponUpgradeEXPCap) {
			//int i = 0;
			foreach (WeaponScript w in InventoryScript.rangedWeapons) {				
				if (w.weaponName == rangedWeaponName) {	
					w.WeaponUpgrade ();
					//this.weaponUpgradeCost = w.weaponUpgradeCost;
				}
				///i++;
			}
		}
	}
	//shop menu

	//save the game
	public void SaveGame() {
		
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.dataPath + "/SaveGame.dat");

		SaveDataFromPlayerStats data = new SaveDataFromPlayerStats ();

		//here comes the data to save
		data.weaponUpgradeBonus = weaponUpgradeBonus;
		data.loreList = loreList;
		data.playerHP = playerHP;
		data.playerArmor = playerArmor;
		data.playerEXP = playerEXP;
		data.playerGOLD = playerGOLD;
		data.mostersKilled = mostersKilled;
		data.monsterSouls = monsterSoulsAmount;
		data.currentLevel = currentLevel; //get the current scene (to load it later)
		data.totalAmmo = totalAmmo;
		data.achievementsID = achievementsID; //save ID of achivemnts player has so far.
		data.weaponsID = weaponsID; //save list of weaponIDs (added when you pick up a weapon on map)
		data.weaponList = weaponList;
		data.notesCollected = notesCollected;

		//save bonuses
		data.ammoBonus = ammoBonus;
		data.soulBonus = soulBonus;
		data.dropChance = dropChance;
		data.armorBonus = armorBonus;
		data.healBonus = healBonus;
		data.maxHP = maxHP;
		data.level = level;
		//save bonuses

		//here comes the data to save

		bf.Serialize (file, data);
		file.Close ();

		//Debug.Log ("Game Saved " + data.playerHP);
	}


	/// <summary>
	/// Saves the game on save point. Pass here the coordinates of player position x y z
	/// </summary>
	public void SaveGameOnSavePoint(float x, float y, float z) {

		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.dataPath + "/SaveGameAtSavePoint.dat");

		SaveDataFromPlayerStats data = new SaveDataFromPlayerStats ();

		//here comes the data to save
		data.weaponUpgradeBonus = weaponUpgradeBonus;
		data.loreList = loreList;
		data.playerHP = playerHP;
		data.playerArmor = playerArmor;
		data.playerEXP = playerEXP;
		data.playerGOLD = playerGOLD;
		data.mostersKilled = mostersKilled;
		data.monsterSouls = monsterSoulsAmount;
		data.currentLevel = currentLevel; //get the current scene (to load it later)
		data.totalAmmo = totalAmmo;
		data.achievementsID = achievementsID; //save ID of achivemnts player has so far.
		data.weaponsID = weaponsID; //save list of weaponIDs (added when you pick up a weapon on map)
		data.weaponList = weaponList;
		data.notesCollected = notesCollected;

		//save bonuses
		data.ammoBonus = ammoBonus;
		data.soulBonus = soulBonus;
		data.dropChance = dropChance;
		data.armorBonus = armorBonus;
		data.healBonus = healBonus;
		data.maxHP = maxHP;
		data.level = level;
		//save bonuses

		//saving list of items and player position
		data.itemsAndMonstersIDs = saveManager.iDList;
		data.playerX = x;
		data.playerY = y;
		data.playerZ = z;

		//saving some special things for the scenes
		data.w01OldKnightTalk = saveManager.w01OldKnightTalk;

		try 
		{
			data.sunIntensity = GameObject.Find("Sun").GetComponent<Light>().intensity;
			Debug.Log (data.sunIntensity);
		} 
		catch 
		{
			Debug.Log ("No sun");
		}

		//saving Beast hp here. For level with Beast
		if (currentLevel == "World02-2(Beast)") {

            GameObject beast = GameObject.Find("Beast");

            if (beast.GetComponent<EnemyHP>().followOnce == true)
            {
                data.beastFollowedOnce = true;
                data.beastHP = beast.GetComponent<EnemyHP> ().hpMonster;
            }

			//Debug.Log (data.beastHP);
		}
	

		if (GameObject.Find ("SaveCube") != null) {		 //find the save cube
			data.saveCubeVisited = GameObject.Find ("SaveCube").GetComponent<SaveCube>().visited;
		}
		bf.Serialize (file, data);
		file.Close ();

		Debug.Log ("Game Saved at Save Point");
	}

	public void LoadGame () {
		if (File.Exists (Application.dataPath + "/SaveGame.dat")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.dataPath + "/SaveGame.dat", FileMode.Open);
			SaveDataFromPlayerStats data = (SaveDataFromPlayerStats)bf.Deserialize (file);
			file.Close ();
			//get the list of all weapon prefabs in My/Prefabs/Weapons and make each a gameobject

		

			weaponPrefabs = Resources.LoadAll ("My/Prefabs/Weapons", typeof(GameObject)).Cast<GameObject> ().ToArray();
			notesCollected = data.notesCollected;
			weaponUpgradeBonus = data.weaponUpgradeBonus;
			loreList = data.loreList;
			playerHP = data.playerHP;
			playerArmor = data.playerArmor;
			playerEXP = data.playerEXP;
			playerGOLD = data.playerGOLD;
			mostersKilled = data.mostersKilled;
			monsterSoulsAmount = data.monsterSouls;

			//LOAD bonuses
			ammoBonus = data.ammoBonus;
			soulBonus = data.soulBonus;
			dropChance = data.dropChance;
			armorBonus = data.armorBonus;
			healBonus = data.healBonus;
			maxHP = data.maxHP;
			level = data.level;
			//LOAD bonuses

			//playerHealingRate = data.playerHealingRate;
			totalAmmo = data.totalAmmo;
			achievementsID = data.achievementsID;
			foreach (int achID in achievementsID) {
				foreach (GameObject achObj in achievementArray) {
					if (achObj.GetComponent<AchievementID> ().ID == achID) {
						achObj.GetComponent<Text> ().color = Color.blue;
					}
				}
			} 
			weaponsID = data.weaponsID;
			weaponList = data.weaponList;
			//circle through weaponIds (they are added when you pick up a weapon) to compare them with the IDs from weapon prefabs
			foreach (int w in weaponsID) 
            {
				//for each ID circle through weapon prefabs
				foreach (GameObject weaponOnMap in weaponPrefabs) 
                {
					//check if weapon prefab is weapon (has weaponscript in it) + check if ID matches ID in playerStats
					if (weaponOnMap.GetComponent<WeaponScript> () != null && weaponOnMap.GetComponent<WeaponScript> ().weaponID == w) {
						GameObject clone = Instantiate (weaponOnMap, transform) as GameObject;
						clone.name = weaponOnMap.name;
						WeaponScript cloneScript = clone.GetComponent<WeaponScript> ();
						//weapon sprite is not loaded I don't know why, so assign it here again!
						cloneScript.weaponSprite = weaponOnMap.GetComponent<WeaponScript>().weaponSprite;
						//add the found weapon to inventory on load
						foreach (WeaponToInventory weapon in weaponList) {
							if (cloneScript.weaponName == weapon.weaponName) {
								//cloned weapon will have a upgrade multiplier as the saved weapon
								cloneScript.weaponUpgradeMultiplier = weapon.weaponUpgradeMultiplier;

								//upgrade the weapon to the saved upgrades							
								//add to stats multipling them by upgrade amount
								cloneScript.weaponDamage += cloneScript.weaponUpgradeAmount*cloneScript.weaponUpgradeMultiplier;
								cloneScript.weaponRange += Mathf.Round(cloneScript.weaponUpgradeAmount*cloneScript.weaponUpgradeMultiplier / 10);
								cloneScript.weaponImpulse += Mathf.Round(cloneScript.weaponUpgradeAmount*cloneScript.weaponUpgradeMultiplier / 10);
								//calculate the cost for the next weapon upgade
								cloneScript.weaponUpgradeEXPCap =  Mathf.Round(cloneScript.upgradeInitialEXP *  Mathf.Pow(weapon.weaponUpgradeMultiplier, 1.2f)); 
								cloneScript.weaponUpgradeCost =  Mathf.Round(cloneScript.upgradeInitialCost *  Mathf.Pow(weapon.weaponUpgradeMultiplier, 1.2f));

							}
						}
						//now add the cloned weapon to the inventory script array
						InventoryScript.rangedWeapons.Add (cloneScript);
						//destroy the cloned weapon from level
						Destroy (clone);
					}
				}	
			}
            if (InventoryScript.rangedWeapons.Count != 0) {
                InventoryScript.SelectBestWeapon ();
            }
          //  InventoryScript.SelectBestWeapon();

		}
	}

	public void LoadGameAtSavePoint () {
		if (File.Exists (Application.dataPath + "/SaveGameAtSavePoint.dat")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.dataPath + "/SaveGameAtSavePoint.dat", FileMode.Open);
			SaveDataFromPlayerStats data = (SaveDataFromPlayerStats)bf.Deserialize (file);
			file.Close ();
			//get the list of all weapon prefabs in My/Prefabs/Weapons and make each a gameobject
			weaponPrefabs = Resources.LoadAll ("My/Prefabs/Weapons", typeof(GameObject)).Cast<GameObject> ().ToArray();

			saveManager.iDList = data.itemsAndMonstersIDs;
			saveManager.w01OldKnightTalk = data.w01OldKnightTalk;			//load the state of dialogue with knigh
			//load the sun intensity
			try 
			{
				GameObject.Find("Sun").GetComponent<Light>().intensity = data.sunIntensity;	
				//Debug.Log (data.sunIntensity);
			} 
			catch 
			{
				Debug.Log ("No sun");
			}

            //BEAST chapter
			if (currentLevel == "World02-2(Beast)") {               
                //if beast already followed player before - don't allow it to multiply its hp once more! and load hp
                if (data.beastFollowedOnce)
                {
                    GameObject beast = GameObject.Find("Beast");                        //find the beast
                    beast.GetComponent<EnemyHP>().followOnce = true;                    
                    beast.GetComponent<EnemyHP> ().hpMonster = data.beastHP;            //assign beast hp from load data to beast.
                }
			}

			//load save cube visited state
			if (GameObject.Find ("SaveCube") != null) {		 //find the save cube
				GameObject.Find ("SaveCube").GetComponent<SaveCube>().visited = data.saveCubeVisited;
			}

			//load player position
			player.transform.position = new Vector3(data.playerX, data.playerY, data.playerZ);

			//initiate the destruction of all objects from array (all dead monsters, all picked items etc)
			saveManager.LoadGameAndDisableIDs();



			notesCollected = data.notesCollected;
			weaponUpgradeBonus = data.weaponUpgradeBonus;
			loreList = data.loreList;
			playerHP = data.playerHP;
			playerArmor = data.playerArmor;
			playerEXP = data.playerEXP;
			playerGOLD = data.playerGOLD;
			mostersKilled = data.mostersKilled;
			monsterSoulsAmount = data.monsterSouls;

			//LOAD bonuses
			ammoBonus = data.ammoBonus;
			soulBonus = data.soulBonus;
			dropChance = data.dropChance;
			armorBonus = data.armorBonus;
			healBonus = data.healBonus;
			maxHP = data.maxHP;
			level = data.level;
			//LOAD bonuses

			//playerHealingRate = data.playerHealingRate;
			totalAmmo = data.totalAmmo;
			/*
			achievementsID = data.achievementsID;
			foreach (int achID in achievementsID) {
				foreach (GameObject achObj in achievementArray) {
					if (achObj.GetComponent<AchievementID> ().ID == achID) {
						achObj.GetComponent<Text> ().color = Color.blue;
					}
				}
			} 
			*/
			weaponsID = data.weaponsID;
			weaponList = data.weaponList;
			//circle through weaponIds (they are added when you pick up a weapon) to compare them with the IDs from weapon prefabs
			foreach (int w in weaponsID) 
            {
				//for each ID circle through weapon prefabs
				foreach (GameObject weaponOnMap in weaponPrefabs) {
					//check if weapon prefab is weapon (has weaponscript in it) + check if ID matches ID in playerStats
					if (weaponOnMap.GetComponent<WeaponScript> () != null && weaponOnMap.GetComponent<WeaponScript> ().weaponID == w) {
						GameObject clone = Instantiate (weaponOnMap, transform) as GameObject;
						clone.name = weaponOnMap.name;
						WeaponScript cloneScript = clone.GetComponent<WeaponScript> ();
						//weapon sprite is not loaded I don't know why, so assign it here again!
						cloneScript.weaponSprite = weaponOnMap.GetComponent<WeaponScript>().weaponSprite;
						//add the found weapon to inventory on load
						foreach (WeaponToInventory weapon in weaponList) {
							if (cloneScript.weaponName == weapon.weaponName) {
								//cloned weapon will have a upgrade multiplier as the saved weapon
								cloneScript.weaponUpgradeMultiplier = weapon.weaponUpgradeMultiplier;

								//upgrade the weapon to the saved upgrades							
								//add to stats multipling them by upgrade amount

									cloneScript.weaponDamage += cloneScript.weaponUpgradeAmount * (cloneScript.weaponUpgradeMultiplier - 1);
								cloneScript.weaponRange += Mathf.Round (cloneScript.weaponUpgradeAmount * (cloneScript.weaponUpgradeMultiplier - 1) / 10);
								cloneScript.weaponImpulse += Mathf.Round (cloneScript.weaponUpgradeAmount * (cloneScript.weaponUpgradeMultiplier - 1) / 10);

								//calculate the cost for the next weapon upgade
								cloneScript.weaponUpgradeEXPCap =  Mathf.Round(cloneScript.upgradeInitialEXP *  Mathf.Pow(weapon.weaponUpgradeMultiplier, 1.2f)); 
								cloneScript.weaponUpgradeCost =  Mathf.Round(cloneScript.upgradeInitialCost *  Mathf.Pow(weapon.weaponUpgradeMultiplier, 1.2f));
							}
						}
						//now add the cloned weapon to the inventory script array
						InventoryScript.rangedWeapons.Add (cloneScript);
						//destroy the cloned weapon from level
						Destroy (clone);
					}
				}	
			}
            if (InventoryScript.rangedWeapons.Count != 0) {
                InventoryScript.SelectBestWeapon ();
            }
           // InventoryScript.SelectBestWeapon();


		}
	}

    /// <summary>
    /// Loads the game when dead. Instantly. Also can use F5 to load
    /// </summary>
    public void LoadGameWhenDead() 
    {
        //if player is dead
        if (player.GetComponent<RayCastScript>().isDead == true)
        {
            
            //initialize floats for player exp
            float expFromChapter = 0;
            float expFromSavePoint = 0;

            string levelChapter = "";
            string levelSavePoint = "";

            //load player exp from save files to later compare them
            if (File.Exists(Application.dataPath + "/SaveGame.dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.dataPath + "/SaveGame.dat", FileMode.Open);
                SaveDataFromPlayerStats dataChapter = (SaveDataFromPlayerStats)bf.Deserialize(file);
                file.Close();

                expFromChapter = dataChapter.playerEXP;
                levelChapter = dataChapter.currentLevel;

            }
            if (File.Exists(Application.dataPath + "/SaveGameAtSavePoint.dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.dataPath + "/SaveGameAtSavePoint.dat", FileMode.Open);
                SaveDataFromPlayerStats dataSavePoint = (SaveDataFromPlayerStats)bf.Deserialize(file);
                file.Close();

                expFromSavePoint = dataSavePoint.playerEXP;
                levelSavePoint = dataSavePoint.currentLevel;
            }

            //destroy all singletons, clear inventory, unpause game, show cursor?      
            Time.timeScale = 1;
            Destroy(this.gameObject);
            GameObject dialogueManager = GameObject.Find("Dialogue Manager");
            InventoryScript.ClearInventory();
            Destroy(dialogueManager);       
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            //if chapter save has more EXP (means player advanced to next level) - load the chapter save
            if (expFromChapter > expFromSavePoint && levelChapter != levelSavePoint)
            { 
                MainMenu.sceneToLoad = levelChapter; //get the saved scene;
                LoadingScreenLogic.sceneName = levelChapter;
                MainMenu.useSaveData = true; //set this so that when the scene is loaded use the save file
                if (levelChapter == currentLevel)
                {
                    SceneManager.LoadScene(levelChapter);
                }
                else
                    SceneManager.LoadScene("LoadingScreen");                
            }

            //if save point has more exp - load save point
            else
            {               
                MainMenu.sceneToLoad = levelSavePoint; //get the saved scene;
                LoadingScreenLogic.sceneName = levelSavePoint;
                MainMenu.useSaveDataAtSavePoint = true; //set this so that when the scene is loaded use the save file
                if (levelSavePoint == currentLevel)
                {
                    SceneManager.LoadScene(levelSavePoint);
                }
                else
                    SceneManager.LoadScene("LoadingScreen"); 
            }
        }

    
    }


}


[Serializable] class SaveDataFromPlayerStats {
	public ArrayList loreList = new ArrayList ();
	public ArrayList weaponList = new ArrayList (); //list of all weapons. just the main info
	public ArrayList achievementsID = new ArrayList();
	public ArrayList weaponsID = new ArrayList(); //use this to get the ID's of weapons player have on save
	public string currentLevel; //get the scene name
	public float weaponUpgradeBonus;			//the collected upgrae bonuses
	public float playerHP;
	public float playerArmor;
	public float monsterSouls;
	//public float playerHealingRate;	//heals this much hp everytime monster is killed
	public float playerEXP;	//current player's exp
	public float mostersKilled;
	public float playerGOLD; //the gold of the player
	public float totalAmmo;
	public int notesCollected;
	public ArrayList weapons = new ArrayList();
	public ArrayList rangedWeapons = new ArrayList();

	public float ammoBonus;		//added
	public float soulBonus;		//added
	public float dropChance;	//added
	public float armorBonus;	//added
	public float healBonus;		//added
	public float maxHP;	//checked
	public float level;

	/// <summary>
	/// The items and monsters IDs. Will be used if game is loaded at savepoint (will destroy all ids from the list in the scene!)
	/// </summary>
	public List<float> itemsAndMonstersIDs;
	public float playerX, playerY, playerZ;

	//Save Point different scenes important stuff
	public bool w01OldKnightTalk;				//use this to store wether you talked to onl knight on chapter 1
	public float sunIntensity;					//use this to store the sun intensity.
	
    //Beast chapter
    public float beastHP;
    public bool beastFollowedOnce;

	public bool saveCubeVisited;
}

[Serializable] public class WeaponToInventory {
	
	public int weaponID;
	public string weaponName;
	public int weaponUpgradeMultiplier;
	public float upgradeEXPCap;
	public float upgradeCost;
	public WeaponToInventory (int ID, string name, int upgradeAmont, float cap, float cost) {
		weaponID = ID;
		weaponName = name;
		weaponUpgradeMultiplier = upgradeAmont;
		upgradeEXPCap = cap;
		upgradeCost = cost;
	}
}
/*
public class AddToInventory {
	public ArrayList weponList = new ArrayList ();

	public void Add () {
		weponList.Add (new WeaponInventory(1, "pistol", 0));
	}
}
*/






//notesCollectedGUI = GameObject.Find("NotesCollected").GetComponent<Text>();
//		GameObject.Find ("MonsterLorePanel").SetActive (false); 
//		GameObject.Find ("AchievementsPanel").SetActive (false);
//weaponsPrefabsArray = Resources.LoadAll (("My/Prefabs/Weapons"), typeof(GameObject)).Cast<GameObject> ().ToArray();
//pause menu. Find it and then deactivate.

//			weaponImage = GameObject.Find ("WeaponImage").GetComponent<Image>(); //hud image to the left
//			rangedWeaponImage = GameObject.Find ("RangedWeaponImage").GetComponent<Image>(); //hud image to the right