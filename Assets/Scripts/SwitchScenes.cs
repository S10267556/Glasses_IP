using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/*
* Author: Wong Zhi Lin
* Date: 17 August 2025
* Description: This script handles the functionality for switching scenes when the player interacts with a door.
*/

public class SwitchScenes : MonoBehaviour
{
    [SerializeField]
    string targetScene; // Level to load when the player interacts with the door

    [SerializeField]
    AudioClip doorSound; //sound to play when the player interacts with the door

    /// <summary>
    /// Called when another collider enters the trigger collider attached to this object.
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            StartCoroutine(doorChangeScene());
        }
    }

    /// <summary>
    /// Coroutine to handle the door interaction and scene transition.
    /// </summary>
    IEnumerator doorChangeScene()
    {
        AudioSource.PlayClipAtPoint(doorSound, transform.position);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(targetScene); //load the scene with index 1 when the player enters the trigger collider
    }
}
