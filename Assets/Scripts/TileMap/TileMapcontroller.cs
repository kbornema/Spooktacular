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

    public OpenRoad(Vector3Int origin, Vector3Int dir, WayPoint wp)
    {

    }
}

public class OpenWayPoint
{
    public WayPoint wayPoint;
    public Vector3Int tilePos;
    public int adjustingTiles;
    
    public OpenWayPoint(Vector3Int _tilePos, WayPoint wp, int _adjustingTiles)
    {
        wayPoint = wp;
        tilePos = _tilePos;
        adjustingTiles = _adjustingTiles;
    }

    void SetIncomingDirection()
    {
        
    }
}

public class TileMapcontroller : MonoBehaviour {

    List<OpenWayPoint> openWaypoints;
    List<OpenRoad> openRoads;

    List<WayPoint> finishedWayPointList;

    Tilemap tileMap;

	// Use this for initialization
	void Start () {
        tileMap = GetComponent<Tilemap>();

        GenerateWayPoints();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GenerateWayPoints()
    {
        Vector3Int startPoint = findWalkableTile();

        


    }

    public Vector3Int findWalkableTile()
    {
        for (int curX = tileMap.cellBounds.position.x; curX < tileMap.cellBounds.size.x; curX++)
        {
            for (int curY = tileMap.cellBounds.position.y; curY < tileMap.cellBounds.size.y; curY++)
            {
                if(tileMap.HasTile(new Vector3Int(curX, curY, 0)))
                {
                    MyTile curTile = tileMap.GetTile(new Vector3Int(curX, curY, 0)) as MyTile;
                    if (curTile.IsWalkable())
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
            // add Incoming road to existing Waypoint
            if(incomingRoad.direction == new Vector3Int(1, 0, 0))
            {
                openWaypoints[wpIndex].wayPoint.down = incomingRoad.lastWayPoint;
                incomingRoad.lastWayPoint.top = openWaypoints[wpIndex].wayPoint;
            }
            else if(incomingRoad.direction == new Vector3Int(-1, 0, 0))
            {
                openWaypoints[wpIndex].wayPoint.top = incomingRoad.lastWayPoint;
                incomingRoad.lastWayPoint.down = openWaypoints[wpIndex].wayPoint;

            }
            else if (incomingRoad.direction == new Vector3Int(0, 1, 0))
            {
                openWaypoints[wpIndex].wayPoint.left = incomingRoad.lastWayPoint;
                incomingRoad.lastWayPoint.right = openWaypoints[wpIndex].wayPoint;

            }
            else if (incomingRoad.direction == new Vector3Int(0, -1, 0))
            {
                openWaypoints[wpIndex].wayPoint.right = incomingRoad.lastWayPoint;
                incomingRoad.lastWayPoint.left = openWaypoints[wpIndex].wayPoint;

            }
            int otherRoadIndex = openRoads.FindIndex(r => r.lastWayPoint == openWaypoints[wpIndex].wayPoint && (r.direction + incomingRoad.direction) == Vector3Int.zero);

            // remove both Roads from the openRoads
            if (otherRoadIndex >= 0)
                openRoads.RemoveAt(otherRoadIndex);
            openRoads.Remove(incomingRoad); 
        }
        else
        {
            // add new OpenCrossRoad
            // openWaypoints.Add(new OpenWayPoint(currentTile, ))

            openRoads.Remove(incomingRoad);
        }
    }

    bool IsWalkable(Vector3Int position)
    {
        MyTile myTile = tileMap.GetTile(new Vector3Int(position.x, position.y, 0)) as MyTile;

        if (myTile == null || myTile.IsWalkable() == false)
            return false;
        else
            return true;
    }

    public void CheckNeighbors(checkabletile tile)
    {
        if (tileMap.HasTile(tile.tilePos) == false)
        {
            Debug.Log("No tile here... Where the Fuck dou you think you're going??");
            return;
        }

        MyTile northTile = tileMap.GetTile(new Vector3Int(tile.tilePos.x + 1, tile.tilePos.y, 0)) as MyTile;
        MyTile southTile = tileMap.GetTile(new Vector3Int(tile.tilePos.x - 1, tile.tilePos.y, 0)) as MyTile;
        MyTile westTile = tileMap.GetTile(new Vector3Int(tile.tilePos.x, tile.tilePos.y + 1, 0)) as MyTile;
        MyTile eastTile = tileMap.GetTile(new Vector3Int(tile.tilePos.x, tile.tilePos.y - 1, 0)) as MyTile;

        int newTileCounter = 0;
        if (northTile.IsWalkable() == true)
        {
            newTileCounter++;
            if (-1 == OpenTiles.FindIndex(t => t.tilePos != new Vector3Int(tile.tilePos.x + 1, tile.tilePos.y, 0)))
                OpenTiles.Add(new checkabletile(new Vector3Int(tile.tilePos.x + 1, tile.tilePos.y, 0), new Vector3Int(-1, 0, 0)));
        }
        if (southTile.IsWalkable() == true)
        {
            newTileCounter++;
            if(-1 == OpenTiles.FindIndex(t => t.tilePos != new Vector3Int(tile.tilePos.x - 1, tile.tilePos.y, 0)))
                OpenTiles.Add(new checkabletile(new Vector3Int(tile.tilePos.x - 1, tile.tilePos.y, 0), new Vector3Int(1, 0, 0)));

        }
        if (eastTile.IsWalkable() == true)
        {
            newTileCounter++;
            if (-1 == OpenTiles.FindIndex(t => t.tilePos != new Vector3Int(tile.tilePos.x, tile.tilePos.y + 1, 0)))
                OpenTiles.Add(new checkabletile(new Vector3Int(tile.tilePos.x, tile.tilePos.y + 1, 0), new Vector3Int(0, -1, 0)));

        }

        if (westTile.IsWalkable() == true)
        {
            newTileCounter++;
            if (-1 == OpenTiles.FindIndex(t => t.tilePos != new Vector3Int(tile.tilePos.x, tile.tilePos.y - 1, 0)))
                OpenTiles.Add(new checkabletile(new Vector3Int(tile.tilePos.x, tile.tilePos.y - 1, 0), new Vector3Int(0, +1, 0)));

        }

        switch(newTileCounter)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
        }

        /*Vector3Int distanceVector = new Vector3Int(0, 0, 0);
        distanceVector.x = ((northTile.IsWalkable()) ? 1 : 0) + ((southTile.IsWalkable()) ? 1 : 0);
        distanceVector.y = ((eastTile.IsWalkable()) ? 1 : 0) + ((westTile.IsWalkable()) ? 1 : 0);

        if(distanceVector.magnitude <= 1)
        {
            // end

        }
        else if (distanceVector.magnitude == 2)
        {
            // straight road
        }
        else if (distanceVector.magnitude > 1 && distanceVector.magnitude < 2)
        {
            // curve
        }
        else if (distanceVector.x + distanceVector.y == 3)
        {
            // threeway croosroads
        }
        else if(distanceVector.x + distanceVector.y == 4)
        {
            // crossroads

        }
        */
    }


}
