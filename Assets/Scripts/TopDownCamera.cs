using UnityEngine;

public class TopDownCameraOrbit : MonoBehaviour
{
    public Transform target; 
    public float distance = 10f;   
    public float height = 8f;       
    public float pitchAngle = 45f; 
    public float rotationSpeed = 5f;

    private float currentX = 0f;

    void LateUpdate()
    {
        if (target == null) return;

       
        if (Input.GetMouseButton(0))
        {
            currentX += Input.GetAxis("Mouse X") * rotationSpeed;
        }

      
        Quaternion rotation = Quaternion.Euler(pitchAngle, currentX, 0);

      
        Vector3 position = target.position - (rotation * Vector3.forward * distance);
   
        position.y += height;

        transform.position = position;

        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}