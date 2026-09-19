using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;          // 따라갈 대상 (플레이어)

    [Header("Camera Position & Angle")]
    public Vector3 offset = new Vector3(0f, 12f, -8f); // 캐릭터와의 거리 및 각도 (쿼터뷰)
    public float followSpeed = 10f;   // 카메라가 따라오는 속도

    void LateUpdate()
    {
        if (target == null) return;

        // 1. 목표 위치 계산 (플레이어 위치 + 오프셋)
        Vector3 targetPosition = target.position + offset;

        // 2. 현재 카메라 위치에서 목표 위치로 부드럽게 이동 (Lerp)
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // 3. 카메라가 항상 플레이어 캐릭터를 바라보도록 설정
        transform.LookAt(target);
    }
}