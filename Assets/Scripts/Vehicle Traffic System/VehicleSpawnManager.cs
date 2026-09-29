using System.Collections;
using UnityEngine;

public class VehicleSpawnManager : MonoBehaviour
{
    public GameObject[] vehicles;
    public Transform[] spawnPoint;
    public float spawnRate = 3f;

    [SerializeField] private VehiclePoolManager vehiclePool;

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
        int roadIndex = Random.Range(0, roadPaths.Length);

        WaypointPath selectedRoad = roadPaths[roadIndex];

        GameObject vehicleGO = vehiclePool.GetVehicleFromPool();

        vehicleGO.transform.position = selectedRoad.spawnPoint.position;
        vehicleGO.transform.rotation = selectedRoad.spawnPoint.rotation;

        vehicleGO.SetActive(true);

        VehiclesController controller =
            vehicleGO.GetComponent<VehiclesController>();

        controller.Initialize(selectedRoad);
    }

}
