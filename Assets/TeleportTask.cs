using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

namespace UnityEngine.XR.Interaction.Toolkit
{
    public class TeleportTask : MonoBehaviour
    {
        [SerializeField] private GameObject TeleportPoint;

        private TeleportationAnchor anchor;
        private static DrawPathToTeleport teleportPath;
        private GameObject arrow;
        private Task task;

        // Start is called before the first frame update
        void Awake()
        {
            if(TeleportPoint == null)
            {
                Debug.LogError("Teleport Point not set in TeleportTask script attached to "
                    + gameObject.name + ". Aborting script.");
                Destroy(this);
            }

            anchor = TeleportPoint.GetComponent<TeleportationAnchor>();
            if (anchor == null)
            {
                Debug.LogError("The given teleport point for the TeleportTask script " +
                    "attached to " + gameObject.name + "does not have a TeleportationAnchor " +
                    "component. Aborting script.");
                Destroy(this);
            }

            task = GetComponent<Task>();
            if (task == null)
            {
                Debug.LogError("No Task component attached to " + gameObject.name +
                    " Aborting TeleportTask script.");
                Destroy(this);
            }

            teleportPath = GameObject.FindObjectOfType<DrawPathToTeleport>();
            if(teleportPath == null)
            {
                Debug.LogWarning("TeleportTask script attached to " + gameObject.name
                    + " could not find any active GameObject of type DrawPathToTeleport.");
            }

            arrow = TeleportPoint.transform.Find("Arrow").gameObject;
            if (arrow == null)
            {
                Debug.LogWarning("TeleportTask script attached to " + gameObject.name
                    + " could not find an arrow attached to its teleport point.");
            }

            task.OnTaskEnable.AddListener(TaskEnabled);
        }

        private void TaskEnabled()
        {
            if(anchor)
            {
                anchor.teleporting.AddListener(Teleported);
            }

            if(arrow)
            {
                arrow.SetActive(true);
            }

            if(teleportPath && TeleportPoint)
            {
                teleportPath.StartDrawingPathToNewTarget(TeleportPoint);
            }

            // Play teleport cue sound
            task.taskManager.TeleportTask();
        }

        private void Teleported(TeleportingEventArgs args)
        {
            task.TryTaskComplete();

            anchor.teleporting.RemoveListener(Teleported);

            if (arrow)
            {
                arrow.SetActive(false);
            }

            if (teleportPath)
            {
                teleportPath.StopDrawingPath();
            }
        }    
    }
}
