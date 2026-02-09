
using UnityEngine;
using UnityEngine.UI;

public class UILogic : MonoBehaviour
{

    [SerializeField] private Text Distance;
    [SerializeField] private Text Time;

    void Update()
    {
        float distancia = CountersLogic._distanceTraveled;
        float tiempo = CountersLogic._elapsedTime;

        Distance.text = $"Distance: {distancia:F2}m";
        Time.text = $"Time: {tiempo:F2}s";
    }
}