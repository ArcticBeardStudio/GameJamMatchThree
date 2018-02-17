using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool debug = false;
    public int numSwaps = 0;

    public int index;
    public int maxHealth;
    public int maxMana;

    private int currentMana = 0;
    private int currentHealth;
    public Board board;

    public GameObject bars;

    private BarScript playerBarscript;
    private SwapReason swapReason = SwapReason.PlayerInput;

    public bool CanMove() { return false; }

    public void ModifyHealth(int amount)
    {
        currentHealth += amount;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if(currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log(this.name + " YOU ARE DED");
        }
        playerBarscript.UpdateHealthBar();
    }

    public void ModifyMana(int amount)
    {
        currentMana += amount;
        if (currentMana > maxMana)
        {
            currentMana = maxMana;
        }
        else if (currentMana <= 0)
        {
            currentMana = 0;
        }
        playerBarscript.UpdateManaBar();
    }

    public Tile selected = null;

    private void Start()
    {
        currentHealth = maxHealth;
        CreateBar();
    }

    public void CreateBar()
    {
        if (bars != null)
        {
            var playerBar = GameObject.Instantiate(bars);
            playerBarscript = playerBar.GetComponent<BarScript>();
            playerBarscript.InitBars(this);
        }
    }

    public float GetManaPercentage()
    {
        return (float)currentMana / (float)maxMana;
    }

    public int GetCurrentMana()
    {
        return currentMana;
    }

    public float GetHealthPercentage()
    {
        return (float)currentHealth / (float)maxHealth;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
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
                
                if (!selected)
                {
                    selected = tile;
                    Debug.LogFormat("Clicked {0} of {1}", tile, board.GetTileType(tile));
                }
                else
                {
                    if(debug)
                    {
                        var swapAction = new SwapAction(board, selected, tile);

                        board.swapStack.Begin();
                        board.swapStack.Add(swapAction);
                        board.swapStack.End();
                    }
                    else
                    {
                        if(board.CheckAdjacent(selected,tile))
                        {
                            var swapAction = new SwapAction(board, selected, tile, swapReason);

                            board.swapStack.Begin();
                            board.swapStack.Add(swapAction);
                            board.swapStack.End();
                        }
                    }

                    selected = null;
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ModifyHealth(-5);
        }
    }
}