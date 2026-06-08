using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [Header("Credits")]
    public GameObject creditsPanel;

    public void OnPlayClicked()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnOptionsClicked()
    {
        Debug.Log("Options - поки що не реалізовано");
    }

    public void OnCreditsClicked()
    {
        creditsPanel.SetActive(true);
    }

    public void OnCloseCreditsClicked()
    {
        creditsPanel.SetActive(false);
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }
}