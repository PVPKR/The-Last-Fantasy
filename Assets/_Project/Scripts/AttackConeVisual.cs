using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class AttackConeVisual : MonoBehaviour
{
    [Header("Cone Settings")]
    [SerializeField] private float radius = 2.5f;
    [SerializeField] private float angle = 90f;
    [SerializeField] private int segments = 30;

    [Header("References")]
    [SerializeField] private Transform directionSource;

    private MeshFilter meshFilter;
    private Mesh coneMesh;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();

        CreateConeMesh();
    }

    private void LateUpdate()
    {
        if (directionSource == null)
            return;

        // 플레이어 모델이 바라보는 Y 방향만 따라감
        transform.rotation = Quaternion.Euler(
            0f,
            directionSource.eulerAngles.y,
            0f
        );
    }

    private void CreateConeMesh()
    {
        coneMesh = new Mesh();
        coneMesh.name = "Attack Cone Mesh";

        int vertexCount = segments + 2;

        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[segments * 3];

        // 부채꼴 중심
        vertices[0] = Vector3.zero;

        float halfAngle = angle * 0.5f;

        for (int i = 0; i <= segments; i++)
        {
            float currentAngle =
                Mathf.Lerp(-halfAngle, halfAngle, (float)i / segments);

            float radian = currentAngle * Mathf.Deg2Rad;

            float x = Mathf.Sin(radian) * radius;
            float z = Mathf.Cos(radian) * radius;

            vertices[i + 1] = new Vector3(x, 0f, z);
        }

        for (int i = 0; i < segments; i++)
        {
            int triangleIndex = i * 3;

            triangles[triangleIndex] = 0;
            triangles[triangleIndex + 1] = i + 1;
            triangles[triangleIndex + 2] = i + 2;
        }

        coneMesh.vertices = vertices;
        coneMesh.triangles = triangles;

        coneMesh.RecalculateNormals();
        coneMesh.RecalculateBounds();

        meshFilter.mesh = coneMesh;
    }

    public void SetRange(float newRadius, float newAngle)
    {
        radius = newRadius;
        angle = newAngle;

        CreateConeMesh();
    }
}