using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace UnityEngine.XR.Interaction.Toolkit
{
    public class DelayedUIButton : MonoBehaviour
    {
        [SerializeField] private Task m_Task;
        [SerializeField] private ScreenController m_ScreenController;
        [SerializeField] private UnityEvent OnPress;

        private XRSimpleInteractable Interactable;

        private void Start()
        {
            if (m_ScreenController == null)
            {
                m_ScreenController = FindObjectOfType<ScreenController>();
                if (m_ScreenController == null)
                {
                    Debug.Log("Screen Controller is not set in DelayedUIButton script attached to "
                    + gameObject.name + ". Aborting script");
                    Destroy(this);
                }
            }

            Interactable = GetComponentInChildren<XRSimpleInteractable>();
            if (Interactable == null)
            {
                Debug.Log("No Interactable component attached to " + gameObject.name
                    + " or its children. Aborting DelayedUIButton script.");
                Destroy(this);
            }

            Interactable.selectEntered.AddListener(ButtonPressed);
        }

        public void SetTask(Task task)
        {
            m_Task = task;
        }

        private void ButtonPressed(SelectEnterEventArgs call)
        {
            // Turn off interactable to prevent accidental double presses
            Interactable.enabled = false;

            OnPress.Invoke();
            if (m_Task)
                m_ScreenController.OnAllDelayedScreensChanged.AddListener(m_Task.TryTaskComplete);
        }
    }
}
