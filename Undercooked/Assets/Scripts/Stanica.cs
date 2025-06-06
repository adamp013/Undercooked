using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public bool hori = false;
    public bool belt = false;
    public bool pult = false;
    public Transform stanicaHolder;
    public GameObject jedloPlaceholder;
    public TMP_Text peneziLabel;
    private int penezi;
    public int potrebnePeniaze = 15;

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
        Timer holder = targetImage.GetComponent<Timer>();;timer.gameObject.SetActive(true);


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
                output = mj.VratVysledokReceptu(input);
                input = null;
            }
            timer.TimeStopped();
        }
        else if (time < timeZhorenie)
        {
            output = mj.VratVysledokReceptu(input);
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
            output = new List<Food>();
            hasOutput = false;
            hasOutputs = false;
            free = true;
        }
    }
    public Food Grab()
    {
        Food o;
        if (hasOutput) //ked ma jeden
        {
            o = output[0];
            output = new List<Food>();
            free = true;
            hasOutput = false;
            activne = false;
        }
        else  //ked ma viacero
        {
            //StartSelect();
            if (output[0].rozlozitelne)
            { 
            o = output[1];
            }
            else
            {
                o = output[0];
                hasOutputs = false;
                free = true;
            }
         
            output.Remove(o);
        }

        if (jedloPlaceholder != null && output.Count <= 1)
        {
            Destroy(jedloPlaceholder);
            output = new List<Food>();
        }
        return o;
    }
    public bool Place(Food jedlo)
    {
        if (!mj.ExistujeRecept(jedlo, typStanice)) //ci ide do stanice
        {
            return false;
        }
        if (typStanice != 7 && typStanice != 1 && typStanice != 6)
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
        else //ak je pult alebo belt
        {
            if (typStanice == 1 || typStanice == 6)
            {
                hasOutput = true;
                output.Add(jedlo);
                output.AddRange(mj.VratVysledokReceptu(jedlo));
                free = false;
                jedloPlaceholder = Instantiate(jedlo.gameObject, stanicaHolder.position, Quaternion.identity);
                jedloPlaceholder.transform.SetParent(stanicaHolder);
            }
            else if (typStanice == 7)
            {
                penezi += mj.VratCenuJedla(jedlo);
                peneziLabel.text = penezi.ToString() + " kačiek";
                if (penezi >= potrebnePeniaze)
                {
                    GameManager.Instance.gt.Finish();
                }
            }
        }
            return true;
    }

    public void StartGame()
    {
        jedloPlaceholder = Instantiate(output[0].gameObject, stanicaHolder.position, Quaternion.identity);
        jedloPlaceholder.transform.SetParent(stanicaHolder);
        output.AddRange(mj.VratVysledokReceptu(output[0]));
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
    }
}
