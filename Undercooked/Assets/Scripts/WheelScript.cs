using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelScript : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject linePrefab;
    public GameObject itemPlaceholderPrefab;

    [Header("Settings")]
    public float radius = 100f;

    void Start()
    {
        
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
                item.transform.localRotation = Quaternion.identity;
            }
        }
    }
    public int Choice(List<Food> jedla) //ma byt int nigger
    {
        GenerateWheel(jedla.Count);
        return 0;
    }
}
