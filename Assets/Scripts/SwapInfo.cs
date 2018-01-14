using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapInfo : ChangeInfo
{
    public Vector2Int p1;
    public Vector2Int p2;

    public SwapInfo(Board board, Vector2Int p1, Vector2Int p2)
        :base(board)
    {
        this.p1 = p1;
        this.p2 = p2;
    }
    public SwapInfo(Board board, Tile t1, Tile t2)
        :base(board)
    {
        this.p1 = t1.boardPos;
        this.p2 = t2.boardPos;
    }

    override public IEnumerator ChangeRoutine(System.Action callback)
    {
        yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));

        Tile t1 = board.GetTile(p1.x, p1.y);
        Tile t2 = board.GetTile(p2.x, p2.y);

        var previousType1 = board.GetTileType(p1.x, p1.y);
        var previousType2 = board.GetTileType(p2.x, p2.y);

        board.SetTileType(p1.x, p1.y, previousType2);
        board.SetTileType(p2.x, p2.y, previousType1);

        if (t1) {
            t1.x = p2.x;
            t1.y = p2.y;
            board.SetTile(t1.x, t1.y, t1);
            t1.transform.localPosition = board.GetTileLocalPosition(t1.x, t1.y);
        }
        if (t2) {
            t2.x = p1.x;
            t2.y = p1.y;
            board.SetTile(t2.x, t2.y, t2);
            t2.transform.localPosition = board.GetTileLocalPosition(t2.x, t2.y);
        }

        isComplete = true;
        callback();        
    }
}
