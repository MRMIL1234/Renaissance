using UnityEngine;
using TMPro;

public class GameEconomy : MonoBehaviour
{
    // Тепер ніхто ззовні не зможе випадково стерти або підмінити Instance
    public static GameEconomy Instance { get; private set; }

    [Header("Налаштування")]
    [SerializeField] private int coins = 100;

    [Header("UI Текст")]
    [SerializeField] private TextMeshProUGUI coinsText;

    // Властивість для безпечного читання кількості монет іншими скриптами
    public int Coins
    {
        get { return coins; }
        set { coins = value; 
        }
    }


    private void Awake()
    {
        // Класичний Singleton захист
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
            coins -= amount;
            UpdateCoinsUI();
        }
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateCoinsUI();
    }

    private void UpdateCoinsUI()
    {
        if (coinsText != null)
        {
            coinsText.text = coins.ToString();
        }
    }
}