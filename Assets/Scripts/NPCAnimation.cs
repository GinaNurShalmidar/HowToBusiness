using UnityEngine;
using UnityEngine.AI;

public class NPCAnimation : MonoBehaviour
{
    public Animator animator;
    public float smoothTime = 0.1f;
    public float referenceSpeed = 1.5f;

    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (animator == null) animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (animator == null || agent == null) return;

        float speed = agent.velocity.magnitude;
        animator.SetFloat("Speed", speed, smoothTime, Time.deltaTime);
        animator.speed = speed > 0.1f ? Mathf.Clamp(speed / referenceSpeed, 0.5f, 1.5f) : 1f;
    }
}