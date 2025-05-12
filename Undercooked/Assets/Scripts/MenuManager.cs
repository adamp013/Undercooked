using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public void LoadMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void LoadFirstlevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
        SaveLastPlayedLevel("Level1");
    }

    public void LoadSecondLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("level2");
        SaveLastPlayedLevel("level2");
    }

    public void LoadThirdLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("level3");
        SaveLastPlayedLevel("level3");
    }

    public void LoadFourthLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("level4");
        SaveLastPlayedLevel("level4");
    }

    public void LoadLevels()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Levels");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadLastPlayedLevel()
    {
        string lastPlayedLevel = PlayerPrefs.GetString("LastPlayedLevel", "Level1"); // Default to "DefaultLevel" if no value is saved
        UnityEngine.SceneManagement.SceneManager.LoadScene(lastPlayedLevel);
    }

    public void SaveLastPlayedLevel(string levelName)
    {
        PlayerPrefs.SetString("LastPlayedLevel", levelName);
        PlayerPrefs.Save();
    }
}
