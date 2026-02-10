using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SimpleUILogic : MonoBehaviour
{
    [Header("Distance UI")]
    public TMP_Text distanceText;
    public Image distanceIcon;

    [Header("Time UI")]
    public TMP_Text timeText;
    public Image timeIcon;

    [Header("Settings")]
    public Color normalColor = Color.white;
    public Color warningColor = Color.yellow;
    public float warningDistance = 50f;

    void Update()
    {
        float dist = CountersLogic.distanceTraveled;
        float time = CountersLogic.elapsedTime;

        distanceText.text = $"<b>DISTANCE:</b> {dist:F1}m";
        timeText.text = $"<b>TIME:</b> {time:F1}s";

        UpdateVisualEffects(dist, time);
    }

    void UpdateVisualEffects(float distance, float time)
    {
        if (distance > warningDistance)
        {
            float lerpValue = Mathf.Clamp01((distance - warningDistance) / 50f);
            distanceText.color = Color.Lerp(normalColor, warningColor, lerpValue);
        }
        else
        {
            distanceText.color = normalColor;
        }

        if (Mathf.Floor(time) % 10 == 0)
        {
            float pulse = (Mathf.Sin(Time.time * 5f) + 1) * 0.5f;
            timeText.color = Color.Lerp(normalColor, Color.cyan, pulse);
        }
        else
        {
            timeText.color = normalColor;
        }
    }
}