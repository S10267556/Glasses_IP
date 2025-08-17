using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class HitByCar : MonoBehaviour
{
    [SerializeField]
    string targetScene;

    [SerializeField]
    AudioClip crashSound;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            StartCoroutine(doorChangeScene());
            ParentDialogue.deaths++; // Increment the death count when the player is hit by a car
        }
    }

    IEnumerator doorChangeScene()
    {
        AudioSource.PlayClipAtPoint(crashSound, transform.position);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(targetScene); //load the scene with index 1 when the player enters the trigger collider
    }
}

