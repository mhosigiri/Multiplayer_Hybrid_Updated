/**************************************************************
 * 
 *                  ANIMATE ON GRAB SCRIPT
 * 
 * Purpose: This script serves the very specific purpose of 
 * triggering the door open animation on the SLS 3D Printer 
 * whenever the door is grabbed. It must be modified to be used
 * for other animators and animations.
 * 
 * Dependencies: 
 *  -   <Animator> : The Animator in charge of the door opening 
 *      animation must be passed to the script. The Animator 
 *      must be attached to the printer object.
 * 
 **************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToggleAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private bool animParam;
    private string animParamName;

    private void Awake()
    {
        if (animator == null)
        {
            TryGetComponent<Animator>(out animator);
            if (animator == null)
            {
                Debug.LogError("No animator component attached to " + gameObject.name +
                    ". ToggleAnimation script terminated.");
                Destroy(this);
            }
        }

        animParam = animator.GetParameter(0).defaultBool;
        animParamName = animator.GetParameter(0).name;
    }

    public void ToggleBool()
    {
        animParam = !animParam;
        animator.SetBool(animParamName, animParam);
    }
}
