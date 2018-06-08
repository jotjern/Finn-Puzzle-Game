using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TilemapManager : MonoBehaviour {

    private Tilemap Map;
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

	void Start () {
		
	}

    public void setLevel(Tilemap m) {
        Map = m;

        List<Vector2Int> boxPositions = Map.GetBoxPositions();


        ClearTiles ();

        for (int i = 0; i < boxPositions.Count; i++)
        {
            GameObject newBox = Instantiate(crate);
            boxes.Add(newBox);
            newBox.transform.parent = transform;
            Box b = Map.tiles [boxPositions [i].x, boxPositions [i].y].box;

            newBox.gameObject.GetComponentInChildren<TMP_Text> ().text = b.steps == 1 ? "" : string.Format("{0}", b.steps);

            b.SetRepresentation (newBox);
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
    }

    public Tilemap GetTilemap() {
        return Map;
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
            

        int n = 0;

        for (int x = 0; x < Map.tiles.GetLength(0); x++)
        {
            for (int y = 0; y < Map.tiles.GetLength(1); y++)
            {

                Box b = Map.tiles [x, y].box;
                if (b != null) {
                    b.GetRepresentation().transform.position = b.GetPos(new Vector2 (x, y));
                }

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

    IEnumerator PlaySound(AudioClip sound, AudioSource src)
    {
        yield return new WaitForSeconds(Box.ACTIONTIME);  
        src.clip = sound;
        src.Play ();
    }


	
	void Update () {
        background.transform.position = new Vector3(Map.tiles.GetLength(0) / 2, Map.tiles.GetLength(1) / 2, 1f);
        background.transform.localScale = new Vector3(Map.tiles.GetLength(0), Map.tiles.GetLength(1), 1f);
        HashSet<Tilemap.GAME_EVENT> events = Map.Step(Time.deltaTime);
        if (events.Contains (Tilemap.GAME_EVENT.BOX_FALL)) {
            StartCoroutine (PlaySound (fallSound, transform.Find ("fall").gameObject.GetComponent<AudioSource> ()));
        }
        if (events.Contains (Tilemap.GAME_EVENT.BUTTON_PRESS)) {
            StartCoroutine (PlaySound (pressButtonSound, transform.Find ("click").gameObject.GetComponent<AudioSource> ()));
        }
        RenderMap();
        if (Map.isWon())
        {
            player.active = false;
            winText.SetActive(true);
        }
	}
}
