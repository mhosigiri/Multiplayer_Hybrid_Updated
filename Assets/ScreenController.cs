using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScreenController : MonoBehaviour
{
    [SerializeField] internal Sprite[] Screens;
    [SerializeField] internal UnityEvent OnAllDelayedScreensChanged;

    private Image m_Image;
    private uint CurrentImage = 0;
    private uint ActiveCoroutines = 0;

    private void Awake()
    {
        if (Screens.Length == 0)
        {
            Debug.LogError("Images array in ScreenController script attached to "
                + gameObject.name + " is empty. Aborting script.");
            Destroy(this);
        }

        m_Image = GetComponentInChildren<Image>();
        if (m_Image == null)
        {
            m_Image = gameObject.AddComponent<Image>();
        }
    }

    public void NextImage()
    {
        if (CurrentImage < (Screens.Length - 1))
            m_Image.sprite = Screens[++CurrentImage];
    }

    internal void SetImageAndSkipNext(Sprite sprite)
    {
        m_Image.sprite = sprite;
        CurrentImage++;
    }

    public void NextImageDelay(float seconds)
    {
        ActiveCoroutines++;
        StartCoroutine(NextAfterDelay(seconds));
    }

    public void SetScreen(int screen_num)
    {
        if (screen_num >= 0 && screen_num < Screens.Length)
        {
            CurrentImage = (uint)screen_num;
            m_Image.sprite = Screens[screen_num];
        }
    }

    public void SetScreen(Sprite screen)
    {
        for (uint i = 0; i < Screens.Length; i++)
        {
            if (Screens[i].Equals(screen))
            {
                CurrentImage = i;
                m_Image.sprite = Screens[i];
            }
        }
    }

    private IEnumerator NextAfterDelay(float seconds)
    {
        if (CurrentImage < (Screens.Length - 1))
        {
            yield return new WaitForSeconds(seconds);
            m_Image.sprite = Screens[++CurrentImage];
        }

        ActiveCoroutines--;
        if (ActiveCoroutines == 0)
        {
            OnAllDelayedScreensChanged.Invoke();
        }
    }
}
