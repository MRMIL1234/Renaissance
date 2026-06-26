using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class Tower : MonoBehaviour
{
    [Header("Базові дані")]
    [SerializeField] private TowerData stats;

    [Header("Налаштування атаки")]
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    public TowerData Stats => stats;
    public int CurrentLevel { get; private set; } = 1;
    public float CurrentDamage { get; private set; }
    public float CurrentCooldown { get; private set; }
    public int CurrentUpgradeCost { get; private set; }

    private float nextAttackTime;
    private Transform currentTarget;

    private void Start()
    {
        if (stats != null)
        {
            // Звертаємося до захищених властивостей з великої літери
            CurrentDamage = stats.BaseDamage;
            CurrentCooldown = stats.BaseAttackCooldown;
            CurrentUpgradeCost = stats.BaseUpgradeCost;

            if (stats.Sprite != null)
                GetComponent<SpriteRenderer>().sprite = stats.Sprite;

            Transform rangeIndicator = transform.Find("RangeIndicator");
            if (rangeIndicator != null) rangeIndicator.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (stats == null) return;

        if (currentTarget == null || Vector2.Distance(transform.position, currentTarget.position) > stats.AttackRadius)
        {
            FindTarget();
        }

        if (currentTarget != null && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + CurrentCooldown;
        }
    }

    public void UpgradeTower()
    {
        CurrentLevel++;
        CurrentDamage *= stats.DamageMultiplier;
        CurrentCooldown *= stats.CooldownMultiplier;
        CurrentUpgradeCost = Mathf.RoundToInt(CurrentUpgradeCost * stats.CostMultiplier);
    }

    private void FindTarget()
    {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, stats.AttackRadius, enemyLayer);
        float shortestDistance = Mathf.Infinity;
        Transform nearestEnemy = null;

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

    private void Attack()
    {
        if (currentTarget == null || projectilePrefab == null) return;

        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
        GameObject projObj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        Projectile projectile = projObj.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.Seek(currentTarget, CurrentDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (stats != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, stats.AttackRadius);
        }
    }
}