using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour {

    [SerializeField]
    LinkedList<WayPoint> currentPath = new LinkedList<WayPoint>();
    List<GameObject> tracks = new List<GameObject>();
    [SerializeField]
    WayPoint currentTarget;

    [SerializeField]
    GameObject TrackPrefab;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    float positionInbetween;
    float oldPosition;
    private int waypointScale = 1;


    public DIRECTION getCurrentDirection()
    {
        return getCurrentPoint().getDirection(currentTarget);
    }

    public float getCurrentInterpolation()
    {
        return positionInbetween;
    }

    public void setCurrentInterploation(float newInterpolation)
    {

        positionInbetween = Mathf.Clamp(newInterpolation, 0, 1);
    }

    public void setTrackPrefab(GameObject prefab)
    {
        TrackPrefab = prefab;
    }

    public GameObject getCurrentTrack()
    {
        return tracks[tracks.Count-1];
    }

    private void CreateTrack()
    {
        //Create Track object //TODO Still in Need for more Tracks
        Vector3 origin = getCurrentPoint().gameObject.transform.position;
        GameObject track = Instantiate(TrackPrefab, origin, Quaternion.identity, transform);
        track.transform.localScale = new Vector3(0, 0, 1);
        oldPosition = 0;
        positionInbetween = 0;

        tracks.Add(track);
    }
	
    public void setNewTarget(DIRECTION newWaypoint)
    {
        WayPoint temp = getCurrentPoint().getWayPoint(newWaypoint);
        if (temp == null) return;

        currentTarget = temp; //onyl set if temp is non Null

        Debug.Log("new Direction: " + newWaypoint);

        Vector3 target = currentTarget.gameObject.transform.position;
        Vector3 origin = getCurrentPoint().gameObject.transform.position;

        //Rotate Track
        Vector3 direction = target - origin;
        float rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //getCurrentTrack().transform.Rotate(Vector3.forward, rotation, Space.Self);
        getCurrentTrack().transform.rotation = Quaternion.AngleAxis(rotation, Vector3.forward);
    }

    internal WayPoint getCurrentPoint()
    {
        if (currentPath == null || currentPath.Count == 0)
            return null;
        return currentPath.Last.Value;
    }

    internal WayPoint getPreviousPoint()
    {
        if (currentPath == null || currentPath.Count == 0)
            return null;
        return currentPath.Last.Previous.Value;
    }

    internal int getCount()
    {
        return currentPath.Count;
    }

    public void addWayPoint(WayPoint newPoint)
    {
        currentPath.AddLast(newPoint);
        CreateTrack();
        //add Track Somehow?
    }

    public void removeLast()
    {
        Debug.Log("Removing " + currentPath.Last.Value);
        currentPath.RemoveLast();
        var track = getCurrentTrack();
        tracks.Remove(track);
        Destroy(track);
        //remove Track Somehow?
    }

    // Update is called once per frame
    void Update()
    {
        // create new Track for each go-through

        if (oldPosition != positionInbetween && currentTarget != null)
        {
            var originObj = getCurrentPoint().gameObject.transform;
            var targetObj = currentTarget.gameObject.transform;
            var trackObj = getCurrentTrack().gameObject.transform;

            //resize
            Vector3 origin = originObj.position;
            Vector3 target = targetObj.position;
            float distance = Vector3.Distance(origin, target);
            var scale = (distance + waypointScale * 2) * positionInbetween;
            //Debug.Log(scale);
            trackObj.localScale = new Vector3(scale, 1, 1);
            //trackObj.localPosition = new Vector3(scale, 0);//TODO both axis 
            
            //relocate
            trackObj.position = origin;

            //set for no new calculation
            oldPosition = positionInbetween;
        }
    }
}
