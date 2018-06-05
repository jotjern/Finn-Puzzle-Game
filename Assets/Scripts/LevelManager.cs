using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public TilemapManager tilemapRenderer;
    public CatController player;
    public int level = -1;

    public enum Level
    {
        Sample,
        Level1
    }

	void Start () {
        tilemapRenderer.Map = LoadLevel(Level.Level1);
    }
	
	void Update () {
        if (tilemapRenderer.Map.won)
        {
            //StartCoroutine(WinInSeconds(3f));
        }
	}

    IEnumerable WinInSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);

    }

    public static Tilemap LoadLevel(Level level)
    {
        Tilemap Map;
        switch (level)
        {
            case Level.Sample:
                Map = new Tilemap(16, 16);
                Map.SetBoxTiles(0, 15, 3, 15, Tile.TileType.EMPTY, true);
                Map.SetBoxTiles(0, 12, 3, 12, Tile.TileType.EMPTY, true);
                Map.SetBoxTiles(0, 10, 3, 10, Tile.TileType.EMPTY, true);
                Map.SetBoxTiles(0, 5, 5, 5, Tile.TileType.WALL);
                break;
            case Level.Level1:
                Debug.Log("Loading level 1");
                Map = new Tilemap(20, 20);
                Map.buttonsToWin = 2;
                Map.SetBoxTiles(0, 0, 5, 0, Tile.TileType.WALL);
                Map.SetTile(0, 1, Tile.TileType.BUTTON);
                Map.SetTile(4, 1, Tile.TileType.BUTTON);

                Map.SetBoxTiles(0, 6, 4, 6, Tile.TileType.WALL);
                Map.SetTile(3, 7, Tile.TileType.EMPTY, true);
                Map.SetBoxTiles(5, 4, 12, 4, Tile.TileType.WALL);
                Map.SetTile(7, 4, Tile.TileType.EMPTY);
                Map.SetTile(7, 3, Tile.TileType.WALL);
                Map.SetBoxTiles(10, 2, 13, 2, Tile.TileType.WALL);
                Map.SetBoxTiles(7, 1, 9, 1, Tile.TileType.WALL);
                Map.SetTile(14, 3, Tile.TileType.WALL);
                Map.SetTile(15, 4, Tile.TileType.WALL);
                Map.SetTile(16, 5, Tile.TileType.WALL);
                Map.SetBoxTiles(10, 7, 15, 7, Tile.TileType.WALL);
                Map.SetBoxTiles(10, 8, 10, 9, Tile.TileType.EMPTY, true);
                break;
            default:
                Debug.Log("Attempted to load invalid level");
                Map = null;
                break;
        }
        return Map;
    }
}
