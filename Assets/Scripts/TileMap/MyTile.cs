using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "MyTile", menuName = "OwnTiles/MyTile")]
public class MyTile : Tile
{
    bool walkable = true;

    public virtual bool IsWalkable()
    {
        return walkable;
    }
}
