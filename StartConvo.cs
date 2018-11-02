using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PixelCrushers.DialogueSystem;

public class StartConvo : MonoBehaviour {
	

    [Header("Conversation stuff")]
    [Tooltip("Place camera here to activate it during the conversation")]
    public GameObject convoCameraToActivate;
	[Tooltip("The name of the convo in dialogue manager to start")]
	public string convoName;
	[Tooltip("If checked, convo will start only once. You will not be able to start it again after the end")]
	public bool once;
    public bool secondConvo;
    public string secondConvoName;

    [Header ("Music stuff")]
	public bool changeMusicOnStart;
	public bool changeMusicOnEnd;
	public GameObject musicToChange;
    [Tooltip("the music for the conversation. It will play if no other music is playing")]
    public bool additionalMusicForConvo;  
    public GameObject additionalConvoMusic;
    public bool stopAdditionalMusicOnEnd;

    [Header("Change Scene stuff")]
    public bool changeSceneOnEnd;
    public bool useLoadingScreen;
    public string sceneToChange;

    private bool okayToPlayAdditionalMusic;
	private bool secondConvoStart;
	private bool convoOnce;
	private GameObject player;
	private Camera cam;
	private GameObject fpsCharacter;
	private bool useCamera;
	private SavePlayerStats playerStats;
	private GameObject[] otherMusic;

	public bool startThisConvo;

    public float iD;

    private SaveManager saveManager;


    void Awake () {
        iD = (1000 * transform.position.x) + transform.position.y + (0.001f * transform.position.z);
    }

	// Use this for initialization
	void Start () {

       

		playerStats = GameObject.Find ("SavePlayerStats").GetComponent<SavePlayerStats>();
		
        saveManager = GameObject.Find ("SavePlayerStats").GetComponent<SaveManager>();

		if (convoCameraToActivate != null) {
			useCamera = true;
		}
		fpsCharacter = GameObject.Find ("FirstPersonCharacter");
		player = GameObject.FindWithTag ("Player");
		cam = Camera.main;	
	}
	
	// Update is called once per frame
	void Update () {

		if (secondConvo && secondConvoStart && !DialogueManager.IsConversationActive) {
			if (playerStats != null && !playerStats.isPause && !playerStats.readingNote) {
				if (Input.GetButtonDown ("Fire1")) {
					Ray ray = cam.ScreenPointToRay (Input.mousePosition);
					RaycastHit hit;
					if (Physics.Raycast (ray, out hit, 10f)) {
                        if (playerStats.playerHP > 0)
                        {
                            if (hit.collider.gameObject == gameObject)
                            {	

                                DialogueManager.StartConversation(secondConvoName, player.transform, transform);

                            }
                        }
					}
				}
			}
		}
		
		if (!convoOnce && !DialogueManager.IsConversationActive) {
			if (playerStats != null && !playerStats.isPause && !playerStats.readingNote) {
				if (startThisConvo) {
					DialogueManager.StartConversation (convoName, player.transform, transform);
					startThisConvo = false;
				}

				if (Input.GetButtonDown ("Fire1")) {
					Ray ray = cam.ScreenPointToRay (Input.mousePosition);
					RaycastHit hit;
					if (Physics.Raycast (ray, out hit, 10f)) {
                        if (hit.collider.gameObject == gameObject)
                        {
                            if (playerStats.playerHP > 0)
                            {
                                if (once)
                                {
                                    convoOnce = true;
                                }
                                if (secondConvo)
                                {
                                    secondConvoStart = true;
                                }
                                DialogueManager.StartConversation(convoName, player.transform, transform);

                                if (once)
                                {
                                    saveManager.AddID(iD);
                                    //Debug.Log("convo added " + iD);
                                }

                            }
                        }
					}
				}
			}

		}
}
	void OnConversationStart (Transform actor) {
        //find all music
        otherMusic = GameObject.FindGameObjectsWithTag("Music");
       
        //play convo music (dont use it with start music)
        if (additionalMusicForConvo)
        {
            okayToPlayAdditionalMusic = true;
            foreach (GameObject other in otherMusic) {
                
                if (other.GetComponent<AudioSource>().isPlaying)
                {
                    okayToPlayAdditionalMusic = false;
                }
            }
            if (okayToPlayAdditionalMusic)
            {
                additionalConvoMusic.GetComponent<AudioSource>().Play();
            }

        }

		if (changeMusicOnStart) {
			foreach (GameObject other in otherMusic) {
				other.GetComponent<AudioSource>().Stop ();
			} 
			musicToChange.GetComponent<AudioSource> ().Play ();
		}
			if (useCamera) {
				fpsCharacter.SetActive (false);
				convoCameraToActivate.SetActive (true);
			}

	}
	void OnConversationEnd (Transform actor) {
		
        if (stopAdditionalMusicOnEnd)
        {
            additionalConvoMusic.GetComponent<AudioSource>().Stop();
        }

		if (changeMusicOnEnd) {
			foreach (GameObject other in otherMusic) {
				other.GetComponent<AudioSource>().Stop ();
			} 
			musicToChange.GetComponent<AudioSource> ().Play ();
		}
		if (useCamera) {
			convoCameraToActivate.SetActive (false);
			fpsCharacter.SetActive (true);
		}
		if (changeSceneOnEnd) {
            if (useLoadingScreen)
            {
                LoadingScreenLogic.sceneName = sceneToChange;
                SceneManager.LoadScene("LoadingScreen");
            }
            else
                SceneManager.LoadScene(sceneToChange);
		}
		//if once is set use onlyOnce to tell script that the convo can be started once

	}
}
