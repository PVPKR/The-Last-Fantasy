using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [Header("Coin Settings")]
    [SerializeField] private int coinAmount = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerCurrency playerCurrency = other.GetComponent<PlayerCurrency>();

        if (playerCurrency == null)
        {
            playerCurrency = other.GetComponentInParent<PlayerCurrency>();
        }

        if (playerCurrency == null)
        {
            Debug.LogWarning("PlayerCurrency를 찾을 수 없습니다.");
            return;
        }

        playerCurrency.AddCoin(coinAmount);

        Destroy(gameObject);
    }
}