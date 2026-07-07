using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("HP UI")]
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private TextMeshProUGUI _hpText;

    [Header("Game Over UI")]
    [SerializeField] private GameObject _gameOverPanel;

    [Header("Upgrade Panel")]
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private TextMeshProUGUI upgradeCostText;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI levelText;
    private Tower currentTower;

    void Start()
    {
        _gameOverPanel.SetActive(false);
        if (upgradePanel != null)
            upgradePanel.SetActive(false);
    }
    public void UpdateHP(int health)
    {
         _hpSlider.value = health;
         _hpText.text = "HP: " + health;
        
    }
    public void ReloadGame()
    {
        SceneManager.LoadScene("Game");
        Time.timeScale = 1f; // Resume the game
    }
    public void ShowGameOver() 
    {
        _gameOverPanel.SetActive(true);
    }
    public void ToMenu()
    {
        _gameOverPanel.SetActive(false);
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f; // Resume the game
    }

    // === UPGRADE PANEL ===
    public void OpenUpgradePanel(Tower tower)
    {
        currentTower = tower;
        if (upgradePanel != null)
            upgradePanel.SetActive(true);

        // Обновляем текст если поля назначены
        if (upgradeCostText != null)
            upgradeCostText.text = "Cost: " + tower.CurrentUpgradeCost;
        if (damageText != null)
            damageText.text = "Damage: " + tower.CurrentDamage;
        if (levelText != null)
            levelText.text = "Level: " + tower.CurrentLevel;
    }

    public void CloseUpgradePanel()
    {
        if (upgradePanel != null)
            upgradePanel.SetActive(false);
        currentTower = null;
    }

    // Вызывается кнопкой "Upgrade" в панели
    public void TryUpgradeTower()
    {
        if (currentTower == null) return;

        // Проверяем хватает ли монет (через GameEconomy)
        if (GameEconomy.Instance != null)
        {
            int cost = currentTower.CurrentUpgradeCost;
            if (GameEconomy.Instance.CanAfford(cost))
            {
                GameEconomy.Instance.SpendCoins(cost);
                currentTower.UpgradeTower();
                // Обновляем текст
                OpenUpgradePanel(currentTower);
            }
        }
    }
}
