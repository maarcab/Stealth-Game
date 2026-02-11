using UnityEngine;

public class CountersLogic : MonoBehaviour
{
 
    private Vector2 lastPosition;
    public static float distanceTraveled = 0f;
    public static float elapsedTime = 0f;
    GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        lastPosition = player.transform.position;
    }
    private void OnEnable()
    {
        elapsedTime = 0;
        distanceTraveled = 0;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
     
        Vector2 actualPos = player.transform.position;
        distanceTraveled += Vector2.Distance(actualPos, lastPosition);
        lastPosition = actualPos;
    }

    public float GetElapsedTime()
    {
        return elapsedTime; 
    }

    public float GetDistanceTraveled()
    {
        return distanceTraveled;
    }
}
