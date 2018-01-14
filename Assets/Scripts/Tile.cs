using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int x;
    public int y;

    public Board board { get; protected set; }

    new public BoxCollider collider { get; protected set; }

    public void Init(int x, int y, Board board)
    {
        this.x = x;
        this.y = y;
        this.board = board;

        transform.localPosition = board.GetTileLocalPosition(x, y);

        collider = gameObject.AddComponent<BoxCollider>();
        collider.size = new Vector3(1, 1, 0.1f);
    }

    public override string ToString()
    {
        return string.Format("({0}, {1})", x, y);
    }
}
