using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStates
{
    PrePlay,
    Playing,
    Waiting,
    PostPlay
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; protected set; }

    public GameStates currentGameState;

    public List<Player> players = new List<Player>();

    public static Player[] GetPlayers() { return null; }
    public static Player GetPlayer(int index) { return null; }
    public static Player GetCurrentPlayer() { return null; }

    public bool IsPlaying() { return false; }
    public bool CanPlayerMove(int index) { return false; }

    public static GameManager CreateGameManager()
    {
        GameObject go = new GameObject("GameManager");
        GameManager gameManager = go.AddComponent<GameManager>();

        gameManager.Init();

        Debug.Assert(instance == null, "There can only be one GameManager (to rule them all)");
        instance = gameManager;

        return gameManager;
    }

    public static Player CreatePlayer()
    {
        Debug.Assert(instance != null, "GameManager must be created before a Player can be created");

        GameObject go = new GameObject("Player" + instance.players.Count);
        Player player = go.AddComponent<Player>();

        player.Init(instance.players.Count);
        instance.players.Add(player);

        return player;
    }

    void Init()
    {
        currentGameState = GameStates.PrePlay;
    }

    void Awake() 
    {
        CreateGameManager();
    }
}
