using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 10f;

    [SerializeField] private Rigidbody rb;
    private Vector2 moveInput;
    private Vector3 surfaceNormal = Vector3.up;
    private bool isColliding = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        moveInput = Vector2.ClampMagnitude(moveInput, 1f);
    }

    private void FixedUpdate()
    {
        isColliding = false;      // reset every physics frame
        surfaceNormal = Vector3.up;
        Move();
        Rotate();
    }

    private void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.normal.y < 0.9f)
            {
                surfaceNormal = contact.normal;
                isColliding = true;
                return;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        surfaceNormal = Vector3.up;
    }

    private void Move()
    {
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);

        if (moveInput.sqrMagnitude < 0.01f)
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            return;
        }

        Vector3 finalDirection = moveDirection;

        if (isColliding)
        {
            Vector3 projected = Vector3.ProjectOnPlane(moveDirection, surfaceNormal);
            finalDirection = projected.sqrMagnitude > 0.01f ? projected.normalized : Vector3.zero;
        }

        rb.linearVelocity = new Vector3(finalDirection.x * moveSpeed, rb.linearVelocity.y, finalDirection.z * moveSpeed);
    }

    private void Rotate()
    {
        if (moveInput.sqrMagnitude < 0.01f) // prevents rotation when player not moving
        {
            return;
        }            

        Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y);
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
    }
}