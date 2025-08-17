using UnityEngine;

/*
* Author: Wong Zhi Lin
* Date: 17 August 2025
* Description: This script handles the car stopping functionality at traffic lights. (Not Using)
*/

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
