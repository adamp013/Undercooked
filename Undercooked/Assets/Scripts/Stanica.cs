using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] public GameObject canvasTimer;
    public ManagerJedla mj;

    public void Select()
    {
        //wheel posuvaj kurzor optional
    }
    public void StartSelect()
    {
        //vygeneruj wheel
    }
    public Food EndSelect(bool isPlayerOne)
    {
        //int
        int poradie = wheel.Choice(output);
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
        time += Time.deltaTime;
        if (time > fullTime)
        {
            if (activneInteractable)
            {
                //end napr zmen timer na zelenu
            }
            else if (!canFire)
            {
                //timer zruz a schovaj
                canvasTimer.SetActive(false);
                output = mj.VratVysledokReceptu(input, typStanice);
            }
            else
            {
                if (time > timeZhorenie)
                {
                    canvasTimer.SetActive(false);
                }
                else
                {

                }
                //timer pri ohni
            }
        }
    }
    public void StartInteract()
    {
        if (!activneInteractable)
        {
            timer.elapsedTime = 0f;
            timer.TimeResumed();
            canvasTimer.SetActive(true);
        }
    }
    public void EndInteract()
    {
        if (activneInteractable) {
            if (time > fullTime) {
                output = mj.VratVysledokReceptu(input, typStanice);
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
            activne = false;
            return o;
        }
        return null;
    }
    public void Place(Food jedlo)
    {
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
