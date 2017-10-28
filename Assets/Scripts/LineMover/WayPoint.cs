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
    GameObject track;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    float positionInbetween;
    float oldPosition;
    [SerializeField]
    WayPoint activeWaypoint;

	// Use this for initialization
	void Start () {
        //TODO interpolate between Position
        Vector3 origin = transform.position;
        Vector3 target = activeWaypoint.gameObject.transform.position;
        float distance = Vector3.Distance(origin, target);
        Debug.Log(distance+1);

    }
	
	// Update is called once per frame
	void Update ()
    {
        if(oldPosition != positionInbetween)
        {
            var targetObj = activeWaypoint.gameObject.transform;
            var trackObj = track.gameObject.transform;

            //resize
            Vector3 origin = transform.position;
            Vector3 target = targetObj.position;
            float distance = Vector3.Distance(origin, target);
            var scale = distance * positionInbetween + 1;
            Debug.Log(scale);
            trackObj.localScale = new Vector3( scale,1,1);
            //targetObj.localPosition = new Vector3(scale, 0);//TODO both axis 


            //Rotate
            Vector3 direction = target - origin;
            float rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.Rotate(Vector3.up, rotation, Space.Self);


            //set for no new calculation
            oldPosition = positionInbetween;
        }
	}
}
