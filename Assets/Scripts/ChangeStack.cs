using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ChangeStack<T>
{
    public Action<T> stackCallback;
    public Action resolveCallback;

    public int length { get { return stack == null ? 0 : stack.Count; } }

    bool isOpen;
    List<T> stack;

    public ChangeStack(Action<T> stackCallback, Action resolveCallback)
    {
        this.stackCallback = stackCallback;
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
        foreach (T change in stack)
        {
            stackCallback(change);
        }
        resolveCallback();
    }
}
