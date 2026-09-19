using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class AttackRangeCircle : MonoBehaviour
{
    [Header("Circle Settings")]
    [SerializeField] private float radius = 2.5f;
    [SerializeField] private int segments = 80;
    [SerializeField] private float lineWidth = 0.05f;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        DrawCircle();
    }

    private void DrawCircle()
    {
        lineRenderer.useWorldSpace = false;
        lineRenderer.loop = true;

        lineRenderer.positionCount = segments;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        for (int i = 0; i < segments; i++)
        {
            float angle =
                ((float)i / segments) * Mathf.PI * 2f;

            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            lineRenderer.SetPosition(
                i,
                new Vector3(x, 0f, z)
            );
        }
    }

    public void SetRadius(float newRadius)
    {
        radius = newRadius;
        DrawCircle();
    }
}