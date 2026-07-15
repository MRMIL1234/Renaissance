using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour
{
    
  
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject settingsPanel;

    void Start()
    {
        creditsPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }
 
    // --- Головне меню ---
 
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
}