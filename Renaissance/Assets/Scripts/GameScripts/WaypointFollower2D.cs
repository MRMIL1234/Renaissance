using UnityEngine;

public class WaypointFollower2D : MonoBehaviour
{
    [Header("Налаштування руху")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private float arrivalDistance = 0.1f;

    private Transform[] waypoints;
    private int currentWaypointIndex = 0;
    private bool hasReachedEnd = false;

    void Start()
    {
        // 1. Шукаємо головний об'єкт шляху за тегом
        GameObject pathObject = GameObject.FindWithTag("GameController");

        if (pathObject != null)
        {
            // 2. Рахуємо скільки всього точок всередині нього лежить
            int childCount = pathObject.transform.childCount;

            if (childCount > 0)
            {
                waypoints = new Transform[childCount];

                for (int i = 0; i < childCount; i++)
                {
                    waypoints[i] = pathObject.transform.GetChild(i);
                }
            }
        }

        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError($"[Помилка] Ворог {gameObject.name} знайшов RoadPath, але всередині нього немає точок, або тег злетів!");
        }
    }

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0 || hasReachedEnd) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        if (targetWaypoint == null) return;

        transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetWaypoint.position) < arrivalDistance)
        {
            if (currentWaypointIndex == waypoints.Length - 1)
            {
                hasReachedEnd = true;
                return;
            }

            currentWaypointIndex++;
        }
    }
}