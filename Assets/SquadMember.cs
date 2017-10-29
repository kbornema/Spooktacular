using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadMember : MonoBehaviour 
{

    public ChildAnimation anim;
    public AnimatedSpriteReplacer spriteReplacer;

    internal void Init(SpriteLookup spriteLookup)
    {
        spriteReplacer.SetLookup(spriteLookup);
    }

    public void SetMoving(bool val)
    {
        anim.SetMoving(val);
    }
}
