using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class MondarMandir : MonoBehaviour
{

    public float arriveThreshold = 0.5f;
    public Vector2 speedRange = new Vector2(1.5f, 3f);

    private NavMeshAgent agent;
    private bool hasDestination = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void Init (Transform destination)
    {
        agent.speed =Random.Range(speedRange.x, speedRange.y);
        agent.SetDestination(destination.position);
        hasDestination = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (!hasDestination || agent.pathPending) return;

        if (agent.remainingDistance <= Mathf.Max(agent.stoppingDistance, arriveThreshold))
        {
            Destroy(gameObject);
        }

    }
}
