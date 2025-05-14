using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerJedla : MonoBehaviour
{
    public List<Jedlo> VratVysledokReceptu(Jedlo jedlo, int stanica) //Output / dalsia forma
    {
        return jedlo.jedla;
    }
    public bool ExistujeRecept(Jedlo jedlo, int stanica) //ci sa da tam jebnut
    {
        return (stanica == jedlo.stanica);
    }
    public float VratCenuJedla(Jedlo jedlo) //cena
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
