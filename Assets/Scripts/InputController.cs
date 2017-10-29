using System;
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
public static class EnumExtension
{
    public static bool areOpposingSides(this DIRECTION first, DIRECTION second) //Lol Brute Force "Logik"
    {
        return first.Equals(DIRECTION.NORTH) && second.Equals(DIRECTION.SOUTH) ||
                first.Equals(DIRECTION.SOUTH) && second.Equals(DIRECTION.NORTH) ||
                first.Equals(DIRECTION.EAST) && second.Equals(DIRECTION.WEST) ||
                first.Equals(DIRECTION.WEST) && second.Equals(DIRECTION.EAST);
    }
}

public class InputController : MonoBehaviour 
{

    [SerializeField]
    float minimumAxisInput;

    Vector2 defaultVector = new Vector2(-1, 1).normalized;

    public bool GetPlayerButtonInput(string buttonName,int playerInputIndex)
    {
        return Input.GetButtonDown(buttonName + "_" + playerInputIndex);
    }

    //TODO add Deadzone
    public DIRECTION GetPlayerDirection(PlayerController playerController)
    {
        int playerInputIndex = playerController.PlayerInputId;
        bool keyboard = playerInputIndex == 4;

        Vector2 inputVector = keyboard ? getKeyboardVector() : GetControllerVector("" + playerInputIndex);
        //Vector2 inputVector = GetControllerVector("" + playerIndex);
        //Debug.Log(inputVector.magnitude);
        if (inputVector.magnitude < minimumAxisInput)
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

    private Vector2 getKeyboardVector()
    {
        Vector2 result = Vector2.zero;

        result = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        return result;
    }

    Vector2 GetControllerVector(string controllerName)
    {
        Vector2 result = Vector2.zero;

        result = new Vector2(Input.GetAxis("X_" + controllerName), Input.GetAxis("Y_" + controllerName)); 

        return result;
    }
}
