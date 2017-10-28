using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "MyTile", menuName = "OwnTiles/MyTile")]
public class MyTile : Tile
{
    protected bool m_walkable = false;

    public virtual bool IsWalkable()
    {
        return m_walkable;
    }
}
