using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFootsteps : MonoBehaviour {

    public AudioClip[] footstepSounds;
    public float minDistance;
    public float maxDistance;
    public float waitBetweenSteps;
    private AudioSource footstepAudioSource;

    private EnemyHP enemyHP;

	// Use this for initialization
	void Start () {

        enemyHP = GetComponent<EnemyHP>();

        if (minDistance == 0)
        {
            minDistance = 5;

        }

        if (maxDistance == 0)
        {
            maxDistance = 80;
        }
        footstepAudioSource = gameObject.AddComponent<AudioSource>();
        footstepAudioSource.spread = 180;
        footstepAudioSource.volume = 0.5f;
        footstepAudioSource.spatialBlend = 1;
        footstepAudioSource.minDistance = minDistance;
        footstepAudioSource.maxDistance = maxDistance;
        footstepAudioSource.rolloffMode = AudioRolloffMode.Logarithmic;

        StartCoroutine(PlayFootStepSound());

	}
	


    private IEnumerator PlayFootStepSound ()
    {
        while (!enemyHP.isDead)
        {
            yield return new WaitForSeconds(waitBetweenSteps);
            if (enemyHP.followPlayer)
            {
                
                int n = Random.Range(1, footstepSounds.Length); 
                footstepAudioSource.clip = footstepSounds[n];
                footstepAudioSource.PlayOneShot(footstepAudioSource.clip);
                footstepSounds[n] = footstepSounds[0];
                footstepSounds[0] = footstepAudioSource.clip;

            }
        }
    }

}
