using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackOutTargets : MonoBehaviour 
{
    [SerializeField]
    private SpriteRenderer[] _targets;


    public void EnableTarget(bool val)
    {
        for (int i = 0; i < _targets.Length; i++)
            _targets[i].enabled = val;
    }
	
}
