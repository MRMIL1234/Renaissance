using UnityEngine;

public class Enemy : MonoBehaviour
{
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