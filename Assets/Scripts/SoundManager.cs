using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : AManager<SoundManager> {

    AudioSource audioPlayer;
    protected override void OnAwake()
    {
        //Do Nothing
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public AudioSource getPlayer(AudioClip soundFile)
    {
        AudioSource audioPlayer = gameObject.AddComponent<AudioSource>();
        audioPlayer.clip = soundFile;
        audioPlayer.volume = 1f;

        return audioPlayer; 
    }

    public AudioSource getPitchedPlayer(AudioClip soundFile, float lowPitchRange, float highPitchRange)
    {
        AudioSource audioPlayer = getPlayer(soundFile);
        audioPlayer.pitch = Random.Range(lowPitchRange, highPitchRange);

        return audioPlayer;
    }

    public void returnSource(AudioSource source)
    {
        Destroy(source);
    }

    public void playAndDestroy(AudioClip soundFile, float volume)
    {
        AudioSource audioPlayer = getPlayer(soundFile);
        audioPlayer.volume = volume;
        StartCoroutine(PlaySound(audioPlayer));
    }

    IEnumerator PlaySound(AudioSource audioSource)
    {
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        returnSource(audioSource);
    }
}