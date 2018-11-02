using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Shop : MonoBehaviour {
	public GameObject shopMenu;
	private SavePlayerStats playerStats;
	private Camera cam;

	private float priceDependingOnLevel;
	private float randomPriceRange;			//the range in % to influence the prices in the shop
	[Header("Initial prices for the shops")]
	public float smallAmmoPrice;			//price for the small ammo
	public float bigAmmoPrice;				//price for big ammo
	public float smallArmorPrice;
	public float bigArmorPrice;
	public float soulPrice;

	private bool visited;					//will not update prices on click

	//[Tooltip("This amount will be added to the weapon upgrade price")]
	//public int weaponUpdatePrice;
	void Awake() {
		
	}
	// Use this for initialization
	void Start () {
		//Debug.Log ("shop ready");
		//smallAmmoPrice = 55;
		playerStats = GameObject.Find ("SavePlayerStats").GetComponent<SavePlayerStats> ();
		//NewPrices ();
		cam = Camera.main;


	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Fire1")) {
			Ray ray = cam.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10f))
            {
                if (playerStats.playerHP > 0)
                {
                    if (hit.collider.gameObject == gameObject)
                    {
                        //if not visited already - update prices
                        if (!visited)
                        {
                            NewPrices();
                        }

                        playerStats.Pause(true);
                        playerStats.isShop = true;	
                        playerStats.currentShop = this;
                        playerStats.eventSystem.SetSelectedGameObject(playerStats.Sell1Soul);
                        visited = true;

                    }
                }
            }
		}
	}

	//new prices will generate the prices for the
	public void NewPrices() {
		priceDependingOnLevel = playerStats.level / 60;
		if (priceDependingOnLevel < 1)
			priceDependingOnLevel = 1;
		//small ammo
		randomPriceRange = Random.Range (0, 160);
		smallAmmoPrice += (smallAmmoPrice * (randomPriceRange / 100)) * priceDependingOnLevel;
		smallAmmoPrice = Mathf.Round (smallAmmoPrice);
		randomPriceRange = 0;
		//big ammo
		randomPriceRange = Random.Range (0, 160);
		bigAmmoPrice += bigAmmoPrice * (randomPriceRange / 100) * priceDependingOnLevel;
		bigAmmoPrice = Mathf.Round (bigAmmoPrice);
		randomPriceRange = 0;
		//small ARMOR
		randomPriceRange = Random.Range (0, 180);
		smallArmorPrice += smallArmorPrice * (randomPriceRange / 100) * priceDependingOnLevel;
		smallArmorPrice = Mathf.Round (smallArmorPrice);
		randomPriceRange = 0;
		//big ARMOR
		randomPriceRange = Random.Range (0, 180);
		bigArmorPrice += bigArmorPrice * (randomPriceRange / 100) * priceDependingOnLevel;
		bigArmorPrice = Mathf.Round (bigArmorPrice);
		randomPriceRange = 0;
		//sell Soul Price
		randomPriceRange = Random.Range (0, 20);
		soulPrice += soulPrice * (randomPriceRange / 100);
		soulPrice = Mathf.Round (soulPrice);
		randomPriceRange = 0;
	}
}
