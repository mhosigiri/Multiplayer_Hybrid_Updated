using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FabCtrlScript : MonoBehaviour
{
    [SerializeField] private GameObject PropellorBlade;

    private Animator m_Animator;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        if (m_Animator == null)
        {
            Debug.LogError("No Animator component attached to " + gameObject.name
                + ". Aborting FabCtrlScript.cs");
            Destroy(this);
        }

        PropellorBlade.SetActive(false);
    }

    public void ChangeToolHead()
    {
        m_Animator.SetTrigger("SwitchToolHead");
    }

    public void StartFabrication()
    {
        PropellorBlade.SetActive(true);

        m_Animator.SetTrigger("StartFabrication");
    }
}
