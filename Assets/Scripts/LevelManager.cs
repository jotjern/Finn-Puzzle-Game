using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public TilemapManager tilemapRenderer;

	void Start () {
        Tilemap currentLevel = new Tilemap(8, 8);
        currentLevel.tiles[0, 7].box = new Box();
        currentLevel.tiles[0, 3] = new Tile(Tile.TileType.WALL);
        tilemapRenderer.Map = currentLevel;
	}
	
	void Update () {
		
	}
}
