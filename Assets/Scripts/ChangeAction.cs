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
    /// Starts the chanage process
    /// </summary>
    /// <param name="callback">Called when change is completed</param>
    public void DoChange(System.Action callback)
    {
        Debug.Assert(board, "Board is null");
        board.StartCoroutine(ChangeRoutine(callback));
    }
    /// <summary>
    /// Called when change is started
    /// </summary>
    virtual public void ChangeStart() { }
    /// <summary>
    /// Called every frame while change is started and not completed
    /// </summary>
    /// <returns>Returns true if change is completed</returns>
    virtual public bool ChangeUpdate() { return true; }
    /// <summary>
    /// Called when change is completed
    /// </summary>
    virtual public void ChangeEnd() { }
    
    /// <summary>
    /// Controls the change lifecycle
    /// </summary>
    /// <param name="callback">Called when change is completed</param>
    virtual public IEnumerator ChangeRoutine(System.Action callback)
    {
        ChangeStart();
        yield return new WaitUntil(ChangeUpdate);
        ChangeEnd();
        
        isComplete = true;
        callback();
    }
}
