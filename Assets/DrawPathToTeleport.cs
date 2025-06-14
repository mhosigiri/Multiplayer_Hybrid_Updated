using System.Collections;
using UnityEngine;
using UnityEngine.AI;

//  Highlights and shows path to teleport anchor
public class DrawPathToTeleport : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private Transform player;
    [SerializeField] private LineRenderer path;
    [SerializeField] private float pathHeightOffset = 0.25f;
    [SerializeField] private float pathUpdateSpeed = 0.25f;

    //[SerializeField] private Material defaultMatrial;
    //[SerializeField] private Material highlightMaterial;

    //private GameObject teleportMarker;
    //private GameObject moveLocationIcon;

    //private MeshRenderer meshRendererTeleportMarker;
    //private MeshRenderer meshRendererMoveLocationIcon;

    private Coroutine drawPathCoroutine;

    void Start()
    {
        path.enabled = true;
        drawPathCoroutine = StartCoroutine(DrawPathToTarget());

        //teleportMarker = target.transform.GetChild(0).gameObject;
        //moveLocationIcon = target.transform.GetChild(1).GetChild(1).GetChild(1).gameObject;

        //meshRendererTeleportMarker = teleportMarker.GetComponent<MeshRenderer>();
        //meshRendererMoveLocationIcon = moveLocationIcon.GetComponent<MeshRenderer>();
    }

    private IEnumerator DrawPathToTarget()
    {
        WaitForSeconds Wait = new WaitForSeconds(pathUpdateSpeed);
        NavMeshPath path = new NavMeshPath();

        // Creates a path using NavMesh.CalculatePath(...) and takes the corners to draw the line
        while (target != null)
        {
            if (NavMesh.CalculatePath(player.position, target.transform.position, NavMesh.AllAreas, path))
            {
                this.path.positionCount = path.corners.Length;

                for (int i = 0; i < path.corners.Length; i++)
                {
                    // Offsets all corners by PathHightOFfset
                    this.path.SetPosition(i, path.corners[i] + Vector3.up * pathHeightOffset);
                }
            }

            yield return Wait;
        }
    }

    public void StartDrawingPathToNewTarget(GameObject NewTarget)
    {
        target = NewTarget;
        path.enabled = true;
        drawPathCoroutine = StartCoroutine(DrawPathToTarget());

        //teleportMarker = target.transform.GetChild(0).gameObject;
        //moveLocationIcon = target.transform.GetChild(1).GetChild(1).GetChild(1).gameObject;

        //meshRendererTeleportMarker = teleportMarker.GetComponent<MeshRenderer>();
        //meshRendererMoveLocationIcon = moveLocationIcon.GetComponent<MeshRenderer>();

        //meshRendererTeleportMarker.material = highlightMaterial;
        //meshRendererMoveLocationIcon.material = highlightMaterial;

        //moveLocationIcon.SetActive(true);
    }

    public void StopDrawingPath()
    {
        StopCoroutine(drawPathCoroutine);
        path.positionCount = 0;
        path.enabled = false;

        //meshRendererTeleportMarker.material = defaultMatrial;
        //meshRendererMoveLocationIcon.material = defaultMatrial;

        //moveLocationIcon.SetActive(false);
    }
}
