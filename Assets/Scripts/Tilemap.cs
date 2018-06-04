using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public enum TileType
    {
        WALL,
        BUTTON,
        EMPTY,
        DOOR
    }
    public TileType type;
    public int state;
    public Box box;
    public Tile(TileType type, int state = 0)
    {
        this.type = type;
        state = 0;
    }

    public override string ToString()
    {
        if (box != null)
        {
            return "X";
        }
        switch (type)
        {
            case TileType.BUTTON:
                return "B";
            case TileType.EMPTY:
                return "[]";
            case TileType.WALL:
                return "#";
            case TileType.DOOR:
                return "+";
            default:
                return "E";
        }
    }
}

public class Tilemap {

    public Tile[,] tiles;
    public List<Box> boxes = new List<Box>();

    public Tilemap(int xs, int ys)
    {
        tiles = new Tile[xs, ys];
        for (int x = 0; x < xs; x++)
        {
            for (int y = 0; y < ys; y++)
            {
                tiles[x, y] = new Tile(Tile.TileType.EMPTY);
            }
        }
    }

    public List<Vector2Int> GetBoxPositions()
    {
        List<Vector2Int> res = new List<Vector2Int>();
        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                if (tiles[x, y].box != null)
                {
                    res.Add(new Vector2Int(x, y));
                }
            }
        }
        return res;
    }

    public void PushBox(int x, int y, bool directionRight) {
        MoveBox(x, y, x + (directionRight ? 1 : -1), y);
    }

    public void MoveBox(int fx, int fy, int tx, int ty)
    {
        if (IsCollidable(tx, ty))
        {
            Debug.Log("Can't move, collider under me");
            return;
        }
        Debug.Log("Moving box");
        tiles[tx, ty].box = tiles[fx, fy].box;
        tiles[fx, fy].box = null;
        UpdateState(tx, ty);
    }

    public int UpdateState(int x, int y)
    {
        switch (tiles[x, y].type)
        {
            case Tile.TileType.BUTTON:
                tiles[x, y].state = tiles[x, y].box == null ? 0 : 1;
                break;
            default:
                tiles[x, y].state = 0;
                break;
        }
        return tiles[x, y].state;
    }

    public override string ToString()
    {
        string ret = "";
        for (int y = tiles.GetLength(1) - 1; y >= 0; y--)
        {
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                ret += tiles[x, y].ToString();
            }
            ret += "\n";
        }
        return ret;
    }

    public bool IsCollidable(int x, int y)
    {
        if (x < 0 || x >= tiles.GetLength(0) || y < 0 || y >= tiles.GetLength(1))
        {
            return true;
        }
        if (tiles[x, y].box != null)
        {
            return true;
        }
        if (tiles[x, y].type == Tile.TileType.WALL)
        {
            return true;
        }
        if (tiles[x, y].type == Tile.TileType.DOOR)
        {
            return true;
        }
        return false;
    }

    public void Step()
    {
        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                if (tiles[x, y].box != null)
                {
                    MoveBox(x, y, x, y - 1);
                }
            }
        }
    }
}

public class Box
{

}
