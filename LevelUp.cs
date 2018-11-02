using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUp : MonoBehaviour {
	private bool start;
	[Header("Text UI")]
	public Text infoText;
	public Text ammoBonus;
	public Text soulBonus;
	public Text dropChance;
	public Text armorBonus;
	public Text healBonus;
	public Text maxHPBonus;
	public Text helpText;

	/*
	[Header ("Bonus Prices")]
	public float ammoPrice;
	public float soulPrice;
	public float dropPrice;
	public float armorPrice;
	public float healPrice;
	public float maxHPPrice;
	*/

	[Header("Bonus EXP cost")]
	public float ammoEXPCost;
	public float soulEXPCost;
	public float dropEXPCost;
	public float armorEXPCost;
	public float healEXPCost;
	public float maxHPEXPCost;

	private SavePlayerStats playerStats;
	private float allCost;

	void Start() {
		
		playerStats = GameObject.Find ("SavePlayerStats").GetComponent<SavePlayerStats> ();
		UpdatePrices ();
		RoundPrices ();
		//Debug.Log ("start");

	}



	void Update() {
		//Debug.Log ("test");
		if (gameObject.activeInHierarchy) {
			if (playerStats.isPause) {
				
				infoText.text = string.Format ("EXP cap: {0}\nAmmo Bonus: {1}%\nSoul Bonus: {2}%\nDrop Chance: {3}%\nArmor Bonus: {4}%\nHeal Bonus: {5}%\nMax HP: {6}\nLevel: {7}", 
					playerStats.playerCurrentEXP, Mathf.Round ((playerStats.ammoBonus - 1) * 10000) / 100, Mathf.Round ((playerStats.soulBonus - 1) * 10000) / 100,	Mathf.Round ((playerStats.dropChance - 1) * 10000) / 100, Mathf.Round ((playerStats.armorBonus - 1) * 10000) / 100, Mathf.Round ((playerStats.healBonus - 1) * 10000) / 100, playerStats.maxHP, playerStats.level);

				maxHPBonus.text = string.Format ("+4 Max HP ({0}EXP)", maxHPEXPCost);
				healBonus.text = string.Format ("+2% Heal Rate ({0}EXP)", healEXPCost);
				soulBonus.text = string.Format ("+3% More Souls ({0}EXP)", soulEXPCost);
				dropChance.text = string.Format ("+1.5% Drop Chance ({0}EXP)", dropEXPCost);
				ammoBonus.text = string.Format ("+3% More Ammo ({0}EXP)", ammoEXPCost);
				armorBonus.text = string.Format ("+4% Armor Bonus ({0}EXP)", armorEXPCost);

				//enable buttons

                //AMMO
                if (playerStats.playerCurrentEXP < ammoEXPCost)
                {
                    ColorBlock cb = ammoBonus.GetComponentInParent<Button>().colors;
                    cb.normalColor = Color.black;
                    ammoBonus.GetComponentInParent<Button>().colors = cb;      
                    ammoBonus.color = Color.red;
                }
                else
                {
                    ColorBlock cb = ammoBonus.GetComponentInParent<Button>().colors;
                    cb.normalColor = Color.green;
                    ammoBonus.GetComponentInParent<Button>().colors = cb;
                    ammoBonus.color = Color.green;
                }
                   
                //SOULS
                if (playerStats.playerCurrentEXP < soulEXPCost)
                {
                    ColorBlock cb = soulBonus.GetComponentInParent<Button>().colors;
                    cb.normalColor = Color.black;
                    soulBonus.GetComponentInParent<Button>().colors = cb;
                    soulBonus.color = Color.red;
                    //playerStats.eventSystem.SetSelectedGameObject(closeButton);
                }
                else
                {
                    ColorBlock cb = soulBonus.GetComponentInParent<Button>().colors;
                    cb.normalColor = Color.green;
                    soulBonus.GetComponentInParent<Button>().colors = cb;
                    soulBonus.color = Color.green;
                }

                //DROP
				if (playerStats.playerCurrentEXP < dropEXPCost)
                {
                    ColorBlock cb = dropChance.GetComponentInParent<Button>().colors;
                    cb.normalColor = Color.black;
                    dropChance.GetComponentInParent<Button>().colors = cb;
                    dropChance.color = Color.red;
                    //playerStats.eventSystem.SetSelectedGameObject(closeButton);
                }
                else
                {
                    ColorBlock cb = dropChance.GetComponentInParent<Button>().colors;
                    cb.normalColor = Color.green;
                    dropChance.GetComponentInParent<Button>().colors = cb;
                    dropChance.color = Color.green;
                }
                   
                //ARMOR
				if (playerStats.playerCurrentEXP < armorEXPCost)
                {
                    ColorBlock cb = armorBonus.GetComponentInParent<Button>().colors;
                    cb.normalColor = Color.black;
                    armorBonus.GetComponentInParent<Button>().colors = cb;
                    armorBonus.color = Color.red;
                   
                }
                else
                {
                    ColorBlock cb = armorBonus.GetComponentInParent<Button>().colors;
                    cb.normalColor = Color.green;
                    armorBonus.GetComponentInParent<Button>().colors = cb;
                    armorBonus.color = Color.green;
                }
					
                //HEAL
				if (playerStats.playerCurrentEXP < healEXPCost)
                {
                    ColorBlock cb = healBonus.GetComponentInParent<Button>().colors;
                    cb.normalColor = Color.black;
                    healBonus.GetComponentInParent<Button>().colors = cb;
                    healBonus.color = Color.red;
                    //playerStats.eventSystem.SetSelectedGameObject(closeButton);
                }
                else
                {
                    ColorBlock cb = healBonus.GetComponentInParent<Button>().colors;
                    cb.normalColor = Color.green;
                    healBonus.GetComponentInParent<Button>().colors = cb;
                    healBonus.color = Color.green;
                }    
					
                //MAX HP
				if (playerStats.playerCurrentEXP < maxHPEXPCost)
                {
                    ColorBlock cb = maxHPBonus.GetComponentInParent<Button>().colors;
                    cb.normalColor = Color.black;
                    maxHPBonus.GetComponentInParent<Button>().colors = cb;
                    maxHPBonus.color = Color.red;
                    //playerStats.eventSystem.SetSelectedGameObject(closeButton);
                }
                else
                {
                    ColorBlock cb = maxHPBonus.GetComponentInParent<Button>().colors;
                    cb.normalColor = Color.green;
                    maxHPBonus.GetComponentInParent<Button>().colors = cb;
                    maxHPBonus.color = Color.green;
                }   
					
			} 
		}
	}


	public void UpdatePrices() {
		//allCost = playerStats.ammoBonus + playerStats.soulBonus + playerStats.dropChance + playerStats.armorBonus + playerStats.healBonus + (playerStats.maxHP - 90);

		ammoEXPCost = (0.01f * Mathf.Pow (playerStats.level, 2.5f)) + Mathf.Pow (playerStats.ammoBonus, 10) + 10 * playerStats.level;
		soulEXPCost = (0.01f * Mathf.Pow (playerStats.level, 2.5f)) + Mathf.Pow (playerStats.soulBonus, 10) + 10 * playerStats.level;
		dropEXPCost = (0.01f * Mathf.Pow (playerStats.level, 2.5f)) + Mathf.Pow (playerStats.dropChance, 9) + 10 * playerStats.level;
		armorEXPCost = (0.01f * Mathf.Pow (playerStats.level, 2.5f)) + Mathf.Pow (playerStats.armorBonus, 10) + 10 * playerStats.level;
		healEXPCost = (0.01f * Mathf.Pow (playerStats.level, 2.5f)) + Mathf.Pow (playerStats.healBonus, 10) + 10 * playerStats.level;
		maxHPEXPCost = (0.01f * Mathf.Pow (playerStats.level, 2.5f)) + Mathf.Pow ((playerStats.maxHP - 100) / 10, 4) + 10 * playerStats.level;		//different formula
		/*
		soulEXPCost = 10 + (playerStats.soulBonus * playerStats.soulBonus) * (playerStats.level*((playerStats.level/20) + 1));
		dropEXPCost = 10 + (playerStats.dropChance * playerStats.dropChance) * (playerStats.level*((playerStats.level/20) + 1));
		armorEXPCost = 10 + (playerStats.armorBonus * playerStats.armorBonus) * (playerStats.level*((playerStats.level/20) + 1));
		healEXPCost = 10 + (playerStats.healBonus * playerStats.healBonus) * (playerStats.level*((playerStats.level/20) + 1));
		maxHPEXPCost = 10 + ((playerStats.maxHP-100) * (playerStats.maxHP-100))/100 * (playerStats.level*((playerStats.level/20) + 1));
		*/
	}

	public void RoundPrices() {
		ammoEXPCost = Mathf.Round (ammoEXPCost * 10) / 10;
		soulEXPCost = Mathf.Round (soulEXPCost * 10) / 10;
		dropEXPCost = Mathf.Round (dropEXPCost * 10) / 10;
		armorEXPCost = Mathf.Round (armorEXPCost * 10) / 10;
		healEXPCost = Mathf.Round (healEXPCost * 10) / 10;
		maxHPEXPCost = Mathf.Round (maxHPEXPCost * 10) / 10;
	}

	public void AmmoBonus() {
        if (playerStats.playerCurrentEXP > ammoEXPCost)
        {
            //ammoEXPCost = playerStats.ammoBonus * 1.05f;
            playerStats.level += 1;
            playerStats.ammoBonus += 0.03f;
            UpdatePrices();
            RoundPrices();
            helpText.text = "You will get more ammo from ammo crates";
        }
	}
	public void SoulBonus() {
        if (playerStats.playerCurrentEXP > soulEXPCost)
        {
            //soulEXPCost = playerStats.soulBonus * 1.05f;
            playerStats.level += 1;
            playerStats.soulBonus += 0.03f;
            UpdatePrices();
            RoundPrices();
            helpText.text = "There are more souls dropped from the monsters";
        }
	}
	public void DropChance() {
        if (playerStats.playerCurrentEXP > dropEXPCost)
        {
            //dropEXPCost *= 1.05f;
            playerStats.level += 1;
            playerStats.dropChance += 0.015f;
            UpdatePrices();
            RoundPrices();
            helpText.text = "There is a better chance for monsters to drop ammo or weapon upgrades when killed";
        }
	}
	public void ArmorBonus() {
        if (playerStats.playerCurrentEXP > armorEXPCost)
        {
            //armorEXPCost *= 1.05f;
            playerStats.level += 1;
            playerStats.armorBonus += 0.04f;
            UpdatePrices();
            RoundPrices();
            helpText.text = "You will get more protection from armor items you pick up";
        }
	}
	public void HealBonus() {
        if (playerStats.playerCurrentEXP > healEXPCost)
        {
            //healEXPCost *= 1.05f;
            playerStats.level += 1;
            playerStats.healBonus += 0.02f;
            UpdatePrices();
            RoundPrices();
            helpText.text = "You will receive more health when you kill monsters";
        }
	}
	public void MaxHP() {
        if (playerStats.playerCurrentEXP > maxHPEXPCost)
        {
            //maxHPEXPCost *= 1.05f;
            playerStats.level += 1;
            playerStats.maxHP += 4;
            playerStats.playerHP += 5;
            playerStats.StatsUpdate();
            UpdatePrices();
            RoundPrices();
            helpText.text = "Your maximum HP is increased by 4";
        }
	}

   
}
