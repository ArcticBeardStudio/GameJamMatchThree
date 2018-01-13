using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public class Tile : MonoBehaviour
{
    public int x;
    public int y;

    public Board board { get; protected set; }

    // Use this for initialization
    void Start()
    {
        BoxCollider collider = gameObject.AddComponent<BoxCollider>();
        collider.size = new Vector3(1, 1);
    }

    public void Init(int x, int y, Board board)
    {
        this.x = x;
        this.y = y;
        this.board = board;

        transform.localPosition = new Vector3(x, y);
    }

    public override string ToString()
    {
        return string.Format("({0}, {1})", x, y);
    }
}
