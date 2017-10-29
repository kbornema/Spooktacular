using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackout : MonoBehaviour {

    public GameObject[] blackoutGroups;
    public float Frequency = 10;
    public float LightBackTime = 15;

    private float t0;
    private bool islighton;

    private BlackOutTargets[] currentBlackedOut;

    private AudioClip _blackOutAudio;

	// Use this for initialization
	void Start ()
    {
        islighton = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (islighton && (int)Time.time % Frequency == 0)
        {
            
            int var = Random.Range(0, blackoutGroups.Length);
            currentBlackedOut = blackoutGroups[var].GetComponentsInChildren<BlackOutTargets>();
            SetEnablesBlackout(currentBlackedOut, false);

            t0 = Time.time;
            islighton = false;
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
