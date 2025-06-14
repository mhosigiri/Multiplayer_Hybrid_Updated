using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace UnityEngine.XR.Interaction.Toolkit
{
    public class MachineDoors : MonoBehaviour
    {
        enum Doors
        {
            Left, Right
        }

        [SerializeField] private Task DoorsCycledTask;
        [SerializeField] private Task DoorsOpenedTask;
        [SerializeField] private Task DoorsClosedTask;

        [SerializeField] private GameObject LeftDoor;
        [SerializeField] private GameObject RightDoor;

        private FlashingHighlight LeftHighlight;
        private FlashingHighlight RightHighlight;

        private Move MoveLeft;
        private Move MoveRight;

        private bool LeftDoorOpen = false;
        private bool RightDoorOpen = false;

        private bool DoorsHaveOpened = false;
        private bool DoorsHaveClosed = false;

        private bool CycleMode = true;


        public void SetCycleMode(bool cycleMode)
        {
            CycleMode = cycleMode;
        }

        private void OnEnable()
        {
            if (LeftDoor == null)
            {
                LeftDoor = GameObject.Find("LeftDoor");
                if (LeftDoor == null)
                {
                    Debug.LogWarning("Left door is not assigned in script MachineDoor attached to GameObject" +
                        gameObject.name + ". Aborting script.");
                }
            }
            if (RightDoor == null)
            {
                RightDoor = GameObject.Find("RightDoor");
                if (RightDoor == null)
                {
                    Debug.LogWarning("Right door is not assigned in script MachineDoor attached to GameObject" +
                        gameObject.name + ". Aborting script.");
                }
            }

            LeftDoor.GetComponent<XRSimpleInteractable>().selectEntered.AddListener(LeftDoorInteracted);
            RightDoor.GetComponent<XRSimpleInteractable>().selectEntered.AddListener(RightDoorInteracted);

            LeftHighlight = LeftDoor.GetComponent<FlashingHighlight>();
            RightHighlight = RightDoor.GetComponent<FlashingHighlight>();

            MoveLeft = LeftDoor.GetComponent<Move>();
            MoveRight = RightDoor.GetComponent<Move>();

            if (CycleMode)
            {
                RightHighlight.enabled = LeftHighlight.enabled = true;
            }
        }

        public void SetDoorsCycledTask(Task task)
        {
            DoorsCycledTask = task;
        }

        public void SetDoorsOpenedTask(Task task)
        {
            DoorsOpenedTask = task;
        }

        public void SetDoorsClosedTask(Task task)
        {
            DoorsClosedTask = task;
        }

        public void EnableDoorHighlights()
        {
            RightHighlight.enabled = LeftHighlight.enabled = true;
        }

        public void DisableDoorHighlights()
        {
            RightHighlight.enabled = LeftHighlight.enabled = false;
        }

        public void LeftDoorInteracted(SelectEnterEventArgs args)
        {
            Debug.Log("Left door interacted");

            MoveLeft.MoveToggle();
            LeftDoorOpen = !LeftDoorOpen;

            Debug.Log("Left door open: " + LeftDoorOpen +
                        "    Right door open: " + RightDoorOpen);


            if (CycleMode)
                DoorCycle(Doors.Left);
            else
            {
                LeftHighlight.enabled = false;
                if (DoorsOpenedTask)
                {
                    if (LeftDoorOpen && RightDoorOpen)
                        DoorsOpenedTask.TryTaskComplete();
                }
                // If task to complete when doors closed, check if doors closed
                if (DoorsClosedTask)
                {
                    if (!LeftDoorOpen && !RightDoorOpen)
                        DoorsClosedTask.TryTaskComplete();
                }
            }
        }

        public void RightDoorInteracted(SelectEnterEventArgs args)
        {
            Debug.Log("Right door interacted");

            MoveRight.MoveToggle();
            RightDoorOpen = !RightDoorOpen;

            Debug.Log("Left door open: " + LeftDoorOpen +
                        "    Right door open: " + RightDoorOpen);


            if (CycleMode)
                DoorCycle(Doors.Right);
            else
            {
                RightHighlight.enabled = false;
                if (DoorsOpenedTask)
                {
                    if (LeftDoorOpen && RightDoorOpen)
                        DoorsOpenedTask.TryTaskComplete();
                }
                // If task to complete when doors closed, check if doors closed
                if (DoorsClosedTask)
                {
                    if (!LeftDoorOpen && !RightDoorOpen)
                        DoorsClosedTask.TryTaskComplete();
                }
            }
        }

        private void DoorCycle(Doors doorInteracted)
        {
            // if both doors are open, set flag that both have been opened
            if (LeftDoorOpen && RightDoorOpen)
            {
                DoorsHaveOpened = true;
            }
            // if both doors are closed, set flag that both have been closed
            else if (!LeftDoorOpen && !RightDoorOpen)
            {
                DoorsHaveClosed = true;
            }

            // if both doors have been opened before and both are now closed,
            // toggle OnDoorsCycled. Turn off flashing highlights.
            if (DoorsHaveOpened && DoorsHaveClosed)
            {
                RightHighlight.enabled = LeftHighlight.enabled = false;
                // Reset cycle variables
                DoorsHaveOpened = DoorsHaveClosed = false;

                if (DoorsCycledTask)
                    DoorsCycledTask.TryTaskComplete();
            }
            // If in intermediate stage between opening/closing both doors, only
            // turn off the flashing highlight for this door.
            else if (LeftDoorOpen != RightDoorOpen)
            {
                if (doorInteracted == Doors.Left)
                    LeftHighlight.enabled = false;
                else
                    RightHighlight.enabled = false;
            }
            // Both doors are open are closed, but cycle is not complete. Instruct user to
            // open/close both doors with flashing highlights to complete the cycle.
            else
                RightHighlight.enabled = LeftHighlight.enabled = true;
        }
    }
}
