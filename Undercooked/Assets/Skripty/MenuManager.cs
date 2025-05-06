using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public void LoadFirstScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("testScene");
    }

     public void LoadMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

   public void QuitGame()
   {
       Application.Quit();
   }
}
