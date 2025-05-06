using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    float elapsedTime = 0f;

    void Update()
    {
        ElapsedTime();
    }

    void ElapsedTime()
    {
        elapsedTime += Time.deltaTime;
        int minutesT = Mathf.FloorToInt(elapsedTime / 60F);
        int secondsT = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutesT, secondsT);
    }
}
