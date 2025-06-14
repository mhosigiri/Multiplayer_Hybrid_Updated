using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    /// <summary>
    /// Manages a UI window with checklist items that are marked off as Tasks are completed.
    /// </summary>
    /// <remarks>
    /// Each checklist item should have a <see cref="Toggle"/> component attached. This script
    /// will treat all child Game Objects with Toggle components as checklist items.
    /// </remarks>
    [AddComponentMenu("Checklist UI")]
    public class ChecklistUI : UIWindow
    {
        /// <summary>
        /// The Task objects corresponding to each checklist item.
        /// The Tasks should be given in the same order as the checklist items.
        /// </summary>
        [SerializeField]
        [Tooltip("The Task objects corresponding to each checklist item." +
            " The Tasks should be given in the same order as the checklist items.")]
        private Task[] Tasks;

        private Toggle[] ChecklistItems;
        private uint TasksCompleted = 0;

        // Start is called before the first frame update
        override internal void Awake()
        {
            if(Tasks.Length == 0)
            {
                Debug.LogError("No Tasks given in the ChecklistUI script attached to "
                    + gameObject.name + ". Aborting script.");
                Destroy(this);
            }

            ChecklistItems = GetComponentsInChildren<Toggle>();
            if (ChecklistItems.Length == 0)
            {
                Debug.LogError("No Toggle objects found in the children of "
                    + gameObject.name + ". Aborting ChecklistUI script.");
                Destroy(this);
            }

            if(Tasks.Length != ChecklistItems.Length)
            {
                Debug.LogError("Mismatch between the amount of Tasks given and the amount" +
                    " of child GameObjects of " + gameObject.name + " with Toggle components." +
                    " Aborting script.");
                Destroy(this);
            }

            // Add listener to show checkmark when task is complete
            foreach(Task task in Tasks)
            {
                task.OnComplete.AddListener(TaskCompleted);
            }

            // Add listener when first task is enabled to set this window as visible
            Tasks[0].OnTaskEnable.AddListener(ShowWindow);

            base.Awake();
        }

        public void SetTasks(Task[] tasks)
        {
            Tasks = tasks;

            if (Tasks.Length != ChecklistItems.Length)
            {
                Debug.LogError("Mismatch between the amount of Tasks given and the amount" +
                    " of child GameObjects of " + gameObject.name + " with Toggle components." +
                    " Aborting script.");
                Destroy(this);
            }

            // Add listener to show checkmark when task is complete
            foreach (Task task in Tasks)
            {
                task.OnComplete.AddListener(TaskCompleted);
            }

            // Add listener when first task is enabled to set this window as visible
            Tasks[0].OnTaskEnable.AddListener(ShowWindow);

            canvasGroup.alpha = 0f;
        }

        internal override void ShowWindow()
        {
            // Play sound to signal new task section beginning to user
            if(Tasks[0].taskManager != null)
                Tasks[0].taskManager.BeginTaskSection();

            base.ShowWindow();
        }

        // Assumes that tasks are completed in the same order that they are given in the Tasks array
        private void TaskCompleted()
        {
            ChecklistItems[TasksCompleted].isOn = true;
            TasksCompleted++;

            // If all tasks are completed, play completion sound and hide this window
            if (TasksCompleted == Tasks.Length)
            {
                Tasks[Tasks.Length - 1].taskManager.TaskSectionComplete();
                StartCoroutine(HideWindow());
            }
        }
    }
}
