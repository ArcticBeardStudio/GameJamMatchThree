using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TileTests {
    [Test]
    public void Tile_Pop_IsDestroyed() {
        GameObject go = new GameObject();
        Tile tile = go.AddComponent<Tile>();
        tile.Pop();

        Assert.IsNull(go);
    }
}
