using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Настройки спавна")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float interval = 2f;
    [SerializeField] private Transform spawnPoint;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 0f, interval);
    }

    public void SpawnEnemy()
    {
        if (enemyPrefab == null) return;
        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
    }

    private void OnDestroy()
    {
        CancelInvoke();
    }
}
