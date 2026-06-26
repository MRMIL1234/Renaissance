using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab; // Reference to the enemy prefab
    [SerializeField] private float interval; // Time interval between spawns
    [SerializeField] private Transform spawnPlace; // Position where enemies will be spawned
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("SpawnEnemies", 0f, interval);
    }
    public void SpawnEnemies()
    {
        Instantiate(enemyPrefab, spawnPlace.position, Quaternion.identity);
    }
    // Update is called once per frame
    void Update()
    {

    }
    [Header("Характеристики")]
    [SerializeField] private float health = 100f;
    [SerializeField] private int coinReward = 25;

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (GameEconomy.Instance != null)
        {
            GameEconomy.Instance.AddCoins(coinReward);
        }
        Destroy(gameObject);
    }
}
