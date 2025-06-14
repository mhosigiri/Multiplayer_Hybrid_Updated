using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSources : MonoBehaviour
{
    [SerializeField] private bool PlayFirstSoundOnAwake = false;

    private AudioSource[] Sources;
    private int CurrentSoundPlaying = -1;

    void Awake()
    {
        // Fill Sources List with all AudioSources attached to this GameObject
        Sources = GetComponents<AudioSource>();

        if (Sources == null)
        {
            Debug.LogError("No AudioSource components attached to " + gameObject.name);
            Destroy(this);
        }

        if (PlayFirstSoundOnAwake)
            PlayFirstSound();
    }

    public void PlayFirstSound()
    {
        Play(0);
    }

    public void PlayNextSound()
    {
        // Check that index is within bounds of array
        if ((CurrentSoundPlaying + 1) < Sources.Length)
        {
            Play(CurrentSoundPlaying + 1);
        }
    }

    public void PlayPreviousSound()
    {
        // Check that index is within bounds of array
        if ((CurrentSoundPlaying - 1) >= 0)
        {
            Play(CurrentSoundPlaying - 1);
        }
    }

    public void PlaySound(int index)
    {
        // Check that index is within bounds of array
        if ((index < Sources.Length) && (index >= 0))
        {
            Play(index);
        }
    }


    public void PlayNextSoundOnComplete()
    {
        if (CurrentSoundPlaying == -1)
            CurrentSoundPlaying++;

        Play(CurrentSoundPlaying);
        StartCoroutine(PlayAfterStop(CurrentSoundPlaying, CurrentSoundPlaying + 1));
    }

    private void Play(int index)
    {
        // Stop current sound that is playing
        if ((CurrentSoundPlaying != index) && (index != -1))
        {
            Sources[CurrentSoundPlaying].Stop();
        }

        // If sound isn't already playing, play sound
        if (!Sources[index].isPlaying)
            Sources[index].Play();

        CurrentSoundPlaying = index;
    }


    private IEnumerator PlayAfterStop(int curr, int next)
    {
        yield return new WaitWhile(() => Sources[curr].isPlaying);

        Play(next);
    }
}
