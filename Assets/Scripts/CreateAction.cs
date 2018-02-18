using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.Tween;

public class CreateAction : ChangeAction
{
    public int x;
    public int y;
    public TileTypes tileType;
    public Tile tile;

    public CreateAction(Board board, int x, int y, TileTypes tileType)
        : base(board)
    {
        this.x = x;
        this.y = y;
        this.tileType = tileType;

        // data logic
        board.SetTileType(x, y, tileType);

        Debug.AssertFormat(!board.GetTile(x, y), "Tile at {0} already exists, cant create there!", board.GetTile(x, y));

        if (tileType != TileTypes.None)
        {
            tile = GameObject.Instantiate<Tile>(board.settings.tilePrefabs[(int)tileType - 1], board.transform);
            tile.Init(x, y, board);
            tile.transform.localPosition += Vector3.up * 10;
            board.SetTile(x, y, tile);
        }
    }

    override public void ChangeStart()
    {
        Vector3 source = tile.transform.localPosition;
        Vector3 target = board.GetTileLocalPosition(x, y);
        TweenFactory.Tween(null, source, target, 1.0f, TweenScaleFunctions.CubicEaseIn,
        (ITween<Vector3> tween) => 
        {
            if (tile) tile.transform.localPosition = tween.CurrentValue;
        },
        (ITween<Vector3> tween) =>
        {
            isComplete = true;
        });
    }
}
