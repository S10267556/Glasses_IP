using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCTest : MonoBehaviour
{
    NavMeshAgent myAgent;

    [SerializeField]
    Transform targetTransform;

    [SerializeField]
    string currentState = "Idle";

    [SerializeField]
    GameObject[] patrolPoints;

    int currentPatrolPoint = 0;

    [SerializeField]
    float idlePeriod = 2f;

    [SerializeField]
    Vector3 moveDirection;

    private Animator animator;


    void Awake()
    {
        myAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        //Start the Idle state coroutine
        StartCoroutine(currentState);
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        moveDirection = myAgent.velocity;

        //Animations
        if (moveDirection == Vector3.zero)
        {
            //Run Idle when not moving
            animator.SetFloat("Speed", 0);
        }
        else
        {
            animator.SetFloat("Speed", 0.5f);
        }
    }

    IEnumerator Idle()
    {
        while (currentState == "Idle")
        {
            //Do Idle  behavior
            yield return null; // Wait until the next frame
            //If i see the target, chase the target
            if (targetTransform != null)
            {
                //Change state to "Chasing"
                StartCoroutine(switchStates("ChaseTarget"));
            }
            else
            {
                yield return new WaitForSeconds(idlePeriod); // Wait before patrolling
                StartCoroutine(switchStates("Patrol")); // Change state to "Patrol"
            }
        }
    }

    IEnumerator Patrol()
    {
        while (currentState == "Patrol")
        {
            //Do Patrol behavior
            yield return null; // Wait until the next frame
            if (targetTransform != null)
            {
                //Change state to "ChaseTarget"
                StartCoroutine(switchStates("ChaseTarget"));
            }
            else
            {
                myAgent.SetDestination(patrolPoints[currentPatrolPoint].transform.position); //move the chaser to the current patrol point
                if (myAgent.transform.position.z == patrolPoints[currentPatrolPoint].transform.position.z) //Once the chaser has reached the patrol point
                {
                    currentPatrolPoint++; //Set the current patrol point to the next one for the next patrol
                    if (currentPatrolPoint >= patrolPoints.Length)
                    {
                        currentPatrolPoint = 0; // Reset to the first patrol point if at the end of the array
                    }
                    StartCoroutine(switchStates("Idle")); //switch back to Idle state
                    Debug.Log("Reached patrol point: " + patrolPoints[currentPatrolPoint]);
                }
            }
        }
    }

    IEnumerator switchStates(string newState)
    {
        if (currentState == newState)
        {
            yield break; // If already in the desired state, exit the coroutine
        }
        //set the current state to the new state
        currentState = newState;

        //Start the coroutine for the new state
        StartCoroutine(currentState);
    }  
}