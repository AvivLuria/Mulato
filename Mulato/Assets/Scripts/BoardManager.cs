using System;
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
        public GameObject enemyTiles;
 
        private List<GridPoint[]> m_board;

        private Transform boardHolder;

        public int numOfBoxs;
        public int numOfPowerUps;
        public int numOfEnemies;

        private void BoardSetup () 
        {
            boardHolder = new GameObject ("Board").transform;
            m_board = new List<GridPoint[]>();

            for (var i = 0; i < rows; i++) {
                m_board.Add(new GridPoint[columns]);
            }

            for (var column = 0; column < columns; column++) {
               for (var row = 0; row < rows; row++) {
				
                    GameObject toInstantiate = floorTiles;
                    m_board[row][column] = new GridPoint()
                    {
                        row = row,
                        column = column,
                        gridPointObject = GridPointObject.Empty
                    };
                    // If a wall
                    if ((row == 0 || column == columns - 1 || column == 0 || row == rows - 1) ||
                        ((row % 2 == 0) && (column % 2 == 0))) {
                            toInstantiate = wallTiles;
                            m_board[row][column].gridPointObject = GridPointObject.Wall;
                        }

                    var instance = Instantiate (toInstantiate, new Vector3 (column, row, 0f), Quaternion.identity) as GameObject;
                    m_board[row][column].gameObject = instance;
                    instance.transform.SetParent(boardHolder);

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
                }
            }
        }

        public void SetupScene(int level) {
            // Initialize the board
            BoardSetup();

            //Instantiate a random number of boxes tiles based on minimum and maximum, at randomized positions.
            LayoutObjectAtRandom (boxTiles, GridPointObject.Box, numOfBoxs);

            //Instantiate a random number of powerUps tiles based on minimum and maximum, at randomized positions.
           // LayoutObjectAtRandom (powerUpsTiles, GridPointObject.PowerUp, numOfPowerUps);

            //Instantiate a random number of enemies based on minimum and maximum, at randomized positions.
            LayoutObjectAtRandom (enemyTiles, GridPointObject.Enemy, numOfEnemies);
        }

        // This function should update the grid
        public void updateMovementPosition(int oldRow, int oldCol, int newRow, int newCol)
        {
            m_board[newRow][newCol].gridPointObject = m_board[oldRow][oldCol].gridPointObject;
            m_board[oldRow][oldCol].gridPointObject = GridPointObject.Empty;
        }

        // Checks if the next place is box or wall
        public bool CanMoveToGridPoint(int rowToMoveTo, int columnToMoveTo)
        {
            var gridPointObject = m_board[rowToMoveTo][columnToMoveTo].gridPointObject;
            return gridPointObject != GridPointObject.Wall && gridPointObject != GridPointObject.Box && gridPointObject != GridPointObject.Enemy;
        }

        public void setFireOn(int row, int col, int power)
        {
            for (int i = 0; i <= power; i++)
            {
                if (m_board[row + i][col].gridPointObject == GridPointObject.Wall)
                {
                    break;
                } else if (m_board[row + i][col].gridPointObject == GridPointObject.Box)
                {
                    m_board[row + i][col].isOnFile = true;
                    break;
                }

                m_board[row + i][col].isOnFile = true;
            }
            for (int i = 0; i <= power; i++)
            {
                if (m_board[row - i][col].gridPointObject == GridPointObject.Wall)
                {
                    break;
                }
                else if (m_board[row - i][col].gridPointObject == GridPointObject.Box)
                {
                    m_board[row - i][col].isOnFile = true;
                    break;
                }

                m_board[row - i][col].isOnFile = true;
            }
            for (int i = 0; i <= power; i++)
            {
                if (m_board[row][col + i].gridPointObject == GridPointObject.Wall)
                {
                    break;
                }
                else if (m_board[row][col + i].gridPointObject == GridPointObject.Box)
                {
                    m_board[row][col + i].isOnFile = true;
                    break;
                }

                m_board[row][col + i].isOnFile = true;
            }
            for (int i = 0; i <= power; i++)
            {
                if (m_board[row][col - i].gridPointObject == GridPointObject.Wall)
                {
                    break;
                }
                else if (m_board[row][col - i].gridPointObject == GridPointObject.Box)
                {
                    m_board[row][col - i].isOnFile = true;
                    break;
                }

                m_board[row][col - i].isOnFile = true;
            }
        }

        public void setFireOff(int row, int col, int power)
        {
            for (int i = 0; i <= power; i++)
            {
                if (m_board[row + i][col].gridPointObject == GridPointObject.Wall)
                {
                    break;
                }
                else if (m_board[row + i][col].gridPointObject == GridPointObject.Box)
                {
                    m_board[row + i][col].isOnFile = false;
                    break;
                }

                m_board[row + i][col].isOnFile = false;
            }
            for (int i = 0; i <= power; i++)
            {
                if (m_board[row - i][col].gridPointObject == GridPointObject.Wall)
                {
                    break;
                }
                else if (m_board[row - i][col].gridPointObject == GridPointObject.Box)
                {
                    m_board[row - i][col].isOnFile = false;
                    break;
                }

                m_board[row - i][col].isOnFile = false;
            }
            for (int i = 0; i <= power; i++)
            {
                if (m_board[row][col + i].gridPointObject == GridPointObject.Wall)
                {
                    break;
                }
                else if (m_board[row][col + i].gridPointObject == GridPointObject.Box)
                {
                    m_board[row][col + i].isOnFile = false;
                    break;
                }

                m_board[row][col + i].isOnFile = false;
            }
            for (int i = 0; i <= power; i++)
            {
                if (m_board[row][col - i].gridPointObject == GridPointObject.Wall)
                {
                    break;
                }
                else if (m_board[row][col - i].gridPointObject == GridPointObject.Box)
                {
                    m_board[row][col - i].isOnFile = false;
                    break;
                }

                m_board[row][col - i].isOnFile = false;
            }
        }

        public GridPointObject checkGrid(int row, int col)
        {
            return m_board[row][col].gridPointObject;
        }
    }
}
