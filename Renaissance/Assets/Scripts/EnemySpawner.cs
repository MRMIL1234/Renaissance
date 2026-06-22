using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{ 
    [SerializeField] private float _interval; // Time interval between spawns

    [SerializeField] private GameObject _enemyPrefab; // Reference to the enemy prefab
    [SerializeField] private Transform _spawnPlace; // Position where enemies will be spawned

    [SerializeField] private int _maxEnemiesInWave; // Maximum number of enemies to spawn in a wave
    [SerializeField] private int _currentEnemiesInWave; // Current number of enemies spawned in the wave
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("SpawnEnemies", 0f, _interval);
    }
    public void SpawnEnemies()
    {
        Instantiate(_enemyPrefab, _spawnPlace.position, Quaternion.identity);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
