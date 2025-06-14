using System.Collections;
using UnityEngine;

/// <summary>
/// Initializes all child Task objects.
/// </summary>

[RequireComponent(typeof(AudioSource))]
public class TaskManager : MonoBehaviour
{
    /// <summary>
    /// Set to <see langword="true"/> if this is a main task to complete. For side tasks (i.e. strictly GUI tasks),
    /// set to <see langword="false"/>.
    /// </summary>
    [SerializeField]
    [Tooltip("Whether these tasks should be reported to Cognitive3D or not.")]
    internal bool ReportToCognitive3D = true;

    /// <summary>
    /// The sound to play when a new task section is begun.
    /// </summary>
    [SerializeField]
    [Tooltip("The sound to play when a new task section is begun.")]
    private AudioClip taskSectionBegun;

    /// <summary>
    /// The sound to play when a task section is completed.
    /// </summary>
    [SerializeField]
    [Tooltip("The sound to play when a task section is completed.")]
    private AudioClip taskSectionCompleted;

    /// <summary>
    /// The sound to play to direct the user to teleport.
    /// </summary>
    [SerializeField]
    [Tooltip("The sound to play to direct the user to teleport.")]
    private AudioClip teleport;

    private AudioSource audioPlayer;

    internal Task[] Tasks;

    /*
    // Initialize variables in Tasks in Tasks array
    private void Awake()
    {
        // Get AudioSource component handle to play sounds
        audioPlayer = GetComponent<AudioSource>();

        // Include all Tasks that are active in the hierarchy
        Tasks = gameObject.GetComponentsInChildren<Task>(false);
        if (Tasks.Length == 0)
        {
            Debug.LogError("No Task components in the child Game Objects of " +
            gameObject.name + ". Aborting TaskManager.");
            Destroy(this);
        }
        /*
        foreach(Task task in Tasks)
            Debug.Log(task.name);
        *//*
        // Set task numbers in Task instances
        Tasks[0].SetTaskNumber(0);
        Tasks[0].taskManager = this;
        for (uint i = 1; i < Tasks.Length; i++)
        {
            Tasks[i].SetTaskNumber(i);
            Tasks[i].taskManager = this;
        }
    }

    // Set Tasks as active
    private void Start()
    {
        // Enable first task, disable all others
        Tasks[0].gameObject.SetActive(true);
        Tasks[0].OnTaskEnable.Invoke();

        for (uint i = 1; i < Tasks.Length; i++)
        {
            Tasks[i].gameObject.SetActive(false);
        }

        // Set first task to start processing TaskComplete calls
        Tasks[0].taskActive = true;
    }
    */

    public void InitializeTasks()
    {
        // Get AudioSource component handle to play sounds
        audioPlayer = GetComponent<AudioSource>();

        // Include all Tasks that are active in the hierarchy
        Tasks = gameObject.GetComponentsInChildren<Task>(false);
        if (Tasks.Length == 0)
        {
            Debug.LogError("No Task components in the child Game Objects of " +
            gameObject.name + ". Aborting TaskManager.");
            Destroy(this);
        }

        // Set task numbers in Task instances
        for (uint i = 0; i < Tasks.Length; i++)
        {
            Tasks[i].SetTaskNumber(i);
            Tasks[i].taskManager = this;
        }

        // Enable first task, disable all others
        Tasks[0].gameObject.SetActive(true);
        Tasks[0].OnTaskEnable.Invoke();

        for (uint i = 1; i < Tasks.Length; i++)
        {
            Tasks[i].gameObject.SetActive(false);
        }

        // Set first task to start processing TaskComplete calls
        Tasks[0].taskActive = true;
    }

    internal void BeginTaskSection()
    {
        if (taskSectionBegun != null)
        {
            audioPlayer.PlayOneShot(taskSectionBegun);
        }
    }

    internal void TaskSectionComplete()
    {
        if (taskSectionCompleted != null)
        {
            audioPlayer.PlayOneShot(taskSectionCompleted);
        }
    }

    internal void TeleportTask()
    {
        if (teleport != null)
        {
            StartCoroutine(QueueSound(teleport));
        }
    }

    private IEnumerator QueueSound(AudioClip sound)
    {
        yield return new WaitUntil(() => audioPlayer.isPlaying == false);
        audioPlayer.PlayOneShot(sound);
    }
}
