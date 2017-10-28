using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackout : MonoBehaviour {

    public GameObject[] GroundLight;
    public float Frequency = 10;
    public float LightBackTime = 15;

    private float t0;
    private bool islighton;
    private List<GameObject> lightsDown;

	// Use this for initialization
	void Start ()
    {
        islighton = true;
        lightsDown = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (islighton && (int)Time.time % Frequency == 0)
        {
            
            int var = Random.Range(0, GroundLight.Length);
            GroundLight[var].SetActive(false);
            lightsDown.Add(GroundLight[var]);
            Debug.Log("Lights Out! " + var);
            t0 = Time.time;
            islighton = false;
        }

        if (!islighton && Time.time - t0 > LightBackTime)
        {
            foreach (GameObject l in lightsDown)
            {
                l.SetActive(true);
            }
            t0 = 0;
            islighton = true;
        }


		
	}
}
