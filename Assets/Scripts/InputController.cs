using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DIRECTION
{
    NORTH,
    EAST,
    SOUTH,
    WEST,
    NONE
}

public class InputController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public DIRECTION GetPlayerDirection(int playerIndex)
    {


        return DIRECTION.NONE;
    }
}
