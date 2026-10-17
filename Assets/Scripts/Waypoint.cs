using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Waypoint : MonoBehaviour
{
    public Waypoint next;

    public float minWaitTime = 0f;
    public float maxWaitTime = 0f;

    public bool isExit = false;


    [SerializeField] private WaypointWalk occupant;

    public List<Waypoint> nextWaypoints = new List<Waypoint>();


    public bool IsFree => occupant == null;


    public bool TryReserve(WaypointWalk walk)
    {
        if (isExit)
        {
            return true;
        }

        if (occupant == null || occupant == walk)
        {
            occupant = walk;
            return true;
        }
        return false;
    }

    public void Release(WaypointWalk walk)
    {
        if (occupant == walk) occupant = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isExit ? Color.red : (maxWaitTime > 0 ? Color.yellow : Color.green);
        Gizmos.DrawSphere(transform.position + Vector3.up * 0.1f, 0.25f);

        if (next != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, next.transform.position);
        }

        if (nextWaypoints != null)
        {
            foreach (var wp in nextWaypoints)
            {
                if (wp != null)
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawLine(transform.position, wp.transform.position);
                }
            }
        }
    }

    public Waypoint GetNextWaypoint()
    {
       
        if (nextWaypoints != null && nextWaypoints.Count > 0)
        {
            List<Waypoint> free = new List<Waypoint>();
            List<Waypoint> active = new List<Waypoint>();

            foreach (var wp in nextWaypoints)
            {
                if (wp == null || !wp.gameObject.activeInHierarchy) continue;

                active.Add(wp);
                if (wp.IsFree) free.Add(wp);
            }

            if (free.Count > 0)
                return free[Random.Range(0, free.Count)];

            if (active.Count > 0)
                return active[Random.Range(0, active.Count)];
        }

        
        Waypoint w = next;
        int guard = 0;
        while (w != null && !w.gameObject.activeInHierarchy && guard < 20)
        {
            w = w.next;
            guard++;
        }

        return w;   
    }
}







