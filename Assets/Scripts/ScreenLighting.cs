using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ScreenLighting : AScreenEffect
{
    [SerializeField]
    private Camera _lightCam;
    private RenderTexture _lightTexture;

    [SerializeField]
    private Color _backgroundColor;
    [SerializeField]
    private Color _ambientColor;
    [SerializeField]
    private float _saturation;

    [SerializeField]
    private bool _autoUpdate;

    public void SetBackgroundColor(Color color)
    {
        _backgroundColor = color;
        _lightCam.backgroundColor = color;
    }

    public Color GetBackgroundColor()
    {
        return _backgroundColor;
    }

    public void SetAmbientColor(Color color)
    {
        if(!_usedMaterial)
            AssignMaterial();

        _ambientColor = color;
        _usedMaterial.SetColor("_AmbientColor", color);
    }


    public Color GetAmbientColor()
    {
        return _ambientColor;
    }

    public void SetSaturation(float val)
    {
        if (!_usedMaterial)
            AssignMaterial();

        _saturation = val;
        _usedMaterial.SetFloat("_Saturation", val);
    }

    public float GetSaturation()
    {
        return _saturation;
    }
    
    protected override void Awake() 
    {
        base.Awake();
        
        SetRenderTexture();
	}

    private void SetRenderTexture()
    {
        _lightTexture = _lightCam.targetTexture;
        _usedMaterial.SetTexture("_MultTex", _lightTexture);
    }

    protected override void BeforeRenderImage()
    {
        Debug.Assert(_usedMaterial);

        if (_lightTexture != _lightCam.targetTexture)
        {
            SetRenderTexture();
        }

        if (_autoUpdate)
        {
            SetBackgroundColor(_backgroundColor);
            SetAmbientColor(_ambientColor);
            SetSaturation(_saturation);
        }
    }

}
