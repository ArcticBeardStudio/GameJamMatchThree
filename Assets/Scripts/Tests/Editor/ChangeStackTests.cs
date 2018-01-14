using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class ChangeStackTests
{
    [Test]
    public void BasicUseCase()
    {
        int i = 0;
        ChangeStack<int> stack = new ChangeStack<int>(x =>
        {
            Assert.AreEqual(i, x);
            i++;
        }, () =>
        {
            Assert.Pass("Resolved");
        });

        stack.Begin();
        for (int j = 0; j < 3; j++)
        {
            stack.Add(j);
        }
        stack.End();
        Assert.Fail("Not Resolved");
    }

    [Test]
    public void StackCallbackGetsCalled()
    {
        ChangeStack<int> stack = new ChangeStack<int>(x => { Assert.AreEqual(1337, x); }, () => { });
        stack.Begin();
        stack.Add(1337);
        stack.End();
    }
    [Test]
    public void ResolveCallbackGetsCalled()
    {
        ChangeStack<int> stack = new ChangeStack<int>(x => { }, () => { Assert.Pass("Resolved"); });
        stack.Begin();
        stack.Add(1337);
        stack.End();
        Assert.Fail("Not Resolved");
    }

    // Adding and not adding
    [Test]
    public void CantAddBeforeBegin()
    {
        ChangeStack<int> stack = new ChangeStack<int>(x => { }, () => { });
        stack.Add(0);
        Assert.AreEqual(0, stack.length);
    }
    [Test]
    public void CanAddAfterBegin()
    {
        ChangeStack<int> stack = new ChangeStack<int>(x => { }, () => { });
        stack.Begin();
        stack.Add(0);
        Assert.AreEqual(1, stack.length);
    }
    [Test]
    public void CantAddAfterBeginAndEnd()
    {
        ChangeStack<int> stack = new ChangeStack<int>(x => { }, () => { });
        stack.Begin();
        stack.End();
        stack.Add(0);
        Assert.AreEqual(0, stack.length);
    }
    [Test]
    public void CanAddAfterBeginAfterEndAfterBegin()
    {
        ChangeStack<int> stack = new ChangeStack<int>(x => { }, () => { });
        stack.Begin();
        stack.End();
        stack.Begin();
        stack.Add(0);
        Assert.AreEqual(1, stack.length);
    }
}
