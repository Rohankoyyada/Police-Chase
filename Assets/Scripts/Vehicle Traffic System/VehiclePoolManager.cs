using System.Collections.Generic;
using UnityEngine;

public class VehiclePoolManager : MonoBehaviour
{
    public GameObject[] vehicles;

    List<GameObject> vehiclePool = new List<GameObject>();

    [SerializeField]
    private int vehiclePoolAmout = 30;

    public static VehiclePoolManager Instance { get; private set; }

    int ReturnRandomIndex()
    {
        int randomIndex = Random.Range(0, vehicles.Length);
        return randomIndex;
    }

    void Awake()
    {
        Instance = this;
        GameObject vehiclePoolHolder = new GameObject("VehiclePoolHolder");
        for (int i = 1; i <= vehiclePoolAmout; i++)
        {
            int currentRandomIndex = ReturnRandomIndex();
            GameObject currentVehicle = Instantiate(vehicles[currentRandomIndex]);
            vehiclePool.Add(currentVehicle);
            currentVehicle.transform.SetParent(vehiclePoolHolder.transform);
            currentVehicle.SetActive(false);
        }
    }

    public GameObject GetVehicleFromPool()
    {
        GameObject returnVehicle = null;
        for (int i = 1; i < vehiclePool.Count; i++)
        {
            GameObject currentVehicle = vehiclePool[i];
            if (currentVehicle.activeInHierarchy == false)
            {
                returnVehicle = currentVehicle;
                break;
            }
        }
        if (returnVehicle == null)
        {
            int currentRandomIndex = ReturnRandomIndex();

            returnVehicle = Instantiate(vehicles[currentRandomIndex]);

            vehiclePool.Add(returnVehicle);
        }
        return returnVehicle;

    }

    public void AddVehicleToPool(GameObject vehicle)
    {
        for (int i = 1; i < vehiclePool.Count; i++)
        {
            GameObject currentvehicle = vehiclePool[i];
            if (currentvehicle.name == vehicle.name)
            {
                if (vehicle.activeInHierarchy == true)
                {
                    vehiclePool[i].SetActive(false);
                    break;
                }
            }
        }
    }

}
