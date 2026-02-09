using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 5.0f;
    [SerializeField] float turnSpeed = 15.0f;

    private Rigidbody2D rb;
    private Vector2 inputVector;
    private bool isMoving;

    public bool IsMoving => isMoving;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputValue value)
    {
        inputVector = value.Get<Vector2>();
    }
    private void FixedUpdate()
    {
        rb.linearVelocity = inputVector * speed;
        isMoving = inputVector.sqrMagnitude > 0.01f;

        if (isMoving)
        {
            float targetAngle = Mathf.Atan2(inputVector.y, inputVector.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);
        }
    }
}
