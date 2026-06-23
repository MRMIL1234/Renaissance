using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{ 
    [SerializeField] private float _interval = 2f; // Time interval between spawns
    [SerializeField] private float _timeBetweenWaves = 5f; // Time between waves of enemies
    [SerializeField] private float _waveTimer;

    [SerializeField] private GameObject _enemyPrefab; // Reference to the enemy prefab
    [SerializeField] private Transform _spawnPlace; // Position where enemies will be spawned

    [SerializeField] private int _maxEnemiesInWave = 3; // Maximum number of enemies to spawn in a wave
    [SerializeField] private int _currentEnemiesInWave = 0; // Current number of enemies spawned in the wave
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(WaveRoutine());
    }
    private IEnumerator WaveRoutine()
    {
        while (_currentEnemiesInWave < _maxEnemiesInWave)
        {
            Instantiate(_enemyPrefab, _spawnPlace.position, Quaternion.identity);
            _currentEnemiesInWave++;
            yield return new WaitForSeconds(_interval);
        }
    }
}
