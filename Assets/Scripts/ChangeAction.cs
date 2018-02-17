using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAction
{
    public Board board;
    public bool isComplete;

    public ChangeAction(Board board)
    {
        this.board = board;
        isComplete = false;
    }

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
