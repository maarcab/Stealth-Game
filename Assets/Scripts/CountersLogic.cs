using UnityEngine;

public class CountersLogic : MonoBehaviour
{
    private Vector2 _lastPosition;
    public static float _distanceTraveled = 0f;
    public static float _elapsedTime = 0f;
    private Vector2 lastPosition;
    public static float distanceTraveled = 0f;
    public static float elapsedTime = 0f;
    GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        _lastPosition = player.transform.position;
        lastPosition = player.transform.position;
    }
    private void OnEnable()
    {
        _elapsedTime = 0;
        _distanceTraveled = 0;
        elapsedTime = 0;
        distanceTraveled = 0;
    }

    void Update()
    {
        _elapsedTime += Time.deltaTime;
        elapsedTime += Time.deltaTime;

        Vector2 actualPos = player.transform.position;
        _distanceTraveled += Vector2.Distance(actualPos, _lastPosition);
        _lastPosition = actualPos;
        distanceTraveled += Vector2.Distance(actualPos, lastPosition);
        lastPosition = actualPos;
    }

    public float GetElapsedTime()
    {
        return _elapsedTime;
        return elapsedTime;
    }

    public float GetDistanceTraveled()
    {
        return _distanceTraveled;
        return distanceTraveled;
    }
}
