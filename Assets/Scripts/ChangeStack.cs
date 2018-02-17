using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ChangeStack<T> where T : ChangeAction
{
    public Action<List<T>> resolveCallback;

    public int length { get { return stack == null ? 0 : stack.Count; } }

    bool isOpen;
    bool isResolved;
    List<T> stack;
    List<T> history;

    public ChangeStack(Action<List<T>> resolveCallback)
    {
        this.resolveCallback = resolveCallback;

        isOpen = false;
        isResolved = false;
    }

    public void Begin()
    {
        isResolved = false;
        isOpen = true;
        stack = new List<T>();
        history = new List<T>();
    }

    public void Add(T change)
    {
        if (!isOpen)
        {
            Debug.LogWarning("You need to call Begin() on the stack before adding changes. ");
            return;
        }
        stack.Add(change);
    }

    public void End()
    {
        isOpen = false;
        foreach (T change in stack.ToArray())
        {
            change.DoChange(CheckResolved);
        }
    }

    void CheckResolved()
    {
        foreach (T change in stack.ToArray())
        {
            if (change.isComplete)
            {
                history.Add(change);
                stack.Remove(change);
            }
        }
        if (stack.Count <= 0 && !isResolved)
        {
            resolveCallback(history);
            isResolved = true;
        }
    }
}
