using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class WaypointWalk : MonoBehaviour
{
    public Waypoint startWayPoint;

    public float reactionTimeMin = 0.3f;
    public float reactionTimeMax = 1.2f;

    public float arriveThreShold = 0.2f;

    private NavMeshAgent agent;
    private Waypoint current;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();   
    }

    private void Start()
    {
        StartCoroutine(RouteRoutine());
    }

    private IEnumerator RouteRoutine()
    {
        Waypoint target = startWayPoint;

        while (target != null)
        {
            bool wasBlocked = false;
            while (!target.TryReserve(this))
            {
                wasBlocked = true;
                yield return new WaitForSeconds(0.1f);
            }

            if (wasBlocked)
                yield return new WaitForSeconds(Random.Range(reactionTimeMin, reactionTimeMax));

            agent.isStopped = false;
            agent.SetDestination(target.transform.position);

            Vector3 direction = (target.transform.position - transform.position).normalized;
            direction.y = 0;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }

            yield return null;
            yield return new WaitUntil(HasArrived);

            if (current != null)
                current.Release(this);

            current = target;
            agent.isStopped = true;

            

            if (current.isExit)
            {
                current.Release(this);
                Destroy(gameObject);
                yield break;
            }

            target = current.GetNextWaypoint();

            if (target == null)
            {
                Debug.LogWarning($"[WaypointWalk] Waypoint setelah '{current.gameObject.name}' bernilai NULL! NPC berhenti.", current);
                current.Release(this);
                yield break;
            }

            float wait = Random.Range(current.minWaitTime, current.maxWaitTime);

            if (wait > 0f)
            {
                yield return new WaitForSeconds(wait);

                HairChanger hair = GetComponent<HairChanger>();
                if (hair != null) hair.ChangeRandomHair();

                MoneyManager.Instance?.AddMoney(10);
            }

            

            

            




        }
    }

    private bool HasArrived()
    {
        if (agent == null || !agent.isOnNavMesh || agent.pathPending) return false;
        return agent.remainingDistance <= Mathf.Max(agent.stoppingDistance, arriveThreShold);
    }
       
}
