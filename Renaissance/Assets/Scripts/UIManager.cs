
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] private TextMeshProUGUI _waveText;
    [SerializeField] private TextMeshProUGUI _waveTimerText;
    [SerializeField] private GameObject _gameOverPanel;
    void Start()
    {
        _gameOverPanel.SetActive(false);
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
    public void ShowWave(int wave)
    {
        _waveText.text = "Wave: " + wave;
    }

    public void ShowWaveTimer(bool show)
    {
        if (_waveTimerText == null) return;
        _waveTimerText.gameObject.SetActive(show);
    }

    public void UpdateWaveTimer(float timeLeft)
    {
        if (_waveTimerText == null) return;
        int secondsLeft = Mathf.CeilToInt(Mathf.Max(timeLeft, 0f));
        _waveTimerText.text = "Наступна хвиля через: " + secondsLeft + "с";
    }
}
 