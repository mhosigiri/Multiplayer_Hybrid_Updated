using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gloves : MonoBehaviour
{
    private Task m_Task;

    public void SetTask(Task task)
    {
        m_Task = task;
    }

    public void GlovesEquipped()
    {
        if (m_Task)
            m_Task.TryTaskComplete();
    }
}
