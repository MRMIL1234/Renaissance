using UnityEngine;
using UnityEngine.UI;

public class BaseManager : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;

    [SerializeField] private int _baseHealth = 100;
    [SerializeField] private int _baseCoins = 30;

    public int BaseHealth
    {
        get { return _baseHealth; }
        set
        {
            _baseHealth = value;
            uiManager.UpdateHP(_baseHealth);
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
            Debug.Log("Base Health: " + BaseHealth);
            if (BaseHealth <= 0)
            {
                Debug.Log("Game Over!");
                // Implement game over logic here
            }
        }

    }
}
