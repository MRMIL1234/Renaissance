using UnityEngine;

public class BaseManager : MonoBehaviour
{
    [SerializeField] private int _baseHealth = 100;
    [SerializeField] private int _baseCoins = 30;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            _baseHealth -= 10;
            Debug.Log("Hit");
            Debug.Log("Base Health: " + _baseHealth);
            if (_baseHealth <= 0)
            {
                Debug.Log("Game Over!");
                // Implement game over logic here
            }
        }

    }
}
