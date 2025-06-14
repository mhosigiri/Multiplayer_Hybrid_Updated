using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace UnityEngine.XR.Interaction.Toolkit.Filtering
{
    public class XRSocketInteractorGameObjectFilter : MonoBehaviour, IXRHoverFilter, IXRSelectFilter
    {
        [SerializeField] private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable[] InteractablesToAllow;

        public bool canProcess { get => isActiveAndEnabled; }

        public bool Process(UnityEngine.XR.Interaction.Toolkit.Interactors.IXRHoverInteractor interactor, UnityEngine.XR.Interaction.Toolkit.Interactables.IXRHoverInteractable interactable)
        {
            foreach (UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable Interactable in InteractablesToAllow)
            {
                if (interactable.transform.gameObject.Equals(Interactable.gameObject))
                {
                    return true;
                }
            }
            return false;
        }

        public bool Process(UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor interactor, UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable interactable)
        {
            foreach (UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable Interactable in InteractablesToAllow)
            {
                if (interactable.transform.gameObject.Equals(Interactable.gameObject))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
