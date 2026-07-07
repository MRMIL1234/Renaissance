using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Налаштування")]
    [SerializeField] private float speed = 8f;
    [SerializeField] private bool rotateTowardsTarget = true;

    private Transform target;
    private float damage;

    // Цей метод залишається public, бо вежа викликає його при спавні снаряду
    public void Seek(Transform _target, float _damage)
    {
        target = _target;
        damage = _damage;
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);

        if (rotateTowardsTarget)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void HitTarget()
    {
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}