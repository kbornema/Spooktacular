using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAnimation : MonoBehaviour 
{
    [SerializeField]
    private AnimationCurve _curve;

    [SerializeField]
    private float _time = 1.0f;
    private float _curTime = 0.0f;
	
	// Update is called once per frame
	private void LateUpdate () 
    {
        float t = _curTime / _time;
        _curTime += Time.deltaTime;

        float curY = _curve.Evaluate(t);
        transform.position += new Vector3(0.0f, curY * Time.deltaTime, 0.0f);
	}
}
