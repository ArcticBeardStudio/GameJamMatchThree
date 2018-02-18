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

    private int currentPlayer = 0;
    
    public static Player[] GetPlayers() { return null; }
    public static Player GetPlayer(int index) { return null; }
    public static Player GetCurrentPlayer()
    {
        return instance.players[instance.currentPlayer];
    }
    public static Player GetOpponentPlayer()
    {
        if(instance.currentPlayer == 0)
        {
            return instance.players.Count >= 2 ? instance.players[1] : null;
        }
        else if (instance.currentPlayer == 1)
        {
            return instance.players.Count >= 1 ? instance.players[0] : null;
        }
        return null;
    }

    public bool IsPlaying()
    {
        return false;
    }

    public bool CanPlayerMove(int index)
    {
        if(currentPlayer == index)
        {
            return true;
        }
        return false;
    }

    public void ChangePlayer()
    {
        if(currentPlayer == 0)
        {
            currentPlayer = 1;
        }
        else
        {
            currentPlayer = 0;
        }
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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ChangePlayer();
        }
    }
}
