using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapAction : ChangeAction
{
    Vector3 startP1;
    Vector3 startP2;

    Tile tile1;
    Tile tile2;

    bool isEmpty1;
    bool isEmpty2;

    public Vector2Int p1;
    public Vector2Int p2;

    float startTime;
    float progress;
    float duration = 1.0f;

    public SwapAction(Board board, Vector2Int p1, Vector2Int p2)
        : base(board)
    {
        this.p1 = p1;
        this.p2 = p2;

        this.tile1 = board.GetTile(p1.x, p1.y);
        this.tile2 = board.GetTile(p2.x, p2.y);

        this.isEmpty1 = board.IsEmpty(p1.x, p1.y);
        this.isEmpty2 = board.IsEmpty(p2.x, p2.y);

        // change data
        var previousType1 = board.GetTileType(p1.x, p1.y);
        var previousType2 = board.GetTileType(p2.x, p2.y);

        board.SetTileType(p1.x, p1.y, previousType2);
        board.SetTileType(p2.x, p2.y, previousType1);

        board.SetTile(p2.x, p2.y, tile1 ? tile1 : null);
        board.SetTile(p1.x, p1.y, tile2 ? tile2 : null);

        if (tile1)
        {
            tile1.x = p2.x;
            tile1.y = p2.y;
        }
        if (tile2)
        {
            tile2.x = p1.x;
            tile2.y = p1.y;
        }
    }
    public SwapAction(Board board, Tile tile1, Tile tile2) : this(board, tile1.boardPos, tile2.boardPos) { }

    bool SwapAnimation(Tile tile1, Tile tile2)
    {
        progress = Time.time - startTime / duration;

        if (tile1)
        {
            tile1.transform.localPosition = Vector3.Lerp(startP1, startP2, tile1.settings.swapCurve.Evaluate(progress));
        }
        if (tile2)
        {
            tile2.transform.localPosition = Vector3.Lerp(startP2, startP1, tile2.settings.swapCurve.Evaluate(progress));
        }

        return progress >= 1.0f;
    }

    override public void ChangeStart()
    {
        // init anim
        startP1 = board.GetTileLocalPosition(p1.x, p1.y);
        startP2 = board.GetTileLocalPosition(p2.x, p2.y);

        startTime = Time.time;
        progress = 0.0f;
    }
    override public IEnumerator ChangeRoutine(System.Action callback)
    {
        // update visuals
        yield return new WaitUntil(() => SwapAnimation(tile1, tile2));

        isComplete = true;
        callback();
    }
}
