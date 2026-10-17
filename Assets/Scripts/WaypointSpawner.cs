using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaypointSpawner : MonoBehaviour
{

    public WaypointWalk[] npcPrefabs;
    public bool spawnOnStart = false;
    private bool isActive;
    public Waypoint firstWaypoint;
    public Transform spawnPoint;

    public float minSpawn = 1.5f;
    public float maxSpawn = 4f;
    // Start is called before the first frame update
    void Start()
    {
        isActive = spawnOnStart;
        StartCoroutine(SpawnLoop());
    }

    public void StartSpawning()
    {
        isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SpawnLoop()
    {
        while (true) { 

            yield return new WaitUntil(() => isActive);
        
            yield return new WaitForSeconds(Random.Range(minSpawn, maxSpawn));

            if (npcPrefabs == null || npcPrefabs.Length == 0)
            {
                Debug.LogWarning("[WaypointSpawner] Slot 'Npc Prefabs' masih kosong!", this);
                yield break;
            }

            if (spawnPoint == null || firstWaypoint == null)
            {
                Debug.LogWarning("[WaypointSpawner] Slot 'Spawn Point' atau 'First Waypoint' belum di-assign!", this);
                yield break;
            }

            
            if (!firstWaypoint.IsFree)
            {
                continue;
            }

            
            int randomIndex = Random.Range(0, npcPrefabs.Length);
            WaypointWalk selectedPrefab = npcPrefabs[randomIndex];

            if (selectedPrefab == null)
            {
                Debug.LogWarning($"[WaypointSpawner] Element index ke-{randomIndex} pada Npc Prefabs bernilai NULL!", this);
                continue;
            }

           
            Vector3 spawnPos = spawnPoint.position;
            if (NavMesh.SamplePosition(spawnPoint.position, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
            {
                spawnPos = hit.position;
            }

            
            WaypointWalk npc = Instantiate(selectedPrefab, spawnPos, spawnPoint.rotation);

            NavMeshAgent agent = npc.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.Warp(spawnPos);
            }

            npc.startWayPoint = firstWaypoint;


        }
    }
}
