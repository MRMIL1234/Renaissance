using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private UIManager uiManager;

    [Header("Enemy Spawning Settings")]
    [SerializeField] private float _interval = 2f; // Time interval between spawns
    [SerializeField] private float _timeBetweenWaves = 10f; // Time between waves of enemies
    [SerializeField] private float _waveTimer;
    [SerializeField] private int _waveCounter;
    [SerializeField] private int _totalWaves = 5; // Total number of waves to spawn
    [SerializeField] private int _maxEnemiesInWave = 3; // Maximum number of enemies to spawn in a wave
    [SerializeField] private int _currentEnemiesInWave = 0; // Current number of enemies spawned in the wave

    [Header("Enemy Prefab and Spawn Position")]
    [SerializeField] private GameObject _enemyPrefab; // Reference to the enemy prefab
    [SerializeField] private Transform _spawnPlace; // Position where enemies will be spawned

    public int WaveCounter
    {
        get { return _waveCounter; }
        set 
        { 
            _waveCounter = value; 
            uiManager.ShowWave(_waveCounter); // Update the UI with the new wave count
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnControllerRoutine());
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
    private IEnumerator SpawnControllerRoutine()
    {
        for (int i = 0; i < _totalWaves; i++)
        {
            WaveCounter++;
            _currentEnemiesInWave = 0;
            yield return StartCoroutine(WaveRoutine());
            yield return new WaitForSeconds(_timeBetweenWaves);
        }
    }
}
