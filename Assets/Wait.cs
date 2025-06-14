/**************************************************************
 * 
 *                       WAIT SCRIPT
 * 
 * Purpose: Waits for the specified number of seconds on Awake
 * 
 **************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Wait : MonoBehaviour
{
    public float Seconds;
    public bool StartTimerOnAwake = true;
    public UnityEvent AfterWait;

    private void Awake()
    {
        StartCoroutine(wait());
    }

    public void StartTimer()
    {
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(Seconds);
        Debug.Log("Wait Complete");
        AfterWait.Invoke();
    }

    public Wait(float Seconds, UnityEvent AfterWait)
    {
        this.Seconds = Seconds;
        this.AfterWait = AfterWait;
    }
}
