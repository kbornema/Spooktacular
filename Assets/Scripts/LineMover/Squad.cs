using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad : MonoBehaviour {

    [SerializeField]
    Path current;

    public void setPath( Path newPath )
    {
        current = newPath;
    }

    public Path getPath()
    {
        return current;
    }   

    //TODO generate waypoint for Current Position

    //public WayPoint getCurrentPoint()
    //{
    //    if(currentPoint == null)
    //    {
    //        currentPoint = currentPath.First.Value;
    //    }
    //    return currentPoint;
    //}

    //public void setPath(LinkedList<WayPoint> newPath)
    //{
    //    currentPath = newPath;
    //}
}
