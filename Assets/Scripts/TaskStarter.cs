using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskStarter : MonoBehaviour
{
    public TaskDoer doer;


    // Use this for initialization
    void Start()
    {
        doer.DoTask(TaskDone);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void TaskDone()
    {
        print("ITS NODE");
    }
}
