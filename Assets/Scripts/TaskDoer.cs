using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskDoer : MonoBehaviour
{
    public TaskStarter starter;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DoTask(System.Action callback)
    {
        StartCoroutine(Task(callback));
    }

    protected IEnumerator Task(System.Action callback)
    {
        while (true)
        {
            if (Time.frameCount > 200) {
                callback();
                yield break;
            }

            yield return null;
        }
    }
}
