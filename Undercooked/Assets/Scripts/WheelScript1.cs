using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelScript : MonoBehaviour
{
    public GameObject linePrefab;
    public GameObject itemPlaceholderPrefab;

    public List<Jedlo> test;
    public List<GameObject> vygenerovane;
    public int highLight = -1;
    private Vector3 normScale;
    public float newScale;
    void Start()
    {
        gameObject.SetActive(false);
        //GenerateWheel(test);
    }

    public void GenerateWheel(List<Jedlo> jedla)
    {
        Debug.Log("Generating");
        gameObject.SetActive(true);
        while (vygenerovane.Count > 0)
        {
            Destroy(vygenerovane[0]);
            vygenerovane.RemoveAt(0);
        }
        vygenerovane = new List<GameObject>();
        int numberOfItems = jedla.Count;
        float angleStep = 360f / numberOfItems; //jeden segment

        for (int i = 0; i < numberOfItems; i++)
        {
            float angle = i * angleStep;
            float angleRad = angle * Mathf.Deg2Rad;
            float itemPosRad = (angle + angleStep / 2) * Mathf.Deg2Rad;

            if (linePrefab != null)
            {
                Vector3 linePos = new Vector3(Mathf.Sin(angleRad) / 4, Mathf.Cos(angleRad) / 4, 0.001f);
                GameObject line = Instantiate(linePrefab, transform);
                vygenerovane.Add(line);
                line.transform.localPosition = linePos;
                line.transform.localRotation = Quaternion.Euler(0, 0, -angle);
            }

            if (jedla[i] != null)
            {
                Vector3 itemPos = new Vector3(Mathf.Sin(itemPosRad) / 4, Mathf.Cos(itemPosRad) / 4, 0.001f);
                itemPos.z = 0.0001f;
                GameObject item = Instantiate(jedla[i].gameObject, transform);
                vygenerovane.Add(item);
                item.transform.localPosition = itemPos;
                item.transform.localRotation = Quaternion.Euler(40, 0, 0);
            }
        }
    }

    public void DestroyWheel()
    {
        this.gameObject.SetActive(true);
        while (vygenerovane.Count > 0)
        {
            Destroy(vygenerovane[0]);
            vygenerovane.RemoveAt(0);
        }
        vygenerovane = new List<GameObject>();
    }
    public void HighLight(int index)
    {
        if (index > 0 && index < vygenerovane.Count && highLight != index)
        {
            if (highLight != -1)
            {
                vygenerovane[highLight].transform.localScale = normScale;
            }
            highLight = index;
                normScale = vygenerovane[index].transform.localScale;
                vygenerovane[index].transform.localScale = newScale * normScale;
        }
    }

    public int Choice(int numberOfSegments, float h, float v)
    {
        if (h == 0f && v == 0f)
        {
            // Ak nie je žiadny vstup, môžeš vrátiť nejakú špeciálnu hodnotu
            // alebo si pamätať posledný vybraný segment. Pre jednoduchosť vraciam -1.
            return -1;
        }

        // Vypočítame uhol v stupňoch od osi +X (smer doprava)
        float angleRad = Mathf.Atan2(v, h);
        float angleDeg = angleRad * Mathf.Rad2Deg;

        // Prevedieme uhol do rozsahu 0 až 360 stupňov
        if (angleDeg < 0)
        {
            angleDeg += 360f;
        }

        // Vypočítame uhol na jeden segment
        float anglePerSegment = 360f / numberOfSegments;

        Debug.Log("Angle: " + angleDeg + ", angleSegment: " + anglePerSegment);
        // Vypočítame index segmentu
        int segmentIndex = Mathf.FloorToInt(angleDeg / anglePerSegment);

        // Zabezpečíme, že index je v platnom rozsahu
        if (segmentIndex < 0)
        {
            segmentIndex = 0;
        }
        else if (segmentIndex >= numberOfSegments)
        {
            segmentIndex = numberOfSegments - 1;
        }

        return segmentIndex;
    }

}

/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelScript : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject linePrefab;
    public GameObject itemPlaceholderPrefab;

    void Start()
    {
          GenerateWheel(3);
    }

    private void GenerateWheel(int numberOfItems)
    {
        float angleStep = 360f / numberOfItems; //jeden segment

        for (int i = 0; i < numberOfItems; i++)
        {
            float angle = i * angleStep; //ciara po objekte
            float angleRad = angle * Mathf.Deg2Rad;
            float itemPosRad = (angle + angleStep / 2) * Mathf.Deg2Rad;

            // === Create Line ===
            if (linePrefab != null)
            {   
                Vector3 linePos = new Vector3(Mathf.Sin(angleRad) / 4, Mathf.Cos(angleRad) / 4, 0.001f);
                GameObject line = Instantiate(linePrefab, transform);
                line.transform.localPosition = linePos;
                line.transform.localRotation = Quaternion.Euler(0, 0, -angle);

            }
            // === Create Item Placeholder ===
            if (itemPlaceholderPrefab != null)
            {
                Vector3 itemPos = new Vector3(Mathf.Sin(itemPosRad) / 4, Mathf.Cos(itemPosRad) / 4, 0.001f);
                itemPos.z = 0.0001f;
                GameObject item = Instantiate(itemPlaceholderPrefab, transform);
                item.transform.localPosition = itemPos;
                item.transform.localRotation = Quaternion.Euler(90, 0, 0);
                Debug.Log("Ok");
            }
        }
    }
    public int Choice(List<Jedlo> jedla)
    {
       // GenerateWheel(5);
        return 0;
    }
}*/
