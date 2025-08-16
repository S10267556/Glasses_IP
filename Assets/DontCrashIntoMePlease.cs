using UnityEngine;

public class CarSensor : MonoBehaviour
{
    public Vector3 boxHalfExtents = new Vector3(1.5f, 1f, 5f); // width, height, depth
    public Vector3 boxOffset = new Vector3(0f, 0.5f, 5f);      // move it forward a bit

    private NPCPathing npc;

    void Awake()
    {
        npc = GetComponent<NPCPathing>();
    }

    void Update()
    {
        // Position and rotation for the detection box
        Vector3 boxCenter = transform.position + transform.TransformDirection(boxOffset);
        Quaternion boxRot = transform.rotation;

        Collider[] hits = Physics.OverlapBox(boxCenter, boxHalfExtents, boxRot);

        bool carAhead = false;
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Car") && hit.gameObject != gameObject)
            {
                carAhead = true;
                break;
            }
        }

        if (carAhead)
            npc.StopForCar();
        else
            npc.GoAfterCar();
    }

    // Debug draw box in Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 boxCenter = transform.position + transform.TransformDirection(boxOffset);
        Gizmos.matrix = Matrix4x4.TRS(boxCenter, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxHalfExtents * 2);
    }
}
