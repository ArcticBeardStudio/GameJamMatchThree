using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ChangeStack<T> where T : ChangeInfo
{
    public Action resolveCallback;

    public int length { get { return stack == null ? 0 : stack.Count; } }

    bool isOpen;
    List<T> stack;

    public ChangeStack(Action resolveCallback)
    {
        this.resolveCallback = resolveCallback;

        isOpen = false;
    }

    public void Begin()
    {
        isOpen = true;
        stack = new List<T>();
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
                stack.Remove(change);
            }
        }
        if (stack.Count <= 0)
        {
            resolveCallback();
        }
    }
}
