﻿using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class BoardManager : SceneSingleton<BoardManager> {
        // This is all the options in the board
        public enum GridPointObject {
            Empty,
            Wall,
            Box,
            PowerUp,
            Enemy,
            Bomb
        }
		
        // This is a single point on the grid
        public class GridPoint {
            public int row;
            public int column;
            public bool isOnFile = false;
            public GridPointObject gridPointObject;
            public GameObject gameObject;
        }

        public int columns;
        public int rows;
        public GameObject floorTiles;
        public GameObject wallTiles;
        public GameObject boxTiles;
        public GameObject powerUpsTiles;
        public GameObject specialBoxTiles;
        public GameObject specialLifeBoxTiles;
        public GameObject specialEnemyFreezeTiles;

        public GameObject enemyBlue;
        public GameObject enemyPink;
        public GameObject enemyPurple;

        private List<GridPoint[]> m_board = null;

        private Transform boardHolder;

        public int numOfBoxs;
        public int numOfPowerUps;
        private int numOfEnemies;
        private int numOfColors;

        public void Start()
        {
            //base.Awake();
           // DontDestroyOnLoad(gameObject);
        }

        public void setNumberOfEnemies(int numberOfEnemies)
        {
            numOfEnemies = numberOfEnemies;
        }

        public void setNumberOfColors(int numberOfColors)
        {
            numOfColors = numberOfColors;
        }

        private void BoardSetup()
        {
            boardHolder = new GameObject("Board").transform;
            m_board = new List<GridPoint[]>();
            for (var i = 0; i < rows; i++)
            {
                m_board.Add(new GridPoint[columns]);
            }

            for (var column = 0; column < columns; column++)
            {
                for (var row = 0; row < rows; row++)
                {
                   
                    {
                        GameObject toInstantiate = floorTiles;

                        m_board[row][column] = new GridPoint()
                        {
                            row = row,
                            column = column,
                            gridPointObject = GridPointObject.Empty
                        };
                        // If a wall
                        if ((row == 0 || column == columns - 1 || column == 0 || row == rows - 1))
                        {
                            toInstantiate = wallTiles;
                            m_board[row][column].gridPointObject = GridPointObject.Wall;
                        }

                        var instance =
                            Instantiate(toInstantiate, new Vector3(column * 1.5f -1.25f, row * 1.5f - 4.8f, 0f), Quaternion.identity) as GameObject;
                        if (instance.GetComponent<floor>() != null)
                        {
                            instance.GetComponent<floor>().gridRow = row;
                            instance.GetComponent<floor>().gridCol = column;
                        }
                        m_board[row][column].gameObject = instance;
                        instance.transform.SetParent(boardHolder);

                    }

                }

            }
        }

        // TODO: this function should be deleted
        public int[] RandomPosition()
        {
            int randomRowIndex = Random.Range(0, m_board.Count);
            int randomColumnIndex = Random.Range(0, m_board[0].Length);
            int[] array = new int[] {randomRowIndex, randomColumnIndex};
            return array;
        }

        // TODO: This function should be deleted
        public void LayoutObjectAtRandom(GameObject obj, GridPointObject gridPointObjectToAdd, int numberOfOccurences)
        {          

            for (int i = 0; i < numberOfOccurences; i++) {
                //    var randomPosition = RandomPosition();
                int randomRowIndex = Random.Range(0, m_board.Count);
                int randomColumnIndex = Random.Range(0, m_board[0].Length);
                var gridPoint = m_board[randomRowIndex][randomColumnIndex];
                var gridPointObject = gridPoint.gridPointObject;
                if (gridPointObject == GridPointObject.Empty)
                {
                    gridPoint.gridPointObject = gridPointObjectToAdd;
                    if (gridPointObjectToAdd == GridPointObject.Enemy)
                    {
                        m_board[randomRowIndex][randomColumnIndex].gridPointObject = GridPointObject.Enemy;
                        var enemy = Instantiate(obj, gridPoint.gameObject.transform.position, Quaternion.identity) as GameObject;
                        enemy.GetComponent<Enemy>().gridRow = randomRowIndex;
                        enemy.GetComponent<Enemy>().gridCol = randomColumnIndex;
                        //new Vector3(gridPoint.gameObject.transform.position.x, gridPoint.gameObject.transform.position.y,-1)
                    }
                    else if (gridPointObjectToAdd == GridPointObject.Box)
                    {
                        m_board[randomRowIndex][randomColumnIndex].gridPointObject = GridPointObject.Box;
                        var box = Instantiate(obj, gridPoint.gameObject.transform.position, Quaternion.identity) as GameObject;
                        box.GetComponent<Box>().gridRow = randomRowIndex;
                        box.GetComponent<Box>().gridCol = randomColumnIndex;
                    } 
                    else
                    {
                        Instantiate(obj, gridPoint.gameObject.transform.position, Quaternion.identity);
                    }
                } else
                {
                    i--;
                }
            }
        }

        public void SetupScene(int level) {
            // Initialize the board
            if(m_board == null) 
            BoardSetup();

            //Instantiate a random number of boxes tiles based on minimum and maximum, at randomized positions.
            LayoutObjectAtRandom (boxTiles, GridPointObject.Box, numOfBoxs);
            LayoutObjectAtRandom(specialBoxTiles, GridPointObject.Box, 1);
            LayoutObjectAtRandom(specialLifeBoxTiles, GridPointObject.Box, 1);
            LayoutObjectAtRandom(specialEnemyFreezeTiles, GridPointObject.Box, 1);
            
            //Instantiate a random number of powerUps tiles based on minimum and maximum, at randomized positions.
            // LayoutObjectAtRandom (powerUpsTiles, GridPointObject.PowerUp, numOfPowerUps);

            setEnemiesOnTheBoard();
        }

        public void clearScene()
        {
            var clones = GameObject.FindGameObjectsWithTag("Box");
            foreach (var clone in clones)
            {
                Destroy(clone);
                m_board[clone.GetComponent<Box>().gridRow][clone.GetComponent<Box>().gridCol].gridPointObject = GridPointObject.Empty;
            }
        }

        
        private void setEnemiesOnTheBoard()
        {
            for (int i = 0; i < numOfColors; i++)
            {
                numOfEnemies--;
                switch (i)
                {
                    case colorManager.colorsOptions.Blue:
                        {
                            GameManager.main.enemiesOnTheBoard[colorManager.colorsOptions.Blue]++;
                            LayoutObjectAtRandom(enemyBlue, GridPointObject.Enemy, 1);
                            break;
                        }
                    case colorManager.colorsOptions.Pink:
                        {
                            GameManager.main.enemiesOnTheBoard[colorManager.colorsOptions.Pink]++;
                            LayoutObjectAtRandom(enemyPink, GridPointObject.Enemy, 1);
                            break;
                        }
                    case colorManager.colorsOptions.Purple:
                        {
                            GameManager.main.enemiesOnTheBoard[colorManager.colorsOptions.Purple]++;
                            LayoutObjectAtRandom(enemyPurple, GridPointObject.Enemy, 1);
                            break;
                        }
                }
            }
            
            while (numOfEnemies > 0) {
                int colorOfEnemy = (int)UnityEngine.Random.Range(0, numOfColors);
                int currentNumberOfEnemy = (int)UnityEngine.Random.Range(1, numOfEnemies);

                switch (colorOfEnemy)
                {
                    case colorManager.colorsOptions.Blue:
                        {
                            GameManager.main.enemiesOnTheBoard[colorManager.colorsOptions.Blue] += currentNumberOfEnemy;
                            LayoutObjectAtRandom(enemyBlue, GridPointObject.Enemy, currentNumberOfEnemy);
                            break;
                        }
                    case colorManager.colorsOptions.Pink:
                        {
                            GameManager.main.enemiesOnTheBoard[colorManager.colorsOptions.Pink] += currentNumberOfEnemy;
                            LayoutObjectAtRandom(enemyPink, GridPointObject.Enemy, currentNumberOfEnemy);
                            break;
                        }
                    case colorManager.colorsOptions.Purple:
                        {
                            GameManager.main.enemiesOnTheBoard[colorManager.colorsOptions.Purple] += currentNumberOfEnemy;
                            LayoutObjectAtRandom(enemyPurple, GridPointObject.Enemy, currentNumberOfEnemy);
                            break;
                        }
                }
                
                numOfEnemies -= currentNumberOfEnemy;
            }
        }

        // This function should update the grid
        public void updateMovementPosition(int oldRow, int oldCol, int newRow, int newCol)
        {
            m_board[newRow][newCol].gridPointObject = m_board[oldRow][oldCol].gridPointObject;
            m_board[oldRow][oldCol].gridPointObject = GridPointObject.Empty;
        }

        public void setBombPosition(int row, int column)
        {
            m_board[row][column].gridPointObject = GridPointObject.Bomb;
        }

        // Checks if the next place is box or wall
        public bool CanMoveToGridPoint(int rowToMoveTo, int columnToMoveTo)
        {
            var gridPointObject = m_board[rowToMoveTo][columnToMoveTo].gridPointObject;
            return gridPointObject != GridPointObject.Wall && 
                    gridPointObject != GridPointObject.Box && 
                    gridPointObject != GridPointObject.Enemy && 
                    gridPointObject != GridPointObject.Bomb;
        }
        
        public GridPointObject checkGrid(int row, int col)
        {
            return m_board[row][col].gridPointObject;
        }
    }
}
