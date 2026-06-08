using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour
{
    public void OnBackClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}