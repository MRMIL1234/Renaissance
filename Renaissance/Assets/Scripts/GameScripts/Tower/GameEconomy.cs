using UnityEngine;
using TMPro;

public class GameEconomy : MonoBehaviour
{
    public static GameEconomy Instance { get; private set; }

    [Header("Налаштування")]
    [SerializeField] private int coins = 100;

    [Header("UI Текст")]
    [SerializeField] private TextMeshProUGUI coinsText;

    public int Coins
    {
        get { return coins; }
        set
        {
            coins = value;
            UpdateCoinsUI();

            PlacementManager placementManager = Object.FindFirstObjectByType<PlacementManager>();
            if (placementManager != null)
            {
                placementManager.RefreshUpgradeUIIfActive();
            }
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateCoinsUI();
    }

    public bool CanAfford(int amount)
    {
        return coins >= amount;
    }

    public void SpendCoins(int amount)
    {
        if (CanAfford(amount))
        {
            Coins -= amount;
        }
    }

    public void AddCoins(int amount)
    {
        Coins += amount;
    }

    private void UpdateCoinsUI()
    {
        if (coinsText != null)
        {
            coinsText.text = coins.ToString();
        }
    }
}