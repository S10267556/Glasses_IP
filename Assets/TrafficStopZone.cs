using UnityEngine;
using System.Collections.Generic;

public class TrafficStopZone : MonoBehaviour
{
    [SerializeField] private TrafficLightController lightController;
    private Queue<NPCPathing> waitingNPCs = new Queue<NPCPathing>();

    private void OnTriggerEnter(Collider other)
    {
        NPCPathing npc = other.GetComponent<NPCPathing>();
        if (npc != null && lightController.vehicleLight == LightState.Red)
        {
            npc.StopForLight();
            waitingNPCs.Enqueue(npc);
        }
    }

    private void Update()
    {
        // When light turns green, release queued NPCs one by one
        if (lightController.vehicleLight == LightState.Green && waitingNPCs.Count > 0)
        {
            NPCPathing npc = waitingNPCs.Dequeue();
            npc.GoAfterLight();
        }
    }
}
