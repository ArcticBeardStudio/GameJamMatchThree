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

        board.SetTile(x, y, null);
    }

    override public void ChangeStart()
    {
        isComplete = true;
    }

    override public void ChangeEnd()
    {
        Debug.AssertFormat(tile, "Tile at {0} must exists, cant remove there!", tile.ToString());

        GameObject.Destroy(tile.gameObject);
    }
}
