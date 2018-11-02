using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using UnityEngine.SceneManagement;

public class HomeManager : MonoBehaviour {
	public GameObject sun;
	public GameObject bad;
	public GameObject guideOne;
	public GameObject guideTwo;
	public ParticleSystem particleSys;
	public AudioSource dayMusic;
	public AudioSource nightMusic;

	private Light sunLight;
	private bool badOnce;							//just to make 1 script
	private bool musicChanged;
	public bool night;								//if true the night is here (light = 0)

	void Start () {
		sunLight = sun.GetComponent<Light> ();	
	}
	
	// Update is called once per frame
	void Update () {

		if (night && !musicChanged) {
			dayMusic.Stop ();
			nightMusic.Play ();
			musicChanged = true;
		}
		if (!night && !musicChanged) {			
			nightMusic.Stop ();
			dayMusic.Play ();
			musicChanged = true;
		}



		if (DialogueManager.IsConversationActive) {
			//Bad. Player may sleep here. The effects are - more particles flying, change the light
			if (DialogueManager.CurrentConversant == bad.transform) {
				if (DialogueManager.CurrentConversationState.subtitle.dialogueEntry.id == 5) {
					if (!badOnce) {
						var em = particleSys.emission;
						//change the night to day or to night
						night = !night;
						//at night light = 0
						if (night) {
							em.rateOverTime = 500;
							sunLight.intensity = 0f;
						}
						//at day light = 1.5
						if (!night) {
							em.rateOverTime = 150;
							sunLight.intensity = 1.5f;
						}
						//make longer fadein
						GetComponent<FadeIn> ().fadeSpeed = 0.01f;
						//make fade in on sleep
						GetComponent<FadeIn> ().MakeFadeIn ();
						badOnce = true;
						musicChanged = false;
                        Destroy(guideOne);
					}
				}
			}
			//Bad

			//Second Guide
			if (DialogueManager.CurrentConversant == guideTwo.transform) {
				if (DialogueManager.CurrentConversationState.subtitle.dialogueEntry.id == 2) {
					SceneManager.LoadScene ("LoadingScreen");
                    LoadingScreenLogic.sceneName = "World05(Storm)";
				}
			}
			//Bad
		}
	}

	void OnConversationEnd () {
		badOnce = false;
	}
}
