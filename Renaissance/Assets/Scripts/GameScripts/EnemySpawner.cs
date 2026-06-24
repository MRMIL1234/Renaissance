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
}
