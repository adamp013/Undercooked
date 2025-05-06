using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    //[SerializeField] private TextMeshProUGUI timerText;
    private bool isTimerRunning = true;
    [SerializeField] float sliderMaxValue = 60f; 
    public float elapsedTime = 0f;

    [SerializeField]public Slider mainSlider;

    void Update()
    {
        if (isTimerRunning)
        {
            ElapsedTime();
        }
    }

    void ElapsedTime()
    {
        elapsedTime += Time.deltaTime;
        int minutesT = Mathf.FloorToInt(elapsedTime / 60F);
        int secondsT = Mathf.FloorToInt(elapsedTime % 60);
        //timerText.text = string.Format("{0:00}:{1:00}", minutesT, secondsT);
        mainSlider.value = elapsedTime;
        mainSlider.maxValue = sliderMaxValue;
    }

    public void TimeStopped()
    {
        isTimerRunning = false;
    }

    public void TimeResumed()
    {
        isTimerRunning = true; 
    }
}
