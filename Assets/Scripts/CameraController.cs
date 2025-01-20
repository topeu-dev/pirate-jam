using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private CinemachineCamera virtualCamera;

    [SerializeField]
    private Transform cameraTarget;

    private GameObject _virtualCameraGameObject;

    private InputAction _movement;
    private InputAction _cameraRotation;
    private float _targetZoom;
    private float _targetRotationY;
    private float _movementSpeedZoomAmplifier = 0.2f;

    [Header("Zoom")]
    [SerializeField]
    private float zoomMin;

    [SerializeField]
    private float zoomMax;

    [SerializeField]
    private float zoomStep;

    [SerializeField]
    private float zoomSpeed;


    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private float movementTime;


    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private float rotationTime;

    [Space]
    [Header("Boundaries")]
    [SerializeField]
    public bool enableBoundaries = true;

    [SerializeField]
    public float BoundaryMinX = -500f;

    [SerializeField]
    public float BoundaryMaxX = 500f;

    [SerializeField]
    public float BoundaryMinZ = -500f;

    [SerializeField]
    public float BoundaryMaxZ = 500f;

    private void Awake()
    {
        _movement = InputActionSingleton.GeneralInputActions.Camera.CameraMovement;
        _cameraRotation = InputActionSingleton.GeneralInputActions.Camera.CameraRotation;
        InputActionSingleton.GeneralInputActions.Camera.Zoom.performed += ZoomCamera;

        _targetZoom = virtualCamera.Lens.OrthographicSize;
        _targetRotationY = virtualCamera.transform.eulerAngles.y;
        _virtualCameraGameObject = virtualCamera.gameObject;
    }


    private void ZoomCamera(InputAction.CallbackContext context)
    {
        float zoomDir = context.ReadValue<float>();
        _targetZoom = zoomDir < 0
            ? Mathf.Clamp(_targetZoom + zoomStep, zoomMin, zoomMax)
            : Mathf.Clamp(_targetZoom - zoomStep, zoomMin, zoomMax);
    }

    private void OnEnable()
    {
        InputActionSingleton.GeneralInputActions.Camera.Zoom.Enable();
        InputActionSingleton.GeneralInputActions.Camera.CameraMovement.Enable();
        InputActionSingleton.GeneralInputActions.Camera.CameraRotation.Enable();
    }

    private void OnDisable()
    {
        InputActionSingleton.GeneralInputActions.Camera.Zoom.Disable();
        InputActionSingleton.GeneralInputActions.Camera.CameraMovement.Disable();
        InputActionSingleton.GeneralInputActions.Camera.CameraRotation.Disable();
    }

    private void HandleZoom()
    {
        if (virtualCamera != null)
        {
            virtualCamera.Lens.OrthographicSize = Mathf.Lerp(virtualCamera.Lens.OrthographicSize, _targetZoom,
                Time.deltaTime * zoomSpeed);
        }
    }

    private void HandleBoundaries()
    {
        if (cameraTarget.position.x > BoundaryMaxX)
            cameraTarget.position = new Vector3(BoundaryMaxX, cameraTarget.position.y, cameraTarget.position.z);
        if (cameraTarget.position.x < BoundaryMinX)
            cameraTarget.position = new Vector3(BoundaryMinX, cameraTarget.position.y, cameraTarget.position.z);
        if (cameraTarget.position.z > BoundaryMaxZ)
            cameraTarget.position = new Vector3(cameraTarget.position.x, cameraTarget.position.y, BoundaryMaxZ);
        if (cameraTarget.position.z < BoundaryMinZ)
            cameraTarget.position = new Vector3(cameraTarget.position.x, cameraTarget.position.y, BoundaryMinZ);
    }

    private void HandleMovementInput()
    {
        Vector2 moveInput = _movement.ReadValue<Vector2>();

        if (moveInput.sqrMagnitude > 0f)
        {
            Vector3 moveVector = new Vector3(moveInput.x, 0, moveInput.y);
            MoveTargetRelativeToCamera(moveVector, 1f);
        }
    }

    private void MoveTargetRelativeToCamera(Vector3 direction, float speed)
    {
        _movementSpeedZoomAmplifier = Mathf.Clamp(virtualCamera.Lens.OrthographicSize / zoomMax, 0.2f, 1f);
        Vector3 camForward = _virtualCameraGameObject.transform.forward;
        Vector3 camRight = _virtualCameraGameObject.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();
        Vector3 relativeDir = (2 * direction.z * camForward) + (camRight * direction.x);

        cameraTarget.Translate(relativeDir * (movementSpeed * speed * Time.deltaTime * _movementSpeedZoomAmplifier));
    }


    private void Update()
    {
        HandleMovementInput();
        HandleZoom();
        HandleRotation();
        HandleBoundaries();
    }

    private void HandleRotation()
    {
        float rotateDir = _cameraRotation.ReadValue<float>();

        if (!Mathf.Approximately(rotateDir, 0f))
        {
            _targetRotationY += -rotateDir * rotationSpeed * Time.deltaTime;
        }

        float currentY = transform.rotation.eulerAngles.y;
        float smoothedRotationY = Mathf.LerpAngle(currentY, _targetRotationY, rotationTime * Time.deltaTime);

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, smoothedRotationY, 0f);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (enableBoundaries)
        {
            Handles.color = Color.green;
            Handles.DrawLine(new Vector3(BoundaryMinX, 0, BoundaryMinZ), new Vector3(BoundaryMaxX, 0, BoundaryMinZ));
            Handles.DrawLine(new Vector3(BoundaryMaxX, 0, BoundaryMinZ), new Vector3(BoundaryMaxX, 0, BoundaryMaxZ));
            Handles.DrawLine(new Vector3(BoundaryMinX, 0, BoundaryMinZ), new Vector3(BoundaryMinX, 0, BoundaryMaxZ));
            Handles.DrawLine(new Vector3(BoundaryMinX, 0, BoundaryMaxZ), new Vector3(BoundaryMaxX, 0, BoundaryMaxZ));
            Handles.Label(new Vector3(BoundaryMinX, 0, 0), $"Min X: {BoundaryMinX}");
            Handles.Label(new Vector3(BoundaryMaxX, 0, 0), $"Max X: {BoundaryMaxX}");
            Handles.Label(new Vector3(0, 0, BoundaryMinZ), $"Min Z: {BoundaryMinZ}");
            Handles.Label(new Vector3(0, 0, BoundaryMaxZ), $"Max Z: {BoundaryMaxZ}");
        }
    }

#endif
}