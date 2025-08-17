using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.AI;

/*
* Author: Wong Zhi Lin
* Date: 17 August 2025
* Description: This script handles the parent NPC's dialogue and interactions.
*/

public class ParentDialogue : MonoBehaviour
{

    NavMeshAgent myAgent; //gets the NavMeshAgent component for pathfinding

    [SerializeField]
    GameObject dialogueObject; // The UI element that displays the dialogue text

    [SerializeField]
    Transform targetTransform; // The target transform for the NPC to look at

    [SerializeField]
    float targetRotation; // The target rotation for the NPC

    [SerializeField]
    string currentState = "Idle"; // The current state of the NPC

    [SerializeField]
    GameObject[] patrolPoints; // The points the NPC will patrol between

    int currentPatrolPoint = 0; // The index of the current patrol point

    [SerializeField]
    float idlePeriod = 2f; // The period the NPC will stay idle

    public TextMeshProUGUI dialogueText; // The UI element that displays the dialogue text

    [SerializeField]
    string[] dialogueLines; // The lines of dialogue for the NPC

    [SerializeField]
    string[] noDeathLines; // The lines of dialogue for the NPC when no deaths have occurred

    [SerializeField]
    string[] oneDeathLines; // The lines of dialogue for the NPC when one death has occurred

    [SerializeField]
    string[] someDeathLines; // The lines of dialogue for the NPC when some deaths have occurred

    [SerializeField]
    string[] manyDeathLines; // The lines of dialogue for the NPC when many deaths have occurred

    [SerializeField]
    float textSpeed = 0.1f; // The speed at which the text is displayed

    public static int deaths = 0; // The number of deaths that have occurred

    private int index = 0; // The index of the current dialogue line

    private Coroutine currentCoroutine; // The current coroutine for displaying dialogue

    [SerializeField]
    Vector3 moveDirection; // The direction the NPC is moving

    private Animator animator; // The animator component for the NPC

    /// <summary>
    /// Agent is assigned when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        myAgent = GetComponent<NavMeshAgent>();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// Called when a new scene is loaded. Assigns dialouge lines according to deaths.
    /// </summary>
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(deaths);

        if (deaths < 1)
        {
            dialogueLines = noDeathLines;
        }
        else if (deaths <= 5)
        {
            dialogueLines = oneDeathLines;
        }
        else if (deaths <= 10)
        {
            dialogueLines = someDeathLines;
        }
        else
        {
            dialogueLines = manyDeathLines;
        }
    }

    /// <summary>
    /// Starts coroutine and gets the animator component.
    /// </summary>
    void Start()
    {
        StartCoroutine(currentState);
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Updates the NPC's state and handles dialogue input.
    /// </summary>
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && currentState == "dialogueLoad")
        {
            if (dialogueText.text == dialogueLines[index])
            {
                nextLine(); // If the dialogue line is fully displayed, go to the next line
            }
            else
            {
                if (currentCoroutine != null)
                {
                    StopCoroutine(currentCoroutine); // Stop the current coroutine if it's running
                }
                dialogueText.text = dialogueLines[index]; // If clicked before the line is fully displayed, show the full line immediately
            }
        }

        if (currentState == "dialogueLoad" && targetTransform != null)
        {
            Vector3 lookDirection = targetTransform.position - myAgent.transform.position;
            lookDirection.y = 0; // Only rotate on the Y axis
            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(lookDirection); // Get the target rotation
                myAgent.transform.rotation = Quaternion.Slerp(myAgent.transform.rotation, targetRot, Time.deltaTime * 5f); // Smoothly rotate
            }
        }

        moveDirection = myAgent.velocity; // Get the current movement direction/speed

        //Animations
        if (moveDirection == Vector3.zero)
        {
            //Run Idle when not moving
            animator.SetFloat("Speed", 0); // Set speed to 0 when idle
        }
        else
        {
            animator.SetFloat("Speed", 0.5f); // Set speed to 0.5 when moving
        }
    }

    /// <summary>
    /// Loads the dialogue for the NPC.
    /// </summary>

    IEnumerator dialogueLoad()
    {
        if (targetTransform != null)
        {
            myAgent.SetDestination(targetTransform.position + Vector3.forward * 2f);
            dialogueObject.SetActive(true); // Enable the dialogue object
            dialogueText.text = string.Empty; // Clear the text before showing the new line
            foreach (char letter in dialogueLines[index].ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(textSpeed);
            }
        }
        else
        {
            StartCoroutine(switchStates("Idle")); // If no target, switch back to Idle state
        }
    }

    /// <summary>
    /// Loads the next line of dialogue for the NPC.
    /// </summary>
    void nextLine()
    {
        if (index < dialogueLines.Length - 1) // Check if there is a next line
        {
            index++; // Move to the next line

            if (currentCoroutine != null) // If a coroutine is already running
            {
                StopCoroutine(currentCoroutine); // Stop the current coroutine if it's running
            }

            StartCoroutine(switchStates("dialogueLoad")); // Start the dialogueLoad coroutine
        }
        else
        {
            dialogueObject.SetActive(false); // Disable the dialogue object when all lines are done
            dialogueText.text = string.Empty; // Clear the text when all lines are done
            index = 0; // Reset index for next dialogue session
            targetTransform = null; // Clear the targetTransform after dialogue ends
            StartCoroutine(switchStates("Idle")); // Switch back to Idle state after finishing dialogue
        }
    }

    /// <summary>
    /// Loads the idle state for the NPC.
    /// </summary>
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
                StartCoroutine(switchStates("dialogueLoad"));
            }
            else
            {
                yield return new WaitForSeconds(idlePeriod); // Wait before patrolling
                StartCoroutine(switchStates("Patrol")); // Change state to "Patrol"
            }
        }
    }

    /// <summary>
    /// Loads the patrol state for the NPC.
    /// </summary>
    IEnumerator Patrol()
    {
        while (currentState == "Patrol")
        {
            //Do Patrol behavior
            yield return null; // Wait until the next frame
            if (targetTransform != null)
            {
                //Change state to "ChaseTarget"
                StartCoroutine(switchStates("dialogueLoad"));
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
                    Debug.Log("Reached patrol point: " + patrolPoints[currentPatrolPoint]); // Log the patrol point reached
                }
            }
        }
    }

    /// <summary>
    /// Switches the NPC's state to a new state with given string.
    /// </summary>
    IEnumerator switchStates(string newState)
    {
        if (currentState == newState)
        {
            yield break; // If already in the desired state, exit the coroutine
        }

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine); // Stop the current coroutine if it exists
        }

        //set the current state to the new state
        currentState = newState;

        //Start the coroutine for the new state
        if (newState == "dialogueLoad")
        {
            currentCoroutine = StartCoroutine(dialogueLoad());
        }
        else if (newState == "Idle")
        {
            currentCoroutine = StartCoroutine(Idle());
        }
        else if (newState == "Patrol")
        {
            currentCoroutine = StartCoroutine(Patrol());
        }
    }

    /// <summary>
    /// Called when another collider enters the trigger collider attached to the NPC.
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //If the player enters the trigger, set targetTransform to the player's transform
            targetTransform = other.transform;
            targetRotation = other.transform.eulerAngles.y; // Store the player's rotation
        }
    }

    /// <summary>
    /// Called when player collider exits the trigger collider attached to the NPC.
    /// </summary>
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //If the player exits the trigger, set targetTransform to null
            targetTransform = null;
        }
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
