///////////////////////////////////////very old file

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Audio;

//using UnityEditor;
using PixelCrushers.DialogueSystem;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.ImageEffects;
using UnityEngine.SceneManagement;

public class RayCastScript : MonoBehaviour
{
	
	public AudioMixerGroup mixer;
	public Camera cam;
	//pickup physics
	private PickUpPhysics pickUp;
	private float pickUpNextTime; //2 secs to pick up or drop object
	public bool pickUpInHands; //chech if you have an object in hands
	private GameObject pickUpObject; //object that is picked up
	//pickup physics

	//camera main
	public AudioClip hitSound1;
	public AudioClip rangedHitSound;
	private GameObject[] music;
	private SavePlayerStats playerStats;
	private bool reloadedAmmo = false;
	private Text hudText;
	public Text rangedWeaponText;
	private Text ammoText;
	private Text totalAmmo;
	public Text meleeWeaponText;
	public bool isDead; //check if player is dead
	private bool isPause;
	private AudioSource playerAudioAttack;
	private float reload; //true if the gun doen't have ammo in it
	private GameObject gunBarrel; //gun barrel as an empty object uset to initialize fire particle at.
	private Text mainText;
	//particles
	public GameObject weaponFireParticle;
	public GameObject weaponHitParticle;
	public GameObject weaponBloodParticle;
	public GameObject splashBloodDecal;
	private GameObject[] hitParticles; 			//additional particles
	private GameObject weaponBloodParticleAdditional;
	//particles
	public FirstPersonController fpsController;
	//private float dieWait = 0f;
	private float newHit = 0f;
	private float newPickup;			//time to wait for the next pickup
	private float reloadRange;
	private RaycastHit hit;
	private Ray ray;
	private AudioSource hitSource;
	private AudioSource audioSourceRangeHit;

	//input
	private bool lmbPressed;
	private bool rmbPressed;
	public bool fPressed;
	private bool qPressed;
	//input

	//fall damage
	public bool enableFallDamage; //set this if you want the player to take fall damage
	//public float fallTimeToGetDamage;
	//public float fallDamage;
	//private float timeOfJump = 0;
	//private float timeOfLand = 0;
	private bool jumpedOnce;
	private float currentSpeed;			//the current speed of the player
	private float prevFrameSpeed;		//the speed that was 1 frame ago
	//fall damage
	//private bool landedOnce;
	private FPSWeapon fpsWeapon;
	private AudioSource reloadAudioSource;
	public AudioClip reloadAmmoSound;

	private float respawnTimes = 0f; //counts the times player used respawn at cube
	private GameObject saveCube; //find the saveCube on the level to add to respawn
	private ParticleSystem slashParticle; 			//the particle of when player hits melee

	private BoxCollider playerBoxCollider;			//the box collider of the player (legs). use it to chech if grounded
	private GameObject deathMessage;					//the gameobject to show the death message


	private SaveManager saveManager;

    public bool enableKillCamInspector;
    public static bool enableKillCam;
    private GameObject killCam;

	void Start ()
	{	



		//0 - smoke. 1 - sparks for terrain. 2 - sparks for hilling shields!
		hitParticles = new GameObject[3];
		hitParticles[0] = Resources.Load ("My/Prefabs/Particles/New/DestructionSmoke") as GameObject;
		hitParticles[1] = Resources.Load ("My/Prefabs/Particles/New/WallHitParticle") as GameObject;
		hitParticles[2] = Resources.Load ("My/Prefabs/Particles/New/ArmorHitSparks") as GameObject;

        //load kill camera
        if (enableKillCamInspector)
        {
            enableKillCam = true;//killCam = Resources.Load("My/Prefabs/Items/KillCamera") as GameObject;
        }

		deathMessage = Resources.Load ("My/Prefabs/DeathMessage") as GameObject;
		playerBoxCollider = GetComponent<BoxCollider> ();
		fpsWeapon = GameObject.Find ("Gun").GetComponent<FPSWeapon> (); //get the gun (used to draw fps gun on screen)
		//the sound of reloading
		reloadAudioSource = gameObject.AddComponent<AudioSource>();
		reloadAudioSource.clip = reloadAmmoSound;
		reloadAudioSource.playOnAwake = false;
		reloadAudioSource.loop = false;	
		reloadAudioSource.outputAudioMixerGroup = mixer;
		reloadAudioSource.spatialBlend = 0f;
		reloadAudioSource.volume = 0.5f;
		//the sound of reloading
		slashParticle = GameObject.Find ("Slash").GetComponent<ParticleSystem> ();			//when melee attack particle

		saveCube = GameObject.Find ("SaveCube");		 //find the save cube
		fpsController = GetComponent<FirstPersonController> ();
		//mixer = Resources.Load ("MasterMixer") as AudioMixer;
		music = GameObject.FindGameObjectsWithTag ("Music");

		//decals	
		//bulletHoleDecal = GameObject.Find ("BulletHoleDecal");
		splashBloodDecal = GameObject.Find ("SplashBloodDecal");
		//decals

		playerStats = GameObject.Find ("SavePlayerStats").GetComponent<SavePlayerStats> ();
		//SAVE MANAGER HERE
		try 
		{
			saveManager = playerStats.gameObject.GetComponent<SaveManager> ();
		} 
		catch 
		{
			Debug.Log ("No Save Manager! Shit");
		}
		//SAVE MANAGER HERE
		weaponFireParticle = Resources.Load ("My/Prefabs/Particles/New/WeaponShootParticle") as GameObject;//GameObject.Find ("FiringParticle");
		weaponHitParticle = GameObject.Find ("BulletHitExplosion");
		weaponBloodParticle = GameObject.Find ("BloodParticle");
		weaponBloodParticleAdditional = Resources.Load ("My/Prefabs/Particles/New/BloodParticleAdditional") as GameObject;
		gunBarrel = GameObject.Find ("GunBarrel");

		//hud texts
//		hudText = GameObject.Find ("PlayerHPText").GetComponent<Text> ();
//		rangedWeaponText = GameObject.Find ("RangedWeaponText").GetComponent<Text> ();
//		ammoText = GameObject.Find ("AmmoText").GetComponent<Text> ();
//		totalAmmo = GameObject.Find ("TotalAmmo").GetComponent<Text> ();
//		meleeWeaponText = GameObject.Find ("MeleeWeaponText").GetComponent<Text> ();

		mainText = GetComponent<PlayerConversations> ().mainText;
		//hud texts

		//sound for firing the gun
		audioSourceRangeHit = gameObject.AddComponent<AudioSource> ();
		audioSourceRangeHit.playOnAwake = false;
		//audioSourceRangeHit.spread = 180;
		//audioSourceRangeHit.spatialBlend = 360;
		audioSourceRangeHit.volume = 0.5f;
		audioSourceRangeHit.outputAudioMixerGroup = mixer;


		
	


	}


	void Awake ()
	{
		Physics.IgnoreLayerCollision (13, 12); //ignore collisions between Ignore Projectiles and Projectiles layer
		Physics.IgnoreLayerCollision (13, 13);//ignore collisions between Projectiles and Projectiles layer
		Physics.IgnoreLayerCollision (2, 13);//ignore collisions between Projectiles and Ignore Raycast

		isDead = false;
		playerAudioAttack = gameObject.AddComponent<AudioSource> ();
		playerAudioAttack.clip = hitSound1;  //audio clip
		playerAudioAttack.playOnAwake = false;
		//playerAudioAttack.spread = 180;
		//playerAudioAttack.spatialBlend = 360;
		playerAudioAttack.volume = 0.5f;
	}


	void Update () {
		//Debug.Log (IsGrounded());
        /*
        if (fpsController.m_CharacterController.velocity.y < -20)
        {
            Debug.Log(fpsController.m_CharacterController.velocity);
        }
        */
		/*
		foreach (Collider coll in Overlap()) {
			Debug.Log (coll.name);
		}
		*/


		/*
		if (PlayerConversations.conversationActive == true || playerStats.readingNote || playerStats.isPause) {
			timeOfJump = Time.time;
		}
		*/
		//fall damage (get the time of jump and the land and calculate if it is big


		//===========================================FALL DAMAGE
		if (enableFallDamage) 
		{
			currentSpeed = fpsController.m_CharacterController.velocity.y;


			if (currentSpeed >= -1 && prevFrameSpeed <= -40) {
				//Debug.Log ("Cyrrent speed: " + currentSpeed);
				//Debug.Log ("Previous speed: " + prevFrameSpeed);
				Debug.Log ("Cyrrent speed: " + currentSpeed);
				Debug.Log ("Previous speed: " + prevFrameSpeed);

				//playerAudioAttack.Play ();

                DamagePlayer();

				playerStats.playerHP += prevFrameSpeed * 1.8f;
				//Debug.Log ("apply fall damage" + prevFrameSpeed * 1.4f);
			}

			prevFrameSpeed = fpsController.m_CharacterController.velocity.y;
		}
		//===========================================FALL DAMAGE



		//input
		lmbPressed = false;
		rmbPressed = false;
		fPressed = false;
		qPressed = false;
		if (Input.GetButton ("Fire1")) {
            if (!PlayerConversations.conversationActive && !SavePlayerStats.disableControlls)
            {
                lmbPressed = true;
            }
		}
        if (Input.GetButton ("PickUp")) {
            if (!PlayerConversations.conversationActive && !SavePlayerStats.disableControlls)
            {
                fPressed = true;
            }
        }
        /*
		if (Input.GetKey ("f")) {
			fPressed = true;
		}
  */      
		if (Input.GetButton ("Fire2")) {
            if (!PlayerConversations.conversationActive && !SavePlayerStats.disableControlls)
            {
                rmbPressed = true;
            }
		}
		if (Input.GetKey ("q")) {
            if (!PlayerConversations.conversationActive && !SavePlayerStats.disableControlls)
            {
                qPressed = true;
            }
		}
		if (pickUpInHands && Time.time >= pickUpNextTime) {
		
            if (Input.GetButton ("PickUp")) {
				pickUp.DropObject (pickUpObject);
				pickUpNextTime = Time.time + 1;
				pickUpInHands = false;
			}
		}
	
        if (Input.GetButtonDown ("ChangeWeapon")) { //change ranged weapon
			InventoryScript.SelectNextWeaponRanged ();
		}

        if(Input.GetKeyDown("p")) 
        {
            InventoryScript.SelectBestWeapon();
        }
        /*
		if (Input.GetKeyDown ("o")) {
			SceneManager.LoadScene ("World01");
		}
		if (Input.GetKeyDown ("l")) {
			SceneManager.LoadScene ("World02(Forest)");
		}
        */
		//input


		if (!isDead) {
			//DEATH
			if (Mathf.Round(playerStats.playerHP) <= 0) {
				PlayerDies ();
			}
			//DEATH

			mainText.text = "";
			if (playerStats.playerEXP > playerStats.playerCurrentEXP) {
				playerStats.StatsUpdate ();
			}


			//ranged attack
			if (playerStats.totalAmmo >= playerStats.ammoConsume) {
				//reload the gun
				if (Time.time > reloadRange && reloadedAmmo == false) {
					reloadedAmmo = true;
					playerStats.currAmmo = playerStats.totalWeaponAmmo;
				}

				//press R to reload (makes currammo 0 so the reload happens)
				if (Input.GetKeyDown (KeyCode.R)) {
					playerStats.currAmmo = 0;
				}
				if (playerStats.currAmmo <= 0 && reloadedAmmo == true && playerStats.totalWeaponAmmo > 0) {
					reloadedAmmo = false;
					reloadRange = Time.time + playerStats.reloadTime;
					reloadAudioSource.Play ();							//play reload sound
                    fpsWeapon.ReloadRecoil();
				}
				if (rmbPressed == true) { 											//check if pressed RMB					

					if (Time.time > reloadRange && reloadedAmmo == true) {	
							
						if (playerStats.currAmmo > 0) {
							reloadRange = Time.time + playerStats.rangedAttackSpeed;
							Attack (true);
						}			
					}					
				}
			}

			//Interract 		
			if (lmbPressed == true) {
				if (!playerStats.isPause) {
					Ray ray = cam.ScreenPointToRay (Input.mousePosition);
					RaycastHit hit;

					if (Physics.Raycast (ray, out hit, 15.0f)) {
						//Debug.Log (hit.collider.name);
						if (Time.time > newHit) { //&& hit.collider.isTrigger == false) { //attack enemy!
							newHit = Time.time + playerStats.playerHitRate;
							Attack (false);//ray, hit, false);
						}

						//dont pickup endlessly! use newHit time.
						if (newPickup < Time.time) {
							newPickup = Time.time + 0.3f;
							//picking up weapons
							if (hit.collider.tag == "PickUpObjects") {		
								hit.collider.GetComponent<WeaponScript> ().PickUpWeapon ();
								saveManager.AddID (hit.collider.GetComponent<WeaponScript> ().iD);
							}
							if (hit.collider.tag == "Ammo") { //when picking up ammo
								playerStats.AmmoPickUp (hit.collider.GetComponent<Ammo> ().ammoAmount, hit.collider.gameObject);
							}
							if (hit.collider.tag == "MonsterSoul") { //when picking up monster soul
								playerStats.SoulPickUp (hit.collider.GetComponent<MonsterSoul> ().soulAmount, hit.collider.GetComponent<MonsterSoul> ().soulEXP, hit.collider.gameObject);
							}
							if (hit.collider.tag == "WeaponUpgrade") { //when picking up ammo
								playerStats.UpgradePickup (hit.collider.GetComponent<WeaponUpgradeItem> ().upgradeAmount, hit.collider.gameObject);
							}
							//when picking up armor
							if (hit.collider.tag == "Armor") { 
								playerStats.ArmorPickUp (hit.collider.GetComponent<Armor> ().armorAmount, hit.collider.gameObject);
							}
							if (hit.collider.tag == "Note") { //when picking up note
								if (!playerStats.readingNote) {
									playerStats.NotePickUp (hit.collider.GetComponent<Notes> ().noteText, hit.collider.gameObject);
								}
							}
							if (hit.collider.tag == "Sign") {		
								playerStats.ReadSign (hit.collider.GetComponent<PostSign>().signText);
							}
						}

					}
				}
			}
		}	
	}
	void FixedUpdate () {
		
			//Pickup F pressed
			if (fPressed == true) {
				if (!playerStats.isPause) {
					Ray ray = cam.ScreenPointToRay (Input.mousePosition);
					RaycastHit hit;
					if (Physics.Raycast (ray, out hit, 10f)) {							
						//when picking up objects to carry
						if (hit.collider.tag == "PickUpPhysics") {
							pickUp = GetComponent<PickUpPhysics> ();
							if (Time.time >= pickUpNextTime) {								
								if (!pickUp.pickedUp) {
									pickUpObject = hit.collider.gameObject;
									pickUp.PickUpObject (pickUpObject);
									pickUpNextTime = Time.time + 1f;
									pickUpInHands = true;
								} 
							}
						}
						//when picking up objects to carry
					}
				}
			}
		if (qPressed == true) {
			if (!playerStats.isPause) {
				Ray ray = cam.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit, 10f)) {							
					//when picking up objects to carry
					if (hit.collider.tag == "Shield") {
						pickUp = GetComponent<PickUpPhysics> ();
						if (Time.time >= pickUpNextTime) {								
							if (!pickUp.pickedUp) {
								pickUpObject = hit.collider.gameObject;
								pickUp.PickUpAsShield (pickUpObject);
								pickUpNextTime = Time.time + 1f;
								pickUpInHands = true;
							} 
						}
					}
					//when picking up objects to carry
				}
			}
		}


	}
	

	/// <summary>
	/// Call this to spawn blood and some sound if the player is damaged!
	/// </summary>
	public void DamagePlayer () {
		playerAudioAttack.Play ();
		//blood particle
		GameObject weaponBloodParticleClone = Instantiate (weaponBloodParticle, transform.position, transform.rotation) as GameObject;
		//weaponBloodParticleClone.AddComponent<KillParticle> (); //script to destroy the particle after it is played
		weaponBloodParticleClone.GetComponent<ParticleSystem> ().Play ();
        Destroy(weaponBloodParticleClone, 2);
		//blood particles
	}


	void Attack (bool isRanged)//Ray ray, RaycastHit hit, bool isRanged)
	{
		Ray ray = cam.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		//check if the fired weapon is melee
		if (!isRanged) { 
					//play slash particle
			if (Physics.Raycast (ray, out hit, 10.0f)) {
				EnemyHP enemy = hit.collider.GetComponent<EnemyHP> ();
				if (hit.collider.tag == "Enemy" && hit.collider.isTrigger == false) {
					if (enemy != null) {
						if (!enemy.isDead) {
							//if (enemy.hpMonster <= playerStats.playerDamage)
								//enemy.Dismember (ray.direction * playerStats.rangedImpulse);
						slashParticle.Play (); 					
						enemy.audioMonsterIsHit.Play ();
						enemy.hpMonster -= playerStats.playerDamage; //-hp form moster
						}

					}
					//bullet hole decal
					//Instantiate (splashBloodDecal, hit.point, Quaternion.FromToRotation (Vector3.back, hit.normal), hit.collider.transform);
							
					GameObject weaponBloodParticleClone;
					weaponBloodParticleClone = Instantiate (weaponBloodParticle, hit.point, hit.transform.rotation) as GameObject;
					weaponBloodParticleClone.AddComponent<KillParticle> ();
					weaponBloodParticleClone.GetComponent<ParticleSystem> ().Play ();

					float randBlood = Random.Range (0, 100);
					if (randBlood > 20) {
						GameObject additionalBlood = Instantiate (weaponBloodParticleAdditional, hit.point, hit.transform.rotation);
						additionalBlood.AddComponent<KillParticle> (); //script to destroy the particle after it is played
						additionalBlood.GetComponent<ParticleSystem> ().Play ();
					}

					hit.collider.GetComponent<Rigidbody> ().AddForce (ray.direction * playerStats.playerDamageImpule, ForceMode.Impulse); //add force to monster
					playerAudioAttack.Play (); //play hit sound

				}
			}
		}
		//check if the fired weapon is ranged
		if (isRanged) 
        { 
           
           
			//begin the attack from firing the bullet (currently no bullet). play fire sound, play fire particle, consume ammo
			playerStats.totalAmmo -= playerStats.ammoConsume; //consume ammo!
			playerStats.currAmmo -= 1; //consume 1 ammo from weapon's clip
			audioSourceRangeHit.clip = playerStats.rangedFireSound; //get the sound of weapon
			audioSourceRangeHit.Play (); //play weapon fire sount
			//playerAudioAttack.Play (); //play hit sound

			//particle of fire the weapon. clone on the first person character camera.
			GameObject firingParticleClone;
			firingParticleClone = Instantiate (weaponFireParticle, gunBarrel.transform.position, gunBarrel.transform.rotation) as GameObject;
			firingParticleClone.AddComponent<KillParticle> ();
			firingParticleClone.GetComponent<ParticleSystem> ().Play ();
			//particle of fire the weapon. clone on the first person character camera.

			Vector3 rayOrigin = cam.ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, 0)); //the point in the center of the sceen (of the camera)
			if (Physics.Raycast (rayOrigin, cam.transform.forward, out hit, playerStats.rangedWeaponRange)) {
				//Debug.Log (hit.collider.name);
				EnemyHP enemy = hit.collider.GetComponent<EnemyHP> ();
				if (hit.collider.isTrigger == false) { //check if hit collider is not trigger
					if (hit.collider.GetComponent<Rigidbody> () != null) {
						hit.collider.GetComponent<Rigidbody> ().AddForce (ray.direction * playerStats.rangedImpulse, ForceMode.Impulse); //add force to things that has rigidbody
					}
					//hit particle clone (explosion where the bullet hits)
					GameObject weaponHitParticleClone;
					weaponHitParticleClone = Instantiate (weaponHitParticle, hit.point, Quaternion.FromToRotation (Vector3.back, hit.normal)) as GameObject; //
					weaponHitParticleClone.AddComponent<KillParticle> (); //script to destroy the particle after it is played
					weaponHitParticleClone.GetComponent<ParticleSystem> ().Play ();

					//additional hit particle
					int randParticleNumber = Random.Range (0, 2);
					int rand = Random.Range (0, 100);

					if (rand > 55) {
						GameObject additionalHitParticle = Instantiate (hitParticles [randParticleNumber], hit.point, hit.transform.rotation) as GameObject;
						additionalHitParticle.AddComponent<KillParticle> ();
						additionalHitParticle.GetComponent<ParticleSystem> ().Play ();
					}
					//if you hit shield the sparks will fly!!
					if (hit.collider.gameObject.layer == 15) {			//check if the layer is Shields (which is used to ignore shelds on enemies
						if (hit.collider.GetComponent<DestructionPart>() != null) {
							hit.collider.GetComponent<DestructionPart> ().hp -= playerStats.rangedDamage;
						}
						GameObject sparkClone;
						sparkClone = Instantiate (hitParticles[2], hit.point, hit.transform.rotation) as GameObject;
						sparkClone.GetComponent<ParticleSystem> ().Play ();
						Destroy (sparkClone, 3);
					}

					//if hit collider is monster
					if (hit.collider.tag == "Enemy") {
						if (enemy != null) {
							if (!enemy.isDead) {
								enemy.audioMonsterIsHit.Play (); //play audio of monster pain
								enemy.followPlayer = true; //monster starts following the player if it is hit.
								enemy.hpMonster -= playerStats.rangedDamage; //-hp form moster
							} 
						}

						//blood particle
						GameObject weaponBloodParticleClone;
						weaponBloodParticleClone = Instantiate (weaponBloodParticle, hit.point, hit.transform.rotation) as GameObject;
						weaponBloodParticleClone.AddComponent<KillParticle> (); //script to destroy the particle after it is played
						weaponBloodParticleClone.GetComponent<ParticleSystem> ().Play ();

						float randBlood = Random.Range (0, 100);
						if (randBlood > 60) {
							GameObject additionalBlood = Instantiate (weaponBloodParticleAdditional, hit.point, hit.transform.rotation);
							additionalBlood.AddComponent<KillParticle> (); //script to destroy the particle after it is played
							additionalBlood.GetComponent<ParticleSystem> ().Play ();
						}
						//blood particle				
					}
				}
			}
            fpsWeapon.recoil += 0.1f;
            fpsWeapon.Recoil (playerStats.weaponShakeCameraAmount);
		}
		}

	//called when player is dead (hp = 0)
	void PlayerDies () {
		GetComponent<FirstPersonController> ().enabled = false;
		isDead = true;
		GameObject deathMsg = Instantiate (deathMessage, null);
		//deathMsg.transform.parent = null;
	}

	public void PlayerRespawnAtCube() {
		respawnTimes++;

		GetComponent<FirstPersonController> ().enabled = true;

		//GetComponentInChildren<NoiseAndGrain> ().intensityMultiplier = respawnTimes;
		isDead = false;

		saveCube.GetComponent<SaveCube> ().Respawn (); //get the method from cube (will restore some HP and teleport to cube)

	}

	public void PlayerRespawnNoCube() {
		respawnTimes++;

		GetComponent<FirstPersonController> ().enabled = true;

		//GetComponentInChildren<NoiseAndGrain> ().intensityMultiplier = respawnTimes;
		isDead = false;

		saveCube.GetComponent<SaveCube> ().RespawnNoCube (playerStats.startingLevelPosition); //get the method from cube (will restore some HP and teleport to cube)

	}

	/*
	void FallDamage (float startJump, float endJump) {
		startJump = transform.position.y;
		endJump = transform.position.y;
		if ((startJump - endJump) >= 10) {
			playerStats.PlayerHPChange (startJump - endJump);
		}
	}

	*/
}




//====================================OLD CODE. UNUSED===========================

//bullet hole decal
//Instantiate (bulletHoleDecal, hit.point, Quaternion.FromToRotation (Vector3.back, hit.normal), hit.collider.transform);
//blood particle



//InventoryScript.ClearInventory ();
//playerStats.playerGOLD = 0;
//dieWait = Time.time + 3;

//GetComponentInChildren<NoiseAndGrain> ().intensityMultiplier = 10;
//GameObject.Find ("PlayerHPText").GetComponent<Text> ().text = "Fuck this shit.";
//GetComponent<PlayerConversations> ().TextChange ("dead");
/*disable all music. NO	
		foreach (GameObject musicSource in music) {
				musicSource.GetComponent<AudioSource> ().Stop ();
			}
		*/


/*
			foreach (Collider coll in Overlap()) {
				Debug.Log (coll.name);
			}
	
			//Debug.Log (IsGrounded());
			if (!fpsController.m_Jumping && jumpedOnce && fpsController.m_CharacterController.isGrounded)
			{
				timeOfLand = Time.time;
				jumpedOnce = false;
				if (timeOfLand - timeOfJump >= fallTimeToGetDamage) { //set here the amount of time it is okay to be in the air
					playerAudioAttack.Play ();
					//blood particle
					GameObject weaponBloodParticleClone;
					weaponBloodParticleClone = Instantiate (weaponBloodParticle, transform.position, transform.rotation) as GameObject;
					weaponBloodParticleClone.AddComponent<KillParticle> (); //script to destroy the particle after it is played
					weaponBloodParticleClone.GetComponent<ParticleSystem> ().Play ();
					//blood particle
					playerStats.playerHP -= ((timeOfLand - timeOfJump) * fallDamage);
					Debug.Log ("hit");
				}
				//landedOnce = true;
				//Debug.Log ("Land");
			}

			if (fpsController.m_Jumping || !fpsController.m_CharacterController.isGrounded) 
			{
				//if (!fpsController.m_CharacterController.isGrounded) {
				if (!jumpedOnce) {		
					timeOfJump = Time.time;				
					jumpedOnce = true;
					Debug.Log ("Jump");
				}
			}
			//if (fpsController.m_CharacterController.isGrounded && jumpedOnce) {

			*/

/*			old interface!!!
hudText.text = "HP: " + Mathf.Round (playerStats.playerHP) +
	"|||ARMOR: " + playerStats.playerArmor +				 
	//"|||Heal Rate (per monster kill): " + playerStats.playerHealingRate +				 
	"|||Damage: " +	Mathf.Round (playerStats.playerDamage) + 
	"|||Force: " + Mathf.Round(playerStats.playerDamageImpule * 100f) / 100f + 
	"|||EXP: " + playerStats.playerEXP + 
	"|||Total Ammo: " + playerStats.totalAmmo + " |||F1 to get to game menu.";

if (playerStats.currAmmo > 0) {
	ammoText.text = "Ammo: " + playerStats.currAmmo + "/" + playerStats.totalWeaponAmmo;
}
if (playerStats.currAmmo <= 0) {
	ammoText.text = "No ammo in weapon!";
}
*/
/*if (isDead) {
if (Time.time > dieWait) {
	if (Input.GetButton ("Fire1")) {
		SceneManager.LoadScene ("MainMenu");
	}
}

}*/

//	}
