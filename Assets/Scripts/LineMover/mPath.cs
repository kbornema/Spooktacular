using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mPath : MonoBehaviour {

    List<WayPoint> currentPath;

    List<GameObject> PathObjects;

    [SerializeField]
    float objectSpawnRate;

    public WayPoint ReturnLastWayPoint()
    {
        if(currentPath.Count > 0)
        {
            return currentPath[currentPath.Count - 1];
        }
        Debug.Log("Found no last Waypoint");
        return null;
    }

    public WayPoint GetFirstPoint()
    {
        WayPoint wp = currentPath[0];
        return wp;
    }

    public void RemoveFirstPoint()
    {
        currentPath.RemoveAt(0);
    }

    public void ClearPathUpToFirstElement()
    {
        currentPath.RemoveRange(1, currentPath.Count - 2);   
    }

    public void AddNewWaypoint(WayPoint wp)
    {
        currentPath.Add(wp);
    }

    public void RemoveLastWayPoint()
    {
        currentPath.RemoveAt(currentPath.Count - 1);
    }

    public int NumberOfWayPointsInPath()
    {
        if(currentPath != null)
            return currentPath.Count;
        return 0;
    }

    // Use this for initialization
    void Start () {
        currentPath = new List<WayPoint>();
        PathObjects = new List<GameObject>();
	}
	
    public void ClearPath()
    {
        currentPath.Clear();
        PathObjects.Clear();
    }

	// Update is called once per frame
	void Update () {
		
	}
}
