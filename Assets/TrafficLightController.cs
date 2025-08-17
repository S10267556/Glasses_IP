using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum LightState { Green, Red }

public class TrafficLightController : MonoBehaviour
{
    [Header("Light Timers")]
    [SerializeField] private float greenTime = 10f;
    [SerializeField] private float redTime = 10f;

    public LightState vehicleLight = LightState.Green;
    public LightState pedestrianLight = LightState.Red;

    private List<UnityEngine.AI.NavMeshAgent> vehicleQueue = new List<UnityEngine.AI.NavMeshAgent>();
    private List<UnityEngine.AI.NavMeshAgent> pedestrianQueue = new List<UnityEngine.AI.NavMeshAgent>();

    private void Start()
    {
        StartCoroutine(CycleLights());
    }

    private IEnumerator CycleLights()
    {
        while (true)
        {
            // Vehicles green, pedestrians red
            vehicleLight = LightState.Green;
            pedestrianLight = LightState.Red;
            ReleaseQueue(vehicleQueue);
            yield return new WaitForSeconds(greenTime);

            // Vehicles red, pedestrians green
            vehicleLight = LightState.Red;
            pedestrianLight = LightState.Green;
            ReleaseQueue(pedestrianQueue);
            yield return new WaitForSeconds(redTime);
        }
    }

    public void AddToQueue(UnityEngine.AI.NavMeshAgent agent, bool isVehicle)
    {
        if (isVehicle)
        {
            if (!vehicleQueue.Contains(agent))
                vehicleQueue.Add(agent);
        }
        else
        {
            if (!pedestrianQueue.Contains(agent))
                pedestrianQueue.Add(agent);
        }

        agent.isStopped = true;
    }

    private void ReleaseQueue(List<UnityEngine.AI.NavMeshAgent> queue)
    {
        foreach (var agent in queue)
        {
            if (agent != null)
                agent.isStopped = false;
        }
        queue.Clear();
    }
}
