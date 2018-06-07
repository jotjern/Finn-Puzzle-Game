using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public TilemapManager tilemapRenderer;
    public CatController player;
    public Vector2[] startPositions;
    public int level = -1;

    private bool winCoroutineStarted = false;

    public enum Level
    {
        Sample,
        Level1,
        Level2,
        Level3,
    }

	void Start () {
        level = SceneMgr.startLevel;
        LoadLevel(level);
    }

    void Update () {
        if (!winCoroutineStarted && tilemapRenderer.Map.won)
        {
            StartCoroutine(WinInSeconds(3f));
        }
        if (Input.GetKeyDown("r"))
        {
            RestartLevel();
        }
	}

    public void RestartLevel()
    {
        LoadLevel(level);
    }

    void LoadLevel(Level level)
    {
        tilemapRenderer.pauseUpdates = true;
        tilemapRenderer.ClearTiles();
        tilemapRenderer.Map = _LoadLevel(level);
        tilemapRenderer.winText.SetActive(false);
        player.transform.position = startPositions[(int)level];
        player.rb.velocity = Vector2.zero;
        player.active = true;
        player.timeSinceLevelLoad = 0;
        tilemapRenderer.pauseUpdates = false;
    }

    void LoadLevel(int level)
    {
        LoadLevel((Level)level);
    }

    IEnumerator WinInSeconds(float seconds)
    {
        winCoroutineStarted = true;
        yield return new WaitForSeconds(seconds);        
        level += 1;
        LoadLevel(level);
		winCoroutineStarted = false;

    }

    public static Tilemap _LoadLevel(Level level)
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
                Map.buttonsToWin = 2;
                Map.SetBoxTiles(0, 8, 15, 8, Tile.TileType.WALL);
                Map.SetTile(3, 9, Tile.TileType.EMPTY, true);
                Map.SetTile(5, 10, Tile.TileType.DOOR);
                Map.tiles[5, 10].buttonPos = new Vector2Int(5, 9);
                Map.SetTile(5, 11, Tile.TileType.EMPTY, true);
                Map.SetTile(5, 9, Tile.TileType.BUTTONBLUE);
                Map.SetTile(6, 9, Tile.TileType.BUTTON);
                Map.SetTile(7, 9, Tile.TileType.BUTTON);

                break;
            case Level.Level3:
                Debug.Log("Loading level 3");
                Map = new Tilemap(16, 16);
                Map.buttonsToWin = 3;
                Map.SetBoxTiles(0, 6, 7, 6, Tile.TileType.WALL);
                Map.SetTile(3, 6, Tile.TileType.DOOR);
                Map.SetBoxTiles(3, 7, 3, 8, Tile.TileType.EMPTY, true);
                Map.tiles[3, 6].buttonPos = new Vector2Int(11, 7);
                Map.SetBoxTiles(9, 6, 12, 6, Tile.TileType.WALL);
                Map.SetTile(9, 7, Tile.TileType.BUTTON);
                Map.SetTile(11, 7, Tile.TileType.BUTTONBLUE);
                Map.SetTile(10, 7, Tile.TileType.EMPTY, true);
                Map.SetTile(7, 7, Tile.TileType.BUTTON);
                Map.SetBoxTiles(2, 4, 4, 4, Tile.TileType.WALL);
                Map.SetTile(3, 5, Tile.TileType.BUTTON);

                break;

            default:
                Debug.Log("Attempted to load invalid level");
                Map = null;
                break;
        }
        return Map;
    }
}
