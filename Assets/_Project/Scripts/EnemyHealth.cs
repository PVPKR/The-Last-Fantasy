using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 30f;

    [Header("UI")]
    [SerializeField] private Slider healthSlider;

    [Header("Drop Settings")]
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private int coinDropAmount = 1;
    [SerializeField] private float dropHeight = 0.3f;

    public float CurrentHealth { get; private set; }

    private bool isDead = false;

    private void Awake()
    {
        CurrentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.minValue = 0f;
            healthSlider.maxValue = maxHealth;
            healthSlider.value = CurrentHealth;
            healthSlider.interactable = false;
        }
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0f || isDead)
            return;

        CurrentHealth -= damage;

        CurrentHealth = Mathf.Clamp(
            CurrentHealth,
            0f,
            maxHealth
        );

        UpdateHealthBar();

        if (CurrentHealth <= 0f)
        {
            Die();
        }
    }

    private void UpdateHealthBar()
    {
        if (healthSlider != null)
        {
            healthSlider.value = CurrentHealth;
        }
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        DropCoin();

        Debug.Log(gameObject.name + " 사망");

        Destroy(gameObject);
    }

    private void DropCoin()
    {
        if (coinPrefab == null)
            return;

        for (int i = 0; i < coinDropAmount; i++)
        {
            Vector3 dropPosition =
                transform.position +
                Vector3.up * dropHeight;

            Instantiate(
                coinPrefab,
                dropPosition,
                Quaternion.identity
            );
        }
    }

    [ContextMenu("Test Damage 10")]
    private void TestDamage()
    {
        TakeDamage(10f);
    }
}