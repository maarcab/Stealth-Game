using UnityEngine;
using UnityEngine.InputSystem;
public enum State
{
    Patroll, Chase
}
public class EnemyMovement : MonoBehaviour
{
 
    Rigidbody2D rb;
    public GameObject sprite;
    Vector2 newDir = Vector2.zero;
    public State state = State.Patroll;
    [SerializeField] float speed;

    //PatrolLogic
    [SerializeField] Transform[] points;
    int pointToGo = 0;
    float distanceTreshold = 0.1f;

    //ChaseLogic
    GameObject player;
    [SerializeField] float playerProximityThreshold = 0.5f; 

    //EndGame
    GameManager gameManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    private void OnEnable()
    {
        ViewField.OnStateChange += NewStateSet;
    }

    private void OnDisable()
    {
        ViewField.OnStateChange -= NewStateSet;
    }

    void Update()
    {
        //Set the direction
        switch (state)
        {
            case State.Patroll:
                if (checkProximity(points[pointToGo].position, transform.position))
                {
                    pointToGo = (pointToGo + 1) % points.Length;
                }
                newDir = movementDirection(points[pointToGo].position, transform.position);
                rb.linearVelocity = newDir * speed;
                break;

            case State.Chase:
                newDir = movementDirection(player.transform.position, transform.position);

                // Si esta muy cerca del jugador, se detiene
                if (Vector2.Distance(transform.position, player.transform.position) <= playerProximityThreshold)
                {
                    newDir = Vector2.zero; // Detenerse
                   // gameManager.OnEndGame(true); //AcabarJuego
                }
                rb.linearVelocity = newDir * speed;
                break;

        }

        //Rotate the sprite
        float angle = Mathf.Atan2(newDir.y, newDir.x) * Mathf.Rad2Deg;
        sprite.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    Vector2 movementDirection(Vector2 target, Vector2 position)
    {
        Vector2 dir = (target - position).normalized;
        return dir;
    }

    bool checkProximity(Vector2 obj, Vector2 position)
    {
        return Vector2.Distance(obj, position) <= distanceTreshold;
    }

    public Vector2 GetDirection()
    {
        return newDir;
    }

    void NewStateSet(State s)
    {
        Debug.Log(s.ToString());
        state = s;
    }
}
