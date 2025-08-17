using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCPathing : MonoBehaviour
{
    private NavMeshAgent myAgent;
    [SerializeField] private Transform targetTransform;
    public string currentState;
    public float arrivalThreshold = 0.5f;

    private bool stoppedForLight = false;
    private bool stoppedForCar = false;

    void Awake()
    {
        myAgent = GetComponent<NavMeshAgent>();
    }

        void Update()
    {
        // Decide if the car should move
        myAgent.isStopped = stoppedForLight || stoppedForCar;
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
            else if (!stoppedForLight) // Only move if NOT stopped
            {
                myAgent.SetDestination(targetTransform.position);

                if (!myAgent.pathPending && myAgent.remainingDistance <= arrivalThreshold)
                    {
                        WaypointInfo info = targetTransform.GetComponent<WaypointInfo>();

                        if (info is TeleportWaypointInfo teleporter)
                        {
                            // Teleport and get next target
                            targetTransform = teleporter.HandleTeleport(transform);
                        }
                        else
                        {
                            targetTransform = info != null ? info.nextTarget : null;
                        }
                    }

            }

            yield return null;
        }
    }

    public void StopForLight()
    {
        stoppedForLight = true;
        myAgent.isStopped = true;
    }

    public void GoAfterLight()
    {
        stoppedForLight = false;
    }

    // --- Called by car sensor ---
    public void StopForCar()
    {
        stoppedForCar = true;
        myAgent.isStopped = true;
    }

    public void GoAfterCar()
    {
        stoppedForCar = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetTransform = other.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetTransform = null;
        }
    }
}
