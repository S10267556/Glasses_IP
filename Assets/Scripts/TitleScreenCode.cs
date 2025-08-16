using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenCode : MonoBehaviour
{
    public void LoadGameScene(string sceneName)
    {
        // Load the specified scene when the start button is pressed
        SceneManager.LoadScene(sceneName);
        ParentDialogue.deaths++; //Adds a death upon loading the game scene
    }

    public void QuitGame()
    {
        // Quit the application when the quit button is pressed
        Debug.Log("Quit Game.");
        Application.Quit();
    }  
}
