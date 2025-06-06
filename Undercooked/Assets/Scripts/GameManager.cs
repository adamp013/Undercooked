using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public gameTimer gt;
    public GameObject UI;
    public static GameManager Instance;
    public TMP_Text Notif;
    
    void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void gameStart()
    {
        gt.StartTimer();
        UI.SetActive(false);
    }

    public void Win()
    {
        UI.SetActive(true);
        if (Notif != null)
        {

        Notif.color = Color.green;
        }
        Notif.text = "VYHRAL SI, ides znova?";
    }

    public void Lose()
    {
        UI.SetActive(true);
        Notif.color = Color.red;
        Notif.text = "prehral si, skusis to znova?";
    }
}
