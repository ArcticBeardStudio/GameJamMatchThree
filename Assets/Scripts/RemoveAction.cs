using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveAction : ChangeAction
{
    public int x;
    public int y;

    public RemoveAction(Board board, int x, int y)
        : base(board)
    {
        this.x = x;
        this.y = y;
    }

    override public IEnumerator ChangeRoutine(System.Action callback)
    {
        yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
        
        Debug.AssertFormat(board.GetTile(x, y), "Tile at {0} must exists, cant remove there!", board.GetTile(x, y).ToString());

        var tile = board.GetTile(x, y);
        board.SetTileType(x, y, TileTypes.None);
        GameObject.Destroy(tile.gameObject);

        isComplete = true;
        callback();        
    }
}
