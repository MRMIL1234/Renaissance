
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
    [SerializeField] private int _currentEnemiesInWave = 0; // Current number of enemies spawned in the wave

    [Header("Сложность")]
    [SerializeField] private int _enemiesPerWaveBonus = 1; // +X врагов на каждую волну
    [SerializeField] private float _extraHealthPerWave = 25f; // +X HP на каждую волну

    [Header("Enemy Prefab and Spawn Position")]
    [SerializeField] private GameObject _enemyPrefab; // Reference to the enemy prefab
    [SerializeField] private Transform _spawnPlace; // Position where enemies will be spawned

    private int _activeEnemyCount;
    private float _waveStartTime;

    public int WaveCounter
    {
        get { return _waveCounter; }
        set 
        { 
            _waveCounter = value; 
            uiManager.ShowWave(_waveCounter); // Update the UI with the new wave count
            if (_waveCounter > _totalWaves)
            {
                Time.timeScale = 0;
                uiManager.ShowVictoryPanel(true);
            }
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
    private IEnumerator SpawnControllerRoutine()
    {
        for (int i = 0; i < _totalWaves; i++)
        {
            WaveCounter++;
            _currentEnemiesInWave = 0;

            // === WAVE ACTIVE (спавн + бой) ===
            _waveStartTime = Time.time;
            uiManager.ShowWaveTimer(false);
            uiManager.ShowWaveActiveTimer(true);

            int waveEnemyCount = 3 + i * _enemiesPerWaveBonus;
            float enemyHealth = 100f + i * _extraHealthPerWave;
            int enemyCoinReward = 25 + i * 5;

            while (_currentEnemiesInWave < waveEnemyCount)
            {
                GameObject enemyObj = Instantiate(_enemyPrefab, _spawnPlace.position, Quaternion.identity);
                Enemy enemy = enemyObj.GetComponent<Enemy>();
                if (enemy != null)
                    enemy.SetStats(enemyHealth, enemyCoinReward);

                _currentEnemiesInWave++;
                _activeEnemyCount++;

                float spawnTimer = 0f;
                while (spawnTimer < _interval)
                {
                    uiManager.UpdateWaveActiveTimer(Time.time - _waveStartTime);
                    spawnTimer += Time.deltaTime;
                    yield return null;
                }
            }

            while (_activeEnemyCount > 0)
            {
                uiManager.UpdateWaveActiveTimer(Time.time - _waveStartTime);
                yield return null;
            }

            uiManager.ShowWaveActiveTimer(false);

            if (_waveCounter == _totalWaves)
            {
                uiManager.ShowVictoryPanel(true);
                yield break;  // выйти из корутины, не идти в rest
            }

            // === REST (3 сек — мигающие фразы) ===
            yield return StartCoroutine(RestRoutine());

            // === COUNTDOWN (10 сек — 00:10 → 00:00) ===
            bool isLastWave = (i == _totalWaves - 1);
            if (!isLastWave)
            {
                yield return StartCoroutine(WaveCountdownRoutine());
            }
        }
    }

    private IEnumerator RestRoutine()
    {
        float endTime = Time.time + rest;
        while (Time.time < endTime)
        {
            uiManager.ShowRandomRestPhrase();
            yield return new WaitForSeconds(Random.Range(0.35f, 0.65f));
            uiManager.HideRestPhrase();
            yield return new WaitForSeconds(Random.Range(0.15f, 0.35f));
        }
        uiManager.HideRestPhrase();
    }

    private IEnumerator WaveCountdownRoutine()
    {
        _waveTimer = _timeBetweenWaves;
        uiManager.ShowWaveTimer(true);

        while (_waveTimer > 0f)
        {
            uiManager.UpdateCountdownTimer(_waveTimer);
            yield return null;
            _waveTimer -= Time.deltaTime;
        }

        uiManager.ShowWaveTimer(false);
    }
}
 