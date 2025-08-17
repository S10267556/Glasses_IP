using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCTest : MonoBehaviour
{
    NavMeshAgent myAgent;
    Animator animator;

    [SerializeField] Transform targetTransform; // Player target
    [SerializeField] string currentState = "Roam";

    [Header("Roaming Settings")]
    [SerializeField] float roamRadius = 10f;     // How far NPC can roam
    [SerializeField] float roamWaitTime = 3f;    // How long NPC waits before picking a new point
    [SerializeField] float idleChance = 0.4f;    // Chance to idle instead of moving (0–1)

    void Awake()
    {
        myAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); // Get Animator on NPC
    }

    void Start()
    {
        StartCoroutine(currentState);
    }

    void Update()
    {
        // Update Animator parameter every frame
        if (animator != null)
        {
            float speed = myAgent.velocity.magnitude; // how fast the NPC is moving
            animator.SetFloat("Speed", speed);
        }
    }

    IEnumerator Roam()
    {
        while (currentState == "Roam")
        {
            if (!myAgent.pathPending && myAgent.remainingDistance <= myAgent.stoppingDistance)
            {
                // Decide whether to idle or move again
                if (Random.value < idleChance)
                {
                    Debug.Log("NPC is idling...");
                    yield return new WaitForSeconds(roamWaitTime); // stand still
                }
                else
                {
                    // Pick a random point to roam to
                    Vector3 newDestination = RandomNavSphere(transform.position, roamRadius, -1);
                    myAgent.SetDestination(newDestination);
                }
            }

            // If player detected → stop roaming and look
            if (targetTransform != null)
            {
                StartCoroutine(SwitchState("LookAtPlayer"));
            }

            yield return null;
        }
    }

    IEnumerator LookAtPlayer()
    {
        // Stop moving
        myAgent.ResetPath();

        while (currentState == "LookAtPlayer")
        {
            if (targetTransform == null)
            {
                // Player left trigger → back to roaming
                StartCoroutine(SwitchState("Roam"));
            }
            else
            {
                // Rotate smoothly toward the player
                Vector3 direction = (targetTransform.position - transform.position).normalized;
                direction.y = 0;

                if (direction != Vector3.zero)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 3f);
                }
            }
            yield return null;
        }
    }

    IEnumerator SwitchState(string newState)
    {
        if (currentState == newState) yield break;

        currentState = newState;
        StartCoroutine(currentState);
        yield return null;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetTransform = other.transform;
            Debug.Log("Player entered trigger, NPC stops and looks!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetTransform = null;
            Debug.Log("Player left trigger, NPC resumes roaming.");
        }
    }

    // Helper: Get a random valid NavMesh position
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * dist;
        randomDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
