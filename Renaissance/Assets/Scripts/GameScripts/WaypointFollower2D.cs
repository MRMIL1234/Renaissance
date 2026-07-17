using UnityEngine;

public class WaypointFollower2D : MonoBehaviour
{
    [Header("Налаштування руху")]
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _arrivalDistance = 0.5f;

    [Header("Animation")]
    [SerializeField] private Animator _animator;

    private Transform[] _waypoints;
    private int _currentWaypointIndex = 0;
    private bool _hasReachedEnd = false;

    void Start()
    {
        // 1. Шукаємо головний об'єкт шляху за тегом
        GameObject pathObject = GameObject.FindWithTag("GameController");

        if (pathObject != null)
        {
            Debug.Log($"[Інформація] Ворог {gameObject.name} знайшов RoadPath: {pathObject.name}");
        }
        else
        {
            Debug.LogError($"[Помилка] Ворог {gameObject.name} не зміг знайти RoadPath з тегом 'GameController'!");
            return;
        }

        PathContainer pathContainer = pathObject.GetComponent<PathContainer>();
        if (pathContainer != null)
        {
            _waypoints = pathContainer.Points;
        }
    }

    void Update()
    {
        if (_waypoints == null || _waypoints.Length == 0 || _hasReachedEnd) return;

        Transform targetWaypoint = _waypoints[_currentWaypointIndex];
        if (targetWaypoint == null) return;

        UpdateDirection((targetWaypoint.position - transform.position).normalized);

        transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, _speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetWaypoint.position) < _arrivalDistance)
        {
            if (_currentWaypointIndex == _waypoints.Length - 1)
            {
                _hasReachedEnd = true;
                _animator.SetBool("isMoving", false);
                return;
            }

            _currentWaypointIndex++;
        }
    }

    private void UpdateDirection(Vector2 direction)
    {
        float moveX = 0f;
        float moveY = 0f;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            moveX = direction.x > 0f ? 1f : -1f;
        }
        else
        {
            moveY = direction.y > 0f ? 1f : -1f;
        }

        _animator.SetFloat("MoveX", moveX);
        _animator.SetFloat("MoveY", moveY);
        _animator.SetBool("isMoving", true);
    }
}