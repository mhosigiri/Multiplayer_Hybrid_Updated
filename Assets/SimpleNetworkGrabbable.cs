using UnityEngine;
using Unity.Netcode;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

// This component ensures that when a player grabs an object, they are given
// network ownership of it. This allows them to control its position and have
// that synchronized to all other players.
[RequireComponent(typeof(XRGrabInteractable))]
public class SimpleNetworkGrabbable : NetworkBehaviour
{
    private XRGrabInteractable grabInteractable;

    void Awake()
    {
        // Get the reference to the XR Grab Interactable component
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    public override void OnNetworkSpawn()
    {
        // Add listeners to the grab and release events.
        // This is a robust way to detect when the object is picked up or dropped.
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    // This method is called when a player grabs the object.
    private void OnGrab(SelectEnterEventArgs args)
    {
        // If the player grabbing the object is not already the owner,
        // send a request to the server to change ownership.
        if (!IsOwner)
        {
            RequestOwnershipServerRpc(NetworkManager.Singleton.LocalClientId);
        }
    }

    // This method is called when a player releases the object.
    private void OnRelease(SelectExitEventArgs args)
    {
        // If the player releasing the object is the owner (and not the server),
        // tell the server to remove their specific ownership. This returns
        // ownership to the server, making physics behave authoritatively again.
        if (IsOwner && !IsServer)
        {
            RemoveOwnershipServerRpc(OwnerClientId);
        }
    }

    // A remote procedure call that runs on the server.
    // The `RequireOwnership = false` is important because a client who doesn't
    // own the object needs to be able to call this.
    [ServerRpc(RequireOwnership = false)]
    private void RequestOwnershipServerRpc(ulong newOwnerId)
    {
        NetworkObject.ChangeOwnership(newOwnerId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void RemoveOwnershipServerRpc(ulong previousOwnerId)
    {
        // We only remove ownership if the client who just owned it is the one making the request.
        if (OwnerClientId == previousOwnerId)
        {
            NetworkObject.RemoveOwnership();
        }
    }

    // It's good practice to clean up the listeners when the object is destroyed.
    public override void OnNetworkDespawn()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }
}