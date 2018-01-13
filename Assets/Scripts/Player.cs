using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    int index;

    public int GetHealth() { return 0; }
    public int GetIndex() { return 0; }

    public Board GetBoard() { return null; }

    public bool CanMove() { return false; }

    public void ApplyDamage(int amount) { }
}
