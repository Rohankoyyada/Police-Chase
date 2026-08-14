using System.Collections;
using UnityEngine;

public class PoliceLights : MonoBehaviour
{
    [SerializeField] private GameObject red_Light;
    [SerializeField] private GameObject blue_Light;
    public float spawn_Rate = 0.2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(StartLights());
    }

   IEnumerator StartLights()
    {
        yield return new WaitForSeconds(spawn_Rate);
        red_Light.SetActive(true);
        blue_Light.SetActive(false);

        yield return new WaitForSeconds(spawn_Rate);
        red_Light.SetActive(false);
        blue_Light.SetActive(true);
        StartCoroutine(StartLights());
    }
}
