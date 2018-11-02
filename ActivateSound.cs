using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSound : MonoBehaviour {
	private AudioSource source;
	public AudioClip clipToPlay;
    public bool showSubtitles;

    public bool dontUseSound;
    [Tooltip("Set time here for how long to show subtitles before destroying. This is used if Dont Use Sound is set")]
    public float timeToShowSubtitle;
    [TextArea(2,6)]
    public string subtitleText;
    private GameObject titleText;

    private GameObject subtitleClone;           //the instantiated subtitle object


    private bool start;

	// Use this for initialization
	void Start () {
        titleText = Resources.Load ("My/Prefabs/TitleText") as GameObject;
		source = GetComponent<AudioSource> ();       
	}	

    void Update () {
      
       
        if (start)
        {
            //if use sound
            if (!dontUseSound)
            {
                if (!source.isPlaying)
                {
                    Destroy(subtitleClone);
                    Destroy(this);
                }
            }
            //if dont use sound - wait for some time before destroying
            else
            {
                Destroy(subtitleClone, timeToShowSubtitle);
                Destroy(this, timeToShowSubtitle);
            }
        }
      

    }


	void OnTriggerEnter (Collider coll) {
        if (!start)
        {
            if (coll.tag == "Player")
            {
                if (showSubtitles)
                {
                    subtitleClone = Instantiate(titleText, transform);
                    subtitleClone.transform.SetParent(null);
                    subtitleClone.GetComponent<RewardText>().ShowSubtitle(subtitleText); 
                    start = true;
                    //Debug.Log("show subs");
                }
                if (clipToPlay != null)
                {
                    source.clip = clipToPlay;
                    source.Play();

                }
                //if there are no subtitles than just destroy this trigger
                if (!start)
                {
                    Destroy(this);
                }
            }
        }
	}
}
