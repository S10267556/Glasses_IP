using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SwitchScenes : MonoBehaviour
{
    [SerializeField]
    string targetScene;

    [SerializeField]
    AudioClip doorSound;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            StartCoroutine(doorChangeScene());
        }
    }

    IEnumerator doorChangeScene()
    {
        AudioSource.PlayClipAtPoint(doorSound, transform.position);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(targetScene); //load the scene with index 1 when the player enters the trigger collider
    }
}
