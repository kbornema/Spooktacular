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

    [SerializeField]
    string buttonString;

    [SerializeField]
    float minimumAxisInput;

    Vector2 defaultVector = new Vector2(-1, 1).normalized;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

    bool GetPlayerButtonInput(string buttonName,int playerIndex)
    {
        return Input.GetButtonDown(buttonName + "_" + playerIndex);
    }

    public DIRECTION GetPlayerDirection(int playerIndex)
    {
        Vector2 inputVector = GetControllerVector("" + playerIndex);
        if(inputVector.magnitude < minimumAxisInput)
        {
            return DIRECTION.NONE;
        }

        inputVector.Normalize();

        float angleToDefaultVector = Mathf.Atan2(inputVector.y, inputVector.x) * Mathf.Rad2Deg + 180;

        if (angleToDefaultVector < 45 || angleToDefaultVector > 315)
            return DIRECTION.WEST;
        if (angleToDefaultVector < 135)
            return DIRECTION.NORTH;
        if (angleToDefaultVector < 225)
            return DIRECTION.EAST;
        else
            return DIRECTION.SOUTH;
    }

    Vector2 GetControllerVector(string controllerName)
    {
        Vector2 result = Vector2.zero;

        result = new Vector2(Input.GetAxis("X_" + controllerName), Input.GetAxis("Y_" + controllerName)); 

        return result;
    }
}
