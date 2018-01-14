using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int numSwaps = 0;

    public int index;
    public int health;
    
    public Board board;

    public bool CanMove() { return false; }

    public void ApplyDamage(int amount)
    {
        health -= amount;
    }

    public Tile selected = null;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Tile tile = hit.collider.GetComponent<Tile>();
                if (!selected)
                {
                    selected = tile;
                    Debug.LogFormat("Clicked {0} of {1}", tile, board.GetTileType(tile));
                }
                else
                {
                    var bajs = new SwapInfo(board, selected, tile);

                    board.swapStack.Begin();
                    board.swapStack.Add(bajs);
                    board.swapStack.End();

                    selected = null;
                }
            }
        }
    }
}