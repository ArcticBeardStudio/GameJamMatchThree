﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int x;
    public int y;

    public Board board { get; protected set; }

    public void Pop()
    {
        Destroy(gameObject);
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
