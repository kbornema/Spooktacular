using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaAnimation : MonoBehaviour 
{
    [SerializeField]
    private AnimationCurve _curve;
    [SerializeField]
    private float _circleTime;
    [SerializeField]
    private FloatRange _alpha;
    private float _midAlpha;
    private float _curTime;

    [SerializeField]
    private SpriteRenderer[] _renderer;

    [SerializeField]
    private bool _randomize;

    private void Start()
    {
        if (_randomize)
        {
            _curTime = _circleTime * Random.value;
        }

        _midAlpha = _alpha.Max - _alpha.Min;

        
    }
	
	
	// Update is called once per frame
	void Update () 
    {
        _curTime += Time.deltaTime;

        float t = _curTime / _circleTime;


        for (int i = 0; i < _renderer.Length; i++)
        {
            Color c = _renderer[i].color;
            c.a = _curve.Evaluate(t) * _midAlpha + _alpha.Min;

            _renderer[i].color = c;
        }
	}
}
