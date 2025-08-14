using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScenes : MonoBehaviour
{
    [SerializeField]
    string targetScene;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //load the scene with index 1 when the player enters the trigger collider
            SceneManager.LoadScene(targetScene);
        }
    }
}
