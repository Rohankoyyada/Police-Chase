using System.Collections;
using UnityEngine;

public class VehicleSpawnManager : MonoBehaviour
{
    public GameObject[] vehicles;
    public Transform[] spawnPoint;
    public float spawnRate = 3f;

    private WaypointPath[] roadPaths;

    void Start()
    {
        //roadPaths = FindObjectsOfType<WaypointPath>();
        roadPaths = Object.FindObjectsByType<WaypointPath>(FindObjectsSortMode.None);


        if (roadPaths.Length == 0)
        {
            Debug.LogError("No WaypointPath found in scene!");
            return;
        }

        StartCoroutine(SpawnVehicle());
    }

    IEnumerator SpawnVehicle()
    {
        while (true)
        {
            SpawnSingleVehicle();
            yield return new WaitForSeconds(spawnRate);
        }
    }

    void SpawnSingleVehicle()
    {
        int vehicleIndex = Random.Range(0, vehicles.Length);
        int roadIndex = Random.Range(0, roadPaths.Length);

        WaypointPath selectedRoad = roadPaths[roadIndex];

        GameObject vehicleGO = Instantiate(
            vehicles[vehicleIndex],
            selectedRoad.spawnPoint.position,
            selectedRoad.spawnPoint.rotation
        );

        VehiclesController controller =
            vehicleGO.GetComponent<VehiclesController>();

        controller.Initialize(selectedRoad);
    }

}
