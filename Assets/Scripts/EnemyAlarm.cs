using UnityEngine;

public class EnemyAlarm : MonoBehaviour
{
    SpriteRenderer alarmRenderer;

    public void PlayerDetected()
    {
        ChangeColor(Color.red);
    }

    public void PlayerLeft()
    {
        ChangeColor(Color.yellow);
    }

    private void ChangeColor(Color color)
    {
        if (alarmRenderer == null) alarmRenderer = GetComponent<SpriteRenderer>();

        alarmRenderer.color = color;
    }
}
