using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject[] npcs;
    public Transform[] wayPoints;
    public int npcCount = 5;

    public int startDelay = 1;
    public int spawnDelay = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(npcCount == 0&&wayPoints.Length==0)
        {
            Debug.Log("Fill the missing details");
        }
        
        StartCoroutine(StartSpawn());
    }

    IEnumerator StartSpawn()
    {
        yield return new WaitForSeconds(startDelay);
        for (int i = 0; i < npcCount; i++)
        {
            SpawnNpc();
            yield return new WaitForSeconds(spawnDelay);
        }
    }
    void SpawnNpc()
    {
        int randomIndex = Random.Range(0, wayPoints.Length);
        Transform spawnPoint=wayPoints[randomIndex];

        int randomnpc_Index=Random.Range(0,npcs.Length);
        GameObject npc=npcs[randomnpc_Index];

        GameObject spawnnpc=Instantiate(npc,spawnPoint.position,spawnPoint.rotation);
        NPC patrol=spawnnpc.GetComponent<NPC>();

        if(patrol != null )
        {
            patrol.waypoints= wayPoints;
            patrol.currentwaypoint = randomIndex;
        }
    }
}
