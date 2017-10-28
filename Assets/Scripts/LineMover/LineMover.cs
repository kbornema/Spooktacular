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
    [SerializeField]
    float directionSwitchThreshhold = 0;

    [Range(0, 5f)]
    [SerializeField]
    float interpolationRate = 0.3f;

    int currentPlayer = 0;  //for Test

    int activeSquadId = 0;
    Squad[] squads = new Squad[1];

    LinkedList<WayPoint> currentList = new LinkedList<WayPoint>();

    [SerializeField]
    InputController controller;
    private bool lineDrawing;

    private void Start()
    {
        //TODO: get InputController
        //TODO: get Player
        //TODO: who generates Squads ?
        //TODO: switch Squads 
        squads[0] = gameObject.AddComponent<Squad>(); //Test 
        LinkedList<WayPoint> list = new LinkedList<WayPoint>();
        list.AddFirst(GameObject.FindGameObjectWithTag("WayPoint").GetComponent<WayPoint>());
        squads[0].setPath(list);
    }

    public void switchSquad( int changeByUnits )
    {
        //TODO change By Units
    }

    public void StartLine()
    {
        lineDrawing = true;

        Squad activeSquad = squads[activeSquadId];
        WayPoint currentPoint = (currentList.Count == 0) ? activeSquad.getCurrentPoint() : currentList.Last.Value;

        if (currentPoint != null)
        {
            Debug.Log("adding " + currentPoint);
            currentList.AddLast(currentPoint);
        }
    }


    // Update is called once per frame
    void Update ()
    {
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
        WayPoint currentPoint = (currentList.Count == 0) ? activeSquad.getCurrentPoint() : currentList.Last.Value;

        //get current direction and interpolation
        DIRECTION currentDirection = currentPoint.getCurrentDirection();
        float currentInterpolation = currentPoint.getCurrentInterpolation();

        if (currentDirection == DIRECTION.NONE && playerdir != DIRECTION.NONE)
        {
            currentPoint.updateWaypoint(playerdir);
            currentDirection = currentPoint.getCurrentDirection();
            //Debug.Log(currentDirection + " curr dir  + player dir " + playerdir);
        }

        //add/remove from interpolation
        float interpolationUpdate = interpolationRate * Time.deltaTime;
        float newInterpolation = currentInterpolation; //will be changed below
        if(playerdir != DIRECTION.NONE)
        {
            if (playerdir.Equals(currentDirection))
            {
                //Debug.Log("Forward Movement");
                newInterpolation += interpolationUpdate;
                newInterpolation = AddWaypoint(playerdir, currentPoint, newInterpolation);
            }
            else if (playerdir.areOpposingSides(currentDirection))//Oposing directions
            {
                newInterpolation -= interpolationUpdate;
                //newInterpolation = RemoveWaypoint(newInterpolation);
            }
            //currentInterpolation <= directionSwitchThreshhold && !playerdir.Equals(DIRECTION.NONE)) //fallback for orthogonal directions 
            else if(currentList.Count > 1 && newInterpolation <= 0f + directionSwitchThreshhold)
            {
                WayPoint prevWaypoint = currentList.Last.Previous.Value;
                var prevDir = currentPoint.getDirection(prevWaypoint);
                if (playerdir.Equals(prevDir))//Oposing directions
                {
                    newInterpolation -= interpolationUpdate;
                    newInterpolation = RemoveWaypoint(newInterpolation);
                }
            }
            //remove waypoints instead of double
            currentPoint.setCurrentInterploation(newInterpolation);
        }

        ////Switch waypoints
        //if(newInterpolation != currentInterpolation)
        //{
        //    newInterpolation = AddWaypoint(playerdir, currentPoint, newInterpolation);
        //    newInterpolation = RemoveWaypoint(newInterpolation);
        //    //adjust interpolation if switching 
        //    currentPoint.setCurrentInterploation(newInterpolation);
        //}
    }

    private float RemoveWaypoint(float newInterpolation)
    {
        //switch to previous Waypoint
        if (newInterpolation <= 0f + directionSwitchThreshhold) //is clamped
        {
            if (currentList.Count > 0)
            {
                Debug.Log("Removing " + currentList.Last.Value);
                currentList.RemoveLast();
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
                currentList.AddLast(newWaypoint);
                newInterpolation = 1f;
            }
        }

        return newInterpolation;
    }
}
