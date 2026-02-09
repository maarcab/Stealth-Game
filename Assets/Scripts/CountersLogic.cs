using UnityEngine;

public class CountersLogic : MonoBehaviour
{
    private Vector2 _lastPosition;
    public static float _distanceTraveled = 0f;
    public static float _elapsedTime = 0f;
    GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        _lastPosition = player.transform.position; 
    }
    private void OnEnable()
    {
        _elapsedTime = 0;
        _distanceTraveled = 0;
    }

    void Update()
    {
        _elapsedTime += Time.deltaTime; 

        Vector2 actualPos = player.transform.position;
        _distanceTraveled += Vector2.Distance(actualPos, _lastPosition);
        _lastPosition = actualPos;
    }

    public float GetElapsedTime()
    {
        return _elapsedTime;
    }

    public float GetDistanceTraveled()
    {
        return _distanceTraveled;
    }
}
