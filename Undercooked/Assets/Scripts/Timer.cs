using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    //[SerializeField] private TextMeshProUGUI timerText;
    private bool isTimerRunning = true;
    public float sliderMaxValue = 30f; 
    public float elapsedTime = 0f;

    [SerializeField] public Image Image;

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
        if (elapsedTime > sliderMaxValue)
        {
            mainSlider.value = sliderMaxValue;
        }
        else
        {
            int minutesT = Mathf.FloorToInt(elapsedTime / 60F);
            int secondsT = Mathf.FloorToInt(elapsedTime % 60);
            mainSlider.value = elapsedTime;
            mainSlider.maxValue = sliderMaxValue;
        }
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
