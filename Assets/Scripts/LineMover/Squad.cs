﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad : MonoBehaviour {

    [SerializeField]
    LinkedList<WayPoint> currentPath;
    [SerializeField]
    WayPoint currentPoint;

    public WayPoint getCurrentPoint()
    {
        if(currentPoint == null)
        {
            currentPoint = currentPath.First.Value;
        }
        return currentPoint;
    }

    public void setPath(LinkedList<WayPoint> newPath)
    {
        currentPath = newPath;
    }
}
