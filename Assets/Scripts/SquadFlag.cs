using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SquadFlag : MonoBehaviour 
{   
    [SerializeField, Range(0.0f, 1.0f)]
    private float _fillAmount;
    public float FillAmount { get { return _fillAmount; } set { _fillAmount = Mathf.Clamp01(value); } }

    [SerializeField]
    private Sprite[] _fillSprites;

    [SerializeField]
    private SpriteRenderer _borderRenderer;
    [SerializeField]
    private SpriteRenderer _fillRenderer;

    private int _oldId = -1;

    private void Update()
    {
        if (_fillSprites.Length <= 0)
            return;

        float inv = 1.0f - _fillAmount;
        int curId = (int)(inv * (_fillSprites.Length - 1));
    
        if(curId != _oldId)
        {
            _fillRenderer.sprite = _fillSprites[curId];
            _oldId = curId;
        }
    }

    public void SetColor(Color color)
    {
        _fillRenderer.color = color;
        _borderRenderer.color = color;
    }
}
