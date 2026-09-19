using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUpgradeSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerCurrency playerCurrency;
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private PlayerHealth playerHealth;

    [Header("Upgrade Settings")]
    [SerializeField] private int maxLevel = 5;

    [Header("Attack Upgrade")]
    [SerializeField] private int attackLevel = 1;
    [SerializeField] private float attackIncreaseAmount = 5f;
    [SerializeField] private int attackBaseCost = 5;
    [SerializeField] private int attackCostIncrease = 5;

    [Header("Health Upgrade")]
    [SerializeField] private int healthLevel = 1;
    [SerializeField] private float healthIncreaseAmount = 20f;
    [SerializeField] private int healthBaseCost = 5;
    [SerializeField] private int healthCostIncrease = 5;

    [Header("Attack UI")]
    [SerializeField] private TMP_Text attackLevelText;
    [SerializeField] private TMP_Text attackCostText;
    [SerializeField] private Button attackUpgradeButton;

    [Header("Health UI")]
    [SerializeField] private TMP_Text healthLevelText;
    [SerializeField] private TMP_Text healthCostText;
    [SerializeField] private Button healthUpgradeButton;

    public int AttackLevel => attackLevel;
    public int HealthLevel => healthLevel;

    public bool IsMaxLevel =>
        attackLevel >= maxLevel &&
        healthLevel >= maxLevel;

    private void Start()
    {
        UpdateUI();
    }

    public void UpgradeAttack()
    {
        if (attackLevel >= maxLevel)
            return;

        int cost = GetAttackUpgradeCost();

        if (!playerCurrency.SpendCoin(cost))
            return;

        attackLevel++;

        playerCombat.IncreaseAttackDamage(
            attackIncreaseAmount
        );

        UpdateUI();
    }

    public void UpgradeHealth()
    {
        if (healthLevel >= maxLevel)
            return;

        int cost = GetHealthUpgradeCost();

        if (!playerCurrency.SpendCoin(cost))
            return;

        healthLevel++;

        playerHealth.IncreaseMaxHealth(
            healthIncreaseAmount
        );

        UpdateUI();
    }

    private int GetAttackUpgradeCost()
    {
        return attackBaseCost +
               ((attackLevel - 1) * attackCostIncrease);
    }

    private int GetHealthUpgradeCost()
    {
        return healthBaseCost +
               ((healthLevel - 1) * healthCostIncrease);
    }

    private void UpdateUI()
    {
        UpdateAttackUI();
        UpdateHealthUI();
    }

    private void UpdateAttackUI()
    {
        if (attackLevelText != null)
        {
            attackLevelText.text =
                "Attack Lv." + attackLevel;
        }

        if (attackLevel >= maxLevel)
        {
            if (attackCostText != null)
                attackCostText.text = "MAX";

            if (attackUpgradeButton != null)
                attackUpgradeButton.interactable = false;
        }
        else
        {
            if (attackCostText != null)
            {
                attackCostText.text =
                    "Coin " + GetAttackUpgradeCost();
            }

            if (attackUpgradeButton != null)
                attackUpgradeButton.interactable = true;
        }
    }

    private void UpdateHealthUI()
    {
        if (healthLevelText != null)
        {
            healthLevelText.text =
                "Health Lv." + healthLevel;
        }

        if (healthLevel >= maxLevel)
        {
            if (healthCostText != null)
                healthCostText.text = "MAX";

            if (healthUpgradeButton != null)
                healthUpgradeButton.interactable = false;
        }
        else
        {
            if (healthCostText != null)
            {
                healthCostText.text =
                    "Coin " + GetHealthUpgradeCost();
            }

            if (healthUpgradeButton != null)
                healthUpgradeButton.interactable = true;
        }
    }
}