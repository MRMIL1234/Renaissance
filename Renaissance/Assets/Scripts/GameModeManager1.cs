using UnityEngine;

// Режими гри, які можна обрати на карті рівнів
public enum GameMode
{
    Endless,
    Tutorial
}

// Статичний клас не прив'язаний до GameObject, тому переживає завантаження нової сцени.
// Зберігає обраний на карті рівнів режим, щоб сцена "Game" знала, що саме запускати.
public static class GameModeManager
{
    public static GameMode SelectedMode { get; private set; } = GameMode.Endless;

    public static void SetMode(GameMode mode)
    {
        SelectedMode = mode;
        Debug.Log("Обрано режим гри: " + mode);
    }
}
