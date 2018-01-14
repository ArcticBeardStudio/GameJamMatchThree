using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeInfo
{
    public Board board;
    public bool isComplete;

    public ChangeInfo(Board board)
    {
        isComplete = false;
    }

    public void DoChange(System.Action callback)
    {
        board.StartCoroutine(ChangeRoutine(callback));
    }
    public IEnumerator ChangeRoutine(System.Action callback)
    {
        yield return new WaitForSeconds(2.0f);
        callback();
    }
}
