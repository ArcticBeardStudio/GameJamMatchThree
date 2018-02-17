using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAction : ChangeAction
{
    public int x;
    public int y;
    public TileTypes tileType;

    public CreateAction(Board board, int x, int y, TileTypes tileType)
        : base(board)
    {
        this.x = x;
        this.y = y;
        this.tileType = tileType;

        // data logic
        board.SetTileType(x, y, tileType);
    }

    override public void ChangeEnd()
    {
        Debug.AssertFormat(!board.GetTile(x, y), "Tile at {0} already exists, cant create there!", board.GetTile(x, y));

        if (tileType != TileTypes.None)
        {
            Tile newTile = GameObject.Instantiate<Tile>(board.settings.tilePrefabs[(int)tileType - 1], board.transform);
            newTile.transform.localPosition = board.GetTileLocalPosition(x, y);
            newTile.Init(x, y, board);
            board.SetTile(x, y, newTile);
        }
    }
}
