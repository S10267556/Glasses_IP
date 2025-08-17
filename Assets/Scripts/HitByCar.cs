using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/*
* Author: Wong Zhi Lin
* Date: 17 August 2025
* Description: This script handles the functionality when the player is hit by a car.
*/

public class HitByCar : MonoBehaviour
{
    [SerializeField]
    string targetScene; // Level to load after the crash

    [SerializeField]
    AudioClip crashSound; // Sound to play on crash

    /// <summary>
    /// Called when player collider enters the trigger collider attached to the NPC.
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            StartCoroutine(doorChangeScene()); // Start the coroutine to change the scene
            ParentDialogue.deaths++; // Increment the death count when the player is hit by a car
        }
    }

    /// <summary>
    /// Coroutine to change the scene after a delay.
    /// </summary>
    IEnumerator doorChangeScene()
    {
        AudioSource.PlayClipAtPoint(crashSound, transform.position); // Play the crash sound
        yield return new WaitForSeconds(2f); // Wait for 2 seconds
        SceneManager.LoadScene(targetScene); // Load the scene with index 1 when the player enters the trigger collider
    }
}

