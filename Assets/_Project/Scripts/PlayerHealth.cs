using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;

    [Header("UI")]
    [SerializeField] private Slider healthSlider;

    public float CurrentHealth { get; private set; }

    public float MaxHealth => maxHealth;

    public bool IsDead => CurrentHealth <= 0f;

    private void Awake()
    {
        CurrentHealth = maxHealth;

        InitializeHealthBar();
    }

    private void InitializeHealthBar()
    {
        if (healthSlider == null)
            return;

        healthSlider.minValue = 0f;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = CurrentHealth;
        healthSlider.interactable = false;
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0f || IsDead)
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

    public void Heal(float amount)
    {
        if (amount <= 0f || IsDead)
            return;

        CurrentHealth += amount;

        CurrentHealth = Mathf.Clamp(
            CurrentHealth,
            0f,
            maxHealth
        );

        UpdateHealthBar();
    }

    public void IncreaseMaxHealth(float amount)
    {
        if (amount <= 0f)
            return;

        maxHealth += amount;

        // 증가한 HP만큼 현재 체력도 증가
        CurrentHealth += amount;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
        }

        UpdateHealthBar();
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
        Debug.Log("Player Dead");

        // 나중에 Game Over UI 연결
    }

    [ContextMenu("Test Damage 10")]
    private void TestDamage()
    {
        TakeDamage(10f);
    }

    [ContextMenu("Test Heal 10")]
    private void TestHeal()
    {
        Heal(10f);
    }
}