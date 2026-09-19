using UnityEngine;

public class HealingZone : MonoBehaviour
{
    [Header("Healing Settings")]
    [SerializeField] private float healPerSecond = 20f;

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth playerHealth =
            other.GetComponentInParent<PlayerHealth>();

        if (playerHealth == null)
            return;

        Debug.Log(
            "Healing Zone 진입 / 현재 HP: " +
            playerHealth.CurrentHealth +
            " / " +
            playerHealth.MaxHealth
        );
    }

    private void OnTriggerStay(Collider other)
    {
        PlayerHealth playerHealth =
            other.GetComponentInParent<PlayerHealth>();

        if (playerHealth == null)
            return;

        if (playerHealth.CurrentHealth >= playerHealth.MaxHealth)
            return;

        float beforeHealth = playerHealth.CurrentHealth;

        playerHealth.Heal(
            healPerSecond * Time.fixedDeltaTime
        );

        Debug.Log(
            "Healing 중: " +
            beforeHealth +
            " → " +
            playerHealth.CurrentHealth
        );
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerHealth playerHealth =
            other.GetComponentInParent<PlayerHealth>();

        if (playerHealth == null)
            return;

        Debug.Log("Healing Zone 이탈");
    }
}