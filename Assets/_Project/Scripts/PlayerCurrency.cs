using UnityEngine;
using TMPro;

public class PlayerCurrency : MonoBehaviour
{
    [Header("Currency")]
    [SerializeField] private int currentCoin = 0;

    [Header("UI")]
    [SerializeField] private TMP_Text coinText;

    public int CurrentCoin => currentCoin;

    private void Start()
    {
        UpdateCoinUI();
    }

    public void AddCoin(int amount)
    {
        if (amount <= 0)
            return;

        currentCoin += amount;

        UpdateCoinUI();
    }

    public bool SpendCoin(int amount)
    {
        if (amount <= 0)
            return false;

        if (currentCoin < amount)
            return false;

        currentCoin -= amount;

        UpdateCoinUI();

        return true;
    }

    public bool HasEnoughCoin(int amount)
    {
        return currentCoin >= amount;
    }

    private void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = currentCoin.ToString();
        }
    }
}