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

    Vector2 GetControllerVector(string controllerName)
    {
        Vector2 result = Vector2.zero;

        result = new Vector2(Input.GetAxis(controllerName + "_X"), Input.GetAxis(controllerName + "_Y")); 

        return result;
    }
}
