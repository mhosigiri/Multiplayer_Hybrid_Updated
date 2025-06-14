/**************************************************************
 * 
 *                      MOVE SCRIPT 
 * 
 * Purpose: This script moves a Game Object from its current
 * position to the endPosition and back. The speed that it moves
 * at can be set with the MovementSpeed variable.
 * 
 * To toggle the object's position between the start and end 
 * positions, call the MoveToggle function.
 * 
 **************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEngine.XR.Interaction.Toolkit
{
    public class Move : MonoBehaviour
    {
        public GameObject ObjectToMove;
        public Transform endPosition;
        public float MovementSpeed = 1;

        [SerializeField] internal UnityEvent OnMoveComplete;

        private Vector3 startPos;
        private Quaternion startRot;

        private void Awake()
        {
            startPos = ObjectToMove.transform.position;
            startRot = ObjectToMove.transform.rotation;

            if (endPosition == null)
                endPosition = transform;
        }

        public void MoveToggle()
        {
            if ((ObjectToMove.transform.position == endPosition.position)
                && (ObjectToMove.transform.rotation == endPosition.rotation))
            {
                Debug.Log("Moving to start position");
                StartCoroutine(MoveObj(startPos, startRot));
            }
            else
            {
                Debug.Log("Moving to end position");
                StartCoroutine(MoveObj(endPosition.position, endPosition.rotation));
            }
        }

        IEnumerator MoveObj(Vector3 pos, Quaternion rot)
        {
            float step;
            // Calculate step for moving position and/or rotation
            if ((ObjectToMove.transform.position - pos).magnitude != 0)
            {
                step = (MovementSpeed / (ObjectToMove.transform.position - pos).magnitude)
                    * Time.fixedDeltaTime;
            }
            // Calculate step for moving rotation only
            else
            {
                step = MovementSpeed * Time.fixedDeltaTime;
            }

            float t = 0;

            Vector3 a = ObjectToMove.transform.position;
            Vector3 b = pos;

            Quaternion c = ObjectToMove.transform.rotation;
            Quaternion d = rot;

            while (t <= 1.0f)
            {
                t += step; // Goes from 0 to 1, incrementing by step each time
                ObjectToMove.transform.position = Vector3.Lerp(a, b, t); // Move objectToMove closer to b
                ObjectToMove.transform.rotation = Quaternion.Lerp(c, d, t);
                yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
            }

            //ObjectToMove.transform.position = pos;
            //ObjectToMove.transform.rotation = rot;

            OnMoveComplete.Invoke();
        }
    }
}
