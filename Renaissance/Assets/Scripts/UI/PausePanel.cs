using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private GameObject _settings;

    [Header("Pause Buttons")]
    [SerializeField] private Button _resumeButton;
    //[SerializeField] private Button _restartButton;
    [SerializeField] private Button _menuButton;
    [SerializeField] private Button _settingsButton;

    [Header("Settings Buttons")]
    [SerializeField] private Button _exitSettings;

    private bool _isPaused;

    private void Start()
    {
        _panel.SetActive(false);

        _resumeButton.onClick.AddListener(Resume);
        //_restartButton.onClick.AddListener(Restart);
        _menuButton.onClick.AddListener(ToMenu);
        _settingsButton.onClick.AddListener(ShowSettings);
        _exitSettings.onClick.AddListener(HideSettings);
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (_isPaused) Resume();
        else Pause();
    }

    private void Pause()
    {
        _isPaused = true;
        _panel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        _isPaused = false;
        _panel.SetActive(false);
        Time.timeScale = 1f;
    }
    //private void Restart()
    //{
    //    Time.timeScale = 1f;
    //    SceneManager.LoadScene("Game");
    //}

    private void ToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    public void ShowSettings()
    {
        _settings.SetActive(true);
    }
    
    public void HideSettings()
    {
        _settings.SetActive(false);
    }

}
