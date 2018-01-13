using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStates {
    Playing,
    Waiting
}

public class GameManager : MonoBehaviour
{   
    GameStates currentGameState;

    public Player[] GetPlayers() { return null; }
    public Player GetPlayer(int index) { return null; }
    public Player GetCurrentPlayer() { return null; }

    public bool IsPlaying() { return false; }
    public bool CanPlayerMove(int index) { return false; }
}
