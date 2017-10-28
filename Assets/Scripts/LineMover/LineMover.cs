using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMover : MonoBehaviour
{
    //    //Waypoint Image
    //    //Waypoint Track Image
    //    //
    //    GameObject Waypoint;
    //    GameObject WaypointActive;
    //    GameObject Track;

    [Range(0,0.5f)]
    float directionSwitchThreshhold = 0;

    [Range(0, 5f)]
    [SerializeField]
    float interpolationRate = 0.3f;

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

        // Move from current Waypoint into direction if waypoint is available
        WayPoint currentPoint = activeSquad.getCurrentPoint();

        //get current direction and interpolation
        DIRECTION currentDirection = currentPoint.getCurrentDirection();
        float currentInterpolation = currentPoint.getCurrentInterpolation();

        //add/remove from interpolation
        float interpolationUpdate = interpolationRate * Time.deltaTime;
        float newInterpolation = currentInterpolation; //will be changed below

        if(playerdir.Equals(currentDirection))
            newInterpolation += interpolationUpdate;
        else 
        

        if(currentInterpolation <= directionSwitchThreshhold)
        {
            //switch is allowed
            //what now?
        }

        //

        WayPoint newWaypoint = currentPoint.getWayPoint(playerdir);



        //Check for Backwards way

	}
}
