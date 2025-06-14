using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Task : MonoBehaviour
{
    // Editor visible fields
    [SerializeField] private GameObject TaskUI;
    [SerializeField] private GameObject ObjectToFlash;
    [SerializeField] internal UnityEvent OnTaskEnable;
    [SerializeField] internal UnityEvent OnComplete;

    // Internal static fields
    // internal static Task[] Tasks;
    internal TaskManager taskManager;
    internal bool taskActive = false;

    // Private instance specific fields
    //private bool FirstInitialization = true;
    private uint TaskNumber;
    private bool Complete = false;
    private FlashingHighlight[] Highlights;

    internal void SetTaskNumber(uint TaskNumber)
    {
        this.TaskNumber = TaskNumber;
    }

    internal uint GetTaskNumber()
    {
        return TaskNumber;
    }

    // Add listeners to OnTaskEnable
    private void Awake()
    {
        if (TaskUI)
            OnTaskEnable.AddListener(SetUIVisible);

        if (ObjectToFlash != null)
        {
            Highlights = ObjectToFlash.GetComponents<FlashingHighlight>();

            if (Highlights == null)
            {
                Debug.LogWarning("No FlashingHighlight component attached to ObjectToFlash" +
                    " in Task script attached to " + gameObject.name);
            }
            else
                OnTaskEnable.AddListener(EnableHighlights);
        }
    }

    // Check all cases that task should not be complete,
    // then complete task
    public void TryTaskComplete()
    {
        if (gameObject.activeInHierarchy && taskActive)
            TaskComplete();
    }

    private void TaskComplete()
    {
        // Mark as complete and inactive
        taskActive = false;
        Complete = true;

        // Invoke special behaviors associated with this task
        OnComplete.Invoke();
        //SpecialTaskBehavior();

        // TODO: Process UI events

        // Send completion to Cognitive3D if send to Cognitive3D flag is set in the manager
#if C3D_DEFAULT
        if (taskManager && taskManager.ReportToCognitive3D)
        {
            new Cognitive3D.CustomEvent("Task Complete")
                .SetProperty("Task Number", TaskNumber + 1)
                .SetProperty("Task Name", gameObject.name)
                .Send();
        }
#endif
        // If there is object to flash, disable flashing components
        if (Highlights != null)
        {
            foreach (FlashingHighlight h in Highlights)
                h.enabled = false;
        }

        // If not the last task, enable the next task
        if (TaskNumber != (taskManager.Tasks.Length - 1))
        {
            Task nextTask = taskManager.Tasks[TaskNumber + 1];
            nextTask.SetEnableTask(true);
            nextTask.taskActive = true;
        }


        // Disable this task
        this.SetEnableTask(false);
    }

    private void SetEnableTask(bool enable)
    {
        // Enable/disable the task
        gameObject.SetActive(enable);

        // If enabling the task, invoke OnTaskEnable
        if (enable)
            OnTaskEnable.Invoke();
    }
    private void EnableHighlights()
    {
        if (Highlights != null)
        {
            foreach (FlashingHighlight h in Highlights)
                h.enabled = true;
        }
    }

    internal void SetUIVisible()
    {
        //TaskUI.SetActive(true);
        UIWindow window = TaskUI.GetComponent<UIWindow>();
        if (window)
        {
            window.ShowWindow();
        }
        else
            TaskUI.SetActive(true);
    }

    private void OnDisable()
    {
        if (TaskUI)
        {
            UIWindow window = TaskUI.GetComponent<UIWindow>();
            if (window)
            {
                // If window is enabled and has canvas group initialized, hide window
                if (window.gameObject.activeInHierarchy && window.GetCanvasGroup())
                    window.StartCoroutine(window.HideWindow());
            }
            else
                TaskUI.SetActive(false);
        }
    }

    //public virtual void SpecialTaskBehavior() { }
}
