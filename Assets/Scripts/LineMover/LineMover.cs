using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMover : MonoBehaviour
{
    //Waypoint Image
    //Waypoint Track Image
    //
    GameObject Waypoint;
    GameObject WaypointActive;
    GameObject Track;

    int currentPlayer = 0;  

    int activeSquadId = 0;
    Squad[] squads;

    InputController controller;

    private void Start()
    {
        //TODO: get InputController
        //TODO: get Player
        //TODO: who generates Squads ?
        //TODO: switch Squads 
    }

    public void switchSquad( int changeByUnits )
    {
        //TODO change By Units
    }


    // Update is called once per frame
    void Update ()
    {
        DIRECTION playerdir = controller.GetPlayerDirection(currentPlayer);
        Squad activeSquad = squads[activeSquadId];

        //current Position --> Startpoint
        //
	}
}
