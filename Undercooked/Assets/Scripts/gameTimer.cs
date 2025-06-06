using UnityEngine;
using TMPro;

public class gameTimer : MonoBehaviour
{

    public float timeLeft = 60f;
    public bool isRunning = true;
    private TMP_Text casovacText;
    
    public void StartTimer()
    {
        isRunning = true;
        casovacText = GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (!isRunning) return;

        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            End();
        }
        else
        {
        int minuty = Mathf.FloorToInt(timeLeft / 60);
        int sekundy = Mathf.FloorToInt(timeLeft % 60);
        casovacText.text = minuty + ":" + sekundy;
        }
        
    }

    public void Finish()
    {
        isRunning=false;
        GameManager.Instance.Win();
    }

    public void End()
    {
        isRunning = false;
        GameManager.Instance.Lose();
    }
}
