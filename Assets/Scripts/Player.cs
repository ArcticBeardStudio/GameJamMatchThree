using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int index;
    public int health;
    
    public Board board;

    public bool CanMove() { return false; }

    public void ApplyDamage(int amount)
    {
        health -= amount;
    }
    
    void Update()
    {
        
    }
}
