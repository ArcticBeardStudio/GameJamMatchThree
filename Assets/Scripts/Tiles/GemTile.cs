using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemTile : Tile
{
    public TileTypes gem = TileTypes.Red;

    public override void DestroyTile()
    {
        var player = GameManager.GetCurrentPlayer().GetComponent<Player>();
        if (player != null && player.isActiveAndEnabled)
        {
            player.ModifyGem(gem);
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
