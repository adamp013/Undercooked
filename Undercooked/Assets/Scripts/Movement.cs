using UnityEngine;
using System.Collections.Generic;

public class Movement : MonoBehaviour
{
    public bool isPlayerOne;
    public int[,] tileMap;
    public List<int> walkableIndices;
    public float speed = 5f;
    public float width = 0.9f; // šírka hráča (používa sa na kolízie)

    void Update()
    {
        float h = isPlayerOne ? Input.GetAxis("Horizontal") : Input.GetAxis("Horizontal2");
        float v = isPlayerOne ? Input.GetAxis("Vertical") : Input.GetAxis("Vertical2");

        Vector3 input = new Vector3(h, 0, v).normalized;
        Vector3 dir = input * speed * Time.deltaTime;
        Vector3 nextPos = transform.position + dir;

        transform.position += GetAllowedMovement(dir);
    }

    Vector3 GetAllowedMovement(Vector3 movementDelta)
    {
        Vector3 fullMove = transform.position + movementDelta;

        float halfWidth = width / 2f;

        // Skontroluj, či môžeš spraviť celý pohyb
        if (IsAreaWalkable(fullMove, halfWidth))
            return movementDelta;

        // Skús X pohyb samostatne
        Vector3 onlyX = new Vector3(movementDelta.x, 0, 0);
        if (onlyX != Vector3.zero && IsAreaWalkable(transform.position + onlyX, halfWidth))
            return onlyX;

        // Skús Z pohyb samostatne
        Vector3 onlyZ = new Vector3(0, 0, movementDelta.z);
        if (onlyZ != Vector3.zero && IsAreaWalkable(transform.position + onlyZ, halfWidth))
            return onlyZ;

        // Nemôžeš sa pohnúť ani čiastočne
        return Vector3.zero;
    }

    bool IsAreaWalkable(Vector3 center, float halfWidth)
    {
        Vector3[] checkPoints = new Vector3[]
        {
            new Vector3(center.x - halfWidth, 0, center.z - halfWidth),
            new Vector3(center.x + halfWidth, 0, center.z - halfWidth),
            new Vector3(center.x - halfWidth, 0, center.z + halfWidth),
            new Vector3(center.x + halfWidth, 0, center.z + halfWidth),
        };

        foreach (var point in checkPoints)
        {
            int x = Mathf.FloorToInt(point.x);
            int z = Mathf.FloorToInt(point.z);

            if (x < 0 || z < 0 || x >= tileMap.GetLength(0) || z >= tileMap.GetLength(1))
                return false;

            int tileIndex = tileMap[x, z];
            if (!walkableIndices.Contains(tileIndex))
                return false;
        }

        return true;
    }
}