using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    //[SerializeField] private TextMeshProUGUI timerText;
    private bool isTimerRunning = false;
    public float sliderMaxValue = 30f;
    public float elapsedTime = 0f;
    public float sirka = 1f;

    public GameObject slider;
    void Update()
    {
        if (isTimerRunning)
        {
            ElapsedTime();
            SliderNastav(elapsedTime, sliderMaxValue);
        }
    }

    void ElapsedTime()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > sliderMaxValue)
        {
            elapsedTime = sliderMaxValue;
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
    public void SliderNastav(float now, float max)
    {
        float pomer = (float)now / max;

        float novaSirka = sirka * pomer;

        slider.transform.localScale = new Vector3(novaSirka, slider.transform.localScale.y, slider.transform.localScale.z);
        slider.transform.localPosition = new Vector3(-(sirka - novaSirka)/2, 0, 0);
    }
}
