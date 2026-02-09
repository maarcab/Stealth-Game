using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;


public class ViewField : MonoBehaviour
{
    [SerializeField] EnemyMovement enemyMovement;
    [SerializeField] float coneDistance = 3.0f;
    [SerializeField] float coneAngle = 90f;
    

    public delegate void StateSet(State newState);
    public static event StateSet OnStateChange;

    GameObject player;

    private void Start()
    {
        enemyMovement = GetComponentInParent<EnemyMovement>();
        player = GameObject.FindGameObjectWithTag("Player");
       
    }

    private void Update()
    {
        bool playerDetected = IsPlayerInRange() && IsPlayerInAngle() && IsRaycastClear();

        if (playerDetected)
        {
            
            OnStateChange?.Invoke(State.Chase); 
        }
        else
        {
            OnStateChange?.Invoke(State.Patroll);
        }
    }

    bool IsPlayerInRange()
    {
        return Vector2.Distance(transform.position, player.transform.position) <= coneDistance;
    }

    bool IsPlayerInAngle()
    {
        Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;
        Vector2 enemyDirection = enemyMovement.GetDirection();
        float angle = Vector2.Angle(enemyDirection, directionToPlayer);
        return (angle <= coneAngle / 2);
    }

    bool IsRaycastClear()
    {
        Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, coneDistance);
        Debug.DrawRay(transform.position, directionToPlayer * coneDistance, Color.blue);

        return hit.collider != null && hit.collider.CompareTag("Player");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, coneDistance);

        Vector2 direction = enemyMovement != null ? enemyMovement.GetDirection().normalized : transform.right;
        float halfAngle = coneAngle / 2;

        Quaternion leftRayRotation = Quaternion.Euler(0, 0, -halfAngle);
        Quaternion rightRayRotation = Quaternion.Euler(0, 0, halfAngle);

        Vector2 leftRayDirection = leftRayRotation * direction;
        Vector2 rightRayDirection = rightRayRotation * direction;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, leftRayDirection * coneDistance);
        Gizmos.DrawRay(transform.position, rightRayDirection * coneDistance);
    }
}