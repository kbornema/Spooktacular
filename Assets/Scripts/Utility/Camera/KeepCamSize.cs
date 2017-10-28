using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera)), ExecuteInEditMode]
public class KeepCamSize : MonoBehaviour 
{
    [SerializeField]
    private Camera myCam;

    [SerializeField]
    private Camera trackedCam;

    private void Start()
    {
        if (!myCam)
            myCam = GetComponent<Camera>();
    }

	// Update is called once per frame
    private void LateUpdate() 
    {
        if (myCam.orthographicSize != trackedCam.orthographicSize)
            myCam.orthographicSize = trackedCam.orthographicSize;
	}
}
