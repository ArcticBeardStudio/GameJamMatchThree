using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldTile : Tile
{
    public int shieldAmount = 1;

    public override void DestroyTile()
    {
        var player = GameManager.GetCurrentPlayer().GetComponent<Player>();
        if (player != null && player.isActiveAndEnabled)
        {
            player.ModifyShield(shieldAmount);
        }
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
