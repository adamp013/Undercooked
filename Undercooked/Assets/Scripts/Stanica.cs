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
    public Jedlo input;
    public List<Jedlo> output;
    public bool hasInput = false;
    public bool hasOutput = false;
    public bool hasOutputs = false;
    public bool free = true;//ma outputy aj inputy na false
    public WheelScript wheel;
    public Timer timer;
    public ManagerJedla mj;
    public bool Player1isTouching = false;
    public bool hori = false;

    public void Select()
    {
        //wheel posuvaj kurzor optional
    }
    public void StartSelect()
    {
        wheel.GenerateWheel(output);
    }
    public Jedlo EndSelect(bool isPlayerOne)
    {
        float h = isPlayerOne ? Input.GetAxis("Horizontal") : Input.GetAxis("Horizontal2");
        float v = isPlayerOne ? Input.GetAxis("Vertical") : Input.GetAxis("Vertical2");
        int poradie = wheel.Choice(output.Count, h,v);
        Debug.Log(poradie);
        Jedlo p = output[poradie];
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
        if (timer != null)
        {
            timer.gameObject.SetActive(true);
        }
        else
        {
            return;
        }


        time += Time.deltaTime;
        if (time > fullTime)
        {
            if (activneInteractable)
            {

            }
            else if (!canFire)
            {
                //timer zruz a schovaj
                EndInteract();
            }
            else
            {
                timer.sliderMaxValue = timeZhorenie;
                if (time > timeZhorenie)
                {
                    EndInteract();
                }
                else if (time > fullTime && !hori)
                {
                    hori = true;
                    EndInteract();
                }
                //timer pri ohni
            }
        }
        else if (!activneInteractable) 
        {

            timer.gameObject.SetActive(true);
        }
    }
    public void StartInteract()
    {
        if (timer != null)
        {
            return;
        }

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
        timer.gameObject.SetActive(false);
        if (activneInteractable)
        {
            if (time > fullTime)
            {
                hasInput = false;
                activne = false;
                output = mj.VratVysledokReceptu(input, typStanice);
                input = null;
            }
            timer.TimeStopped();
        }
        else if (time < timeZhorenie)
        {
            output = mj.VratVysledokReceptu(input, typStanice);
            input = null;
            hasInput = false;
            if (output.Count == 1)
            {
                hasOutput = true;
            }
            else if (output.Count > 1)
            {
                hasOutputs = true;
            }
            else
            {
                free = true;
            }
            if (!canFire)
            {
                timer.gameObject.SetActive(false);
                timer.TimeStopped();
            }
        }
        else if (canFire)
        {
            output = new List<Jedlo>();
            hasOutput = false;
            hasOutputs = false;
            free = true;
        }
    }
    public Jedlo Grab()
    {
        if (hasOutput && output.Count == 1)
        {
            Jedlo o = output[0];
            output = new List<Jedlo>();
            input = null;
            stavRozlozenia = false;
            free = true;
            hasOutput = false;
            activne = false;
            return o;
        }
        else if (stavRozlozenia)
        {
            if (output.Count == 1)
            {
                Jedlo o = output[0];
                output = new List<Jedlo>();
                input = null;
                stavRozlozenia = false;
                free = true;
                hasOutput = false;
                activne = false;
                return o;
            }
            else
            {
                Jedlo o = input;
                o.jedla = output;
                output = new List<Jedlo>();
                input = null;
                stavRozlozenia = false;
                free = true;
                hasOutput = false;
                activne = false;
                return o;
            }
        }
        return null;
    }
    public void Place(Jedlo jedlo)
    {
        if (Pult)
        {
            if (mj.ExistujeRecept(jedlo, 0))
            {
                stavRozlozenia = true;
                if (mj.ExistujeRecept(jedlo, 0))
                {
                    stavRozlozenia = true;
                    input = jedlo;
                    output = jedlo.jedla;
                    if (output.Count == 1)
                    {
                        hasOutput = true;
                        hasOutputs = false;
                    }
                    else if (output.Count > 1)
                    {
                        hasOutputs = true;
                        hasOutput = false;
                    }
                    hasInput = true;
                    free = false;
                }
                return;
            }
        }
        if (Belt)
        {
            if (cm != null)
            {
                float coins = mj.VratCenuJedla(jedlo);
                cm.AddCoins(coins);
                return;
            }
        }
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
        if (!activneInteractable && (hasInput || hori))
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
        ChildUpdate();
    }
    public bool ZakaznikovStol = false;
    public bool Pult = false;
    public bool Belt = false;
    public List<Jedlo> menuZakaznika;//potrebne len pri stoleZakaznika
    public CoinManager cm;
    public bool stavRozlozenia = false;
    public void ChildUpdate()
    {
        if (ZakaznikovStol)
        {
            if (free)
            {
                Jedlo noveJedlo = menuZakaznika[Random.Range(0, menuZakaznika.Count)];
                if (mj.ExistujeRecept(noveJedlo, 0))
                {
                    stavRozlozenia = true;
                    input = noveJedlo;
                    output = noveJedlo.jedla;
                    if (output.Count == 1)
                    {
                        hasOutput = true;
                        hasOutputs = false;
                    }
                    else if (output.Count > 1)
                    {
                        hasOutputs = true;
                        hasOutput = false;
                    }
                    hasInput = true;
                    free = false;
                }
            }
        }
    }
}
