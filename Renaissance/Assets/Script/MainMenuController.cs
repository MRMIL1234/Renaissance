using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject creditsPanel;
    public GameObject settingsPanel;

    void Start()
    {
        settingsPanel.SetActive(false);
    }

    public void OnPlayClicked()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnCreditsClicked()
    {
        creditsPanel.SetActive(true);
    }

    public void OnSettingsClicked()
    {
        settingsPanel.SetActive(true);
    }

    public void OnCloseCreditsClicked()
    {
        creditsPanel.SetActive(false);
    }

    public void OnCloseSettingsClicked()
    {
        settingsPanel.SetActive(false);
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }
}