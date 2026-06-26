using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Характеристики")]
    [SerializeField] private float health = 100f;
    [SerializeField] private int coinReward = 25;

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

        // Уведомляем BaseManager что враг дошёл (если на нём был BaseManager-триггер)
        Destroy(gameObject);
    }

    public float Health => health;
    public float MaxHealth => 100f; // можно вынести в поле если нужно
}
