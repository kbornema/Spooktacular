using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public struct checkabletile
{
    public checkabletile(Vector3Int _tilePos, Vector3Int _directionToPrevieousTile)
    {
        tilePos = _tilePos;
        directionToPreviousTile = _directionToPrevieousTile;
    }

    public Vector3Int tilePos;
    public Vector3Int directionToPreviousTile;
}

public class OpenRoad
{
    public Vector3Int origin;
    public Vector3Int direction;
    public WayPoint lastWayPoint;

    public OpenRoad(Vector3Int _origin, Vector3Int _dir, WayPoint _wp)
    {
        origin = _origin;
        direction = _dir;
        lastWayPoint = _wp;
    }
}

public class OpenWayPoint
{
    public WayPoint wayPoint;
    public Vector3Int tilePos;
    public int numAdustingRoads;
    public int numCheckedRoads;
    
    public OpenWayPoint(Vector3Int _tilePos, WayPoint wp, int _adjustingTiles)
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

    [SerializeField]
    public GameObject wayPointPrefab;

    [SerializeField]
    private Tilemap tileMap;

	// Use this for initialization
	void Start () {

        if(tileMap)
            tileMap = GetComponent<Tilemap>();

	}
	

    [ContextMenu("Generate")]
    public void GenerateWayPoints()
    {
        Vector3Int startPoint = findWalkableTile();

        openWaypoints = new List<OpenWayPoint>();
        openRoads = new List<OpenRoad>();
        finishedWayPointList = new List<WayPoint>();

        WayPoint originalWayPoint = Instantiate(wayPointPrefab).GetComponent<WayPoint>();
        finishedWayPointList.Add(originalWayPoint);

        originalWayPoint.transform.SetParent(transform);
        originalWayPoint.transform.position = tileMap.CellToWorld(startPoint);
        originalWayPoint.transform.localScale = Vector3.one * 2;

        int counter = 0;
        if (IsWalkable(startPoint + new Vector3Int(1, 0, 0)))
        {
            counter++;
            openRoads.Add(new OpenRoad(startPoint, new Vector3Int(1, 0, 0), originalWayPoint));
        }
        if (IsWalkable(startPoint + new Vector3Int(-1, 0, 0)))
        {
            counter++;
            openRoads.Add(new OpenRoad(startPoint, new Vector3Int(-1, 0, 0), originalWayPoint));

        }
        if (IsWalkable(startPoint + new Vector3Int(0, 1, 0)))
        {
            counter++;
            openRoads.Add(new OpenRoad(startPoint, new Vector3Int(0, 1, 0), originalWayPoint));

        }
        if (IsWalkable(startPoint + new Vector3Int(0, -1, 0)))
        {
            counter++;
            openRoads.Add(new OpenRoad(startPoint, new Vector3Int(0, -1, 0), originalWayPoint));

        }

        OpenWayPoint originalOpenWayPoint = new OpenWayPoint(startPoint, originalWayPoint, getNumberOfAdjustantWalkableTiles(startPoint));
        openWaypoints.Add(originalOpenWayPoint);

        int i = 0;
        while (openRoads.Count > 0 && i < 500) 
        {
            Debug.Log(openRoads.Count);
            ProcessRoad(openRoads[0]);
            i++;
        }

    }

    public int getNumberOfAdjustantWalkableTiles(Vector3Int tilePos)
    {
        int counter = 0;
        if (IsWalkable(tilePos + new Vector3Int(1, 0, 0 ))) { counter++; }
        if (IsWalkable(tilePos + new Vector3Int(-1, 0, 0))) { counter++; }
        if (IsWalkable(tilePos + new Vector3Int(0, 1, 0))) { counter++; }
        if (IsWalkable(tilePos + new Vector3Int(0, -1, 0))) { counter++; }

        return counter;
    }

    public Vector3Int findWalkableTile()
    {
        for (int curX = tileMap.cellBounds.position.x; curX < tileMap.cellBounds.size.x; curX++)
        {
            for (int curY = tileMap.cellBounds.position.y; curY < tileMap.cellBounds.size.y; curY++)
            {
                if(tileMap.HasTile(new Vector3Int(curX, curY, 0)))
                {
                    RoadTile curTile = tileMap.GetTile(new Vector3Int(curX, curY, 0)) as RoadTile;
                    if (curTile != null)
                        return new Vector3Int(curX, curY, 0);
                }
            }
        }
        Debug.Log("No walkable Tile found on Tilemap!");
        return new Vector3Int(0,0,0);
    }

    public void ProcessRoad(OpenRoad road)
    {
        Vector3Int currenttile = road.origin + road.direction;

        while(IsRoad(currenttile, road.direction))
        {
            currenttile += road.direction;
        }
        ProcessCrossRoad(currenttile, road);
    }

    public bool IsRoad(Vector3Int tilePos, Vector3Int dir)
    {
        bool result = false;

        Vector3Int alternateDir = new Vector3Int(dir.y, dir.x, 0);

        // assumes that previous tile on road is walkable
        if (IsWalkable(tilePos + dir) == true && IsWalkable(tilePos + alternateDir) == false && IsWalkable(tilePos - alternateDir) == false)
            result = true;

        return result;
    }

    public void ProcessCrossRoad(Vector3Int currentTile, OpenRoad incomingRoad)
    {
        int wpIndex = openWaypoints.FindIndex(wp => wp.tilePos == currentTile);
        if (wpIndex >= 0)
        {
            Debug.Log("Found a Road to an OLD WayPoint");
            // add Incoming road to existing Waypoint
            if(incomingRoad.direction == new Vector3Int(0, 1, 0))
            {
                openWaypoints[wpIndex].wayPoint.down = incomingRoad.lastWayPoint;
                incomingRoad.lastWayPoint.top = openWaypoints[wpIndex].wayPoint;
            }
            else if(incomingRoad.direction == new Vector3Int(0, -1, 0))
            {
                openWaypoints[wpIndex].wayPoint.top = incomingRoad.lastWayPoint;
                incomingRoad.lastWayPoint.down = openWaypoints[wpIndex].wayPoint;

            }
            else if (incomingRoad.direction == new Vector3Int(1, 0, 0))
            {
                openWaypoints[wpIndex].wayPoint.left = incomingRoad.lastWayPoint;
                incomingRoad.lastWayPoint.right = openWaypoints[wpIndex].wayPoint;

            }
            else if (incomingRoad.direction == new Vector3Int(-1, 0, 0))
            {
                openWaypoints[wpIndex].wayPoint.right = incomingRoad.lastWayPoint;
                incomingRoad.lastWayPoint.left = openWaypoints[wpIndex].wayPoint;

            }
            int otherRoadIndex = openRoads.FindIndex(r => r.lastWayPoint == openWaypoints[wpIndex].wayPoint && (r.direction + incomingRoad.direction).magnitude <= 0.01f);

            //int otherRoadIndex = openRoads.FindIndex(r => r.lastWayPoint == openWaypoints[wpIndex].wayPoint);

            // remove both Roads from the openRoads
            if (otherRoadIndex >= 0)
            {
                openRoads.RemoveAt(otherRoadIndex);
                Debug.Log("Removed OtherRoad");
                if ((++openWaypoints[wpIndex].numCheckedRoads) >= openWaypoints[wpIndex].numAdustingRoads)
                    openWaypoints.RemoveAt(wpIndex);
                
            }
            openRoads.Remove(incomingRoad);
            Debug.Log("Removed IncomingRoad");
            OpenWayPoint otherOpenWayPoint = openWaypoints.Find(wp => wp.wayPoint == incomingRoad.lastWayPoint);

            // TODO check why this can be null?!
            if (otherOpenWayPoint != null && (++otherOpenWayPoint.numCheckedRoads) >= otherOpenWayPoint.numAdustingRoads)
            {
                openWaypoints.Remove(otherOpenWayPoint);
            }
        }
        else
        {
            // add new OpenCrossRoad
            // openWaypoints.Add(new OpenWayPoint(currentTile, ))

            Debug.Log("Found a Road to an NEW WayPoint");

            WayPoint currentWP = Instantiate(wayPointPrefab).GetComponent<WayPoint>();

            currentWP.transform.SetParent(transform);
            currentWP.gameObject.transform.position = tileMap.CellToWorld(currentTile);

            finishedWayPointList.Add(currentWP);
            // Add incomingRoad to WayPoint
            // Find New Outgoing Roads and Add them to Openroads

            int outgoingRoads = 0;

            if (incomingRoad.direction == new Vector3Int(1, 0, 0))
            {
                currentWP.left = incomingRoad.lastWayPoint;
                incomingRoad.lastWayPoint.right = currentWP;
            }
            else
            {
                if(IsWalkable(currentTile + new Vector3Int(-1, 0, 0)))
                {
                    openRoads.Add(new OpenRoad(currentTile, new Vector3Int(-1, 0, 0), currentWP));
                    outgoingRoads++;
                }
            }

            if(incomingRoad.direction == new Vector3Int(-1, 0, 0))
            {
                currentWP.right = incomingRoad.lastWayPoint;
                incomingRoad.lastWayPoint.left = currentWP;
            }
            else
            {
                if (IsWalkable(currentTile + new Vector3Int(1, 0, 0)))
                {
                    openRoads.Add(new OpenRoad(currentTile, new Vector3Int(1, 0, 0), currentWP));
                    outgoingRoads++;
                }
            }

            if (incomingRoad.direction == new Vector3Int(0, 1, 0))
            {
                currentWP.down = incomingRoad.lastWayPoint;
                incomingRoad.lastWayPoint.top = currentWP;
            }
            else
            {
                if (IsWalkable(currentTile + new Vector3Int(0, -1, 0)))
                {
                    openRoads.Add(new OpenRoad(currentTile, new Vector3Int(0, -1, 0), currentWP));
                    outgoingRoads++;
                }
            }

            if (incomingRoad.direction == new Vector3Int(0, -1, 0))
            {
                currentWP.top = incomingRoad.lastWayPoint;
                incomingRoad.lastWayPoint.down = currentWP;
            }
            else
            {
                if (IsWalkable(currentTile + new Vector3Int(0, 1, 0)))
                {
                    openRoads.Add(new OpenRoad(currentTile, new Vector3Int(0, 1, 0), currentWP));
                    outgoingRoads++;
                }
            }

            OpenWayPoint openWp = new OpenWayPoint(currentTile, currentWP, outgoingRoads);
            openWaypoints.Add(openWp);
            openWp.numCheckedRoads = 0;

            OpenWayPoint otherWaypoint = openWaypoints.Find(wp => wp.wayPoint == incomingRoad.lastWayPoint);
            openRoads.Remove(incomingRoad);
            if(++(otherWaypoint.numCheckedRoads) >= otherWaypoint.numAdustingRoads)
            {
                openWaypoints.Remove(otherWaypoint);
            }
        }
    }

    bool IsWalkable(Vector3Int position)
    {
        RoadTile myTile = tileMap.GetTile(new Vector3Int(position.x, position.y, 0)) as RoadTile;

        if (myTile == null)
            return false;
        else
            return true;
    }
}
