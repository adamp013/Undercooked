using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stanica : MonoBehaviour
{
    public float fullTime = 5f;
    public float timeZhorenie = 15f;
    public bool canFire = true;//mozu horiet veci
    public bool activneInteractable = true; // ci musis pri nej stat
    public bool activne = false; // ci stanica pracuje
    public float time = 0f;
    public int typStanice;
    public Food input;
    public List<Food> output;
    public bool hasInput = false;
    public bool hasOutput = false;
    public bool hasOutputs = false;
    public bool free = true;//ma outputy aj inputy na false
    public WheelScript wheel;
    public Timer timer;
    public Image targetImage;
    public ManagerJedla mj;
    public bool Player1isTouching = false;

    public void Select()
    {
        //wheel posuvaj kurzor optional
    }
    public void StartSelect()
    {
        wheel.GenerateWheel(output);
    }
    public Food EndSelect(bool isPlayerOne)
    {
        //int
        float h = isPlayerOne ? Input.GetAxis("Horizontal") : Input.GetAxis("Horizontal2");
        float v = isPlayerOne ? Input.GetAxis("Vertical") : Input.GetAxis("Vertical2");
        int poradie = wheel.Choice(output.Count, h,v);
        Food p = output[poradie];
        output.RemoveAt(poradie);
        if (output.Count == 1)
        {
            hasOutput = true;
            hasOutputs = false;
        }
        return p;
    }
    public void Interact()
    {

        //timer.gameObject.SetActive(true);

        Timer holder = targetImage.GetComponent<Timer>();
        

        
        time += Time.deltaTime;
        if (time > fullTime)
        {
            if (activneInteractable)
            {
                if (holder != null && holder.Image != null)
                {
                    holder.Image.color = Color.green; //SPRAV TO GREEN
                }
            }
            else if (!canFire)
            {
                //timer zruz a schovaj
                timer.gameObject.SetActive(false);
                output = mj.VratVysledokReceptu(input, typStanice);
            }
            else
            {
                timer.sliderMaxValue = timeZhorenie;
                if (time > timeZhorenie)
                {
                    timer.gameObject.SetActive(false);
                }
                else
                {

                }
                //timer pri ohni
            }
        }
        else
        {

        }
    }
    public void StartInteract()
    {
        if (canFire && time > fullTime)
        {
            timer.sliderMaxValue = timeZhorenie;
        }
        else
        {
            timer.sliderMaxValue = fullTime;
        }
        timer.gameObject.SetActive(true);
        timer.TimeResumed();

        if (!activneInteractable)
        {
            timer.elapsedTime = 0f;
        }

    }
    public void EndInteract()
    {
        if (activneInteractable) {
            if (time > fullTime) {
                hasInput = false;
                activne = false;
                output = mj.VratVysledokReceptu(input, typStanice);
                input = null;
            }
            timer.TimeStopped();
        }
    }
    public Food Grab()
    {
        if (hasOutput)
        {
            Food o = output[0];
            output = new List<Food>();
            free = true;
            hasOutput = false;
            activne = false;
            return o;
        }
        return null;
    }
    public void Place(Food jedlo)
    {
        hasInput = true;
        input = jedlo;
        free = false;
        if (!activneInteractable)
        {
            StartInteract();
        }
        else
        {
            timer.elapsedTime = 0f;
            timer.TimeStopped();
        }
    }

    void Update()
    {
        if (!activneInteractable && hasInput)
        {
            activne = true;
            Interact();
        }
        if (output.Count == 1)
        {
            hasOutput = true;
            hasOutputs = false;
        }
        if (output.Count > 1)
        {
            hasOutputs = true;
            hasOutput = false;
        }
    }
}
