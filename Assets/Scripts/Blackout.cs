using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackout : MonoBehaviour {

    public GameObject[] blackoutGroups;
    public GameObject MainCamera;
    public float Frequency = 12;
    public float LightBackTime = 15;
    public float volume = 0.5f;

    private float t0;
    private bool islighton;

    [SerializeField]
    private float NightCurve = 0;

    private BlackOutTargets[] currentBlackedOut;

    [SerializeField]
    private AudioClip _blackOutAudio;

	// Use this for initialization
	void Start ()
    {
        islighton = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
        NightCurve = MainCamera.GetComponent<ScreenLighting>().ReadNightCurve;

        if (islighton && (int)Time.time % Frequency == 0 && NightCurve >= 0.55f)
        {
            
            int var = Random.Range(0, blackoutGroups.Length);
            currentBlackedOut = blackoutGroups[var].GetComponentsInChildren<BlackOutTargets>();
            SetEnablesBlackout(currentBlackedOut, false);

            t0 = Time.time;
            islighton = false;

            if(_blackOutAudio)
                SoundManager.Instance.playAndDestroy(_blackOutAudio, volume);
        }

        if (!islighton && Time.time - t0 > LightBackTime)
        {
            SetEnablesBlackout(currentBlackedOut, true);
            t0 = 0;
            islighton = true;
        }


		
	}

    private void SetEnablesBlackout(BlackOutTargets[] targets,  bool val)
    {
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i].EnableTarget(val);   
        }
    }
}
