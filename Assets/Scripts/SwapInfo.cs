using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapInfo : ChangeInfo
{
    public Tile tile;
    public Tile tile2;

    public SwapInfo(Board board, Tile tile, Tile tile2)
        :base(board)
    {
        this.tile = tile;
        this.tile2 = tile2;
    }

    override public IEnumerator ChangeRoutine(System.Action callback)
    {
        yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));

        var previousTilePos = new Vector2Int(tile.x, tile.y);
        var previousTile2Pos = new Vector2Int(tile2.x, tile2.y);
        var previousTileType = board.GetTileType(tile);
        var previousTile2Type = board.GetTileType(tile2);

        tile.x = previousTile2Pos.x;
        tile.y = previousTile2Pos.y;
        tile2.x = previousTilePos.x;
        tile2.y = previousTilePos.y;
        board.SetTileType(tile, previousTileType);
        board.SetTileType(tile2, previousTile2Type);
        board.FindMatches(new Vector2Int(tile.x, tile.y), board.GetTileType(tile));
        board.FindMatches(new Vector2Int(tile2.x, tile2.y), board.GetTileType(tile2));

        tile.transform.localPosition = board.GetTileLocalPosition(tile.x, tile.y);
        tile2.transform.localPosition = board.GetTileLocalPosition(tile2.x, tile2.y);

        isComplete = true;
        callback();        
    }
}
