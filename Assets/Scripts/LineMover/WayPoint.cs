using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WayPoint : MonoBehaviour {

    [SerializeField]
    WayPoint top;
    [SerializeField]
    WayPoint right;
    [SerializeField]
    WayPoint down;
    [SerializeField]
    WayPoint Left;

    [SerializeField]
    GameObject TrackPrefab;

    GameObject track;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    float positionInbetween;
    float oldPosition;
    [SerializeField]
    WayPoint activeWaypoint;
    private int waypointScale = 1;

    // Use this for initialization
    void Start () { 

        Vector3 origin = transform.position;
        Vector3 target = activeWaypoint.gameObject.transform.position;
        float distance = Vector3.Distance(origin, target);
        Debug.Log(distance+1);

        //Create Track object
        track = Instantiate(TrackPrefab, origin, Quaternion.identity, transform);
        var trackObj = track.gameObject.transform;

        //Rotate Track
        Vector3 direction = target - origin;
        float rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        trackObj.transform.Rotate(Vector3.forward, rotation, Space.Self);

        //Rescale Track
        trackObj.transform.localScale = new Vector3(0, 0, 1);

    }

    public void updateWaypoint( DIRECTION newWaypoint)
    {
        switch( newWaypoint)
        {
            case DIRECTION.NORTH:
                activeWaypoint = top;
                break;
            case DIRECTION.EAST:
                activeWaypoint = right;
                break;
            case DIRECTION.SOUTH:
                activeWaypoint = down;
                break;
            case DIRECTION.WEST:
                activeWaypoint = Left;
                break;
        }
    }

    public float getCurrentInterpolation()
    {
        return positionInbetween;
    }

    public void setCurrentInterploation( float newInterpolation )
    {
 
        positionInbetween = Mathf.Clamp( newInterpolation, 0, 1);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(oldPosition != positionInbetween && activeWaypoint != null)
        {
            var targetObj = activeWaypoint.gameObject.transform;
            var trackObj = track.gameObject.transform;

            //resize
            Vector3 origin = transform.position;
            Vector3 target = targetObj.position;
            float distance = Vector3.Distance(origin, target);
            var scale = (distance + waypointScale *2) * positionInbetween; 
            Debug.Log(scale);
            trackObj.localScale = new Vector3( scale,1,1);
            //trackObj.localPosition = new Vector3(scale, 0);//TODO both axis 
            
            //set for no new calculation
            oldPosition = positionInbetween;
        }
	}
}
