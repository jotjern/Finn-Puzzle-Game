using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
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
        tilemap.tiles[0, 0] = new Tile(Tile.TileType.DOOR);
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
    public void TestBoxButton()
    {
        Tilemap tilemap = new Tilemap(2, 2);
        tilemap.tiles[0, 0] = new Tile(Tile.TileType.BUTTON);
        tilemap.tiles[0, 1].box = new Box();
        tilemap.Step();
        Assert.AreEqual(1, tilemap.tiles[0, 0].state, tilemap.ToString());
    }
}
