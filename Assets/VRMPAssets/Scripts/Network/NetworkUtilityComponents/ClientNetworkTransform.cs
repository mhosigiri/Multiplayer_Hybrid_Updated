using UnityEngine;
using Unity.Netcode.Components;

namespace Unity.Multiplayer.Samples.Utilities.ClientAuthority
{
    /// <summary>
    /// ClientNetworkTransform class is responsible for updating the
    /// <see cref="NetworkTransform"/> from the local owner perspective.
    /// </summary>
    [DisallowMultipleComponent]
    public class ClientNetworkTransform : NetworkTransform
    {
        /// <summary>
        /// If true, only the Server can update the transform of the object.
        /// </summary>
        // Make this a public property with [field: SerializeField]
        // This makes it show up in the Inspector and links it directly to the property getter/setter.
        //[Tooltip("Determines Local or Server transform updating.")]
        //[SerializeField] protected bool ServerAuthoritative = false; // Renamed to follow C# naming conventions (PascalCase for public properties)

        ///<inheritdoc/>
        protected override bool OnIsServerAuthoritative()
        {
            // Now, this override will return the value of your public property
            return false;
        }
    }
}