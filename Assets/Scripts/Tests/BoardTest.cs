using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class BoardTest
{

    [Test]
    public void BoardTestSimplePasses()
    {
        Assert.AreEqual("Bajs", "Bajs");
    }

    [Test]
    public void Board_TryInitAndReturnTiletype_WillReturnTileinfo()
    {
        var BoardGameObject = new GameObject("Board");
        var board = BoardGameObject.AddComponent<Board>();
        //board.Init();

        Debug.Log(board.GetTileType(board.width - 1, board.height - 1));
        Assert.AreEqual(typeof(TileTypes), board.GetTileType(board.width - 1, board.height - 1).GetType());
    }

    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
    public IEnumerator BoardTestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // yield to skip a frame
        yield return null;
    }
}