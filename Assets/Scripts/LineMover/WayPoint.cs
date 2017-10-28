using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WayPoint : MonoBehaviour {

    [SerializeField]
    public WayPoint top;
    [SerializeField]
    public WayPoint right;
    [SerializeField]
    public WayPoint down;
    [SerializeField]
    public WayPoint left;

    public WayPoint getWayPoint(DIRECTION newWaypoint)
    {
        switch (newWaypoint)
        {
            case DIRECTION.NORTH:
                return top;
            case DIRECTION.EAST:
                return right;
            case DIRECTION.SOUTH:
                return down;
            case DIRECTION.WEST:
                return left;
            default:
                return null;
        }
    }


    public DIRECTION getDirection(WayPoint givenWaypoint)
    {
        if (givenWaypoint == null)
            return DIRECTION.NONE;

        if (givenWaypoint == top)
            return DIRECTION.NORTH;
        if (givenWaypoint == right)
            return DIRECTION.EAST;
        if (givenWaypoint == down)
            return DIRECTION.SOUTH;
        if (givenWaypoint == left)
            return DIRECTION.WEST;

        return DIRECTION.NONE;
    }

}
