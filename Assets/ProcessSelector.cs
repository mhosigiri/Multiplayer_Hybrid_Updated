using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessSelector : MonoBehaviour
{
    [SerializeField] private Animator PrintBed;
    [SerializeField] private SetControlScreenSprites FlatScreen, FingerScreen, ScallopScreen;

    /// <summary>
    /// List of Prefabs to Instantiate for each Process Type.
    /// Order should be: FLAT, FINGER, SCALLOP, ADD_CNC, CNC.
    /// </summary>
    [SerializeField]
    [Tooltip("Process step Game Objects for each Process Type." +
        "Each Game Object should have a Task Manager attached and its Tasks should be in child Game Objects." +
        "Order should be: FLAT, FINGER, SCALLOP, ADD_CNC, CNC.")]
    GameObject[] TaskManagers;

    internal enum ProcessTypes
    {
        FLAT, FINGER, SCALLOP, ADD_CNC, CNC
    }

    private void Awake()
    {
        foreach (GameObject o in TaskManagers)
        {
            if (o)
                o.SetActive(false);
        }
    }

    internal void ProcessTypeSelected(ProcessTypes type)
    {
        if (TaskManagers[(int)type] == null)
        {
            Debug.LogError("No Prefab defined for process type "
                + type.ToString() + ". Process type not selected.");
        }
        else
        {
            // Set control screen sprites
            // Repair processes have control screen sprite scripts on
            // separate game objects from the task managers
            if (((int)type >= 0) && ((int)type <= 2))
            {
                switch (type)
                {
                    case ProcessTypes.FLAT:
                        FlatScreen.SetSprites();
                        break;
                    case ProcessTypes.FINGER:
                        FingerScreen.SetSprites();
                        break;
                    case ProcessTypes.SCALLOP:
                        ScallopScreen.SetSprites();
                        break;
                }
            }
            // Replace processes have control screen sprite scripts
            // attached to same game object as task managers
            else
            {
                if (TaskManagers[(int)type].GetComponent<SetControlScreenSprites>())
                    TaskManagers[(int)type].GetComponent<SetControlScreenSprites>().SetSprites();
            }

            // Set task manager active and initialize tasks
            TaskManagers[(int)type].SetActive(true);
            TaskManagers[(int)type].GetComponent<TaskManager>().InitializeTasks();

            // Set process type for fabrication animation
            if (PrintBed)
            {
                PrintBed.SetInteger("ProcessType", (int)type);
            }
        }

        /*
        switch(type)
        {
            case ProcessTypes.FLAT:
                Instantiate(Prefab_FLAT);
                break;
            case ProcessTypes.FINGER:

                break;
            case ProcessTypes.SCALLOP:

                break;
            case ProcessTypes.ADD_CNC:

                break;
            case ProcessTypes.CNC:

                break;
        }
        */
    }
}
