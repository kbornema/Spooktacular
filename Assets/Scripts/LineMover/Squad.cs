using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad : MonoBehaviour {

    [SerializeField]
    List<WayPoint> currentPath;
    [SerializeField]
    WayPoint currentPoint;

    public WayPoint getCurrentPoint()
    {
        return currentPoint;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
