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

    void Init()
    {
        currentGameState = GameStates.PrePlay;
        //Give each player a healthbar
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
