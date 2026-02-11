using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 5.0f;
    
    private Rigidbody2D rb;
    private Vector2 inputVector;
    private bool isMoving;
    private Animator animator;

    public bool IsMoving => isMoving;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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
           

            animator.SetFloat("MoveX", inputVector.x);
            animator.SetFloat("MoveY", inputVector.y);
        }
    }
    public Vector2 GetMoveInput()
    {
        return rb.linearVelocity.normalized;
    }
}
