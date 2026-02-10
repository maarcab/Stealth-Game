using UnityEngine;

public class ViewFieldRender : MonoBehaviour
{
    [SerializeField] ViewField viewField;
    [SerializeField] int resolution = 30;
    [SerializeField] float lineWidth = 0.1f;

    private LineRenderer lineRenderer;

    void Start()
    {
        if (viewField == null)
        {
            viewField = GetComponent<ViewField>();
        }
        if (viewField == null)
        {
            viewField = GetComponentInParent<ViewField>();
        }
        if (viewField == null)
        {
            Debug.LogError("ViewFieldRender: No se encontró componente ViewField.");
            return;
        }
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        
        lineRenderer.positionCount = resolution + 3; 
        lineRenderer.useWorldSpace = true;
        lineRenderer.loop = true; 
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = new Color(1, 0, 0, 0.5f);
        lineRenderer.endColor = new Color(1, 0, 0, 0.5f);
    }

    void Update()
    {
        if (viewField == null || lineRenderer == null) return;

        if (viewField.enemyMovement == null)
        {
            viewField.enemyMovement = viewField.GetComponentInParent<EnemyMovement>();
            if (viewField.enemyMovement == null) return;
        }
        DrawConeMesh();
    }

    void DrawConeMesh()
    {
        Vector2 center = viewField.transform.position;
        Vector2 direction;
        if (viewField.enemyMovement != null)
        {
            direction = viewField.enemyMovement.GetDirection().normalized;
        }
        else
        {
            direction = viewField.transform.right;
        }
        float halfAngle = viewField.coneAngle * Mathf.Deg2Rad / 2;
        float distance = viewField.coneDistance;

        lineRenderer.SetPosition(0, center);
       
        for (int i = 0; i <= resolution; i++)
        {
            float angle = -halfAngle + (2 * halfAngle * i / resolution);

            
            Vector2 dir = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg) * direction;

            Vector2 point = center + dir * distance;
            
            lineRenderer.SetPosition(i + 1, point);
        }
        lineRenderer.SetPosition(resolution + 2, center);
    }

    void OnDestroy()
    {
        if (lineRenderer != null)
            Destroy(lineRenderer);
    }
}