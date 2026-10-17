using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// OPSIONAL: klik kiri untuk berjalan ke titik (NavMesh).
/// Pakai ini ATAU PlayerMovement, jangan keduanya di objek yang sama.
/// Pasang di Player bersama NavMeshAgent. Bake NavMesh di scene.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class ClickToMove : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask = ~0;
    [SerializeField] private float walkSpeed = 3.5f;
    [SerializeField] private float runSpeed = 6.5f;

    private NavMeshAgent agent;
    private Camera cam;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        cam = Camera.main;
    }

    private void Update()
    {
        agent.speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 200f, groundMask)
                && NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, 2f, NavMesh.AllAreas))
            {
                agent.SetDestination(navHit.position);
            }
        }
    }
}