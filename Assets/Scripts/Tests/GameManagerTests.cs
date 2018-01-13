using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class GameManagerTests
{
    [Test]
    public void GameObject_CreatePlayer_InstanceValid()
    {
        GameManager.CreateGameManager();
        Player player = GameManager.CreatePlayer();

        Assert.AreSame(player, GameManager.instance.players[GameManager.instance.players.Count - 1]);
    }

    [Test]
    public void GameObject_CreateGameManager_InstanceValid()
    {
        GameManager gameManager = GameManager.CreateGameManager();
        Assert.AreSame(gameManager, GameManager.instance);
    }
}
