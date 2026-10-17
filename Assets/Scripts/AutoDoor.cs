using UnityEngine;

public class AutoDoor : MonoBehaviour
{
    public float openAngle = 90f;     // pakai -90 kalau arahnya terbalik
    public float speed = 4f;
    public string npcTag = "NPC";

    private Quaternion closedRot;
    private Quaternion openRot;
    private int npcCount = 0;         // hitung NPC di area pintu

    private void Start()
    {
        closedRot = transform.localRotation;
        openRot = closedRot * Quaternion.Euler(0f, openAngle, 0f);
    }

    private void Update()
    {
        Quaternion target = npcCount > 0 ? openRot : closedRot;
        transform.localRotation = Quaternion.Slerp(
            transform.localRotation, target, Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(npcTag)) npcCount++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(npcTag)) npcCount = Mathf.Max(0, npcCount - 1);
    }
}