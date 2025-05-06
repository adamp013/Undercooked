using UnityEngine;
using System.Collections.Generic;

public class Movement : MonoBehaviour
{
    public bool isPlayerOne;
    public int[,] tileMap;
    public List<int> walkableIndices;
    public float speed = 5f;
    public float width = 0.9f; // šírka hráča (používa sa na kolízie)

    public int obs = 0;
    void Update()
    {
        if (tileMap != null)
        {
            obs = tileMap.GetLength(0) + tileMap.GetLength(1);
            float h = isPlayerOne ? Input.GetAxis("Horizontal") : Input.GetAxis("Horizontal2");
            float v = isPlayerOne ? Input.GetAxis("Vertical") : Input.GetAxis("Vertical2");

            Vector3 input = new Vector3(h, 0, v).normalized;
            Vector3 dir = input * speed * Time.deltaTime;

            transform.position += GetAllowedMovement(dir);
        }
    }

    Vector3 GetAllowedMovement(Vector3 movementDelta)
    {
        Vector3 pos = transform.position;
        float halfWidth = width / 2f;


        Vector3[] cp = new Vector3[4];
        cp[0] = new Vector3(pos.x - halfWidth, 0, pos.z - halfWidth);
        cp[1] = new Vector3(pos.x + halfWidth, 0, pos.z - halfWidth);
        cp[2] = new Vector3(pos.x + halfWidth, 0, pos.z + halfWidth);
        cp[3] = new Vector3(pos.x - halfWidth, 0, pos.z + halfWidth);

        Vector3 xMovement = new Vector3(movementDelta.x, 0, 0);
        Vector3 zMovement = new Vector3(0, 0, movementDelta.z);

        bool xDih = false;
        bool zDih = false;

        Vector3[] g = cp;
        for (int i = 0; i < 4; i++)
        {
            xDih = xDih || IsColliding(g[i] + xMovement);
        }

        for (int i = 0; i < 4; i++)
        {
            zDih = zDih || IsColliding(g[i] + zMovement);
        }

        return (!xDih ? xMovement : new Vector3(0,0,0)) + (!zDih ? zMovement : new Vector3(0,0,0));
    }

    bool IsColliding(Vector3 center)
    {
        int x = Mathf.RoundToInt(center.x);
        int z = Mathf.RoundToInt(center.z);

        if (x < 0 || z < 0 || x >= tileMap.GetLength(0) || z >= tileMap.GetLength(1))
        {
            return true;
        }

        return !walkableIndices.Contains(tileMap[x, z]);
    }
}