using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public TilemapManager tilemapRenderer;
    public CatController player;
    public int level = -1;

    private bool winCoroutineStarted = false;

    public enum Level
    {
        Sample,
        Level1,
        Level2,
    }

	void Start () {
        tilemapRenderer.Map = LoadLevel(Level.Level2);
        level = 0;
    }
	
	void Update () {
        if (!winCoroutineStarted && tilemapRenderer.Map.won)
        {
            StartCoroutine(WinInSeconds(3f));
        }
        if (Input.GetKeyDown("r"))
        {
            tilemapRenderer.ClearTiles();
            tilemapRenderer.Map = LoadLevel(Level.Sample);
            tilemapRenderer.winText.SetActive(false);
            player.active = true;
        }
	}

    IEnumerator WinInSeconds(float seconds)
    {
        winCoroutineStarted = true;
        yield return new WaitForSeconds(seconds);        
        level += 1;
        tilemapRenderer.ClearTiles();
        tilemapRenderer.Map = LoadLevel((Level)(level));
        tilemapRenderer.Map.won = false;
        tilemapRenderer.winText.SetActive(false);
        player.active = true;
		winCoroutineStarted = false;

    }

    public static Tilemap LoadLevel(Level level)
    {
        Tilemap Map;
        switch (level)
        {
            case Level.Sample:
                Map = new Tilemap(16, 16);
                Map.SetBoxTiles(0, 8, 15, 8, Tile.TileType.WALL);
                Map.SetTile(7, 9, Tile.TileType.DOOR);
                Map.tiles[7, 9].buttonPos = new Vector2Int(4, 9);
                Map.SetTile(6, 9, Tile.TileType.EMPTY, true);
                Map.SetTile(4, 9, Tile.TileType.BUTTONBLUE, true);
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

            case Level.Level2:
                Debug.Log("Loading level 2");
                Map = new Tilemap(16, 16);
                Map.buttonsToWin = 1;
                Map.SetBoxTiles(0, 8, 8, 8, Tile.TileType.WALL);
                Map.SetTile(1, 8, Tile.TileType.DOOR);
                Map.tiles[1, 8].buttonPos = new Vector2Int(2, 9);
                Map.SetTile(2, 9, Tile.TileType.EMPTY, true);
                Map.SetTile(2, 10, Tile.TileType.DOOR, false);
                Map.tiles[2, 10].buttonPos = new Vector2Int(3, 9);
                Map.SetTile(2, 11, Tile.TileType.EMPTY, true);
                Map.SetTile(3, 9, Tile.TileType.BUTTONBLUE);
                Map.SetTile(4, 9, Tile.TileType.BUTTONBLUE);
                Map.SetBoxTiles(0, 5, 8, 5, Tile.TileType.WALL);

                break;
            default:
                Debug.Log("Attempted to load invalid level");
                Map = null;
                break;
        }
        return Map;
    }
}
