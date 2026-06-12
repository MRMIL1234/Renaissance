using UnityEngine;

public class WaypointFollower2D : MonoBehaviour
{
    [Header("Налаштування шляху")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float arrivalDistance = 0.1f;

    private int currentWaypointIndex = 0;
    private bool hasReachedEnd = false;

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0 || hasReachedEnd) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
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

    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length < 2) return;

        Gizmos.color = Color.green;
        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i] == null) continue;

            Gizmos.DrawSphere(waypoints[i].position, 0.2f);

            if (i < waypoints.Length - 1 && waypoints[i + 1] != null)
            {
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            }
        }
    }
}