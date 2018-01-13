using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Tile selected;

    // Use this for initialization
    void Start()
    {
        selected = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Tile tile = hit.collider.GetComponent<Tile>();

                if (tile != null)
                {
                    if (selected == null || tile.board != selected.board)
                    {
                        selected = tile;
                        print("hit " + selected.ToString());
                    }
                    else if (selected.board == tile.board)
                    {
                        if (!tile.board.SwapTiles(selected, tile))
                        {
                            selected = tile;
                            print("hit " + selected.ToString());
                        }
                        else
                        {
                            print("swap " + selected.ToString() + " <-> " + tile.ToString());
                            selected = null;
                        }
                    }
                    else
                    {
                        print("what?");
                    }
                }
            }
        }
    }
}
