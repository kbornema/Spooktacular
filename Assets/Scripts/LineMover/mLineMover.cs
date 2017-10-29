using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mLineMover : MonoBehaviour {

    [Range(0, 0.5f)]
    [SerializeField]
    float directionSwitchThreshhold = 0;

    [Range(0, 5f)]
    [SerializeField]
    float interpolationRate = 0.3f;

    // new Linemover for every Player
    int currentPlayer = 0;

    int activeSquadId = 0;

    [SerializeField]
    PlayerController playerController;

    Squad activeSquad;

    [SerializeField]
    InputController controller;

    private bool drawingPath;

    private mPath newPath;

    private WayPoint curPoint;
    private WayPoint nextWayPoint;

    Vector3 currentPosition;

    private DIRECTION currentDir;

    private float interpolation;

    // Use this for initialization
    void Start () {
        activeSquad = playerController.Squads[activeSquadId];
        drawingPath = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (drawingPath == false)
        {
            if(controller.GetPlayerButtonInput("Button1", playerController.PlayerId))
            {
                drawingPath = true;
                StartDrawing();
                
            }
        }
        else
        {
            if (controller.GetPlayerButtonInput("Button2", playerController.PlayerId))
            {
                activeSquad.setPath(newPath);

                drawingPath = false;

                return;
            }
            else
            {
                MoveLine();
            }
        }
    }
      

    void MoveLine()
    {
        DIRECTION inputDirection = controller.GetPlayerDirection(playerController.PlayerId);
        // We are in Range of the next Checkpoint or slightly ahead of it
        // change of direction is still possible!
        if ((currentPosition - nextWayPoint.transform.position).magnitude < directionSwitchThreshhold)
        {
            if (currentDir != inputDirection)
            {
                currentDir = controller.GetPlayerDirection(playerController.PlayerId);
                currentPosition = nextWayPoint.transform.position;
            }
            else
            {
                currentPosition += GetDirectionVector(currentDir) * interpolationRate;
                // is now out of range of Waypoint --> set new Waypoint
                if ((currentPosition - nextWayPoint.transform.position).magnitude > directionSwitchThreshhold)
                {
                    if (newPath.ReturnLastWayPoint() == nextWayPoint)
                    {
                        if(newPath.NumberOfWayPointsInPath() <= 1)
                        {
                            nextWayPoint = nextWayPoint.getWayPoint(inputDirection);
                        }
                        else
                        {
                            newPath.RemoveLastWayPoint();
                            nextWayPoint = newPath.ReturnLastWayPoint();
                        }
                    }
                    else if(nextWayPoint.getWayPoint(inputDirection) == newPath.ReturnLastWayPoint())
                    {
                        nextWayPoint = newPath.ReturnLastWayPoint();
                    }
                    else
                    {
                        newPath.AddNewWaypoint(nextWayPoint);
                        nextWayPoint = nextWayPoint.getWayPoint(currentDir);
                    }
                }
            }
        }
        else
        {
            // Default move forward!
            if (inputDirection == currentDir)
            {
                currentPosition += GetDirectionVector(currentDir) * interpolationRate;
            }
            // Directions are opposite --> move Back
            else if ((GetDirectionVector(inputDirection) + GetDirectionVector(currentDir)).magnitude <= 0.001)
            {
                currentPosition += GetDirectionVector(inputDirection) * interpolationRate;
                // directionswitch --> switch targetWaypoint
                nextWayPoint = newPath.ReturnLastWayPoint();
            }
            else
            {

            }
        }
    }

    void SwitchActiveSquad()
    {
        activeSquad = playerController.Squads[activeSquadId];
        
    }

    void StartDrawing()
    {
        newPath = new mPath();
        activeSquad.currentPath.ClearPathUpToFirstElement();
    }

    // Delete ??
   /* void ChangePathOfActiveSquad()
    {
        activeSquad = playerController.Squads[activeSquadId];
        // TODO add other stuff 

        curPoint = activeSquad.getCurrentPoint();
        currentDir = activeSquad.direction;
        interpolation = activeSquad.interpolation;

        drawingPath = true;
    }
    */

    Vector3 GetDirectionVector(DIRECTION dir)
    {
        switch(dir)
        {
            case DIRECTION.NORTH: return new Vector3(0, 1, 0);
            case DIRECTION.SOUTH: return new Vector3(0, -1, 0);
            case DIRECTION.WEST: return new Vector3(-1, 0, 0);
            case DIRECTION.EAST: return new Vector3(1, 0, 0);
            default: return Vector3.zero;
        }
    }
}
