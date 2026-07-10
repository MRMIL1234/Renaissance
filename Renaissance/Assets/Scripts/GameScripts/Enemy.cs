using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Характеристики")]
    [SerializeField] private float health = 100f;
    [SerializeField] private int coinReward = 25;

    public static event System.Action OnEnemyDied;

    private bool _isDead = false;

    public void TakeDamage(float damage)
    {
        if (_isDead) return;

        health -= damage;
        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        _isDead = true;

        if (GameEconomy.Instance != null)
        {
            GameEconomy.Instance.AddCoins(coinReward);
        }

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        OnEnemyDied?.Invoke();
    }

    public float Health => health;
    public float MaxHealth => 100f; // можно вынести в поле если нужно
}
