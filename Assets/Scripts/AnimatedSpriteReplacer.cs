using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedSpriteReplacer : MonoBehaviour 
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private SpriteLookup _lookUp;


    private Sprite _lastSprite;
    private Dictionary<Sprite, Sprite> _spriteDictionary;

    private bool _curSpriteValid = false;

    
	private void Awake() 
    {   
        if (_lookUp)
            Setup(_lookUp);

        else
            enabled = false;
	}

    private void Setup(SpriteLookup lookup)
    {
        this._lookUp = lookup;
        _lastSprite = _spriteRenderer.sprite;
        _spriteDictionary = _lookUp.GetReplacementDict();
    }
	
	void LateUpdate () 
    {
        Sprite curSprite = _spriteRenderer.sprite;

        // Cache stuff of the current "original" sprite to make the replacement lookup easier:
        if (_spriteRenderer.sprite != _lastSprite)
        {   
            _lastSprite = _spriteRenderer.sprite;
            _curSpriteValid = _spriteDictionary.ContainsKey(curSprite);
        }
        
        if (_curSpriteValid)
            _spriteRenderer.sprite = _spriteDictionary[curSprite];
	}

    public void SetLookup(SpriteLookup spriteLookup)
    {
        Setup(spriteLookup);
    }
}
