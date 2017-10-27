using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class ZOrderStatic : MonoBehaviour 
{
    public const float ORDER_SCALE = 25.0f;

    private Renderer[] _renderers; // = new List<Renderer>();

    [SerializeField]
    private GameObject _pivot;
    [SerializeField]
    private float _sortOffset = 0.0f;

    private int _oldOrder = int.MaxValue;

    private void Reset()
    {
        if (_pivot == null)
            _pivot = gameObject;
    }

	// Use this for initialization
	private void Start () 
    {
        Reset();

        _renderers = GetComponents<Renderer>();

        if (_renderers.Length == 0)
        {
            Debug.LogWarning("ZOrderStatic component with name " + gameObject.name + " has no renderer to sort!");
        }

        UpdateZ();
	}

    public void UpdateZ()
    {
        if (_renderers == null)
            Start();

        int curOrder = -(int)((_pivot.transform.position.y + _sortOffset )* ZOrderStatic.ORDER_SCALE);

        if (curOrder != _oldOrder)
        {
            for (int i = 0; i < _renderers.Length; i++)
                _renderers[i].sortingOrder = curOrder;

            _oldOrder = curOrder;
        }
    }
    
#if UNITY_EDITOR
    void Update()
    {
        if (Application.isEditor && !EditorApplication.isPlaying)
        {
            if (!_pivot)
                _pivot = gameObject;

            UpdateZ();
        }

        else
        {
            enabled = false;
        }
    }
#endif



}
