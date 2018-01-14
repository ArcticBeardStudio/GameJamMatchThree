using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class GameManagerTests
{
    [Test]
    public void GameObject_CreateGameManager_InstanceValid()
    {
        GameManager gameManager = GameManager.CreateGameManager();
        
        Assert.IsTrue(gameManager);
        Assert.AreSame(gameManager, GameManager.instance);
    }

    [Test]
    public void GameObject_CreatePlayer_InstanceValid()
    {
        GameManager.CreateGameManager();
        Player player = GameManager.CreatePlayer();

        Assert.IsTrue(player);
        Assert.AreSame(player, GameManager.instance.players[player.index]);
    }

    [Test]
    public void GameObject_CreateBoard_InstanceValid()
    {
        GameManager.CreateGameManager();
        Player player = GameManager.CreatePlayer();
        Board board = GameManager.CreateBoard(player.index);

        Assert.IsTrue(board);
        Assert.AreSame(board, player.board);
    }
}
