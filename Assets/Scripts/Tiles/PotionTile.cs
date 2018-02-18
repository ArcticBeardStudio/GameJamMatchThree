using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionTile : Tile
{
    public int healAmount = 1;

    public override void DestroyTile()
    {
        var player = GameManager.GetCurrentPlayer().GetComponent<Player>();
        if (player != null && player.isActiveAndEnabled)
        {
            player.ModifyHealth(healAmount);
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
