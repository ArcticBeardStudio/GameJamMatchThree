using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordTile : Tile
{
    public int damageAmount = -2;

    public override void DestroyTile()
    {
        var opponent = GameManager.GetOpponentPlayer().GetComponent<Player>();
        if (opponent != null && opponent.isActiveAndEnabled)
        {
            opponent.ModifyHealth(damageAmount);
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
