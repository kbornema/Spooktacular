using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class ZOrderDynamic : MonoBehaviour 
{
    [SerializeField]
    private GameObject _myPivot;
    [SerializeField]
    private float _sortOffset = 0.0f;

    private Renderer[] _renderers;
    private int _oldOrder = int.MaxValue;
    
    public void SetPivot(GameObject pivot)
    {
        this._myPivot = pivot;
        _oldOrder = int.MaxValue;
        UpdateZ();
    }

    private void Start()
    {
        if (this._myPivot == null)
            SetPivot(gameObject);

        _renderers = GetComponents<Renderer>();
        
        if (_renderers.Length == 0)
            Debug.LogWarning("ZOrderComponent with name " + gameObject.name + " has no renderer to sort!");

        UpdateZ();
    }
	
	private void LateUpdate () 
    {
        UpdateZ();
	}

    private void UpdateZ()
    {
        if (_renderers == null)
            Start();

        int curOrder = -(int)((_myPivot.transform.position.y + _sortOffset) * ZOrderStatic.ORDER_SCALE);

        if (curOrder != _oldOrder)
        {
            for (int i = 0; i < _renderers.Length; i++)
                _renderers[i].sortingOrder = curOrder;
          
            _oldOrder = curOrder;
        }
    }
}
