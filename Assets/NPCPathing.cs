// File: Assets/Scripts/NPCPathing.cs
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCPathing : MonoBehaviour
{
    private NavMeshAgent myAgent;
    [SerializeField] private Transform targetTransform;
    public string currentState;
    public float arrivalThreshold = 0.5f; // Distance to consider target reached

    void Awake()
    {
        myAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        StartCoroutine(SwitchState("Idle"));
    }

    IEnumerator SwitchState(string newState)
    {
        if (currentState == newState)
            yield break;

        currentState = newState;
        StartCoroutine(currentState);
    }

    IEnumerator Idle()
    {
        while (currentState == "Idle")
        {
            if (targetTransform != null)
                StartCoroutine(SwitchState("ChaseTarget"));

            yield return null;
        }
    }

    IEnumerator ChaseTarget()
    {
        while (currentState == "ChaseTarget")
        {
            if (targetTransform == null)
            {
                StartCoroutine(SwitchState("Idle"));
            }
            else
            {
                myAgent.SetDestination(targetTransform.position);

                // Check if reached target
                if (!myAgent.pathPending && myAgent.remainingDistance <= arrivalThreshold)
                {
                    WaypointInfo info = targetTransform.GetComponent<WaypointInfo>();
                    targetTransform = info != null ? info.nextTarget : null;
                }
            }

            yield return null;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            targetTransform = other.transform;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            targetTransform = null;
    }
}
