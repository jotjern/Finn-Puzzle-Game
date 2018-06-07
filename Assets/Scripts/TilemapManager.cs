using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TilemapManager : MonoBehaviour {

    public Tilemap Map;
    public GameObject crate;
    public GameObject tile;
    public int UpdatesPerSecond;
    public GameObject background;
    public Sprite buttonPressedSprite;
    public Sprite buttonNotPressedSprite;
    public Sprite wallSprite;
    public Sprite doorSprite;
    public Sprite doorSpriteOpen;
    public Sprite buttonPressedSpriteBlue;
	public Sprite buttonNotPressedSpriteBlue;
    public CatController player;
    public GameObject winText;
    public bool pauseUpdates = false;
    public AudioClip pressButtonSound, fallSound;

    private List<GameObject> boxes = new List<GameObject>();
    private List<GameObject> tiles = new List<GameObject>();
    private float timeSinceLastTilemapStep = Mathf.Infinity;

	void Start () {
		
	}

    public Vector2Int GetTilePosFromTransformPos(Vector3 pos)
    {
        return new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
    }

    public void ClearTiles()
    {
        foreach (GameObject tile in tiles)
        {
            Destroy(tile);
        }
        foreach (GameObject box in boxes)
        {
            Destroy(box);
        }
        tiles = new List<GameObject>();
        boxes = new List<GameObject>();
    }

    void RenderMap()
    {
        if (pauseUpdates)
        {
            return;
        }
        List<Vector2Int> boxPositions = Map.GetBoxPositions();
        while (boxes.Count < boxPositions.Count)
        {
            GameObject newBox = Instantiate(crate);
            boxes.Add(newBox);
            newBox.transform.parent = transform;
        }
        while (boxes.Count > boxPositions.Count)
        {
            Destroy(boxes[0]);
            boxes.RemoveAt(0);
        }

        for (int i = 0; i < boxPositions.Count; i++)
        {
            boxes[i].transform.position = new Vector2(boxPositions[i].x, boxPositions[i].y);
			int steps = Map.tiles [boxPositions [i].x, boxPositions [i].y].box.steps;

			boxes [i].gameObject.GetComponentInChildren<TMP_Text> ().text = steps == 1 ? "" : string.Format("{0}", steps);
        }
        int emptyTiles = 0;
        for (int x = 0; x < Map.tiles.GetLength(0); x++)
        {
            for (int y = 0; y < Map.tiles.GetLength(1); y++)
            {
                if (Map.tiles[x, y].type == Tile.TileType.EMPTY)
                {
                    emptyTiles += 1;
                }
            }
        }
        while (tiles.Count + emptyTiles < Map.tiles.Length)
        {
            GameObject newTile = Instantiate(tile);
            tiles.Add(newTile);
            newTile.transform.parent = transform;
        }
        while (tiles.Count + emptyTiles > Map.tiles.Length)
        {
            Destroy(tiles[0]);
            tiles.RemoveAt(0);
        }
        int n = 0;
        for (int x = 0; x < Map.tiles.GetLength(0); x++)
        {
            for (int y = 0; y < Map.tiles.GetLength(1); y++)
            {
                if (Map.tiles[x, y].type == Tile.TileType.EMPTY)
                    continue;
                if (Map.tiles[x, y].type == Tile.TileType.BUTTON || Map.tiles[x, y].type == Tile.TileType.BUTTONBLUE)
                    tiles[n].GetComponent<Collider2D>().enabled = false;
                else
                    tiles[n].GetComponent<Collider2D>().enabled = true;
                tiles[n].name = Map.tiles[x, y].type.ToString();
                bool active = Map.tiles[x, y].type != Tile.TileType.EMPTY;
                tiles[n].transform.position = new Vector2(x, y);
                SpriteRenderer renderer = tiles[n].GetComponent<SpriteRenderer>();
                Collider2D collider = tiles[n].GetComponent<Collider2D>();
                n++;
                switch (Map.tiles[x, y].type)
                {
                    case Tile.TileType.BUTTON:
                        if (Map.tiles[x, y].box != null)
                        {
                            renderer.sprite = buttonPressedSprite;
                        } else
                        {
                            renderer.sprite = buttonNotPressedSprite;
                        }
                        break;
                    case Tile.TileType.BUTTONBLUE:
                        if (Map.tiles[x, y].box != null)
                        {
                            renderer.sprite = buttonPressedSpriteBlue;
                        }
                        else
                        {
                            renderer.sprite = buttonNotPressedSpriteBlue;
                        }
                        break;
                    case Tile.TileType.DOOR:
                        Vector2Int buttonPos = Map.tiles[x, y].buttonPos;
                        if (Map.tiles[buttonPos.x, buttonPos.y].box == null)
                        {
                            renderer.sprite = doorSprite;
                            collider.enabled = true;
                        } else
                        {
                            renderer.sprite = doorSpriteOpen;
                            collider.enabled = false;
                        }
                        break;
                    case Tile.TileType.WALL:
                        renderer.sprite = wallSprite;
                        break;
                }
            }
        }
    }
	
	void Update () {
        timeSinceLastTilemapStep += Time.deltaTime;
        if (timeSinceLastTilemapStep > 1f / UpdatesPerSecond)
        {
            background.transform.position = new Vector3(Map.tiles.GetLength(0) / 2, Map.tiles.GetLength(1) / 2, 1f);
            background.transform.localScale = new Vector3(Map.tiles.GetLength(0), Map.tiles.GetLength(1), 1f);
            timeSinceLastTilemapStep = 0;
            HashSet<Tilemap.GAME_EVENT> events = Map.Step();
            if (events.Contains (Tilemap.GAME_EVENT.BOX_FALL)) {
                GameObject go = transform.Find ("fall").gameObject;
                go.GetComponent<AudioSource> ().clip = fallSound;
                go.GetComponent<AudioSource> ().Play ();
            }
            if (events.Contains (Tilemap.GAME_EVENT.BUTTON_PRESS)) {
                GameObject go = transform.Find ("click").gameObject;
                go.GetComponent<AudioSource> ().clip = pressButtonSound;
                go.GetComponent<AudioSource> ().Play ();  
            }
            RenderMap();
            if (Map.isWon())
            {
                player.active = false;
                winText.SetActive(true);
            }
        }
	}
}
