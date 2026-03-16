using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float followSpeed = 10f;

    [Header("Sway")]
    [SerializeField] private float swayAmount = 1.5f;
    [SerializeField] private float swaySpeed = 4f;
    [SerializeField] private float tiltAngle = 3f;

    private Vector3 _prevTargetPos;
    private Vector3 _swayOffset;
    private float _tilt;

    private void Start()
    {
        _prevTargetPos = target.position;
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
        // Uses camera-local axes so tilt always feels correct regardless of rotation
        float tiltX = Vector3.Dot(velocity, transform.forward) * tiltAngle * 0.1f;
        float tiltZ = -Vector3.Dot(velocity, transform.right) * tiltAngle * 0.1f;
        Quaternion targetTilt = Quaternion.Euler(tiltX, 0f, tiltZ);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetTilt, swaySpeed * Time.fixedDeltaTime);

        // Follow + sway
        Vector3 targetPos = target.position + _swayOffset;
        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.fixedDeltaTime);
    }
}