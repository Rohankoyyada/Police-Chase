using UnityEngine;

public class VehiclesController : MonoBehaviour
{
    public WaypointPath waypointPath;
    private Transform[] wayPoints;

    public float moveSpeed = 5f;
    public float rotateSpeed = 5f;

    private int currentIndex = 0;
    private float reachDistance = 0.2f;

    // 🔑 EXPLICIT INITIALIZATION
    public void Initialize(WaypointPath path)
    {
        waypointPath = path;

        if (waypointPath == null)
        {
            Debug.LogError("WaypointPath is NULL during Initialize", this);
            gameObject.SetActive(false);
            return;
        }

        wayPoints = waypointPath.GetWaypoints();

        if (wayPoints == null || wayPoints.Length == 0)
        {
            Debug.LogError("No waypoints found on path", this);
            gameObject.SetActive(false);
            return;
        }

        currentIndex = 0;
        transform.position = wayPoints[0].position;
    }

    void Update()
    {
        if (wayPoints == null) return;
        MoveandRotate();
    }

    void MoveandRotate()
    {
        Transform target = wayPoints[currentIndex];

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            moveSpeed * Time.deltaTime
        );

        Vector3 direction = (target.position - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                lookRotation,
                rotateSpeed * Time.deltaTime
            );
        }

        if (Vector3.Distance(transform.position, target.position) < reachDistance)
        {
            currentIndex++;

            if (currentIndex >= wayPoints.Length)
            {
                Destroy(gameObject);
            }
        }
    }
}
