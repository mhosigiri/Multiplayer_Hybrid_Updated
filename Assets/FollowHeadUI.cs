using UnityEngine;

public class FollowHeadUI : UIWindow
{
    private GameObject menu;
    [SerializeField] private Transform head;
    [SerializeField] private float spawnDistance = 2;

    [SerializeField] private bool turnOnSmoothing;

    [SerializeField] private float movementDistanceThreshold = 0.1f;
    [SerializeField] private float lerpDuration = 0.5f;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private Vector3 cameraViewPostion;
    private float elapsedTime = 0;

    [SerializeField] private AnimationCurve curve;

    [Header("Rotation Settings")]

    [SerializeField] private BillboardType billboardType;

    [Header("Lock Rotation")]
    [SerializeField] private bool lockX;
    [SerializeField] private bool lockY;
    [SerializeField] private bool lockZ;

    private Vector3 originalRotation;

    public enum BillboardType { LookAtCamera, CameraForward };


    override internal void Start()
    {
        menu = gameObject;
        UpdateEndPosition();
        menu.transform.position = endPosition;

        base.Start();
    }

    void Update()
    {

    }

    // Late update so that it updates once everything is finished moving
    void LateUpdate()
    {
        if (menu.activeSelf)
        {
            RepositionUI();
            RotateUI();
        }
    }

    private void RepositionUI()
    {
        if(turnOnSmoothing) {
            cameraViewPostion = head.position + new Vector3(head.forward.x, head.forward.y - 0.25f, head.forward.z).normalized * spawnDistance;

            if (Vector3.Distance(cameraViewPostion, endPosition) > movementDistanceThreshold)
            {
                UpdateEndPosition();
                UpdateStartPosition();

                elapsedTime = 0;
            }

            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / lerpDuration;

            menu.transform.position = Vector3.Lerp(startPosition, endPosition, curve.Evaluate(percentageComplete));
        }  else
        {
            menu.transform.position = head.position + new Vector3(head.forward.x, head.forward.y - 0.25f, head.forward.z).normalized * spawnDistance;
        }
    }

    private void RotateUI()
    {
        // Two ways people billboard things.
        switch (billboardType)
        {
            case BillboardType.LookAtCamera:
                transform.LookAt(Camera.main.transform.position, Vector3.up);
                transform.forward *= -1;
                break;
            case BillboardType.CameraForward:
                transform.forward = Camera.main.transform.forward;
                break;
            default:
                break;
        }
        // Modify the rotation in Euler space to lock certain dimensions.
        Vector3 rotation = transform.rotation.eulerAngles;
        if (lockX) { rotation.x = originalRotation.x; }
        if (lockY) { rotation.y = originalRotation.y; }
        if (lockZ) { rotation.z = originalRotation.z; }
        transform.rotation = Quaternion.Euler(rotation);
    }

    private void UpdateEndPosition()
    {
        endPosition = head.position + new Vector3(head.forward.x, head.forward.y - 0.25f, head.forward.z).normalized * spawnDistance;
    }

    private void UpdateStartPosition()
    {
        startPosition = menu.transform.position;
    }

}
