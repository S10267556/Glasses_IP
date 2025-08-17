using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SimpleNPCPatrol : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float waitTime = 2f;

    private NavMeshAgent agent;
    private int currentPatrolIndex = 0;
    private bool isWaiting = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("No NavMeshAgent found on " + gameObject.name);
            return;
        }

        if (patrolPoints.Length > 0)
        {
            GoToNextPatrolPoint();
        }
    }

    void Update()
    {
        if (agent == null || patrolPoints.Length == 0 || isWaiting)
            return;

        // Check if we've reached the destination
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            StartCoroutine(WaitAndMoveToNext());
        }
    }

    IEnumerator WaitAndMoveToNext()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        GoToNextPatrolPoint();
        isWaiting = false;
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0)
            return;

        agent.destination = patrolPoints[currentPatrolIndex].position;
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }
}