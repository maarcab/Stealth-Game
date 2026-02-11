using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement movement;

    void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (movement != null && animator != null)
        {
            animator.SetBool("Walk", movement.IsMoving);

            if (movement.IsMoving)
            {
                Vector2 dir = movement.GetMoveInput();
                animator.SetFloat("MoveX", dir.x);
                animator.SetFloat("MoveY", dir.y);
            }
        }
    }
}
