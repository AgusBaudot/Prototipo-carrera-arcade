using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    
    public void Play()
    {
        SceneManager.LoadScene("Carrera");
    }
    
    public void Exit()
    {
        Application.Quit();
    }
}
