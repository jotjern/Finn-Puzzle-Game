using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public enum TileType
    {
        WALL,
        DOOR,
        BUTTON,
        BUTTONBLUE,
        EMPTY,
    }
    public TileType type;
    public Box box;
    public Vector2Int buttonPos;
    public Tile(TileType type)
    {
        this.type = type;
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
            case TileType.BUTTONBLUE:
                return "P";
            case TileType.DOOR:
                return "=";
            default:
                return "E";
        }
    }
}

public class Tilemap {

    public Tile[,] tiles;
    public List<Box> boxes = new List<Box>();
    public int buttonsToWin = 1;
    public bool won = false;

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

    public void SetTile(int x, int y, Tile.TileType type, bool box=false)
    {
        tiles[x, y] = new Tile(type);
        if (box)
        {
            tiles[x, y].box = new Box();
        }
    }

    public void SetBoxTiles(int fx, int fy, int tx, int ty, Tile.TileType type, bool box=false)
    {
        for (int x = fx; x <= tx; x++)
        {
            for (int y = fy; y <= ty; y++)
            {
                SetTile(x, y, type, box);
            }
        }
    }

    public void MoveBox(int fx, int fy, int tx, int ty)
    {
        if (IsCollidable(tx, ty))
        {
            return;
        }
        tiles[tx, ty].box = tiles[fx, fy].box;
        tiles[fx, fy].box = null;
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
            Debug.Log("Checking if door collidable");
            Vector2Int buttonPos = tiles[x, y].buttonPos;
            if (tiles[buttonPos.x, buttonPos.y].box == null)
            {
                return true;
            }
            return false;
        }
        return false;
    }

    public void Step()
    {
        int pressedButtons = 0;
        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                if (tiles[x, y].box != null)
                {
                    MoveBox(x, y, x, y - 1);
                }
                if (tiles[x, y].type == Tile.TileType.BUTTON && tiles[x, y].box != null)
                    pressedButtons += 1;
            }
        }
        won = pressedButtons >= buttonsToWin;
    }
}

public class Box
{

}
