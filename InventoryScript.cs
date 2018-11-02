using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class InventoryScript {

	public static ArrayList weapons = new ArrayList();
	public static ArrayList rangedWeapons = new ArrayList();
	//public static List<WeaponScript> weaponListMelee = new List<WeaponScript>();
	//public static List<WeaponScript> weaponListRanged = new List<WeaponScript>();
	public static SavePlayerStats playerStats;
	public static int weaponNumber;
	public static int rangedWeaponNumber;
	public static RayCastScript player;
	// Use this for initialization
	public static void Start () {
		//weapons = new ArrayList();

	}

	public static void SelectWeapon (int number) {  // select specific weapon by its index
		
		if (weapons.Count != 0) {
		playerStats = GameObject.Find ("SavePlayerStats").GetComponent<SavePlayerStats> ();
		weaponNumber = number;
		if (weaponNumber >= weapons.Count) {
			weaponNumber = 0;
		}
		WeaponScript currWeapon = weapons[weaponNumber] as WeaponScript;
			playerStats.WeaponPickUp (currWeapon);
	}
		//ChangeHudText ();

	}
	public static void SelectNextWeapon() {  // select next weapon
		SelectWeapon(weaponNumber+1);
		//ChangeHudText ();
	}

	public static void SelectWeaponRanged (int number) {		
		
		if (rangedWeapons.Count != 0) {
			playerStats = GameObject.Find ("SavePlayerStats").GetComponent<SavePlayerStats> ();
			rangedWeaponNumber = number;
			if (rangedWeaponNumber >= rangedWeapons.Count) {
				rangedWeaponNumber = 0;
			}
			WeaponScript currWeapon = rangedWeapons[rangedWeaponNumber] as WeaponScript;
			//playerStats.rangedWeapons.Add (currWeapon);
			playerStats.WeaponPickUp (currWeapon);
	}
		//ChangeHudText ();

	}
	public static void SelectNextWeaponRanged() {  // select next weapon
		SelectWeaponRanged(rangedWeaponNumber+1);
		//ChangeHudText ();
	}

	public static void ChangeHudText () {
		playerStats = GameObject.Find ("SavePlayerStats").GetComponent<SavePlayerStats> ();
		player = GameObject.FindWithTag ("Player").GetComponent<RayCastScript>(); //get player main script

		//change hud text with the info on the current RANGED weapon
//		player.rangedWeaponText.text = playerStats.rangedWeaponName + "\n" + "Damage: " + playerStats.rangedDamage + "\n" + "Reload Time: " + playerStats.rangedAttackSpeed + "\n"
//			+ "Range: " + playerStats.rangedWeaponRange + "\n" + "Weapon Power: " + playerStats.rangedImpulse + "\n" + "Ammo Consume: " + playerStats.ammoConsume;
		
		//change hud text with the info on the current MELEE weapon
//		player.meleeWeaponText.text = "'" + playerStats.weaponName + "'" + "\n" + " Damage: " + playerStats.weaponDamage + "\n" + " AttackSpeed: " + playerStats.weaponAttackSpeed + 
//			"\n" + "Weapon Power: " + playerStats.weaponImpulse; 
	}

	public static void ClearInventory () {		
		weapons.Clear ();
		rangedWeapons.Clear ();
		weapons = new ArrayList ();
		rangedWeapons = new ArrayList ();
	}
	public static void LoadWeaponsFromSave () {
		playerStats = GameObject.Find ("SavePlayerStats").GetComponent<SavePlayerStats> ();
		rangedWeapons = playerStats.rangedWeapons;
	}

	public static void AddWeaponsToList () {
		playerStats = GameObject.Find ("SavePlayerStats").GetComponent<SavePlayerStats> ();
		playerStats.rangedWeapons = rangedWeapons;
	}

    public static void SelectBestWeapon () 
    {
        if (rangedWeapons.Count != 0)
        {
            float dps = 0;
            //Debug.Log("best weapon finding...");
            foreach (WeaponScript weapon in rangedWeapons)
            {
                if (dps < weapon.weaponUpgradeMultiplier)
                {
                    dps = weapon.weaponUpgradeMultiplier;
                   // Debug.Log(dps);
                }
            }
            //Debug.Log(dps + " Best one");
            foreach (WeaponScript weapon in rangedWeapons)
            {
               // Debug.Log(weapon.weaponUpgradeMultiplier + "   " + dps);
                if (dps == weapon.weaponUpgradeMultiplier)
                {
                   // Debug.Log(rangedWeapons.IndexOf(weapon));
                    SelectWeaponRanged(rangedWeapons.IndexOf(weapon));
                }
            }
        }
    }
		
}
