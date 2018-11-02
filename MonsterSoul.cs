using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSoul : MonoBehaviour {
	public float soulEXP;				//exp from the monster stored in this sphere
	public float soulAmount;			//the amount of souls this souls holds
	private float soulScale;			//the scale of sphere depending on the soul amount
    private MeshRenderer meshRenderer;	

    public Material bossMaterial;
    public Material spawnerMaterial;

	public bool isBoss;
	public bool isSpawner;

	//for save manager
	public bool dropped;
	public float iD;

	void Awake () {
		if (!dropped) {
			iD = (1000 * transform.position.x) + transform.position.y + (0.001f * transform.position.z);
		}
	}

	void Start () {
		//Debug.Log ("soul dropped");
		//soulLight = GetComponentInChildren<Light> ();


		soulScale = 0.8f;
		if (isBoss) {
			soulScale += 0.8f;
            meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.material = bossMaterial;
			//soulLight.color = Color.red;
		}
		if (!isBoss && isSpawner) {
			soulScale += 0.3f;
            meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.material = spawnerMaterial;
			//soulLight.color = Color.magenta;
		}
		//soulLight.intensity = soulScale*2.7f;
		//soulLight.range = soulScale*4f;
		transform.localScale = new Vector3 (soulScale, soulScale, soulScale);

        try 
        {
        Destroy(GetComponentInChildren<ParticleSystem>().gameObject, 10);
        } 
        catch 
        {
        }
	}
}

