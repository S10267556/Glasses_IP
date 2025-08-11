using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.AI;

public class ParentDialogue : MonoBehaviour
{

    NavMeshAgent myAgent;

    [SerializeField]
    GameObject dialogueObject;

    [SerializeField]
    Transform targetTransform;

    [SerializeField]
    string currentState = "Idle";

    [SerializeField]
    GameObject[] patrolPoints;

    int currentPatrolPoint = 0;

    [SerializeField]
    float idlePeriod = 2f;

    public TextMeshProUGUI dialogueText;

    [SerializeField]
    string[] dialogueLines;

    [SerializeField]
    string[] noDeathLines;

    [SerializeField]
    string[] oneDeathLines;

    [SerializeField]
    string[] someDeathLines;

    [SerializeField]
    string[] manyDeathLines;

    [SerializeField]
    float textSpeed = 0.1f;

    [SerializeField]
    int deaths = 0;

    private int index = 0;

    private Coroutine currentCoroutine;

    void Awake()
    {
        myAgent = GetComponent<NavMeshAgent>();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (deaths == 0)
        {
            dialogueLines = noDeathLines;
        }
        else if (deaths == 1)
        {
            dialogueLines = oneDeathLines;
        }
        else if (deaths > 3 && deaths < 5)
        {
            dialogueLines = someDeathLines;
        }
        else
        {
            dialogueLines = manyDeathLines;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(currentState);
    }

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
                if(currentCoroutine != null)
                {
                    StopCoroutine(currentCoroutine); // Stop the current coroutine if it's running
                }
                dialogueText.text = dialogueLines[index]; // If clicked before the line is fully displayed, show the full line immediately
            }
        }
    }

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

    void nextLine()
    {
        if (index < dialogueLines.Length - 1)
        {
            index++;
            
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine); // Stop the current coroutine if it's running
            }

            StartCoroutine(switchStates("dialogueLoad"));
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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //If the player enters the trigger, set targetTransform to the player's transform
            targetTransform = other.transform;
        }
    }

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
