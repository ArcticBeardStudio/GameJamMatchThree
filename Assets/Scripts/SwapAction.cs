using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapAction : ChangeAction
{
    public Vector2Int p1;
    public Vector2Int p2;

    Vector3 startP1;
    Vector3 startP2;

    float startTime;
    float progress;
    float duration = 1.0f;

    public SwapAction(Board board, Vector2Int p1, Vector2Int p2)
        : base(board)
    {
        this.p1 = p1;
        this.p2 = p2;
    }
    public SwapAction(Board board, Tile t1, Tile t2)
        : base(board)
    {
        this.p1 = t1.boardPos;
        this.p2 = t2.boardPos;
    }

    bool SwapAnimation(Tile t1, Tile t2)
    {
        progress = Time.time - startTime / duration;

        if (t1)
        {
            t1.transform.position = Vector3.Lerp(startP1, startP2, t1.settings.swapCurve.Evaluate(progress));
        }
        if (t2)
        {
            t2.transform.position = Vector3.Lerp(startP2, startP1, t2.settings.swapCurve.Evaluate(progress));
        }

        return progress >= 1.0f;
    }

    override public void ChangeStart()
    {
        startP1 = board.GetTile(p1.x, p1.y).transform.position;
        startP2 = board.GetTile(p2.x, p2.y).transform.position;

        startTime = Time.time;
        progress = 0.0f;
    }
    override public IEnumerator ChangeRoutine(System.Action callback)
    {
        Tile t1 = board.GetTile(p1.x, p1.y);
        Tile t2 = board.GetTile(p2.x, p2.y);

        yield return new WaitUntil(() => SwapAnimation(t1, t2));

        var previousType1 = board.GetTileType(p1.x, p1.y);
        var previousType2 = board.GetTileType(p2.x, p2.y);

        board.SetTileType(p1.x, p1.y, previousType2);
        board.SetTileType(p2.x, p2.y, previousType1);

        if (t1)
        {
            t1.x = p2.x;
            t1.y = p2.y;
            board.SetTile(t1.x, t1.y, t1);
            t1.transform.localPosition = board.GetTileLocalPosition(t1.x, t1.y);
        }
        if (t2)
        {
            t2.x = p1.x;
            t2.y = p1.y;
            board.SetTile(t2.x, t2.y, t2);
            t2.transform.localPosition = board.GetTileLocalPosition(t2.x, t2.y);
        }

        isComplete = true;
        callback();
    }
}
