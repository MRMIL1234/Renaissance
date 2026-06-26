using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BaseManager : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;

    [SerializeField] private int _baseHealth = 100;
    [SerializeField] private int _baseCoins = 30; // пока не используется, но пусть будет инкапсулирован

    public int BaseHealth
    {
        get { return _baseHealth; }
        set
        {
            if (value <= 0)
            {
                _baseHealth = 0;
                uiManager.UpdateHP(_baseHealth);
                GameOverState();
            }
            else
            {
                _baseHealth = value;
                uiManager.UpdateHP(_baseHealth);
            }
        }
    }
    void Start()
    {
        uiManager.UpdateHP(_baseHealth);
    }
    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            BaseHealth -= 10;
            Debug.Log("Hit");
            Destroy(other.gameObject);
            Debug.Log("Base Health: " + BaseHealth);
        }

    }
    public void GameOverState()
    {
        uiManager.ShowGameOver();
        Time.timeScale = 0f; // Pause the game
    }
}
