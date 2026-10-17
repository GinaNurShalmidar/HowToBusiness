using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.AI;
public class SpawnerNPCMondar : MonoBehaviour
{
    [System.Serializable]
    public class npcMasuk
    {
        public string nama = "NPC";
        public MondarMandir prefab;
        [Min(0f)] public float weight = 1f;

    }

    public List <npcMasuk> listNpc = new List <npcMasuk>();

    public Transform[] spawnPoints;
    public Transform[] destroyPoints;

    public float minSpawn = 3f;
    public float maxSpawn = 8f;
    public int maxAlive = 10;

    private readonly List<MondarMandir> alive = new List<MondarMandir> ();
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private MondarMandir PickRandomPrefab()
    {
        float total = 0f;
        foreach (var e in listNpc)
        {
            if (e.prefab != null) total += e.weight;
        }

        if (total <= 0f) return null;

        float roll = Random.Range(0f, total);
        float sum = 0f;

        foreach (var e in listNpc)
        {
            if (e.prefab ==null) continue;
            sum += e.weight;
            if (roll <= sum) return e.prefab;
        }
        return null;
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawn, maxSpawn));

            if (listNpc.Count == 0 || spawnPoints.Length == 0 || destroyPoints.Length == 0)
            {
                Debug.LogWarning("[SpawnerNPCMondar] listNpc, SpawnPoints, atau destroy Point masih kosong!", this);
                yield break;
            }

            alive.RemoveAll(n => n == null);
            if (alive.Count >= maxAlive) continue;

            MondarMandir prefab = PickRandomPrefab();
            if (prefab == null) continue;

            Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Transform dp = destroyPoints[Random.Range(0, destroyPoints.Length)];

            Vector3 pos = sp.position;
            if (NavMesh.SamplePosition(sp.position, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            pos = hit.position;

            MondarMandir npc = Instantiate(prefab, pos, sp.rotation);

            NavMeshAgent agent = npc.GetComponent<NavMeshAgent>();
            if (agent != null) agent.Warp(pos);

            npc.Init(dp);
            alive.Add(npc);

        }
        
    }
}
