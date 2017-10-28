using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildAnimation : MonoBehaviour {

    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private float _idleTime;
    [SerializeField]
    private float _moveTime;
    [SerializeField]
    private float _timeOffset;

    [SerializeField]
    private bool _isMoving;
    private bool _oldIsMoving = false;

    private void OnEnable()
    {
        float offset = Random.value * _timeOffset;
        _animator.SetFloat("IdleSpeed", 1.0f / (_idleTime + offset));
        _animator.SetFloat("MoveSpeed", 1.0f / (_moveTime + offset));
    }

    public void SetMoving(bool val)
    {
        _animator.SetBool("IsMoving", val);
    }

    private void Update()
    {
        if(_oldIsMoving != _isMoving)
        {
            _oldIsMoving = _isMoving;
            SetMoving(_isMoving);
        }
    }

}
