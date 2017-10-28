using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class PP2DGridLayer : APP2DLayer
{
    [Header("Grid Settings")]
    [SerializeField]
    private Vector2 _division = new Vector2(1.0f, 1.0f);
    public Vector2 Division { get { return _division; } set { _division = value; } }

    [SerializeField]
    private Vector2 _offset = new Vector2(0.0f, 0.0f);
    public Vector2 Offset { get { return _offset; } set { _offset = value; } }

    [Header("Placing Settings")]
    [SerializeField]
    private APP2DSet _objectSet;
    [SerializeField]
    private APP2DSet.GetOptions _getType = APP2DSet.GetOptions.Mains;
    [SerializeField]
    private BitCheckings _bitChecking = BitCheckings.None;
    [SerializeField]
    private bool _updateNeighbours = true;

    [Header("Other")]
    [SerializeField]
    private DrawSettings _drawSettings;
    [SerializeField, ReadOnly]
    private List<Element> _objectList;

    public Vec2i GetFirstVector()
    {
        return _objectList[0].id;
    }

    private List<GameObject> _currentPrefabs;

    private Vec2i _lastDragCellId = new Vec2i(int.MaxValue, int.MaxValue);
    private Vec2i _rectangleDragStart = new Vec2i(int.MaxValue, int.MaxValue);

    public override void OnMouseClick(Vector2 worldPos, int button, PP2DToolType tool)
    {
        if (tool == PP2DToolType.Single)
            HandleSingleTool(WorldToCell(worldPos), button);

        else if (tool == PP2DToolType.Rectangle)
            _rectangleDragStart = WorldToCell(worldPos);
    }

    public override void OnMouseDrag(Vector2 dragStart, Vector2 worldPos, int button, PP2DToolType tool)
    {
        Vec2i cellId = WorldToCell(worldPos);

        if(cellId != _lastDragCellId)
        {
            if(tool == PP2DToolType.Single)
                HandleSingleTool(cellId, button);
        }
          

        _lastDragCellId = cellId;
    }

    public override void OnMouseRelease(Vector2 worldPos, int button, PP2DToolType tool)
    {
        _lastDragCellId = new Vec2i(int.MaxValue, int.MaxValue);

        if(tool == PP2DToolType.Rectangle)
        {
            Vec2i curCell = WorldToCell(worldPos);

            Vec2i min = new Vec2i(Mathf.Min(curCell.x, _rectangleDragStart.x), Mathf.Min(curCell.y, _rectangleDragStart.y));
            Vec2i max = new Vec2i(Mathf.Max(curCell.x, _rectangleDragStart.x), Mathf.Max(curCell.y, _rectangleDragStart.y));

            for (int i = min.x; i <= max.x; i++)
            {
                for (int j = min.y; j <= max.y; j++)
                {
                    HandleSingleTool(new Vec2i(i, j), button);
                }
            }
        }
    }

    private void HandleSingleTool(Vec2i cellId, int button)
    {
        if (button == 0 && _objectSet)
        {
            CreateTileAt(cellId, _updateNeighbours);
        }

        else if (button == 1)
        {
            Remove(cellId, _updateNeighbours);
        }
    }

    private void Remove(Vec2i cellId, bool _updateNeighbours)
    {
        Element e = Get(cellId);

        if(e != null)
        {
            Remove(e);

            if (_updateNeighbours)
                UpdateNeighbours(cellId);
        }
    }

    private GameObject GetInstance(Vec2i cellId)
    {
        int tileId = -1;

        if (_bitChecking == BitCheckings.Bit4)
            tileId = GetTileId4(cellId);

        else if (_bitChecking == BitCheckings.Bit8)
            tileId = GetTileId8(cellId);

        List<GameObject> prefabs = _objectSet.GetObjectPrefabs(tileId, _getType);

        GameObject prefab = prefabs[Random.Range(0, prefabs.Count)];

        GameObject instance = null;

#if UNITY_EDITOR
        instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
#endif

        //TODO: remove: just for testing here:

        instance.transform.SetParent(transform);
        instance.transform.position = CellToWorld(cellId);

        return instance;
    }

    private void CreateTileAt(Vec2i cellId, bool updateNeighbours)
    {
        Add(cellId, GetInstance(cellId), true);

        if (updateNeighbours)
            UpdateNeighbours(cellId);
    }

    private void UpdateNeighbours(Vec2i cellId)
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                Vec2i cur = cellId + new Vec2i(i, j);
                Replace(cur);
            }
        }
    }
#if UNITY_EDITOR
    public override void OnRepaint(UnityEditor.SceneView view, bool isDragging, Vector2 dragStart, Vector2 curMousePos, int button, PP2DToolType tool)
    {
        if (_drawSettings.drawGrid)
            DrawGridInView(view);


        if(!isDragging)
        {
            DrawCellAtMouse(curMousePos);
        }

        else 
        {
            if(tool == PP2DToolType.Rectangle)
                DrawSelectionRectangle(dragStart, curMousePos);

            else if(tool == PP2DToolType.Single)
                DrawCellAtMouse(curMousePos);
        }
    }
#endif
    /// <summary> Rounds the given world coordinate to the world position of the closest cell. </summary>
    private Vector2 GetClosestCellPosition(Vector2 world)
    {
        return VecUtil.RoundToNearestVector2f(world - _offset, _division);
    }

    /// <summary> Computes the center of the given cell in world coordinates. </summary>
    public Vector2 CellToWorld(Vec2i cellId)
    {
        return new Vector2((float)cellId.x / _division.x, (float)cellId.y / _division.y) + _offset;
    }

    /// <summary> Computes the id of a cell for the given world position. </summary>
    public Vec2i WorldToCell(Vector2 worldPoint)
    {
        Vector2 closestGridPoint = GetClosestCellPosition(worldPoint);
        return new Vec2i((int)(closestGridPoint.x * _division.x), (int)(closestGridPoint.y * _division.y));
    }

    #region CONTAINER_STUFF

    private bool Remove(Element element)
    {
        if (element == null)
            return false;

        if(element.obj != null)
            DestroyObj(element.obj);
            
        _objectList.Remove(element);
        return true;
    }

    public void Replace(Vec2i id)
    {
        var element = Get(id);

        if (element != null)
            Replace(id, GetInstance(id));
    }

    public void Replace(Vec2i id, GameObject obj)
    {
        var element = Get(id);

        if(element != null)
        {
            GameObject old = element.obj;
            element.obj = obj;

            DestroyObj(old);
        }
    }

    public void Add(Vec2i id, GameObject obj, bool replaceIfExists)
    {
        var element = Get(id);

        if(replaceIfExists && element != null)
            Replace(id, obj);
        
        else if(element == null)
            _objectList.Add(new Element(id, obj));
    }

    public bool HasTile(Vec2i id)
    {
        return Get(id) != null;
    }

    private Element Get(Vec2i id)
    {
        var element = _objectList.Find(x => x.id == id);

        if (element != null && element.obj == null)
        {
            Remove(element);
            element = null;
        }

        return element;
    }

    private void DestroyObj(GameObject obj)
    {
        if(obj)
        {
            DestroyImmediate(obj);
        }
    }

    public bool Contains(Vec2i id)
    {
        return Get(id) != null;
    }


    #endregion

    #region DRAWING_STUFF

#if UNITY_EDITOR

    private void DrawCellAtMouse(Vector2 pos)
    {
        Color oldColor = Handles.color;
        Handles.color = _drawSettings.selectionColor;

        EditorUtil.DrawWireRectanglePosSize(CellToWorld(WorldToCell(pos)), new Vector2(1.0f / _division.x, 1.0f / _division.y));

        Handles.color = oldColor;
    }

    private void DrawSelectionRectangle(Vector2 dragStart, Vector2 curMousePos)
    {
        Color oldColor = Handles.color;
        Handles.color = _drawSettings.selectionColor;

        Vector2 min;
        Vector2 max;
        VecUtil.MinMax(CellToWorld(WorldToCell(dragStart)), CellToWorld(WorldToCell(curMousePos)), out min, out max);

        Vector2 offset = new Vector2(0.5f / _division.x, 0.5f / _division.y);

        EditorUtil.DrawWireRectangleMinMax(min - offset, max + offset);

        Handles.color = oldColor;
    }

    private void DrawGridInView(SceneView view)
    {
        Color oldColor = Handles.color;

        Handles.color = _drawSettings.gridColor;

        Vec2i min = WorldToCell(view.camera.GetCameraMin()) - new Vec2i(1, 1);
        Vec2i max = WorldToCell(view.camera.GetCameraMax()) + new Vec2i(1, 1);

        Vector2 halfSize = new Vector2(0.5f / _division.x, 0.5f / _division.y);

        //horizontal lines:
        for (int i = min.x; i <= max.x; i++)
            Handles.DrawLine(CellToWorld(new Vec2i(i, min.y)) - new Vector2(halfSize.x, 0.0f), CellToWorld(new Vec2i(i, max.y)) - new Vector2(halfSize.x, 0.0f));

        //vertical lines:
        for (int i = min.y; i <= max.y; i++)
            Handles.DrawLine(CellToWorld(new Vec2i(min.x, i)) - new Vector2(0.0f, halfSize.y), CellToWorld(new Vec2i(max.x, i)) - new Vector2(0.0f, halfSize.y));

        Handles.color = oldColor;
    }

    private void DrawCellAt(Vector2 worldPos)
    {
        Handles.DrawWireCube(worldPos, new Vector3(1.0f / _division.x, 1.0f / _division.y, 0.0f));
    }

    private void DrawCellAt(Vec2i cellId)
    {
        DrawCellAt(CellToWorld(cellId));
    }

#endif

    [System.Serializable]
    private class DrawSettings
    {
        public bool drawGrid = true;
        public Color gridColor = new Color(1.0f, 1.0f, 1.0f, 0.125f);
        public Color selectionColor = new Color(1.0f, 1.0f, 1.0f, 0.125f);
    }

    #endregion
    
    #region BIT_CHECKING

    public enum BitCheckings
    {
        None,
        Bit4,
        Bit8
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

    private bool Check(Vec2i tileId, Vec2i offset)
    {
        Vec2i neightbourId = tileId + offset;
        return Contains(neightbourId);
    }

    private bool CheckDown(Vec2i tileId)
    {
        return Check(tileId, new Vec2i(0, -1));
    }

    private bool CheckUp(Vec2i tileId)
    {
        return Check(tileId, new Vec2i(0, 1));
    }

    private bool CheckLeft(Vec2i tileId)
    {
        return Check(tileId, new Vec2i(-1, 0));
    }

    private bool CheckRight(Vec2i tileId)
    {
        return Check(tileId, new Vec2i(1, 0));
    }

    private bool CheckRightUp(Vec2i tileId)
    {
        return Check(tileId, new Vec2i(1, 1));
    }

    private bool CheckUpLeft(Vec2i tileId)
    {
        return Check(tileId, new Vec2i(-1, 1));
    }

    private bool CheckLeftDown(Vec2i tileId)
    {
        return Check(tileId, new Vec2i(-1, -1));
    }

    private bool CheckDownRight(Vec2i tileId)
    {
        return Check(tileId, new Vec2i(1, -1));
    }


    private int GetBitMask4(Vec2i tileId)
    {
        TileNeighbour bitMask = TileNeighbour.None;

        if (CheckUp(tileId))
            bitMask |= TileNeighbour.Up;

        if (CheckLeft(tileId))
            bitMask |= TileNeighbour.Left;

        if (CheckDown(tileId))
            bitMask |= TileNeighbour.Down;

        if (CheckRight(tileId))
            bitMask |= TileNeighbour.Right;

        return (int)bitMask;
    }

    private int GetBitMask8(Vec2i tileId)
    {
        TileNeighbour bitMask = TileNeighbour.None;

        int edgeCount = 0;
        bool hasEdgeUp = false;
        bool hasEdgeLeft = false;
        bool hasEdgeDown = false;
        bool hasEdgeRight = false;

        if (CheckUp(tileId))
        {
            bitMask |= TileNeighbour.Up;
            hasEdgeUp = true;
            edgeCount++;
        }

        if (CheckLeft(tileId))
        {
            bitMask |= TileNeighbour.Left;
            hasEdgeLeft = true;
            edgeCount++;
        }

        if (CheckDown(tileId))
        {
            bitMask |= TileNeighbour.Down;
            hasEdgeDown = true;
            edgeCount++;
        }

        if (CheckRight(tileId))
        {
            bitMask |= TileNeighbour.Right;
            hasEdgeRight = true;
            edgeCount++;
        }

        //no adjacent edges, ignore corners OR if one edge is set, ignre corners
        if (edgeCount == 0 || edgeCount == 1)
            return (int)bitMask;

        //if two edges are opposite, the corner doesnt matter
        if (edgeCount == 2)
        {
            if (hasEdgeUp && hasEdgeLeft && CheckUpLeft(tileId))
                bitMask |= TileNeighbour.UpLeft;

            if (hasEdgeLeft && hasEdgeDown && CheckLeftDown(tileId))
                bitMask |= TileNeighbour.Leftdown;

            if (hasEdgeDown && hasEdgeRight && CheckDownRight(tileId))
                bitMask |= TileNeighbour.DownRight;

            if (hasEdgeRight && hasEdgeUp && CheckRightUp(tileId))
                bitMask |= TileNeighbour.RightUp;

            return (int)bitMask;
        }

        if (edgeCount == 3)
        {
            if (!hasEdgeDown)
            {
                if (CheckUpLeft(tileId))
                    bitMask |= TileNeighbour.UpLeft;

                if (CheckRightUp(tileId))
                    bitMask |= TileNeighbour.RightUp;
            }

            else if (!hasEdgeRight)
            {
                if (CheckUpLeft(tileId))
                    bitMask |= TileNeighbour.UpLeft;

                if (CheckLeftDown(tileId))
                    bitMask |= TileNeighbour.Leftdown;
            }

            else if (!hasEdgeUp)
            {
                if (CheckDownRight(tileId))
                {
                    bitMask |= TileNeighbour.DownRight;
                }

                if (CheckLeftDown(tileId))
                    bitMask |= TileNeighbour.Leftdown;
            }

            else if (!hasEdgeLeft)
            {
                if (CheckDownRight(tileId))
                    bitMask |= TileNeighbour.DownRight;

                if (CheckRightUp(tileId))
                    bitMask |= TileNeighbour.RightUp;
            }

            return (int)bitMask;
        }

        //all edges are set: all corners can be relevant:
        if (edgeCount == 4)
        {
            if (CheckUpLeft(tileId))
                bitMask |= TileNeighbour.UpLeft;

            if (CheckLeftDown(tileId))
                bitMask |= TileNeighbour.Leftdown;

            if (CheckDownRight(tileId))
                bitMask |= TileNeighbour.DownRight;

            if (CheckRightUp(tileId))
                bitMask |= TileNeighbour.RightUp;

            return (int)bitMask;
        }

        Debug.Log("Case didnt met the cases: " + bitMask);
        Debug.Assert(false);
        return (int)bitMask;
    }

    private int GetTileId8(Vec2i cellId)
    {
        int bitMask = GetBitMask8(cellId);

        for (int i = 0; i < TileBitIdMapper.TILE_ID_TO_BITS_8.Length; i++)
        {
            if (TileBitIdMapper.TILE_ID_TO_BITS_8[i] == bitMask)
                return i;
        }

        return -1;
    }

    private int GetTileId4(Vec2i cellId)
    {
        int bitMask = GetBitMask4(cellId);

        for (int i = 0; i < TileBitIdMapper.TILE_ID_TO_BITS_4.Length; i++)
        {
            if (TileBitIdMapper.TILE_ID_TO_BITS_4[i] == bitMask)
                return i;
        }

        return -1;
    }

    #endregion

    [System.Serializable]
    private class Element
    {
        public Vec2i id = new Vec2i();
        public GameObject obj = null;

        public Element(){}

        public Element(Vec2i id, GameObject obj)
        {
            this.id = id;
            this.obj = obj;
        }

    }
}
