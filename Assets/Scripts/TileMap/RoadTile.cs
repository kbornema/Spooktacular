using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "RoadTile", menuName = "OwnTiles/RoadTile")]
public class RoadTile : MyTile
{
    public Sprite[] m_Sprites;
    public Sprite m_Preview;

    public override bool IsWalkable()
    {
        return true;
    }

    // This refreshes itself and other RoadTiles that are orthogonally and diagonally adjacent
    public override void RefreshTile(Vector3Int location, ITilemap tilemap)
    {
        for (int yd = -1; yd <= 1; yd++)
            for (int xd = -1; xd <= 1; xd++)
            {
                Vector3Int position = new Vector3Int(location.x + xd, location.y + yd, location.z);
                if (HasRoadTile(tilemap, position))
                    tilemap.RefreshTile(position);
            }
    }
    // This determines which sprite is used based on the RoadTiles that are adjacent to it and rotates it to fit the other tiles.
    // As the rotation is determined by the RoadTile, the TileFlags.OverrideTransform is set for the tile.
    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        int bitMask = GetBitMask4(tilemap, new Vec2i(location.x, location.y));


        int index = TileBitIdMapper.GetTileId4FromMask(bitMask);

        Debug.Log(index);

        if (index >= 0 && index < m_Sprites.Length)
        {
            tileData.sprite = m_Sprites[index];
            tileData.color = Color.white;
            tileData.transform.SetTRS(Vector3.zero, Quaternion.identity, Vector3.one);
            tileData.flags = TileFlags.LockTransform;
            tileData.colliderType = ColliderType.None;
        }
        else
        {
            Debug.LogWarning("Not enough sprites in RoadTile instance");
        }
    }


#if UNITY_EDITOR
    // The following is a helper that adds a menu item to create a RoadTile Asset
    [MenuItem("Assets/Create/RoadTile")]
    public static void CreateRoadTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Road Tile", "New Road Tile", "Asset", "Save Road Tile", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<RoadTile>(), path);
    }
#endif


    private bool Check(ITilemap map, Vec2i tileId, Vec2i offset)
    {
        Vec2i neightbourId = tileId + offset;

        Vector3Int vec = new Vector3Int(neightbourId.x, neightbourId.y, 0);
        return HasRoadTile(map, vec);
    }

    // This determines if the Tile at the position is the same RoadTile.
    private bool HasRoadTile(ITilemap tilemap, Vector3Int position)
    {
        return tilemap.GetTile(position) == this;
    }

    private bool CheckDown(ITilemap map, Vec2i tileId)
    {
        return Check(map, tileId, new Vec2i(0, -1));
    }

    private bool CheckUp(ITilemap map, Vec2i tileId)
    {
        return Check(map, tileId, new Vec2i(0, 1));
    }

    private bool CheckLeft(ITilemap map, Vec2i tileId)
    {
        return Check(map, tileId, new Vec2i(-1, 0));
    }

    private bool CheckRight(ITilemap map, Vec2i tileId)
    {
        return Check(map, tileId, new Vec2i(1, 0));
    }

    private bool CheckRightUp(ITilemap map, Vec2i tileId)
    {
        return Check(map, tileId, new Vec2i(1, 1));
    }

    private bool CheckUpLeft(ITilemap map, Vec2i tileId)
    {
        return Check(map, tileId, new Vec2i(-1, 1));
    }

    private bool CheckLeftDown(ITilemap map, Vec2i tileId)
    {
        return Check(map, tileId, new Vec2i(-1, -1));
    }

    private bool CheckDownRight(ITilemap map, Vec2i tileId)
    {
        return Check(map, tileId, new Vec2i(1, -1));
    }


    [System.Flags]
    public enum TileNeighbour
    {
        None = 0,
        Up = 1,
        UpLeft = 2,
        Left = 4,
        Leftdown = 8,
        Down = 16,
        DownRight = 32,
        Right = 64,
        RightUp = 128,
    }

    private int GetBitMask4(ITilemap map, Vec2i tileId)
    {
        TileNeighbour bitMask = TileNeighbour.None;

        if (CheckUp(map, tileId))
            bitMask |= TileNeighbour.Up;

        if (CheckLeft(map, tileId))
            bitMask |= TileNeighbour.Left;

        if (CheckDown(map, tileId))
            bitMask |= TileNeighbour.Down;

        if (CheckRight(map, tileId))
            bitMask |= TileNeighbour.Right;

        return (int)bitMask;
    }
}
