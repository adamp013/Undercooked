using UnityEngine;
using System.Collections.Generic;

public class MapCreator : MonoBehaviour
{
    public List<GameObject> tilePrefabs; // prefabrikáty reprezentujúce rôzne typy dlaždíc (index = typ)
    public int[,] tileMap; // 2D pole indexov, napr. 0 = podlaha, 1 = stena
    public List<int> walkableTileIndices; // spoločný zoznam priechodných dlaždíc pre všetkých hráčov
    public (int, int)[] playerStartPositions; // počiatočné pozície hráčov (x, z)
    public GameObject playerPrefab;

    void Start()
    {
        tileMap = new int[,] {
            {1,1,2,1},
            {1,0,0,3},
            {1,0,0,1},
            {1,2,2,1}
        };
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

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                int tileIndex = tileMap[x, z];
                if (tileIndex >= 0 && tileIndex < tilePrefabs.Count)
                {
                    Vector3 position = new Vector3(x, 0, z); // každý tile je 1 unit od seba
                    Instantiate(tilePrefabs[tileIndex], position, Quaternion.identity, transform);
                }
                else
                {
                    Debug.LogWarning($"Tile index {tileIndex} mimo rozsah tilePrefabs na ({x}, {z})");
                }
            }
        }
    }

    void SpawnPlayers()
    {
        for (int i = 0; i < playerStartPositions.Length; i++)
        {
            (int x, int z) = playerStartPositions[i];
            Vector3 position = new Vector3(x, 0.5f, z); // Y = 0.5f pre hráča nad dlaždicou
            GameObject player = Instantiate(playerPrefab, position, Quaternion.identity);
            
            Movement move = player.GetComponent<Movement>();
            move.isPlayerOne = (i == 0);
            move.tileMap = tileMap;
            move.walkableIndices = walkableTileIndices;
        }
    }
}
