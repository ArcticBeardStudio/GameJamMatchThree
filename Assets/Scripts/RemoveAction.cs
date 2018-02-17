using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveAction : ChangeAction
{
    public int x;
    public int y;

    Tile tile;

    public RemoveAction(Board board, int x, int y)
        : base(board)
    {
        this.x = x;
        this.y = y;
        this.tile = board.GetTile(x, y);

        // data logic
        board.SetTileType(x, y, TileTypes.None);
    }

    override public void ChangeEnd()
    {
        Debug.AssertFormat(tile, "Tile at {0} must exists, cant remove there!", tile.ToString());

        board.SetTile(x, y, null);
        GameObject.Destroy(tile.gameObject);
    }
}
