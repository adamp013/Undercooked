using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerJedla : MonoBehaviour
{
    public List<Food> VratVysledokReceptu(Food jedlo) //Output / dalsia forma
    {
        return jedlo.jedla;
    }
    public bool ExistujeRecept(Food jedlo, int stanica) //ci sa da tam jebnut
    {
        if (stanica == 1 || stanica == 7 || stanica == 6)
        {
            return true;
        }
        return (stanica == jedlo.stanica);
    }
    public int VratCenuJedla(Food jedlo) //cena
    {
        return jedlo.cena;
    }


    
    void Start()
    {

    }
    void Update()
    {
        
    }
}
