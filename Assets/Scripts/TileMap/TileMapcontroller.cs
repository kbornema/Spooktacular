using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public struct checkabletile
{
    public checkabletile(Vec2i _tilePos, Vec2i _directionToPrevieousTile)
    {
        tilePos = _tilePos;
        directionToPreviousTile = _directionToPrevieousTile;
    }

    public Vec2i tilePos;
    public Vec2i directionToPreviousTile;
}

public class OpenRoad
{
    public Vec2i origin;
    public Vec2i direction;
    public WayPoint lastWayPoint;

    public OpenRoad(Vec2i _origin, Vec2i _dir, WayPoint _wp)
    {
        origin = _origin;
        direction = _dir;
        lastWayPoint = _wp;
    }
}

public class OpenWayPoint
{
    public WayPoint wayPoint;
    public Vec2i tilePos;
    public int numAdustingRoads;
    public int numCheckedRoads;
    
    public OpenWayPoint(Vec2i _tilePos, WayPoint wp, int _adjustingTiles)
    {
        wayPoint = wp;
        tilePos = _tilePos;
        numAdustingRoads = _adjustingTiles;
    }

    void SetIncomingDirection()
    {
        
    }
}

public class TileMapcontroller : MonoBehaviour {

    List<OpenWayPoint> openWaypoints;
    List<OpenRoad> openRoads;

    List<WayPoint> finishedWayPointList;

    public List<WayPoint> GetWaypointList()
    {
        return new List<WayPoint>(finishedWayPointList);
    }

    [SerializeField]
    public GameObject wayPointPrefab;

    [SerializeField]
    private PP2DGridLayer tileMap;

    public List<WayPoint> WayPoints()
    {
        return finishedWayPointList;
    }

    private void Awake()
    {   
        GenerateWayPoints();
    }
	

    
    public void GenerateWayPoints()
    {   
        for (int j = 0; j < transform.childCount; j++)
            Destroy(transform.GetChild(j));

        if (finishedWayPointList != null)
            finishedWayPointList.Clear();
        else
            finishedWayPointList = new List<WayPoint>();

        Vec2i startPoint = findWalkableTile();

        openWaypoints = new List<OpenWayPoint>();
        openRoads = new List<OpenRoad>();
        finishedWayPointList = new List<WayPoint>();

        WayPoint originalWayPoint = Instantiate(wayPointPrefab).GetComponent<WayPoint>();
        finishedWayPointList.Add(originalWayPoint);

        originalWayPoint.transform.SetParent(transform);
        originalWayPoint.transform.position = tileMap.CellToWorld(startPoint);
        originalWayPoint.transform.localScale = Vector3.one * 2;

        int counter = 0;
        if (IsWalkable(startPoint + new Vec2i(1, 0)))
        {
            counter++;
            openRoads.Add(new OpenRoad(startPoint, new Vec2i(1, 0), originalWayPoint));
        }

        if (IsWalkable(startPoint + new Vec2i(-1, 0)))
        {
            counter++;
            openRoads.Add(new OpenRoad(startPoint, new Vec2i(-1, 0), originalWayPoint));
        }

        if (IsWalkable(startPoint + new Vec2i(0, 1)))
        {
            counter++;
            openRoads.Add(new OpenRoad(startPoint, new Vec2i(0, 1), originalWayPoint));
        }

        if (IsWalkable(startPoint + new Vec2i(0, -1)))
        {
            counter++;
            openRoads.Add(new OpenRoad(startPoint, new Vec2i(0, -1), originalWayPoint));

        }

        OpenWayPoint originalOpenWayPoint = new OpenWayPoint(startPoint, originalWayPoint, getNumberOfAdjustantWalkableTiles(startPoint));
        openWaypoints.Add(originalOpenWayPoint);

        int i = 0;
        while (openRoads.Count > 0 && i < 100) 
        {
            ProcessRoad(openRoads[0]);
            i++;
        }


    }

    public int getNumberOfAdjustantWalkableTiles(Vec2i tilePos)
    {
        int counter = 0;
        if(IsWalkable(tilePos + new Vec2i(1, 0))) { counter++; }
        if (IsWalkable(tilePos + new Vec2i(-1, 0))) { counter++; }
        if (IsWalkable(tilePos + new Vec2i(0, 1))) { counter++; }
        if (IsWalkable(tilePos + new Vec2i(0, -1))) { counter++; }

        return counter;
    }

    public Vec2i findWalkableTile()
    {   
        return tileMap.GetFirstVector();
    }

    public void ProcessRoad(OpenRoad road)
    {
        Vec2i currenttile = road.origin + road.direction;

        while(IsRoad(currenttile, road.direction))
        {
            currenttile += road.direction;
        }
        ProcessCrossRoad(currenttile, road);
    }

    public bool IsRoad(Vec2i tilePos, Vec2i dir)
    {
        bool result = false;

        Vec2i alternateDir = new Vec2i(dir.y, dir.x);

        // assumes that previous tile on road is walkable
        if (IsWalkable(tilePos + dir) == true && IsWalkable(tilePos + alternateDir) == false && IsWalkable(tilePos - alternateDir) == false)
            result = true;

        return result;
    }

    public void ProcessCrossRoad(Vec2i currentTile, OpenRoad incomingRoad)
    {
        int wpIndex = openWaypoints.FindIndex(wp => wp.tilePos == currentTile);
        if (wpIndex >= 0)
        {   
            // add Incoming road to existing Waypoint
            if(incomingRoad.direction == new Vec2i(0, 1))
            {
                openWaypoints[wpIndex].wayPoint.down = incomingRoad.lastWayPoint;
                incomingRoad.lastWayPoint.top = openWaypoints[wpIndex].wayPoint;
            }
            else if(incomingRoad.direction == new Vec2i(0, -1))
            {
                openWaypoints[wpIndex].wayPoint.top = incomingRoad.lastWayPoint;
                incomingRoad.lastWayPoint.down = openWaypoints[wpIndex].wayPoint;

            }
            else if (incomingRoad.direction == new Vec2i(1, 0))
            {
                openWaypoints[wpIndex].wayPoint.left = incomingRoad.lastWayPoint;
                incomingRoad.lastWayPoint.right = openWaypoints[wpIndex].wayPoint;

            }
            else if (incomingRoad.direction == new Vec2i(-1, 0))
            {
                openWaypoints[wpIndex].wayPoint.right = incomingRoad.lastWayPoint;
                incomingRoad.lastWayPoint.left = openWaypoints[wpIndex].wayPoint;

            }
            int otherRoadIndex = openRoads.FindIndex(r => r.lastWayPoint == openWaypoints[wpIndex].wayPoint && (r.direction + incomingRoad.direction) == Vec2i.zero);

            // remove both Roads from the openRoads
            if (otherRoadIndex >= 0)
            {
                openRoads.RemoveAt(otherRoadIndex);
                if (++openWaypoints[wpIndex].numCheckedRoads >= openWaypoints[wpIndex].numAdustingRoads)
                    openWaypoints.RemoveAt(wpIndex);
                
            }
            openRoads.Remove(incomingRoad);
            OpenWayPoint otherOpenWayPoint = openWaypoints.Find(wp => wp.wayPoint == incomingRoad.lastWayPoint);

            // TODO check why this can be null?!
            if (otherOpenWayPoint != null && ++otherOpenWayPoint.numCheckedRoads >= otherOpenWayPoint.numAdustingRoads)
            {
                openWaypoints.Remove(otherOpenWayPoint);
            }
        }
        else
        {
            // add new OpenCrossRoad
            // openWaypoints.Add(new OpenWayPoint(currentTile, ))
            WayPoint currentWP = Instantiate(wayPointPrefab).GetComponent<WayPoint>();

            currentWP.transform.SetParent(transform);
            currentWP.gameObject.transform.position = tileMap.CellToWorld(currentTile);

            finishedWayPointList.Add(currentWP);
            // Add incomingRoad to WayPoint
            // Find New Outgoing Roads and Add them to Openroads

            int outgoingRoads = 0;

            if (incomingRoad.direction == new Vec2i(1, 0))
            {
                currentWP.left = incomingRoad.lastWayPoint;
                incomingRoad.lastWayPoint.right = currentWP;
            }
            else
            {
                if(IsWalkable(currentTile + new Vec2i(-1, 0)))
                {
                    openRoads.Add(new OpenRoad(currentTile, new Vec2i(-1, 0), currentWP));
                    outgoingRoads++;
                }
            }

            if(incomingRoad.direction == new Vec2i(-1, 0))
            {
                currentWP.right = incomingRoad.lastWayPoint;
                incomingRoad.lastWayPoint.left = currentWP;
            }
            else
            {
                if (IsWalkable(currentTile + new Vec2i(1, 0)))
                {
                    openRoads.Add(new OpenRoad(currentTile, new Vec2i(1, 0), currentWP));
                    outgoingRoads++;
                }
            }

            if (incomingRoad.direction == new Vec2i(0, 1))
            {
                currentWP.down = incomingRoad.lastWayPoint;
                incomingRoad.lastWayPoint.top = currentWP;

            }
            else
            {
                if (IsWalkable(currentTile + new Vec2i(0, -1)))
                {
                    openRoads.Add(new OpenRoad(currentTile, new Vec2i(0, -1), currentWP));
                    outgoingRoads++;
                }
            }

            if (incomingRoad.direction == new Vec2i(0, -1))
            {
                currentWP.top = incomingRoad.lastWayPoint;
                incomingRoad.lastWayPoint.down = currentWP;
            }
            else
            {
                if (IsWalkable(currentTile + new Vec2i(0, 1)))
                {
                    openRoads.Add(new OpenRoad(currentTile, new Vec2i(0, 1), currentWP));
                    outgoingRoads++;
                }
            }

            OpenWayPoint openWp = new OpenWayPoint(currentTile, currentWP, outgoingRoads);
            openWaypoints.Add(openWp);
            openWp.numCheckedRoads = 0;

            openRoads.Remove(incomingRoad);
        }
    }

    bool IsWalkable(Vec2i position)
    {   
        return tileMap.HasTile(position);
    }
}
