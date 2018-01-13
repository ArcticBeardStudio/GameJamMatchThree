using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class TileTests {
    [UnityTest]
    public IEnumerator Tile_Pop_IsDestroyed() {
        GameObject go = new GameObject();
        Tile tile = go.AddComponent<Tile>();
        tile.Pop();

        yield return null;

        Assert.False(go);
        Assert.False(tile);
    }
}
