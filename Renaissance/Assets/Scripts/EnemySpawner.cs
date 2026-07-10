
using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private float _interval = 2f; // Time interval between spawns
    [SerializeField] private float _timeBetweenWaves = 10f; // Time between waves of enemies
    [SerializeField] private float _waveTimer;
    [SerializeField] private int _waveCounter;
    [SerializeField] private int rest = 3;
    [SerializeField] private int _totalWaves = 5; // Total number of waves to spawn
    [SerializeField] private int _maxEnemiesInWave = 3; // Maximum number of enemies to spawn in a wave
    [SerializeField] private int _currentEnemiesInWave = 0; // Current number of enemies spawned in the wave

    [Header("Enemy Prefab and Spawn Position")]
    [SerializeField] private GameObject _enemyPrefab; // Reference to the enemy prefab
    [SerializeField] private Transform _spawnPlace; // Position where enemies will be spawned

    private int _activeEnemyCount;

    public int WaveCounter
    {
        get { return _waveCounter; }
        set 
        { 
            _waveCounter = value; 
            uiManager.ShowWave(_waveCounter); // Update the UI with the new wave count
        }
    }
    void Start()
    {
        Enemy.OnEnemyDied += OnEnemyDied;
        StartCoroutine(SpawnControllerRoutine());
    }

    void OnDestroy()
    {
        Enemy.OnEnemyDied -= OnEnemyDied;
    }

    private void OnEnemyDied()
    {
        _activeEnemyCount--;
    }
    private IEnumerator WaveRoutine()
    {
        while (_currentEnemiesInWave < _maxEnemiesInWave)
        {
            Instantiate(_enemyPrefab, _spawnPlace.position, Quaternion.identity);
            _currentEnemiesInWave++;
            _activeEnemyCount++;
            yield return new WaitForSeconds(_interval);
        }
    }
    private IEnumerator SpawnControllerRoutine()
    {
        for (int i = 0; i < _totalWaves; i++)
        {
            WaveCounter++;
            _currentEnemiesInWave = 0;

            // Ховаємо таймер на час активного спавну хвилі
            uiManager.ShowWaveTimer(false);
            yield return StartCoroutine(WaveRoutine());

            while (_activeEnemyCount > 0)
            {
                yield return null;
            }
            yield return new WaitForSeconds(rest);
            // Показуємо таймер до наступної хвилі (окрім випадку, коли хвиля остання)
            bool isLastWave = (i == _totalWaves - 1);
            if (!isLastWave)
            {
                yield return StartCoroutine(WaveCountdownRoutine());
            }
        }
    }

    private IEnumerator WaveCountdownRoutine()
    {
        _waveTimer = _timeBetweenWaves;
        uiManager.ShowWaveTimer(true);

        while (_waveTimer > 0f)
        {
            uiManager.UpdateWaveTimer(_waveTimer);
            yield return null;
            _waveTimer -= Time.deltaTime;
        }

        uiManager.ShowWaveTimer(false);
    }
}
 