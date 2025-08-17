using UnityEngine;

public class CarStopTest : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            other.GetComponent<NPCPathing>().StopForLight();
        }
    }
}
