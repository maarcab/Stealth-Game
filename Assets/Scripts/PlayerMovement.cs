using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 5.0f;
    
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
        isMoving = inputVector.sqrMagnitude > 0.01f;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = inputVector * speed;
    }

    public Vector2 GetMoveInput()
    {
        return inputVector;
    }
}
