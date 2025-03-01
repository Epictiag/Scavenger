﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 8;
    public int rows = 8;
    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();
    private int days = 1;
    // Start is called before the first frame update
    void Start()
    {
        gridPositions.Clear();

        for (int x = 1; x < columns - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    // Clears our list gridPositions and prepares it to generate a new board.
    private void InitialiseList()
    {
        // Clear our list gridPositions.
        gridPositions.Clear();

        // Loop through x axis (columns).
        for (int x = 1; x < columns - 1; x++)
        {
            // Within each column, loop through y axis (rows).
            for (int y = 1; y < rows - 1; y++)
            {
                // At each index add a new Vector3 to our list with the x and y coordinates of that position.
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    private void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;

        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    private Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    private void LayoutObjectAtRandom(GameObject[] tiles, int minmum, int maximum)
    {
        int objectCount = Random.Range(minmum, maximum + 1);

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPos = RandomPosition();
            GameObject tileChoice = tiles[Random.Range(0, tiles.Length)];
            Instantiate(tileChoice, randomPos, Quaternion.identity);
        }
    }

    public void SetupScene(int level)
    {
        //BoardSetup();
        //InitialiseList();
        //LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        //LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
        //int enemyCount = (int)Mathf.Log(level, 2f);
        //enemyCount += 1;
        //LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        //Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
        
        TextAsset levelAsset = Resources.Load("day1") as TextAsset;
        
        if (days == 2)
        {
            levelAsset = Resources.Load("day2") as TextAsset;
        }
        else if (days == 3)
        {
            levelAsset = Resources.Load("day3") as TextAsset;
        }


        string[] lines = levelAsset.text.Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            print(lines[i]);
            for (int j = 0; j < lines[i].Length; j++)
            {
                //floor
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if (j < 10)
                {
                    Instantiate(toInstantiate, new Vector3(j - 1, lines.Length - i - 2, 0f), Quaternion.identity);
                }

                if (lines[i][j] == 'x')
                {
                    //wall
                    GameObject wall = wallTiles[Random.Range(0, wallTiles.Length)];
                    Instantiate(wall, new Vector3(j - 1, lines.Length - i - 2, 0f), Quaternion.identity);
                }
                else if (lines[i][j] == 'F')
                {
                    //food
                    GameObject food = foodTiles[Random.Range(0, foodTiles.Length)];
                    Instantiate(food, new Vector3(j - 1, lines.Length - i - 2, 0f), Quaternion.identity);
                }
                else if (lines[i][j] == 'E')
                {
                    //enemys
                    GameObject enemy = enemyTiles[Random.Range(0, enemyTiles.Length)];
                    Instantiate(enemy, new Vector3(j - 1, lines.Length - i - 2, 0f), Quaternion.identity);
                }
                else if (lines[i][j] == 'T')
                {
                    //exit
                    Instantiate(exit, new Vector3(j - 1, lines.Length - i - 2, 0f), Quaternion.identity);
                }
                
            }

        }
        days++;
    }
}
