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
        tilemapRenderer.Map = LoadLevel(Level.Sample);
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
                Map.SetTile(0, 0, Tile.TileType.WALL);
                Map.SetTile(0, 1, Tile.TileType.BUTTON);
                Map.SetTile(0, 2, Tile.TileType.EMPTY, true);
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
                Map = new Tilemap(5, 5);
                Map.buttonsToWin = 1;
                Map.SetBoxTiles(0, 0, 4, 0, Tile.TileType.WALL);
                Map.SetTile(0, 1, Tile.TileType.EMPTY, true);
                Map.SetTile(0, 1, Tile.TileType.BUTTON, false);
                break;
            default:
                Debug.Log("Attempted to load invalid level");
                Map = null;
                break;
        }
        return Map;
    }
}
