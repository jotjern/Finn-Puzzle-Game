using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public TilemapManager tilemapRenderer;
    public SceneMgr sceneMgr;
    public CatController player;
    public int level = -1;

    private bool winCoroutineStarted = false;

    public enum Level
    {
        Sample,
        Tutorial1,
        Tutorial2,
        Level1,
        Level2,
        Level3,
        Level4,
        Level5,
        Level6,
        EndLevel,
        GameFinish
    }
		
    void Start () {
        level = SceneMgr.startLevel;
        LoadLevel(level);
    }

    void Update () {
        if (!winCoroutineStarted && tilemapRenderer.GetTilemap().isWon())
        {
            StartCoroutine(WinInSeconds(3f));
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }

        if (Input.GetKeyDown (KeyCode.N)) {
            StartCoroutine(WinInSeconds(0f));
        }
        if (Input.GetKeyDown (KeyCode.P)) {
            StartCoroutine(WinInSeconds(0f, false));
        }
    }

    public void RestartLevel()
    {
        LoadLevel(level);
    }

    void LoadLevel(Level level)
    {
        if (level == Level.GameFinish)
        {
            sceneMgr.LoadMenuScene();
            return;
        }
        tilemapRenderer.pauseUpdates = true;
        tilemapRenderer.ClearTiles();
        tilemapRenderer.setLevel(_LoadLevel(level));
        tilemapRenderer.winText.SetActive(false);
        player.rb.velocity = Vector2.zero;
        player.active = true;
        player.timeSinceLevelLoad = 0;
        tilemapRenderer.pauseUpdates = false;
        player.transform.position = tilemapRenderer.GetTilemap().playerStartPos;
        player.startPosition = tilemapRenderer.GetTilemap().playerStartPos;
    }

    void LoadLevel(int level)
    {
        LoadLevel((Level)level);
    }

    IEnumerator WinInSeconds(float seconds, bool forward = true)
    {
        winCoroutineStarted = true;
        yield return new WaitForSeconds(seconds);        
        level += forward ? 1 : -1;
        LoadLevel(level);
        winCoroutineStarted = false;

    }

    public static Tilemap _LoadLevel(Level level)
    {
        Tilemap Map;
        switch (level)
        {
            case Level.Sample:
                Map = new Tilemap (16, 16, 1);
                Map.addDoor (new Vector2Int (4, 9), new Vector2Int (7, 9));
                Map.SetBoxTiles(0, 8, 15, 8, Tile.TileType.WALL);
                Map.tiles[7, 9].buttonPos = new Vector2Int(4, 9);
                Map.SetTile(6, 9, Tile.TileType.EMPTY, true);
                break;
            case Level.Tutorial1:
                Map = new Tilemap(16, 16, 1, new Vector2Int(0, 12));
                Map.SetBoxTiles(0, 10, 5, 10, Tile.TileType.WALL);
                Map.SetTile(2, 11, Tile.TileType.EMPTY, true);
                Map.SetTile(4, 11, Tile.TileType.BUTTON);

                break;
            case Level.Tutorial2:
                Map = new Tilemap(16, 16, 1, new Vector2Int(0, 12));
                Map.SetBoxTiles(0, 10, 5, 10, Tile.TileType.WALL);
                Map.SetTile(2, 11, Tile.TileType.EMPTY, true);
                Map.addDoor(new Vector2Int(4, 11), new Vector2Int(7, 13));
                Map.SetTile(7, 14, Tile.TileType.EMPTY, true);
                Map.SetTile(7, 7, Tile.TileType.BUTTON);
                Map.SetTile(7, 6, Tile.TileType.WALL);

                break;
            case Level.Level1:
                Map = new Tilemap(20, 20, 2, new Vector2(1, 9));
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
                Map = new Tilemap(16, 16, 2, new Vector2(1, 9));
                Map.addDoor (new Vector2Int (5, 9), new Vector2Int (5, 10));
                Map.SetBoxTiles(0, 8, 15, 8, Tile.TileType.WALL);
                Map.SetTile(3, 9, Tile.TileType.EMPTY, true);
                Map.SetTile(5, 10, Tile.TileType.DOOR);
                Map.tiles[5, 10].buttonPos = new Vector2Int(5, 9);
                Map.SetTile(5, 11, Tile.TileType.EMPTY, true);
                Map.SetTile(6, 9, Tile.TileType.BUTTON);
                Map.SetTile(7, 9, Tile.TileType.BUTTON);

                break;
            case Level.Level3:
                Map = new Tilemap (16, 16, 3, new Vector2 (1, 9));
                Map.SetBoxTiles (0, 6, 7, 6, Tile.TileType.WALL);
                Map.SetBoxTiles (3, 7, 3, 8, Tile.TileType.EMPTY, true);
                Map.addDoor (new Vector2Int (11, 7), new Vector2Int (3, 6));
                Map.tiles[3, 6].buttonPos = new Vector2Int(11, 7);
                Map.SetBoxTiles(9, 6, 12, 6, Tile.TileType.WALL);
                Map.SetTile(9, 7, Tile.TileType.BUTTON);
                Map.SetTile(10, 7, Tile.TileType.EMPTY, true);
                Map.SetTile(7, 7, Tile.TileType.BUTTON);
                Map.SetBoxTiles(2, 4, 4, 4, Tile.TileType.WALL);
                Map.SetTile(3, 5, Tile.TileType.BUTTON);

                break;

            case Level.Level4:
                Map = new Tilemap (20, 4, 1);
                Map.addDoor (new Vector2Int (3, 2), new Vector2Int (7, 2), new Vector2Int (12, 1), new Vector2Int (14, 2));
                Map.SetBoxTiles (0, 1, 6, 1, Tile.TileType.WALL);
                Map.SetBoxTiles (8, 1, 10, 1, Tile.TileType.WALL);
                Map.SetTile (13, 1, Tile.TileType.WALL);
                Map.SetTile (15, 1, Tile.TileType.WALL);
                Map.SetTile (17, 1, Tile.TileType.WALL);
                Map.SetTile (17, 2, Tile.TileType.BUTTON);
                Map.SetBoxTiles (0, 0, 17, 0, Tile.TileType.WALL);
                Map.tiles [3, 2].box = new Box (1);
                Map.tiles [5, 2].box = new Box (2);


                break;
            case Level.Level5:
                Map = new Tilemap(16, 16, 1, new Vector2Int(3, 12));
                Map.SetBoxTiles(0, 8, 5, 8, Tile.TileType.WALL);
                Map.SetTile(6, 4, Tile.TileType.WALL);
                Map.SetTile(5, 4, Tile.TileType.WALL);
                Map.SetBoxTiles(7, 5, 9, 5, Tile.TileType.WALL);
                Map.SetTile(9, 3, Tile.TileType.WALL);
                Map.SetTile(12, 3, Tile.TileType.WALL);
                Map.SetTile(10, 2, Tile.TileType.WALL);
                Map.SetTile(11, 2, Tile.TileType.WALL);
                Map.SetTile(10, 3, Tile.TileType.BUTTON);
                Map.addDoor(new Vector2Int(9, 12), new Vector2Int(2, 9), new Vector2Int(11, 6));
                Map.SetTile(8, 12, Tile.TileType.EMPTY, true);
                Map.SetBoxTiles(7, 11, 12, 11, Tile.TileType.WALL);
                Map.SetTile(3, 10, Tile.TileType.WALL);
                Map.tiles[3, 11].box = new Box(2);
                Map.SetTile(5, 11, Tile.TileType.WALL);
                break;

            case Level.Level6:
                Map = new Tilemap(16, 16, 2, new Vector2Int(8, 10));

                Map.SetTile(4, 13, Tile.TileType.EMPTY, true);
                Map.tiles[6, 12].box = new Box(2);
                Map.SetTile(4, 12, Tile.TileType.WALL);
                Map.SetTile(3, 12, Tile.TileType.WALL);
                Map.SetTile(6, 11, Tile.TileType.WALL);
                Map.SetTile(7, 11, Tile.TileType.WALL);
                Map.SetTile(4, 9, Tile.TileType.WALL);
                Map.SetTile(4, 8, Tile.TileType.WALL);
                Map.SetTile(6, 8, Tile.TileType.WALL);
                Map.SetTile(7, 9, Tile.TileType.WALL);
                Map.SetTile(8, 9, Tile.TileType.WALL);
                Map.addDoor(new Vector2Int(7, 10), new Vector2Int(5, 8));
                Map.SetTile(4, 5, Tile.TileType.WALL);
                Map.SetBoxTiles(7, 5, 11, 5, Tile.TileType.WALL);
                Map.SetBoxTiles(4, 4, 7, 4, Tile.TileType.WALL);
                Map.SetTile(7, 6, Tile.TileType.BUTTON);
                Map.SetTile(5, 5, Tile.TileType.BUTTON);

                break;

            case Level.EndLevel:
                Map = new Tilemap (50, 50, 1);

                Map.SetBoxTiles (0, 0, 49, 0, Tile.TileType.WALL);


                // F
                Map.SetBoxTiles (2, 2, 2, 5, Tile.TileType.WALL);
                Map.SetBoxTiles (2, 6, 6, 6, Tile.TileType.WALL);
                Map.SetBoxTiles (2, 4, 4, 4, Tile.TileType.WALL);


                // i
                Map.SetBoxTiles (8, 2, 8, 4, Tile.TileType.WALL);
                Map.SetBoxTiles (8, 6, 8, 6, Tile.TileType.WALL);

                // n
                Map.SetBoxTiles (10, 2, 10, 4, Tile.TileType.WALL);
                Map.SetBoxTiles (11, 5, 12, 5, Tile.TileType.WALL); 
                Map.SetBoxTiles (13, 2, 13, 4, Tile.TileType.WALL);

                // n
                Map.SetBoxTiles (15, 2, 15, 4, Tile.TileType.WALL);
                Map.SetBoxTiles (16, 5, 17, 5, Tile.TileType.WALL); 
                Map.SetBoxTiles (18, 2, 18, 4, Tile.TileType.WALL);

                // .

                Map.SetTile (20, 2, Tile.TileType.BUTTON);
                Map.SetTile (20, 1, Tile.TileType.WALL);
                Map.tiles [20, 21].box = new Box ();

                // n
                Map.SetBoxTiles (22, 2, 22, 4, Tile.TileType.WALL);
                Map.SetBoxTiles (23, 5, 24, 5, Tile.TileType.WALL); 
                Map.SetBoxTiles (25, 2, 25, 4, Tile.TileType.WALL);

                Map.tiles [24, 1].box = new Box ();
                Map.addDoor (new Vector2Int (25, 1), new Vector2Int (20, 20));

                // o
                Map.SetBoxTiles (27, 3, 27, 4, Tile.TileType.WALL);
                Map.SetBoxTiles (28, 5, 29, 5, Tile.TileType.WALL); 
                Map.SetBoxTiles (28, 2, 29, 2, Tile.TileType.WALL); 
                Map.SetBoxTiles (30, 3, 30, 4, Tile.TileType.WALL);
             


                break;
            default:
                Debug.Log("Attempted to load invalid level");
                Map = null;
                break;
        }
        return Map;
    }
}
