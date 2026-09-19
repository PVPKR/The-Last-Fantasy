using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackDamage = 10f;
    public float attackRange = 1.3f;
    public float attackCooldown = 1.5f;
    public float rotationSpeed = 8f;

    [Header("References")]
    public Transform player;
    public Transform characterModel;
    public Animator animator;

    private PlayerHealth playerHealth;
    private float lastAttackTime;

    void Start()
    {
        // Player가 Inspector에서 연결되지 않았다면 자동으로 찾기
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }

        // PlayerHealth 가져오기
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
        }
    }

    void Update()
    {
        if (player == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        // 공격 범위 안에 있을 때
        if (distance <= attackRange)
        {
            LookAtPlayer();

            // 공격 쿨타임 확인
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();
            }
        }
    }

    void LookAtPlayer()
    {
        Vector3 direction = player.position - transform.position;
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

    void Attack()
    {
        lastAttackTime = Time.time;

        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
    }

    // Attack 애니메이션의 타격 순간에 Animation Event로 호출
    public void DealDamage()
    {
        if (player == null || playerHealth == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        // 실제 타격 순간에도 범위 안인지 다시 확인
        if (distance <= attackRange)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }
}