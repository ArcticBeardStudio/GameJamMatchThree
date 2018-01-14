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
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Tile tile = hit.collider.GetComponent<Tile>();
                tile.Pop();
            }
        }
    }
}
