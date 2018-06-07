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
    public int buttonsToWin;
	public Vector2 playerStartPos;
    public bool won = false;

	public Tilemap(int xs, int ys, int buttonsToWin = 1, Vector2? playerStartPos = null)
    {
        tiles = new Tile[xs, ys];
        for (int x = 0; x < xs; x++)
        {
            for (int y = 0; y < ys; y++)
            {
                tiles[x, y] = new Tile(Tile.TileType.EMPTY);
            }
        }
		this.buttonsToWin = buttonsToWin;
		this.playerStartPos = playerStartPos.GetValueOrDefault(new Vector2(3f, 3f));
    }

	public void addDoor(Vector2Int button, params Vector2Int[] doors) {
		foreach (Vector2Int d in doors) {
			tiles [d.x, d.y] = new Tile (Tile.TileType.DOOR);
			tiles[d.x, d.y].buttonPos = new Vector2Int(button.x, button.y);
		}
		tiles [button.x, button.y] = new Tile (Tile.TileType.BUTTONBLUE);
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
		int s = 0;
		int xmod = (directionRight ? 1 : -1);
		while (isInBounds (x, y) && tiles[x,y].box != null && s < tiles[x,y].box.steps) {
			MoveBox(x, y, x + xmod, y);
			s++;
			x += xmod;
		}
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

	public bool MoveBox(int fx, int fy, int tx, int ty)
    {
        if (IsCollidable(tx, ty))
        {
            return false;
        }
        tiles[tx, ty].box = tiles[fx, fy].box;
        tiles[fx, fy].box = null;
		return true;
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

	public bool isInBounds(int x, int y) {
		return 0 <= x && x < tiles.GetLength (0) && 0 <= y && y < tiles.GetLength (1);
	}

    public bool IsCollidable(int x, int y)
    {
		if (!isInBounds(x, y))
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
	public int steps;
	public Box (int s = 1) {
		steps = s;
	}
}
