using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;

/// <summary>
/// Manages a UI window.
/// </summary>
[AddComponentMenu("UI Window")]
[RequireComponent(typeof(CanvasGroup))]
public class UIWindow : MonoBehaviour
{
    /// <summary>
    /// The amount of time for the window to fade in/out when the window is enabled/disabled.
    /// </summary>
    [SerializeField]
    [Tooltip("The amount of time for the window to fade in/out when the window is enabled/disabled.")]
    private float FadeTime = 1.0f;

    /// <summary>
    /// Whether or not the window should be visible at program Start.
    /// </summary>
    [SerializeField]
    [Tooltip("Whether or not the window should be visible at program Start.")]
    internal bool HideAtStart = true;

    /*
    /// <summary>
    /// The Task Manager that controls this UI window.
    /// </summary>
    [SerializeField]
    [Tooltip("The Task Manager that controls this UI window.")]
    private TaskManager taskManager;
    */

    protected CanvasGroup canvasGroup;
    private Transform player;
    private GameObject m_Video;

    private AudioSource voiceAudio;
    private bool hasSpoken = false;
    //private bool Startup = true;

    internal virtual void Awake()
    {

    }

    internal virtual void Start()
    {
        // If no Canvas Group component attached to this Game Object, create one
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = new CanvasGroup();
        }


        player = GameObject.FindGameObjectWithTag("MainCamera").transform;

        // If there is a video component attached to this object, get pointer to it
        if (GetComponentInChildren<VideoPlayer>(true))
            m_Video = GetComponentInChildren<VideoPlayer>(true).gameObject;

        if (HideAtStart)
        {
            canvasGroup.alpha = 0f;
        }
        else
        {
            ShowWindow();
        }
    }

    internal CanvasGroup GetCanvasGroup()
    {
        return canvasGroup;
    }

    private void Update()
    {
        transform.LookAt(player);
    }
    /*
    protected virtual void OnEnable()
    {
        if(Startup)
        {
            if(!HideAtStart)
                ShowWindow();
            Startup = false;
            Debug.Log("UIWindow " + gameObject.name + " startup set to false");
        }
        else
        {
            ShowWindow();
        }
    }
    */
    internal virtual void ShowWindow()
    {
        Debug.Log("Show window " + gameObject.name);

        canvasGroup.alpha = 0f;
        gameObject.SetActive(true);
        StartCoroutine(Fade(1.0f));

        // If transform.Find("Background/Title")
        // Read title
        if (hasSpoken) return;

        Transform titleTransform = transform.Find("Background/Title");
        if (titleTransform != null)
        {
            AudioSource titleAudio = titleTransform.GetComponent<AudioSource>();

            if (titleAudio != null && titleAudio.clip != null)
            {
                titleAudio.Play();
                hasSpoken = true;
                Debug.Log("Playing voice clip" + titleAudio.clip.name);
            }
            else
            {
                Debug.LogWarning("AudioSource missing.");
            }
        }
        else
        {
            Debug.LogWarning("Background/Title not found.");
        }
        // If transform.Find("Background/Reason")
        // Read reason
    }

    internal virtual IEnumerator HideWindow()
    {
        // If not already hidden, start fade coroutine
        if (gameObject.activeInHierarchy && isActiveAndEnabled)
            yield return Fade(0.0f);

        if (m_Video)
            m_Video.SetActive(false);

        gameObject.SetActive(false);
    }

    private IEnumerator Fade(float endingAlpha)
    {
        float startingAlpha = canvasGroup.alpha;
        float t = 0f;

        while (t < FadeTime)
        {
            canvasGroup.alpha = Mathf.Lerp(startingAlpha, endingAlpha, t / FadeTime);
            t += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        canvasGroup.alpha = endingAlpha;

        if (m_Video)
            m_Video.SetActive(true);
    }
    /*
    // TO-DO: Add text-to-speech voiceover of the text in the window
    private void OnBecameVisible()
    {
        // If transform.Find("Background/Title")
        // Read title
        if (hasSpoken) return;

        Transform titleTransform = transform.Find("Background/Title");
        if (titleTransform != null)
        {
            AudioSource titleAudio = titleTransform.GetComponent<AudioSource>();

            if (titleAudio != null && titleAudio.clip != null)
            {
                titleAudio.Play();
                hasSpoken = true;
                Debug.Log("Playing voice clip" + titleAudio.clip.name);
            }
            else
            {
                Debug.LogWarning("AudioSource missing.");
            }
        }
        else
        {
            Debug.LogWarning("Background/Title not found.");
        }
        // If transform.Find("Background/Reason")
        // Read reason

    }
    */
}
