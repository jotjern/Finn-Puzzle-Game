using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TilemapTests {
    [Test]
    public void TestGravity()
    {
        Tilemap tilemap = new Tilemap(2, 2);
        tilemap.tiles[0, 1].box = new Box();
        tilemap.Step();
        Assert.IsNotNull(tilemap.tiles[0, 0].box);
        Assert.IsNull(tilemap.tiles[0, 1].box);
    }
    [Test]
    public void TestGravityBoxCollision()
    {
        Tilemap tilemap = new Tilemap(2, 2);
        tilemap.tiles[0, 1].box = new Box();
        tilemap.tiles[0, 0].box = new Box();
        tilemap.Step();
        Assert.IsNotNull(tilemap.tiles[0, 0].box, tilemap.ToString());
        Assert.IsNotNull(tilemap.tiles[0, 1].box, tilemap.ToString());
        Assert.AreNotSame(tilemap.tiles[0, 0].box, tilemap.tiles[0, 1].box, tilemap.ToString());
    }
    [Test]
    public void TestGravityWallCollision()
    {
        Tilemap tilemap = new Tilemap(2, 2);
        tilemap.tiles[0, 1].box = new Box();
        tilemap.tiles[0, 0] = new Tile(Tile.TileType.WALL);
        tilemap.Step();
        Assert.IsNotNull(tilemap.tiles[0, 1].box, tilemap.ToString());
        Assert.IsNull(tilemap.tiles[0, 0].box, tilemap.ToString());
    }

    [Test]
    public void TestGravityDoorCollision()
    {
        Tilemap tilemap = new Tilemap(2, 2);
        tilemap.tiles[0, 1].box = new Box();
        tilemap.tiles[0, 0] = new Tile(Tile.TileType.WALL);
        tilemap.Step();
        Assert.IsNotNull(tilemap.tiles[0, 1].box, tilemap.ToString());
        Assert.IsNull(tilemap.tiles[0, 0].box, tilemap.ToString());
    }

    [Test]
    public void TestPush()
    {
        Tilemap tilemap = new Tilemap(2, 2);
        tilemap.tiles[0, 0].box = new Box();
        tilemap.PushBox(0, 0, true);
        Assert.IsNull(tilemap.tiles[0, 0].box, tilemap.ToString());
        Assert.IsNotNull(tilemap.tiles[1, 0].box, tilemap.ToString());
    }

    [Test]
    public void TestLoadLevels()
    {
        foreach (LevelManager.Level level in System.Enum.GetValues(typeof(LevelManager.Level)))
        {
            Assert.IsNotNull(LevelManager._LoadLevel(level));
        }
    }
    [Test]
    public void TestPushRightOutBounds()
    {
        Tilemap tilemap = new Tilemap(2, 2);
        tilemap.tiles[1, 0].box = new Box();
        tilemap.PushBox(1, 0, true);
        Assert.IsNotNull(tilemap.tiles[1, 0].box, tilemap.ToString());
    }
    [Test]
    public void TestPushLeftOutBounds()
    {
        Tilemap tilemap = new Tilemap(2, 2);
        tilemap.tiles[0, 0].box = new Box();
        tilemap.PushBox(0, 0, false);
        Assert.IsNotNull(tilemap.tiles[0, 0].box, tilemap.ToString());
    }

    [Test]
    public void TestDoorPassThrough()
    {
        Tilemap tilemap = new Tilemap(16, 16);
        tilemap.SetBoxTiles(0, 8, 15, 8, Tile.TileType.WALL);
        tilemap.SetTile(7, 9, Tile.TileType.DOOR);
        tilemap.tiles[7, 9].buttonPos = new Vector2Int(4, 9);
        tilemap.SetTile(6, 9, Tile.TileType.EMPTY, true);
        tilemap.SetTile(4, 9, Tile.TileType.BUTTONBLUE);
        tilemap.SetTile(4, 10, Tile.TileType.EMPTY, true);
        tilemap.Step();
        tilemap.PushBox(6, 9, true);
        Assert.IsNotNull(tilemap.tiles[7, 9].box, tilemap.ToString());
        Assert.IsNull(tilemap.tiles[6, 9].box, tilemap.ToString());
    }

    [Test]
    public void TestDoorNotPassThrough()
    {
        Tilemap tilemap = new Tilemap(16, 16);
        tilemap.SetBoxTiles(0, 8, 15, 8, Tile.TileType.WALL);
        tilemap.SetTile(7, 9, Tile.TileType.DOOR);
        tilemap.tiles[7, 9].buttonPos = new Vector2Int(4, 9);
        tilemap.SetTile(6, 9, Tile.TileType.EMPTY, true);
        tilemap.SetTile(4, 9, Tile.TileType.BUTTONBLUE);
        tilemap.Step();
        tilemap.PushBox(6, 9, true);
        Assert.IsNull(tilemap.tiles[7, 9].box, tilemap.ToString());
        Assert.IsNotNull(tilemap.tiles[6, 9].box, tilemap.ToString());
    }

	[Test]
	public void TestPushTwoStep()
	{
		Tilemap tilemap = new Tilemap(3, 3);
		tilemap.tiles[0, 0].box = new Box(2);
		tilemap.PushBox(0, 0, true);
		Assert.IsNull(tilemap.tiles[0, 0].box, tilemap.ToString());
		Assert.IsNotNull(tilemap.tiles[2, 0].box, tilemap.ToString());
	}

	[Test]
	public void TestPushTwoStepStop()
	{
		Tilemap tilemap = new Tilemap(3, 3);
		tilemap.tiles[0, 0].box = new Box(2);
		tilemap.tiles[2, 0].type = Tile.TileType.WALL;
		tilemap.PushBox(0, 0, true);
		Assert.IsNull(tilemap.tiles[0, 0].box, tilemap.ToString());
		Assert.IsNotNull(tilemap.tiles[1, 0].box, tilemap.ToString());
	}

	[Test]
	public void TestPushTwoStepBoxCollide()
	{
		Tilemap tilemap = new Tilemap(3, 3);
		tilemap.tiles[0, 0].box = new Box(2);
		tilemap.tiles[2, 0].box = new Box(2);
		tilemap.PushBox(0, 0, true);
		Assert.IsNull(tilemap.tiles[0, 0].box, tilemap.ToString());
		Assert.IsNotNull(tilemap.tiles[1, 0].box, tilemap.ToString());
	}

	[Test]
	public void TestPushTwoStepBounds()
	{
		Tilemap tilemap = new Tilemap(2, 3);
		tilemap.tiles[0, 0].box = new Box(2);
		tilemap.PushBox(0, 0, true);
		Assert.IsNull(tilemap.tiles[0, 0].box, tilemap.ToString());
		Assert.IsNotNull(tilemap.tiles[1, 0].box, tilemap.ToString());
	}

    [Test]
    public void TestPushTwoStepBoxCollideNeighbours()
    {
        Tilemap tilemap = new Tilemap(3, 3);
        tilemap.tiles[0, 0].box = new Box(2);
        tilemap.tiles[1, 0].box = new Box(2);
        tilemap.PushBox(0, 0, true);
        Assert.IsNotNull(tilemap.tiles[0, 0].box, tilemap.ToString());
        Assert.IsNotNull(tilemap.tiles[1, 0].box, tilemap.ToString());
    }
}
