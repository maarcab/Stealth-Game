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

    EnemyAlarm alarm;
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

    private Animator animator;
    public bool isWalking = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        viewField = GetComponentInChildren<ViewField>();
        animator = GetComponentInChildren<Animator>();
        alarm = GetComponentInChildren<EnemyAlarm>();
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

        if (animator != null)
        {
            animator.SetBool("Walk", isWalking);
            if (newDir.magnitude > 0.01f)
            {
                animator.SetFloat("MoveX", newDir.x);
                animator.SetFloat("MoveY", newDir.y);
            }
        }

        //Set the direction
        switch (state)
        {
            case State.Patroll:
                isWalking = true;
                alarm.PlayerLeft();
                if (checkProximity(points[pointToGo].position, transform.position))
                {
                    pointToGo = (pointToGo + 1) % points.Length;
                }
                newDir = movementDirection(points[pointToGo].position, transform.position);
                rb.linearVelocity = newDir * speed;
                break;

            case State.Chase:
                isWalking = true;
                newDir = movementDirection(player.transform.position, transform.position);
                alarm.PlayerDetected();
                // Si esta muy cerca del jugador, se detiene
                if (Vector2.Distance(transform.position, player.transform.position) <= playerProximityThreshold)
                {
                    newDir = Vector2.zero; // Detenerse
                    rb.linearVelocity = Vector2.zero;
                    gameManager.OnEndGame(true); //AcabarJuego
                   
                }
                rb.linearVelocity = newDir * speed;
                break;

            case State.Waiting:
                isWalking = false;
                rb.linearVelocity = Vector2.zero;
                alarm.PlayerLeft();
                Vector2 targetVec = points[pointToGo].position - transform.position;
                Vector2 targetDir = targetVec.normalized;

                if (newDir == Vector2.zero) newDir = targetDir;

                newDir = Vector3.RotateTowards(newDir, targetDir, rotationSpeed * Time.deltaTime, 1f);
                newDir.Normalize();

                float angle = Vector2.Angle(newDir, targetDir);
                if (angle < 2f)
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
