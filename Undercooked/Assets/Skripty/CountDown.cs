using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] float remainingTime = 0f;

    void Update()
    {
        remainingTime -= Time.deltaTime;
        int minutesC = Mathf.FloorToInt(remainingTime / 60F);
        int secondsC = Mathf.FloorToInt(remainingTime % 60);
        countdownText.text = string.Format("{0:00}:{1:00}", minutesC, secondsC);

        if (remainingTime <= 0f)
        {
            countdownText.text = "00:00";
            countdownText.color = Color.red;
            remainingTime = 0f;
        }
    }
}
