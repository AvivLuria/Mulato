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
            Enemy,
            Player,
            PowerUp,
            Bomb,
            MovingBox
        }
		
        // This is a single point on the grid
        public class GridPoint {
            public int row;
            public int column;
            public bool isOnFile = false;
            public GridPointObject gridPointObject;
            public GameObject gameObject;
        }

        [Serializable]
        public class Count
        {
            public int minimum;
            public int maximum;

            public Count (int min, int max)
            {
                minimum = min;
                maximum = max;
            }
        }
        //TODO delete this vars later
        public int moveableBoxPositionRow;
        public int moveableBoxPositionCol;

      
        public int playerPositionRow;
        public int playerPositionCol;
        public int columns;
        public int rows;
        public Count wallCount = new Count (5, 9);
        public Count powerUpsCount = new Count (1, 5);
        public GameObject floorTiles;
        public GameObject wallTiles;
        public GameObject boxTiles;
        public GameObject powerUpsTiles;
        public GameObject enemyTiles;
        public GameObject Player;
        public GameObject movingBox;
        
        
       
       
        
        private List<GridPoint[]> m_board;

        private Transform boardHolder;

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

        // Place the player in his default position
        private void setupPlayer()
        {
            var gridPoint = m_board[playerPositionRow][playerPositionCol];
            gridPoint.gridPointObject = GridPointObject.Player;
            gridPoint.gameObject = Instantiate(Player, gridPoint.gameObject.transform.position, Quaternion.identity) as GameObject;
        }
        //TODO: this function should be deleted
        private void setupMoveableBoxes()
        {
           
            var gridPoint = m_board[moveableBoxPositionRow][moveableBoxPositionCol];
            gridPoint.gridPointObject = GridPointObject.MovingBox;
            
            gridPoint.gameObject = Instantiate(movingBox, gridPoint.gameObject.transform.position, Quaternion.identity) as GameObject;
            gridPoint.gameObject.GetComponent<Box>().gridRow = moveableBoxPositionRow;
            gridPoint.gameObject.GetComponent<Box>().gridCol = moveableBoxPositionCol;
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
        public void LayoutObjectAtRandom(GameObject obj, GridPointObject gridPointObjectToAdd, int minimum, int maximum)
        {
            var objectCount = Random.Range (minimum, maximum + 1);

            for (int i = 0; i < objectCount; i++) {
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
                    } else
                    {
                        Instantiate(obj, gridPoint.gameObject.transform.position, Quaternion.identity);
                    }
                }
            }
        }

        public void SetupScene(int level) {
            //Creates the outer walls and floor.
            BoardSetup ();

            //Instantiate a random number of boxes tiles based on minimum and maximum, at randomized positions.
            LayoutObjectAtRandom (boxTiles, GridPointObject.Box, wallCount.minimum, wallCount.maximum);

            //Instantiate a random number of powerUps tiles based on minimum and maximum, at randomized positions.
            LayoutObjectAtRandom (powerUpsTiles, GridPointObject.PowerUp, powerUpsCount.minimum, powerUpsCount.maximum);

            // Setup player position on the map
            setupPlayer();
            //TODO: this should be deleted
            setupMoveableBoxes();
            //Determine number of enemies based on current level number, based on a logarithmic progression
            //TODO: should be changed
            var enemyCount = 10;//(int)Mathf.Log(level, 2f);

            //Instantiate a random number of enemies based on minimum and maximum, at randomized positions.
            LayoutObjectAtRandom (enemyTiles, GridPointObject.Enemy, enemyCount, enemyCount);
        }

        // This function should update the grid
        public void updateMovementPosition(int oldRow, int oldCol, int newRow, int newCol)
        {
            m_board[newRow][newCol].gridPointObject = m_board[oldRow][oldCol].gridPointObject;
            m_board[oldRow][oldCol].gridPointObject = GridPointObject.Empty;
        }
        //TODO : added for moveableBox to check if can move
        private bool checkMoveBox(int rowToMove, int colToMove,int curRow, int curCol)
        {
            var gridPointObject = m_board[rowToMove][colToMove].gridPointObject;
            var box = m_board[curRow][curCol].gameObject;
            if (gridPointObject == GridPointObject.Empty)
            {
                updateMovementPosition(curRow, curCol, rowToMove, colToMove);
                box.GetComponent<Box>().gridRow = rowToMove;
                box.GetComponent<Box>().gridCol = colToMove;
                return true;
            }
            else return false;
        }

        public GridPointObject cc(int row, int col)
        {
            return m_board[row][col].gridPointObject;
        }
        // Checks if the next place is box or wall
        //TODO - changed for moveableBox
        public bool CanMoveToGridPoint(int rowToMoveTo, int columnToMoveTo,int player)
        {
            var gridPointObject = m_board[rowToMoveTo][columnToMoveTo].gridPointObject;
            
            if (gridPointObject == GridPointObject.MovingBox && player == 1)
            {
                //direction to check to move for moveable box
                if (playerPositionCol == columnToMoveTo)
                {
                    return playerPositionRow > rowToMoveTo
                        ? checkMoveBox(rowToMoveTo - 1, columnToMoveTo, rowToMoveTo, columnToMoveTo)
                        : checkMoveBox(rowToMoveTo + 1, columnToMoveTo, rowToMoveTo, columnToMoveTo);
                }
                else
                {
                    return playerPositionCol > columnToMoveTo
                        ? checkMoveBox(rowToMoveTo , columnToMoveTo - 1, rowToMoveTo, columnToMoveTo)
                        : checkMoveBox(rowToMoveTo , columnToMoveTo + 1, rowToMoveTo, columnToMoveTo);
                }
            } else
            return gridPointObject != GridPointObject.Wall && gridPointObject != GridPointObject.Box && gridPointObject != GridPointObject.MovingBox;
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
