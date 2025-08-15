using UnityEngine;
using System.Collections.Generic;

public class CarStopZone : MonoBehaviour
{
    public LightState vehicleLight = LightState.Red;
    private Queue<NPCPathing> waitingNPCs = new Queue<NPCPathing>();

    private void OnTriggerEnter(Collider other)
    {
        NPCPathing npc = other.GetComponent<NPCPathing>();
        if (npc != null)
        {
            npc.StopForLight();
            waitingNPCs.Enqueue(npc);
        }
    }

    private void Update()
    {
        if (waitingNPCs.Count > 0)
        {
            NPCPathing npc = waitingNPCs.Dequeue();
            npc.GoAfterLight();
        }
    }
}
