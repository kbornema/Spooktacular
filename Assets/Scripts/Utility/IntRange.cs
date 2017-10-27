using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IntRange
{
    [SerializeField]
    private int _min;
    public int Min { get { return _min; } }

    public void SetMin(int min)
    {
        this._min = min;
    }

    [SerializeField]
    private int _max;
    public int Max { get { return _max; } }

    public void SetMax(int max)
    {
        this._max = max;
    }

    public int GetRandVal()
    {
        return GetInterpolated(Random.value);
    }

    public int GetInterpolated(float t)
    {
        Debug.Assert(_min <= _max);
        return (int)((1.0f - t) * (float)_min + t * (float)_max);
    }

    public float InverseInterpolation(int value)
    {
        Debug.Assert(_min <= _max);
        return Mathf.InverseLerp((float)_min, (float)_max, (float)value);
    }
	
    public IntRange(int min, int max)
    {
        Debug.Assert(min <= max);
        this._min = min;
        this._max = max;
    }
}
