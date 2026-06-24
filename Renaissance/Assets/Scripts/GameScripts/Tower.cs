using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class Tower : MonoBehaviour
{
    public TowerData stats;

    [Header("Налаштування ШІ")]
    public LayerMask enemyLayer; // Слой, на якому знаходяться вороги

    private float nextAttackTime; // Таймер для кулдауну
    private Transform currentTarget; // Поточна ціль башти

    void Start()
    {
        if (stats != null && stats.sprite != null)
        {
            GetComponent<SpriteRenderer>().sprite = stats.sprite;
        }
    }

    void Update()
    {
        if (stats == null) return;

        // Якщо цілі немає або вона вийшла за межі радіусу атаки — шукаємо нову
        if (currentTarget == null || Vector2.Distance(transform.position, currentTarget.position) > stats.attackRadius)
        {
            FindTarget();
        }

        // Якщо ціль є і кулдаун пройшов — атакуємо
        if (currentTarget != null && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + stats.attackCooldown; // Скидаємо таймер
        }
    }

    void FindTarget()
    {
        // Шукаємо всі колайдери ворогів у радіусі атаки
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, stats.attackRadius, enemyLayer);

        float shortestDistance = Mathf.Infinity;
        Transform nearestEnemy = null;

        // Проходимося по всіх знайдених ворогах і обираємо найближчого
        foreach (Collider2D enemyCollider in enemiesInRange)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemyCollider.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemyCollider.transform;
            }
        }

        currentTarget = nearestEnemy;
    }

    void Attack()
    {
        if (currentTarget == null) return;

        // Наносимо шкоду ворогу через його скрипт
        Enemy enemyScript = currentTarget.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.TakeDamage(stats.damage);
            Debug.Log($"{stats.towerName} стріляє в {currentTarget.name} і наносить {stats.damage} шкоди!");

            // Сюди в майбутньому можна додати спавн префабу кулі (Projectile), 
            // щоб вона фізично летіла у ворога.
        }
    }

    void OnDrawGizmosSelected()
    {
        if (stats != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, stats.attackRadius);
        }
    }
}