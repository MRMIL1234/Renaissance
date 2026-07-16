using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour
{


    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject modePanel;

    void Start()
    {
        creditsPanel.SetActive(false);
        settingsPanel.SetActive(false);
        modePanel.SetActive(false);
    }

    // --- Головне меню ---

    public void OnPlayClicked()
    {
        modePanel.SetActive(true);
    }

    public void OnCreditsClicked()
    {
        creditsPanel.SetActive(true);
    }

    public void OnSettingsClicked()
    {
        settingsPanel.SetActive(true);
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }

    // --- Закрити панелі ---

    public void OnCloseCreditsClicked()
    {
        creditsPanel.SetActive(false);
        Debug.Log("Закриваємо панель з інформацією про розробників");
    }

    public void OnCloseSettingsClicked()
    {
        settingsPanel.SetActive(false);
    }

    public void OnCloseModeClicked()
    {
        settingsPanel.SetActive(false);
    }

    //--- Вибір режиму гри ---
    public void OnEducationModeClicked()
    {
        SceneManager.LoadScene("Game");
    }
    public void OnCampaignModeClicked()
    {
        SceneManager.LoadScene("LevelMap");
    }
}