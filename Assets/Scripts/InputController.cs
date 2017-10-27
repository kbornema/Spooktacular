using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    enum DIRECTION
    {
        NORTH,
        EAST,
        SOUTH,
        WEST
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    DIRECTION GetPlayerDirection(int playerIndex)
    {


        return DIRECTION.NORTH;
    }
}
