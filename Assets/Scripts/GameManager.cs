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
        if (instance) return instance;

        GameObject go = new GameObject("GameManager");
        GameManager gameManager = go.AddComponent<GameManager>();

        return gameManager;
    }

    public static Player CreatePlayer()
    {
        Debug.Assert(instance, "GameManager must be created before a Player can be created");

        GameObject go = new GameObject("Player" + instance.players.Count);
        Player player = go.AddComponent<Player>();

        player.Init(instance.players.Count);
        instance.players.Add(player);

        return player;
    }

    public static Board CreateBoard(int playerIndex)
    {
        Debug.Assert(instance, "GameManager must be created before a Board can be created");
        Debug.AssertFormat(playerIndex < instance.players.Count, "Player index {0} exceeds player count", playerIndex);
        Debug.AssertFormat(instance.players[playerIndex], "Player at index {0} not valid", playerIndex);

        GameObject go = new GameObject("Board" + playerIndex);
        Board board = go.AddComponent<Board>();
        Player player = instance.players[playerIndex];

        board.Init();
        player.board = board;

        return board;
    }

    void Init()
    {
        currentGameState = GameStates.PrePlay;
    }

    void Awake()
    {
        if (!instance)
        {
            instance = this;
            Init();
        }
    }
}
