 using UnityEngine;
using System.Collections.Generic;

public class MapCreator : MonoBehaviour
{
    public List<GameObject> tilePrefabs; // prefabrikáty reprezentujúce rôzne typy dlaždíc (index = typ)
    public GameObject floor;
    public int[,] tileMap; // 2D pole indexov, napr. 0 = podlaha, 1 = stena
    public int[,] rotationMap;
    public GameObject[,] stMap;
    public List<int> walkableTileIndices; // spoločný zoznam priechodných dlaždíc pre všetkých hráčov
    public (int, int)[] playerStartPositions; // počiatočné pozície hráčov (x, z)
    public GameObject playerPrefab;
    public float scale;
    void Start()
    {
        //0 - floor
        //1 - pult
        //2 - fridge
        //3 - mixer
        //4 - dryer
        //5 - workbench
        //6 - zakaznikov stol
        //7 - belt
        //potom vyzualne
        //8 - stolicka zakaznika
        tileMap = new int[,] {
            {1,1,3,3,1,1,1},
            {1,0,0,0,0,0,1},
            {2,0,1,5,1,0,4},
            {2,0,0,0,0,0,4},
            {1,1,1,0,1,1,1},
            {0,0,0,0,0,0,0},
            {0,6,0,6,0,6,0},
            {0,0,0,0,0,0,0}
        };
        //tu je rotacia
        //0-default dopredu, 1 VPRAVO, 2 dole, 3 vlavo
        
        rotationMap = new int[,]{
            {1,1,1,1,1,1,1},
            {0,0,0,0,0,0,2},
            {0,0,3,1,3,0,2},
            {0,0,0,0,0,0,2},
            {0,3,3,0,3,3,0},
            {0,0,0,0,0,0,2},
            {0,0,0,0,0,0,2},
            {3,3,3,3,3,3,3}
        };

        walkableTileIndices = new List<int>() { 0 };
        playerStartPositions = new (int,int)[2] {
            (1,1),
            (1,2)
        };
        GenerateMap();
        SpawnPlayers();
    }

    void GenerateMap()
    {
        int width = tileMap.GetLength(0);
        int height = tileMap.GetLength(1);
        stMap = new GameObject[width,height];
        Quaternion[] rotacie = new Quaternion[4]
        {
            Quaternion.LookRotation(Vector3.forward),
            Quaternion.LookRotation(Vector3.right),
            Quaternion.LookRotation(Vector3.back),
            Quaternion.LookRotation(Vector3.left)
        };

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                int tileIndex = tileMap[x, z];
                if (tileIndex >= 0 && tileIndex < tilePrefabs.Count)
                {
                    Vector3 position = new Vector3(x, 0, z);
                    Quaternion rot = rotacie[rotationMap[x,z]];
                    stMap[x, z] = Instantiate(tilePrefabs[tileIndex], position * scale + transform.position, rot, transform);
                }
                else
                {
                    Debug.LogWarning($"Tile index {tileIndex} mimo rozsah tilePrefabs na ({x}, {z})");
                }
            }
        }
        Vector3 centerPosition = new Vector3((width - 1) / 2f, -0.67f, (height - 1) / 2f) * scale + transform.position;

        // Instantiate floor at that position
        Instantiate(floor, centerPosition, Quaternion.identity, transform);

    }

    void SpawnPlayers()
    {
        for (int i = 0; i < playerStartPositions.Length; i++)
        {
            (int x, int z) = playerStartPositions[i];
            if (x >= 0 && x < tileMap.GetLength(0) && z >= 0 && z < tileMap.GetLength(1) && walkableTileIndices.Contains(tileMap[x, z]))
            {
                Vector3 position = new Vector3(x, 0, z);
                GameObject player = Instantiate(playerPrefab, position * scale + transform.position, Quaternion.identity, transform);

                Movement move = player.GetComponent<Movement>();
                move.isPlayerOne = (i == 0);
                move.tileMap = tileMap;
                move.stMap = stMap;
                move.walkableIndices = walkableTileIndices;
            }
            else
            {
                Debug.LogError($"Počiatočná pozícia hráča {i+1} ({x}, {z}) nie je na priechodnej dlaždici!");
            }
        }
    }
}