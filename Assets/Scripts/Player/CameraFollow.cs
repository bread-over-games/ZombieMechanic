using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform target;
    [SerializeField] private float followSpeed = 10f;

    [Header("Sway")]
    [SerializeField] private float swayAmount = 1.5f;
    [SerializeField] private float swaySpeed = 4f;
    [SerializeField] private float tiltAngle = 3f;

    [Header("Zoom")]
    [SerializeField] private float defaultHeight = 10f;
    [SerializeField] private float zoomedHeight = 5f;
    [SerializeField] private float defaultFOV = 60f;
    [SerializeField] private float zoomedFOV = 45f;
    [SerializeField] private float zoomSpeed = 3f;

    private Vector3 _prevTargetPos;
    private Vector3 _swayOffset;    

    private float _targetHeight;
    private float _targetFOV;

    private void OnEnable()
    {
        PlayerInteraction.OnInteractableApproached += _ => ZoomIn();
        PlayerInteraction.OnInteractableLeft += _ => ZoomOut();
    }

    private void OnDisable()
    {
        PlayerInteraction.OnInteractableApproached -= _ => ZoomIn();
        PlayerInteraction.OnInteractableLeft -= _ => ZoomOut();
    }

    private void Start()
    {
        _prevTargetPos = target.position;

        _targetHeight = defaultHeight;
        _targetFOV = defaultFOV;
    }

    private void FixedUpdate()
    {
        // Derive velocity from target movement
        Vector3 velocity = (target.position - _prevTargetPos) / Time.fixedDeltaTime;
        _prevTargetPos = target.position;

        // Sway offset follows velocity on X/Z, smoothed out
        Vector3 targetSway = new Vector3(velocity.x, 0f, velocity.z) * swayAmount * 0.1f;
        _swayOffset = Vector3.Lerp(_swayOffset, targetSway, swaySpeed * Time.fixedDeltaTime);

        // Tilt on the camera's local X/Z axes based on world velocity
        float tiltX = Vector3.Dot(velocity, transform.forward) * tiltAngle * 0.1f;
        float tiltZ = -Vector3.Dot(velocity, transform.right) * tiltAngle * 0.1f;
        Quaternion targetTilt = Quaternion.Euler(tiltX, 0f, tiltZ);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetTilt, swaySpeed * Time.fixedDeltaTime);

        // Follow + sway, zoom height applied on Y
        Vector3 targetPos = target.position + _swayOffset;
        targetPos.y = Mathf.Lerp(transform.position.y, _targetHeight, zoomSpeed * Time.fixedDeltaTime);
        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.fixedDeltaTime);

        // Smooth FOV transition
        if (_camera != null)
            _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, _targetFOV, zoomSpeed * Time.fixedDeltaTime);
    }

    public void ZoomIn()
    {
        _targetHeight = zoomedHeight;
        _targetFOV = zoomedFOV;
    }

    public void ZoomOut()
    {
        _targetHeight = defaultHeight;
        _targetFOV = defaultFOV;
    }
}