using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Характеристики")]
    public float health = 100f;

    // Метод, який викликає башта, коли стріляє у ворога
    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} був знищений!");
        // Тут можна додати ефекти смерті або нарахування золота гравцю
        Destroy(gameObject);
    }
}