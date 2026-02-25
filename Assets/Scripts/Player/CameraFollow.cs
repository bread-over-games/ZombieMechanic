using UnityEngine;

public class CameraFollow : MonoBehaviour
{    
    [SerializeField] private Transform target;    
    [SerializeField] private float followSpeed = 10f;

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position, followSpeed * Time.deltaTime);
    }
}