using UnityEngine;

public class tutorialButton : MonoBehaviour
{
    public GameObject tutorialWindow;
    private bool open;

    public void ButtonClicked()
    {
        tutorialWindow.SetActive(!open);
    }
}
