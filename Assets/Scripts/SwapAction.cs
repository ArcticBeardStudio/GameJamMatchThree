using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.Tween;

public enum SwapReason
{
    PlayerInput,
    Collapse
}


public class SwapAction : ChangeAction
{
    Vector3 startP1;
    Vector3 startP2;

    Tile tile1;
    Tile tile2;

    public Vector2Int p1;
    public Vector2Int p2;

    public SwapReason swapReason;

    float startTime;
    float progress;
    float duration = 1.0f;

    bool isComplete1 = false;
    bool isComplete2 = false;

    public SwapAction(Board board, Vector2Int p1, Vector2Int p2, SwapReason reasonForSwap = SwapReason.Collapse)
        : base(board)
    {
        this.p1 = p1;
        this.p2 = p2;

        this.tile1 = board.GetTile(p1.x, p1.y);
        this.tile2 = board.GetTile(p2.x, p2.y);

        this.swapReason = reasonForSwap;
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
    public SwapAction(Board board, Tile tile1, Tile tile2, SwapReason swapReason = SwapReason.Collapse) : this(board, tile1.boardPos, tile2.boardPos, swapReason) { }

    override public void ChangeStart()
    {
        // init anim
        startP1 = board.GetTileLocalPosition(p1.x, p1.y);
        startP2 = board.GetTileLocalPosition(p2.x, p2.y);

        startTime = Time.time;
        progress = 0.0f;

        if (tile1)
        {
            TweenFactory.Tween(null, startP1, startP2, duration, tile1.settings.swapCurve.Evaluate,
            (ITween<Vector3> tween) =>
            {
                if (tile1) tile1.transform.localPosition = tween.CurrentValue;
            },
            (ITween<Vector3> tween) =>
            {
                isComplete1 = true;
                CheckComplete();
            });
        }
        else
        {
            isComplete1 = true;
            CheckComplete();
        }
        if (tile2)
        {
            TweenFactory.Tween(null, startP2, startP1, duration, tile2.settings.swapCurve.Evaluate,
            (ITween<Vector3> tween) =>
            {
                if (tile2) tile2.transform.localPosition = tween.CurrentValue;
            },
            (ITween<Vector3> tween) =>
            {
                isComplete2 = true;
                CheckComplete();
            });
        }
        else
        {
            isComplete2 = true;
            CheckComplete();
        }
    }

    void CheckComplete()
    {
        if (isComplete1 && isComplete2)
        {
            isComplete = true;
        }
    }
}
