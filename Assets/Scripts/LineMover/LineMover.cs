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

    int currentPlayer = 0;  //for Test

    int activeSquadId = 0;
    Squad[] squads = new Squad[1];

    LinkedList<WayPoint> currentList = new LinkedList<WayPoint>();

    [SerializeField]
    InputController controller;

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


    // Update is called once per frame
    void Update ()
    {
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
            currentDirection = playerdir;
        }
            Debug.Log(currentDirection + " curr dir  + player dir " + playerdir);

        //add/remove from interpolation
        float interpolationUpdate = interpolationRate * Time.deltaTime;
        float newInterpolation = currentInterpolation; //will be changed below
        if(playerdir != DIRECTION.NONE)
        {
            if (playerdir.Equals(currentDirection))
            {
                newInterpolation += interpolationUpdate;
            }
            else if (playerdir.areOpposingSides(currentDirection) || //Oposing directions
                currentInterpolation <= directionSwitchThreshhold) //fallback for orthogonal directions 
            {
                newInterpolation -= interpolationUpdate;
            }
        }

        //Switch waypoints
        currentPoint.setCurrentInterploation(newInterpolation);
        if(currentPoint.getCurrentInterpolation() == 1f) //is clamped
        {
            WayPoint newWaypoint = currentPoint.getWayPoint(playerdir);
            if(newWaypoint!=null)
                currentList.AddLast(newWaypoint);
        }
        //switch to previous Waypoint
        if (currentPoint.getCurrentInterpolation() == 0f) //is clamped
        {
            currentList.RemoveLast();
        }
	}
}
