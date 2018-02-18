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

    private GameObject PlayerUI;
    private BarsUI playerBarscript;
    private PlayerStatsUI playerStatsUIScript;

    private int redGemsNeeded = 7;
    private int greenGemsNeeded = 3;
    private int blueGemsNeeded = 4;

    private int currentShield = 0;

    private SwapReason swapReason = SwapReason.PlayerInput;

    public bool CanMove() { return false; }

    public void ModifyHealth(int amount)
    {
        if(amount < 0)
        {
            //Send in -amount so damage will be positive
            TakeDamage(-amount);
        }
        else
        {
            currentHealth += amount;
            if(currentHealth>maxHealth)
            {
                currentHealth = maxHealth;
            }
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

    public void ModifyShield(int amount)
    {
        currentShield += amount;
        if (currentShield < 0)
        {
            currentShield = 0;
        }
        playerStatsUIScript.UpdateShieldText(currentShield);
    }

    public void ModifyGem(TileTypes tileType)
    {
        var value = 0;
        switch (tileType)
        {
            case TileTypes.Red:
                redGemsNeeded--;
                value = redGemsNeeded;
                break;
            case TileTypes.Green:
                greenGemsNeeded--;
                value = greenGemsNeeded;
                break;
            case TileTypes.Blue:
                blueGemsNeeded--;
                value = blueGemsNeeded;
                break;
        }
        if(value >= 0)
        {
            playerStatsUIScript.UpdateGemText(value, tileType);
        }
        CheckForSpecialAttack();
    }

    private void CheckForSpecialAttack()
    {
        if(redGemsNeeded <= 0 && greenGemsNeeded <= 0 && blueGemsNeeded <= 0)
        {
            playerStatsUIScript.SpecialAttackButton.gameObject.SetActive(true);
        }
    }

    private void TakeDamage(int amount)
    {
        //Remove the damage amount from the shield instead of taking damage
        if(amount <= currentShield)
        {
            ModifyShield(-amount);
        }
        //Remove our shield from the damage before taking damage
        else
        {
            amount -= currentShield;
            ModifyShield(-currentShield);

            currentHealth -= amount;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Debug.Log(this.name + " YOU ARE DED");
            }
        }
        
    }
    public Tile selected = null;

    private void Start()
    {
        currentHealth = maxHealth;
        if (bars != null)
        {
            PlayerUI = GameObject.Instantiate(bars);
            if(PlayerUI == null)
            {
                return;
            }

            CreateBar();
            InitGemsUI();
        }
    }

    public void CreateBar()
    {
        playerBarscript = PlayerUI.GetComponent<BarsUI>();
        if(playerBarscript != null)
        {
            playerBarscript.InitBars(this);
        }
    }

    public void InitGemsUI()
    {
        playerStatsUIScript = PlayerUI.GetComponent<PlayerStatsUI>();
        if (playerStatsUIScript != null)
        {
            playerStatsUIScript.Init(redGemsNeeded,greenGemsNeeded,blueGemsNeeded, currentShield);
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