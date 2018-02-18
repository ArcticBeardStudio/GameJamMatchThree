using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.Tween;

public enum RemoveReason
{
    PlayerInput,
    Destroy
}

public class RemoveAction : ChangeAction
{
    public int x;
    public int y;
    public RemoveReason removeReason;
    Tile tile;

    public RemoveAction(Board board, int x, int y, RemoveReason reasonForRemove = RemoveReason.Destroy)
        : base(board)
    {
        this.x = x;
        this.y = y;
        this.tile = board.GetTile(x, y);
        this.removeReason = reasonForRemove;

        // data logic
        if(reasonForRemove == RemoveReason.PlayerInput)
        {
            this.tile.DestroyTile();
        }
        
        board.SetTileType(x, y, TileTypes.None);

        board.SetTile(x, y, null);
    }

    override public void ChangeStart()
    {
        TweenFactory.Tween(null, 1.0f, 0.0f, 0.2f, TweenScaleFunctions.QuinticEaseIn,
        (ITween<float> tween) =>
        {
            if (tile) tile.transform.localScale = Vector3.one * tween.CurrentValue;
        },
        (ITween<float> tween) =>
        {
            isComplete = true;
        });
    }

    override public void ChangeEnd()
    {
        Debug.AssertFormat(tile, "Tile at {0} must exists, cant remove there!", tile.ToString());

        GameObject.Destroy(tile.gameObject);
    }
}
