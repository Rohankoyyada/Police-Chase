using UnityEngine;

public class WaypointPath : MonoBehaviour
{
    public Transform spawnPoint;
    public Transform[] waypoints;

    void Awake()
    {
        waypoints = new Transform[transform.childCount];

        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = transform.GetChild(i);
        }
    }

    public Transform[] GetWaypoints()
    {
        return waypoints;
    }
}
