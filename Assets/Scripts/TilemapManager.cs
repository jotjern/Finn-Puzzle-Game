using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapManager : MonoBehaviour {

    public Tilemap Map;
    public GameObject crate;
    public GameObject tile;
    public int UpdatesPerSecond;
    public Sprite[] tileTypeSprites;

    private List<GameObject> boxes = new List<GameObject>();
    private List<GameObject> tiles = new List<GameObject>();
    private float timeSinceLastTilemapStep = Mathf.Infinity;

	void Start () {
		
	}

    void RenderMap()
    {
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
        }
        for (int i = 0; i < boxPositions.Count; i++)
        {
            boxes[i].transform.position = new Vector2(boxPositions[i].x, boxPositions[i].y);
        }
        while (tiles.Count < Map.tiles.Length)
        {
            GameObject newTile = Instantiate(tile);
            tiles.Add(newTile);
            newTile.transform.parent = transform;
        }
        while (tiles.Count > Map.tiles.Length)
        {
            Destroy(tiles[0]);
        }
        int n = 0;
        for (int x = 0; x < Map.tiles.GetLength(0); x++)
        {
            for (int y = 0; y < Map.tiles.GetLength(1); y++)
            {
                bool active = Map.tiles[x, y].type == Tile.TileType.EMPTY;
                tiles[n].SetActive(active);
                if (!active)
                    continue;
                tiles[n].transform.position = new Vector2(x, y);
                tiles[n].GetComponent<SpriteRenderer>().sprite = tileTypeSprites[(int)Map.tiles[x, y].type];
                n++;
            }
        }
    }
	
	void Update () {
        timeSinceLastTilemapStep += Time.deltaTime;
        if (timeSinceLastTilemapStep > 1f / UpdatesPerSecond)
        {
            timeSinceLastTilemapStep = 0;
            Map.Step();
            RenderMap();
        }
	}
}
