using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace UnityEngine.XR.Interaction.Toolkit
{
    public class Key : MonoBehaviour
    {
        [SerializeField] internal UnityEvent OnLock;
        [SerializeField] internal UnityEvent OnUnlock;

        private Move moveScript;
        private FlashingHighlight flashingHighlight;
        private XRBaseInteractable interactable;

        private bool locked = false;

        private void Start()
        {
            moveScript = GetComponent<Move>();
            if (moveScript == null)
            {
                Debug.LogError("No Move component attached to " + gameObject.name
                    + ". Aborting Key.cs.");
                Destroy(this);
            }

            flashingHighlight = GetComponentInChildren<FlashingHighlight>();
            if (flashingHighlight == null)
            {
                Debug.LogError("No Flashing Highlight component attached to " + gameObject.name
                    + ". Aborting Key.cs.");
                Destroy(this);
            }

            interactable = GetComponent<XRBaseInteractable>();
            if (interactable == null)
            {
                Debug.LogError("No interactable component attached to " + gameObject.name
                    + ". Aborting Key.cs.");
                Destroy(this);
            }

            interactable.selectEntered.AddListener(KeyInteracted);
        }

        private void OnEnable()
        {
            if (interactable)
                interactable.selectEntered.AddListener(KeyInteracted);
        }

        private void OnDisable()
        {
            if (interactable)
                interactable.selectEntered.RemoveListener(KeyInteracted);
        }

        public void AddLockTask(Task task)
        {
            OnLock.AddListener(task.TryTaskComplete);
        }

        public void AddUnlockTask(Task task)
        {
            OnUnlock.AddListener(task.TryTaskComplete);
        }

        private void KeyInteracted(SelectEnterEventArgs args)
        {
            locked = !locked;
            moveScript.OnMoveComplete.AddListener(MoveComplete);
            moveScript.MoveToggle();
            flashingHighlight.enabled = false;
        }

        private void MoveComplete()
        {
            if (locked)
            {
                OnLock.Invoke();
            }
            else
            {
                OnUnlock.Invoke();
            }
        }
    }
}
