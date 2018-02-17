using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for handling changes on the board.
/// See <cref="CreateAction">, <cref="SwapAction"> or <cref="RemoveAction"> for implementation examples.
/// </summary>
public class ChangeAction
{
    public Board board;
    public bool isComplete;

    /// <summary>
    /// Initializes a new change with default values
    /// </summary>
    /// <param name="board">Which board this change affects</param>
    public ChangeAction(Board board)
    {
        this.board = board;
        isComplete = false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="callback"></param>
    public void DoChange(System.Action callback)
    {
        Debug.Assert(board, "Board is null");
        ChangeStart();
        board.StartCoroutine(ChangeRoutine(callback));
    }
    virtual public void ChangeStart() { }
    virtual public IEnumerator ChangeRoutine(System.Action callback)
    {
        yield return new WaitForSeconds(2.0f);
        isComplete = true;
        callback();
    }
}
