using UnityEngine;
using UnityEngine.UI;

public class PlayerRespawnSystem : MonoBehaviour
{
    [Header("Death UI")]
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private Button respawnButton;

    private PlayerHealth playerHealth;
    private PlayerMovement playerMovement;
    private PlayerCombat playerCombat;

    private Rigidbody rb;
    private Animator animator;

    private Vector3 spawnPosition;
    private Quaternion spawnRotation;

    private bool waitingForRespawn = false;

    private void Awake()
    {
        // 게임 시작 위치 저장
        spawnPosition = transform.position;
        spawnRotation = transform.rotation;

        playerHealth = GetComponent<PlayerHealth>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCombat = GetComponentInChildren<PlayerCombat>();

        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

        // 게임 시작 시 Death UI 숨김
        if (deathPanel != null)
        {
            deathPanel.SetActive(false);
        }

        // Respawn 버튼 자동 연결
        if (respawnButton != null)
        {
            respawnButton.onClick.AddListener(Respawn);
        }

        Time.timeScale = 1f;
    }

    private void OnDestroy()
    {
        if (respawnButton != null)
        {
            respawnButton.onClick.RemoveListener(Respawn);
        }
    }

    public void HandleDeath()
    {
        if (waitingForRespawn)
            return;

        waitingForRespawn = true;

        // 플레이어 이동 정지
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        // 플레이어 공격 정지
        if (playerCombat != null)
        {
            playerCombat.enabled = false;
        }

        // Rigidbody 움직임 정지
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Death UI 표시
        if (deathPanel != null)
        {
            deathPanel.SetActive(true);
            deathPanel.transform.SetAsLastSibling();
        }

        // 게임 일시정지
        Time.timeScale = 0f;
    }

    public void Respawn()
    {
        if (!waitingForRespawn)
            return;

        // 게임 시간 다시 시작
        Time.timeScale = 1f;

        // 시작 위치로 이동
        if (rb != null)
        {
            rb.position = spawnPosition;
            rb.rotation = spawnRotation;

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        else
        {
            transform.position = spawnPosition;
            transform.rotation = spawnRotation;
        }

        // HP 최대치로 회복
        if (playerHealth != null)
        {
            playerHealth.RestoreFullHealth();
        }

        // 애니메이션 초기화
        if (animator != null)
        {
            animator.Rebind();
            animator.Update(0f);
        }

        // 이동 다시 활성화
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }

        // 공격 다시 활성화
        if (playerCombat != null)
        {
            playerCombat.enabled = true;
        }

        // Death UI 숨김
        if (deathPanel != null)
        {
            deathPanel.SetActive(false);
        }

        waitingForRespawn = false;

        Debug.Log("Player Respawn");
    }
}