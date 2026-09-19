using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 15f;

    [Header("References")]
    public FloatingJoystick joystick;
    public Transform characterModel;
    public Animator animator;

    private Rigidbody rb;
    private Vector3 moveDirection; // 입력값을 저장할 변수

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 1. 조이스틱 입력값을 전역 변수에 저장 (Update에서 한 번만 연산)
        moveDirection = new Vector3(joystick.Horizontal, 0f, joystick.Vertical);

        // 2. 이동 여부 판단 (데드존 적용)
        bool isMoving = moveDirection.magnitude > 0.1f;

        // 3. 애니메이션 처리
        if (animator != null)
        {
            animator.SetBool("isRun", isMoving);
        }

        // 4. 회전 처리
        if (isMoving)
        {
            Vector3 rotDir = moveDirection.normalized;
            Quaternion targetRotation = Quaternion.LookRotation(rotDir);

            characterModel.rotation = Quaternion.Slerp(
                characterModel.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    void FixedUpdate()
    {
        // 5. 물리 이동 처리 (Update에서 계산해 둔 방향 사용)
        if (moveDirection.magnitude > 0.1f)
        {
            Vector3 moveDir = moveDirection.normalized;

            rb.MovePosition(
                rb.position + moveDir * moveSpeed * Time.fixedDeltaTime
            );
        }
    }
}