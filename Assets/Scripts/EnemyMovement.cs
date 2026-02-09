using UnityEngine;
using UnityEngine.InputSystem;

public enum State
{
    Patroll, Chase, Waiting
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

    //WaitingLogic
    [SerializeField] float rotationSpeed = 2f;
    [SerializeField] float waitTime = 2f;
    private float waitTimer;

    //EndGame
    GameManager gameManager;

    ViewField viewField;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        viewField = GetComponentInChildren<ViewField>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    private void OnEnable()
    {
        viewField.OnStateChange += NewStateSet;
    }

    private void OnDisable()
    {
        viewField.OnStateChange -= NewStateSet;
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

            case State.Waiting:
                rb.linearVelocity = Vector2.zero;

                Vector2 targetLook = points[pointToGo].position - transform.position;
                float targetAngle = Mathf.Atan2(targetLook.y, targetLook.x) * Mathf.Rad2Deg;
                float currentAngle = sprite.transform.rotation.eulerAngles.z;
                float smoothAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * 100 * Time.deltaTime);

                sprite.transform.rotation = Quaternion.Euler(0, 0, smoothAngle);

                newDir = new Vector2(Mathf.Cos(smoothAngle * Mathf.Deg2Rad), Mathf.Sin(smoothAngle * Mathf.Deg2Rad));

                if (Mathf.Abs(Mathf.DeltaAngle(currentAngle, targetAngle)) < 5f) 
                {
                    waitTimer += Time.deltaTime;
                    if (waitTimer >= waitTime)
                    {
                        waitTimer = 0;
                        pointToGo = (pointToGo + 1) % points.Length; 
                    }
                }
                break;


        }

        //Rotate the sprite
        if (state != State.Waiting)
        {
            float angle = Mathf.Atan2(newDir.y, newDir.x) * Mathf.Rad2Deg;
            sprite.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
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
