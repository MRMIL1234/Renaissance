using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    [Header("Gameplay UI")]
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] private TextMeshProUGUI _waveText;

    [Header("Game Over UI")]
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
}
