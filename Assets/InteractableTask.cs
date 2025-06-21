using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace UnityEngine.XR.Interaction.Toolkit
{
    public class InteractableTask : MonoBehaviour
    {
        [SerializeField] private XRBaseInteractable m_Interactable;
        private Outline m_Outline;

        private Task m_Task;

        // Start is called before the first frame update
        private void Awake()
        {
            m_Task = GetComponent<Task>();
            if (m_Task == null)
            {
                Debug.LogError("No Task component attached to " + gameObject.name +
                    " Aborting InteractableTask script.");
                Destroy(this);
            }

            m_Interactable.selectEntered.AddListener(ButtonPressed);
            m_Task.OnComplete.AddListener(RemoveListener);

            m_Outline = m_Interactable.GetComponent<Outline>();
            if(m_Outline)
            {
                m_Task.OnTaskEnable.AddListener(EnableOutline);
                m_Task.OnComplete.AddListener(DisableOutline);
            }
        }

        private void ButtonPressed(SelectEnterEventArgs args)
        {
            m_Task.TryTaskComplete();
        }

        private void RemoveListener()
        {
            m_Interactable.selectEntered.RemoveListener(ButtonPressed);
        }

        private void EnableOutline()
        {
            m_Outline.enabled = true;
        }

        private void DisableOutline()
        {
            m_Outline.enabled = false;
        }
    }
}
