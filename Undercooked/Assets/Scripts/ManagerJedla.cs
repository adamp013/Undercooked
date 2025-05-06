using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerJedla : MonoBehaviour
{
    public Food jedlo;
    public List<int> VratVysledokReceptu(Food jedlo) //Output / dalsia forma
    {
        return jedlo.jedla;
    }
    public bool ExistujeRecept(Food jedlo, int stanica) //ci sa da tam jebnut
    {
        return (stanica == jedlo.stanica);
    }
    public float VratCenuJedla(Food jedlo) //cena
    {
        return jedlo.cena;
    }


    
    void Start()
    {
        Debug.Log(VratVysledokReceptu(jedlo));
    }
    void Update()
    {
        
    }
}
