using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMover : MonoBehaviour
{
    /*
    [Range(0,0.5f)]
    [SerializeField]
    float directionSwitchThreshhold = 0;

    [Range(0, 5f)]
    [SerializeField]
    float interpolationRate = 0.3f;

    [SerializeField]
    GameObject TrackPrefab;

    int currentPlayer = 0;  //for Test

    int activeSquadId = 0;

    [SerializeField]
    Squad[] squads = new Squad[1];

    [SerializeField]
    InputController controller;
    private bool lineDrawing;
    private Path currentPath;

    private void Start()
    {
        //TODO: get InputController
        //TODO: get Player
        //TODO: who generates Squads ?
        //TODO: switch Squads 
        squads[0] = gameObject.AddComponent<Squad>(); //Test 
    }

    public void switchSquad( int changeByUnits )
    {
        //TODO change By Units
    }

    public void StartLine()
    {
        lineDrawing = true;

        Squad activeSquad = squads[activeSquadId];

        //Create Path for Squad
        currentPath = gameObject.AddComponent<Path>(); //
        //TODO how to get current Waypoint
        currentPath.setTrackPrefab(TrackPrefab);
        currentPath.addWayPoint(GameObject.FindGameObjectWithTag("WayPoint").GetComponent<WayPoint>()); 

        //Reinsert
        //activeSquad.setPath(currentPath);
    }


    // Update is called once per frame
    void Update ()
    {
        /*
        if (!lineDrawing)
        {
            //Start drawing
            if (controller.GetPlayerButtonInput("Button1", currentPlayer))
            {
                StartLine();
            }
            return;
        }

        //Finish Drawing ???

        DIRECTION playerdir = controller.GetPlayerDirection(currentPlayer);
        Squad activeSquad = squads[activeSquadId];

        // Move from current Waypoint into direction if waypoint is available
        WayPoint currentPoint = currentPath.getCurrentPoint();

        //get current direction and interpolation
        //DIRECTION currentDirection = activeSquad.getPath().getCurrentDirection();
        //float currentInterpolation = activeSquad.getPath().getCurrentInterpolation();

        if (currentDirection == DIRECTION.NONE && playerdir != DIRECTION.NONE)
        {
            // Update to next Target
            activeSquad.getPath().setNewTarget(playerdir);
            currentDirection = activeSquad.getPath().getCurrentDirection();
            currentInterpolation = 0f;

            //Debug.Log(currentDirection + " curr dir  + player dir " + playerdir);
        }

        //add/remove from interpolation
        float interpolationUpdate = interpolationRate * Time.deltaTime;
        float newInterpolation = currentInterpolation; //will be changed below

        if(playerdir != DIRECTION.NONE)
        {
            if (currentPath.getCount() > 1 && newInterpolation <= 0f + directionSwitchThreshhold)
            {
                WayPoint prevWaypoint = currentPath.getPreviousPoint();
                var prevDir = currentPoint.getDirection(prevWaypoint);
                if (playerdir.Equals(prevDir))//Oposing directions
                {
                    newInterpolation -= interpolationUpdate;
                    newInterpolation = RemoveWaypoint(newInterpolation);
                    activeSquad.getPath().setCurrentInterploation(newInterpolation);
                    return; //Abort any further changes otherwise also check normal behaviour
                }
            }

            if (playerdir.Equals(currentDirection)) // move to new waypoint or reverse to prev
            {
                //Debug.Log("Forward Movement");
                newInterpolation += interpolationUpdate;
                newInterpolation = AddWaypoint(playerdir, currentPoint, newInterpolation);
            }
            else if (playerdir.areOpposingSides(currentDirection))//Oposing directions
            {
                newInterpolation -= interpolationUpdate;
            }
            //remove waypoints instead of double
            activeSquad.getPath().setCurrentInterploation(newInterpolation);
        }
    }

    //Current Order
    // calc interpolate
    // add new Point
    // use Interpolate
    // No reSize

        //Correct order
        // Calc interploate
        // use Interploate
        // add new oiyy

    private float RemoveWaypoint(float newInterpolation)
    {
        //switch to previous Waypoint
        if (newInterpolation <= 0f + directionSwitchThreshhold) //is clamped
        {
            if (currentPath.getCount()> 0)
            {
                currentPath.removeLast();
                newInterpolation = 0f;
            }
        }

        return newInterpolation;
    }

    private float AddWaypoint(DIRECTION playerdir, WayPoint currentPoint, float newInterpolation)
    {
        if (newInterpolation >= 1f - directionSwitchThreshhold) //is clamped
        {
            WayPoint newWaypoint = currentPoint.getWayPoint(playerdir);
            if (newWaypoint != null)
            {
                Debug.Log("adding " + newWaypoint);
                currentPath.addWayPoint(newWaypoint);
                newInterpolation = 1f;
            }
        }

        return newInterpolation;
        
    }*/
}
