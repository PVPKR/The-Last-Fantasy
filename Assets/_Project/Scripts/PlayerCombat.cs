using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float detectionRange = 3f;
    [SerializeField] private float attackRange = 2.5f;
    [SerializeField] private float attackAngle = 90f;
    [SerializeField] private float attackCooldown = 0.8f;

    [Header("Rotation Settings")]
    [SerializeField] private float attackRotationSpeed = 15f;
    [SerializeField] private float attackRotationThreshold = 5f;

    [Header("Visual Settings")]
    [SerializeField] private float coneDisplayDuration = 0.25f;

    [Header("References")]
    [SerializeField] private Transform playerRoot;
    [SerializeField] private Transform characterModel;
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private GameObject attackRangeVisual;
    [SerializeField] private GameObject attackConeVisual;

    private float lastAttackTime = -999f;

    private EnemyHealth currentTarget;
    private Coroutine coneCoroutine;

    public float AttackDamage => attackDamage;

    private void Start()
    {
        if (attackRangeVisual != null)
        {
            attackRangeVisual.SetActive(false);
        }

        if (attackConeVisual != null)
        {
            attackConeVisual.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerRoot == null || characterModel == null)
            return;

        // 주변에 몬스터가 있는지 확인
        Collider[] nearbyEnemies = Physics.OverlapSphere(
            playerRoot.position,
            detectionRange,
            enemyLayer
        );

        bool hasEnemyNearby = nearbyEnemies.Length > 0;

        // 주변에 적이 있으면 원형 공격 범위 표시
        if (attackRangeVisual != null)
        {
            attackRangeVisual.SetActive(hasEnemyNearby);
        }

        // 공격 범위 안에서 가장 가까운 적 찾기
        currentTarget = FindNearestEnemy();

        if (currentTarget == null)
            return;

        // 현재 캐릭터가 몬스터를 얼마나 바라보고 있는지 확인
        float angleToTarget =
            GetAngleToTarget(currentTarget.transform);

        // 적을 충분히 바라봤고 공격 쿨타임이 끝났다면 공격
        if (angleToTarget <= attackRotationThreshold)
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();
            }
        }
    }

    private void LateUpdate()
    {
        // 이동 방향 회전보다 전투 타겟 방향 회전을 우선
        if (currentTarget != null)
        {
            RotateTowardsTarget(currentTarget.transform);
        }
    }

    private EnemyHealth FindNearestEnemy()
    {
        Collider[] enemies = Physics.OverlapSphere(
            playerRoot.position,
            attackRange,
            enemyLayer
        );

        EnemyHealth nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        HashSet<EnemyHealth> checkedEnemies =
            new HashSet<EnemyHealth>();

        foreach (Collider enemyCollider in enemies)
        {
            EnemyHealth enemyHealth =
                enemyCollider.GetComponentInParent<EnemyHealth>();

            if (enemyHealth == null)
                continue;

            if (enemyHealth.CurrentHealth <= 0f)
                continue;

            if (checkedEnemies.Contains(enemyHealth))
                continue;

            checkedEnemies.Add(enemyHealth);

            float distance = Vector3.Distance(
                playerRoot.position,
                enemyHealth.transform.position
            );

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemyHealth;
            }
        }

        return nearestEnemy;
    }

    private void RotateTowardsTarget(Transform target)
    {
        Vector3 direction =
            target.position - characterModel.position;

        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.01f)
            return;

        Quaternion targetRotation =
            Quaternion.LookRotation(direction.normalized);

        characterModel.rotation = Quaternion.Slerp(
            characterModel.rotation,
            targetRotation,
            attackRotationSpeed * Time.deltaTime
        );
    }

    private float GetAngleToTarget(Transform target)
    {
        Vector3 direction =
            target.position - characterModel.position;

        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.01f)
            return 0f;

        Vector3 forward = characterModel.forward;
        forward.y = 0f;

        return Vector3.Angle(
            forward.normalized,
            direction.normalized
        );
    }

    private void Attack()
    {
        lastAttackTime = Time.time;

        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        // 공격 순간 부채꼴 범위 표시
        if (attackConeVisual != null)
        {
            if (coneCoroutine != null)
            {
                StopCoroutine(coneCoroutine);
            }

            coneCoroutine = StartCoroutine(
                ShowAttackCone()
            );
        }
    }

    private IEnumerator ShowAttackCone()
    {
        attackConeVisual.SetActive(true);

        yield return new WaitForSeconds(
            coneDisplayDuration
        );

        attackConeVisual.SetActive(false);

        coneCoroutine = null;
    }

    // Attack 애니메이션의 Animation Event에서 호출
    public void DealDamage()
    {
        Collider[] enemies = Physics.OverlapSphere(
            playerRoot.position,
            attackRange,
            enemyLayer
        );

        Vector3 forward = characterModel.forward;
        forward.y = 0f;

        HashSet<EnemyHealth> damagedEnemies =
            new HashSet<EnemyHealth>();

        foreach (Collider enemyCollider in enemies)
        {
            EnemyHealth enemyHealth =
                enemyCollider.GetComponentInParent<EnemyHealth>();

            if (enemyHealth == null)
                continue;

            if (enemyHealth.CurrentHealth <= 0f)
                continue;

            if (damagedEnemies.Contains(enemyHealth))
                continue;

            Vector3 direction =
                enemyHealth.transform.position - playerRoot.position;

            direction.y = 0f;

            if (direction.sqrMagnitude <= 0.01f)
                continue;

            float angle = Vector3.Angle(
                forward.normalized,
                direction.normalized
            );

            // 정면 부채꼴 범위 안에 있는 적만 공격
            if (angle <= attackAngle * 0.5f)
            {
                enemyHealth.TakeDamage(attackDamage);

                damagedEnemies.Add(enemyHealth);
            }
        }
    }

    // 공격력 강화 시스템에서 호출
    public void IncreaseAttackDamage(float amount)
    {
        if (amount <= 0f)
            return;

        attackDamage += amount;

        Debug.Log("Attack Damage: " + attackDamage);
    }
}