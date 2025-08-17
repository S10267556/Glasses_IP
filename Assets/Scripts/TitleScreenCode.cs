using UnityEngine;
using UnityEngine.SceneManagement;

/*
* Author: Wong Zhi Lin
* Date: 17 August 2025
* Description: This script handles the title screen functionality.
*/

public class TitleScreenCode : MonoBehaviour
{
    public void LoadGameScene(string sceneName)
    {
        // Load the specified scene when the start button is pressed
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        // Quit the application when the quit button is pressed
        Debug.Log("Quit Game."); // Sends a message to the console
        Application.Quit();
    }
}
