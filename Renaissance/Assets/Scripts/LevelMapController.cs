using UnityEngine;
using UnityEngine.SceneManagement;

// Вішається на об'єкт сцени "LevelMap".
// Кнопки в Inspector прив'язуються до відповідних публічних методів:
// "Нескінченний режим" -> OnEndlessModeClicked
// "Навчальний режим"   -> OnTutorialModeClicked
// "Вихід"               -> OnExitClicked
public class LevelMapController : MonoBehaviour
{
    [Header("Назва ігрової сцени")]
    [SerializeField] private string gameSceneName = "Game";

    // --- Вибір режиму ---

    public void OnEndlessModeClicked()
    {
        GameModeManager.SetMode(GameMode.Endless);
        LoadGameScene();
    }

    public void OnTutorialModeClicked()
    {
        GameModeManager.SetMode(GameMode.Tutorial);
        LoadGameScene();
    }

    private void LoadGameScene()
    {
        Time.timeScale = 1f; // про всяк випадок, якщо гра була на паузі
        SceneManager.LoadScene(gameSceneName);
    }

    // --- Вихід ---

    public void OnExitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
