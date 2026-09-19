using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float rotationSpeed = 8f;
    public float detectionRange = 20f;
    public float attackRange = 1.3f;

    [Header("References")]
    public Transform player;
    public Transform characterModel;
    public Animator animator;

    private Rigidbody rb;
    private bool isChasing;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }
    }

    void Update()
    {
        if (player == null)
            return;

        float distance =
            Vector3.Distance(transform.position, player.position);

        // 감지 범위 안이면서 공격 범위 밖이면 추적
        isChasing =
            distance <= detectionRange &&
            distance > attackRange;

        if (animator != null)
        {
            animator.SetBool("isRun", isChasing);
        }

        if (isChasing)
        {
            RotateTowardsPlayer();
        }
    }

    void FixedUpdate()
    {
        if (!isChasing || player == null || rb == null)
            return;

        Vector3 direction = player.position - rb.position;
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.01f)
            return;

        Vector3 moveDirection = direction.normalized;

        rb.MovePosition(
            rb.position +
            moveDirection * moveSpeed * Time.fixedDeltaTime
        );
    }

    void RotateTowardsPlayer()
    {
        Vector3 direction =
            player.position - transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.01f)
            return;

        Quaternion targetRotation =
            Quaternion.LookRotation(direction.normalized);

        if (characterModel != null)
        {
            characterModel.rotation = Quaternion.Slerp(
                characterModel.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }
}