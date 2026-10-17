using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Tuning")]
    public float moveSpeed = 6f;
    public float rotationSpeed = 15f; // Semakin tinggi semakin cepat berputar
    public float acceleration = 12f;  // Kecepatan tambah laju
    public float deceleration = 14f;  // Kecepatan rem/berhenti

    private CharacterController controller;
    private Vector3 currentVelocity;
    private Vector3 targetVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        // 1. Ambil input WASD / Panah
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // .normalized penting biar gerak miring (W+D) kecepatannya tidak melipat ganda
        Vector3 inputDirection = new Vector3(horizontal, 0f, vertical).normalized;

        // 2. Tentukan target kecepatan berdasarkan input
        targetVelocity = inputDirection * moveSpeed;

        // 3. Olah akselerasi & pengereman (Inersia halus)
        float speedChangeRate = (inputDirection.magnitude > 0.1f) ? acceleration : deceleration;
        currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, speedChangeRate * Time.deltaTime);

        // Eksekusi pergerakan
        controller.Move(currentVelocity * Time.deltaTime);

        // 4. Rotasi halus menghadap arah jalan
        if (inputDirection.magnitude >= 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}