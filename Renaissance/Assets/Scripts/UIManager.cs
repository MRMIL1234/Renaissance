
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] private TextMeshProUGUI _waveText;
    [SerializeField] private TextMeshProUGUI _waveTimerText;
    [SerializeField] private GameObject _gameOverPanel;

    [Header("Wave Timer")]
    [SerializeField] private TextMeshProUGUI _waveActiveTimerText;
    [SerializeField] private TextMeshProUGUI _restPhraseText;

    private int _lastBaseHealth = 100;

    private readonly string[] _highHpPhrases = { "GREAT!", "WELL", "ULTRA-wave", "AMAZING!", "PERFECT!" };
    private readonly string[] _midHpPhrases = { "NICE", "GOOD", "KEEP IT UP" };
    private readonly string[] _lowHpPhrases = { "Oh no", "Really?", "What?", "HOLD THE LINE!" };

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
         _lastBaseHealth = health;
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
    public void ShowWave(int wave)
    {
        _waveText.text = "Wave: " + wave;
    }

    public void ShowWaveTimer(bool show)
    {
        if (_waveTimerText == null) return;
        _waveTimerText.gameObject.SetActive(show);
    }

    public void UpdateCountdownTimer(float timeLeft)
    {
        if (_waveTimerText == null) return;
        _waveTimerText.text = FormatTime(timeLeft);
    }

    public void ShowWaveActiveTimer(bool show)
    {
        if (_waveActiveTimerText != null)
            _waveActiveTimerText.gameObject.SetActive(show);
    }

    public void UpdateWaveActiveTimer(float elapsed)
    {
        if (_waveActiveTimerText == null) return;
        _waveActiveTimerText.text = FormatTime(elapsed);
    }

    public void ShowRandomRestPhrase()
    {
        if (_restPhraseText == null) return;

        string[] pool;
        if (_lastBaseHealth > 90)
            pool = _highHpPhrases;
        else if (_lastBaseHealth > 50)
            pool = _midHpPhrases;
        else
            pool = _lowHpPhrases;

        _restPhraseText.text = pool[Random.Range(0, pool.Length)];
        _restPhraseText.gameObject.SetActive(true);
    }

    public void HideRestPhrase()
    {
        if (_restPhraseText == null) return;
        _restPhraseText.gameObject.SetActive(false);
    }

    private string FormatTime(float seconds)
    {
        int sec = Mathf.CeilToInt(Mathf.Max(seconds, 0f));
        return $"{sec / 60:D2}:{sec % 60:D2}";
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
 