using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateInfo : ChangeInfo
{
    public int x;
    public int y;
    public TileTypes tileType;

    public CreateInfo(Board board, int x, int y, TileTypes tileType)
        : base(board)
    {
        this.x = x;
        this.y = y;
        this.tileType = tileType;
        
        board.SetTileType(x, y, tileType);
    }

    override public IEnumerator ChangeRoutine(System.Action callback)
    {
        yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
        
        Debug.AssertFormat(!board.GetTile(x, y), "Tile at {0} already exists, cant create there!", board.GetTile(x, y));

        if (tileType != TileTypes.None)
        {
            Tile newTile = GameObject.Instantiate<Tile>(board.settings.tilePrefabs[(int)tileType], board.transform);
            newTile.transform.localPosition = board.GetTileLocalPosition(x, y);
            newTile.Init(x, y, board);
            board.SetTile(x, y, newTile);
        }

        isComplete = true;
        callback();        
    }
}
