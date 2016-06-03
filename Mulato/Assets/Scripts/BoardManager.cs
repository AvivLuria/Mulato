using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class BoardManager : MonoBehaviour
        // : SceneSingleton<BoardManager>
    {

       

        // This is all the options in the board
        public enum GridPointObject
        {
            Empty,
            Wall,
            Box,
            PowerUp,
            Enemy,
            Bomb
        }

        // This is a single point on the grid
        public class GridPoint
        {
            public int row;
            public int column;
            public bool isOnFile = false;
            public GridPointObject gridPointObject;
            public GameObject gameObject;
        }

        
        public const int k_Columns = 8;
        public const int k_Rows = 11;

        #region game objects
        //public GameObject boardManager;
        public GameObject floorTiles;
        public GameObject wallTiles;
        public GameObject boxWallTiles;
        public GameObject boxTiles;
        public GameObject powerUpsTiles;
        public GameObject specialBoxTiles;
        public GameObject specialLifeBoxTiles;
        public GameObject specialEnemyFreezeTiles;
        public GameObject enemyBlue;
        public GameObject enemyPink;
        public GameObject enemyPurple;
        #endregion
        private List<GridPoint[]> m_GridPointList = null;
        public List<GameObject> enemies { get; set; }
        private List<GameObject> boxesFloor = null;

        private BombManagerBoxMovingObjectsUpdateBombGridPoint m_UpdateBombGridPoint;
        private MovingObjectsCanMoveOnBoard m_CanMove;
        private Transform boardHolder;

        public int[] wallPostions { get; set; }
        public int m_NumOfNomralBoxes { get; set; }
        public int m_NumOfLifeBoxes { get; set; }
        public int m_NumOfSpecialBombBoxes { get; set; }
        public int m_NumOfFreezeBoxes { get; set; }
        public int m_NumOfEnemies { get; set; }
        public int m_NumOfColors { get; set; }
        public int m_Level { get; set; }
        /*
        void Start()
        {
            m_UpdateBombGridPoint = new BombManagerBoxMovingObjectsUpdateBombGridPoint(UpdateGridPointObject);
            m_SetBombOnBoard = new BombManagerSetBombOnBoard(SetBombOnGrid);
            m_CanMove = new MovingObjectsCanMoveOnBoard(CanMoveToGridPoint);
        }
        */
        /*public GameObject CreateBoardManager(int level)
        {
            var board = Instantiate(boardManager,new Vector3(0,0,0),Quaternion.identity) as GameObject;
           /* m_GridPointList = new List<GridPoint[]>();
            m_Level = level;
            m_NumOfNomralBoxes = 0;
            m_NumOfLifeBoxes = 0;
            m_NumOfSpecialBombBoxes = 0;
            m_NumOfFreezeBoxes = 0;

            m_UpdateBombGridPoint = new BombManagerBoxMovingObjectsUpdateBombGridPoint(UpdateGridPointObject);
            m_SetBombOnBoard = new BombManagerSetBombOnBoard(SetBombOnGrid);
            m_CanMove = new MovingObjectsCanMoveOnBoard(CanMoveToGridPoint);
            
            return board;
        }*/

        /* public void setNumberOfEnemies(int numberOfEnemies)
         {
             numOfEnemies = numberOfEnemies;
         }

         public void setNumberOfColors(int numberOfColors)
         {
             numOfColors = numberOfColors;
         }
         */

       
 
        private void createEnemiesOnTheBoard()
        {
            for (int i = 0; i < m_NumOfColors; i++)
            {
                m_NumOfEnemies--;
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

            while (m_NumOfEnemies > 0)
            {
                int colorOfEnemy = (int)UnityEngine.Random.Range(0, m_NumOfColors);
                int currentNumberOfEnemy = (int)UnityEngine.Random.Range(1, m_NumOfEnemies);

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

                m_NumOfEnemies -= currentNumberOfEnemy;
            }
        }

        public void SetBombOnGrid(int row, int column)
        {
            m_GridPointList[row][column].gridPointObject = GridPointObject.Bomb;
        }
       
        //creates Box Wall in game in the int array postions
        private void setUpWallsOnBoard(int[] wallPos)
        {
            for (int i = 0; i < wallPos.Length; i++)
            {
                int randomRowIndex = wallPos[i] / 10;
                int randomColumnIndex = wallPos[i] % 10;
                var gridPoint = m_GridPointList[randomRowIndex][randomColumnIndex];
                m_GridPointList[randomRowIndex][randomColumnIndex].gridPointObject = GridPointObject.Wall;
                var box = Instantiate(boxWallTiles, gridPoint.gameObject.transform.position, Quaternion.identity) as GameObject;
                boxesFloor.Add(box);
            }
        }

        //Scene Builder <><>
        public void StartScene()
        {
            enemies = new List<GameObject>();
            boxesFloor = new List<GameObject>();
            // Initialize the board
            /*if (m_Board == null)
                BoardSetup();*/
            // Initialize the walls in the board
            BoardSetup();
            setUpWallsOnBoard(wallPostions);

            //Instantiate a random number of boxes tiles based on minimum and maximum, at randomized positions.
            LayoutObjectAtRandom(boxTiles, GridPointObject.Box, m_NumOfNomralBoxes);
            LayoutObjectAtRandom(specialBoxTiles, GridPointObject.Box, m_NumOfSpecialBombBoxes);
            LayoutObjectAtRandom(specialLifeBoxTiles, GridPointObject.Box, m_NumOfLifeBoxes);
            LayoutObjectAtRandom(specialEnemyFreezeTiles, GridPointObject.Box, m_NumOfFreezeBoxes);
            if (m_Level != 0) { 
                createEnemiesOnTheBoard();
            } else
            {
                setUpEnemiesOnTheBoardManualy(m_Level);
            }
        }

        private void setUpEnemiesOnTheBoardManualy(int level)
        {
            if (level == 0)
            {
                m_NumOfEnemies-=4;
                GameManager.main.enemiesOnTheBoard[colorManager.colorsOptions.Blue]=2;
                GameManager.main.enemiesOnTheBoard[colorManager.colorsOptions.Pink]=2;
                ///
                m_GridPointList[1][1].gridPointObject = GridPointObject.Enemy;
                var gridPoint = m_GridPointList[1][1];
                var enemy = Instantiate(enemyBlue, gridPoint.gameObject.transform.position, Quaternion.identity) as GameObject;
                enemy.GetComponent<Enemy>().gridRow = 1;
                enemy.GetComponent<Enemy>().gridCol = 1;
                enemy.GetComponent<Enemy>().setBoardListener(this.gameObject);
                enemies.Add(enemy.gameObject);
                ///
                m_GridPointList[9][6].gridPointObject = GridPointObject.Enemy;
                gridPoint = m_GridPointList[9][6];
                enemy = Instantiate(enemyBlue, gridPoint.gameObject.transform.position, Quaternion.identity) as GameObject;
                enemy.GetComponent<Enemy>().gridRow = 9;
                enemy.GetComponent<Enemy>().gridCol = 6;
                enemy.GetComponent<Enemy>().setBoardListener(this.gameObject);
                enemies.Add(enemy.gameObject);
                ///
                m_GridPointList[1][6].gridPointObject = GridPointObject.Enemy;
                gridPoint = m_GridPointList[1][6];
                enemy = Instantiate(enemyPink, gridPoint.gameObject.transform.position, Quaternion.identity) as GameObject;
                enemy.GetComponent<Enemy>().gridRow = 1;
                enemy.GetComponent<Enemy>().gridCol = 6;
                enemy.GetComponent<Enemy>().setBoardListener(this.gameObject);
                enemies.Add(enemy.gameObject);
                ///
                m_GridPointList[9][1].gridPointObject = GridPointObject.Enemy;
                gridPoint = m_GridPointList[9][1];
                enemy = Instantiate(enemyPink, gridPoint.gameObject.transform.position, Quaternion.identity) as GameObject;
                enemy.GetComponent<Enemy>().gridRow = 9;
                enemy.GetComponent<Enemy>().gridCol = 1;
                enemy.GetComponent<Enemy>().setBoardListener(this.gameObject);
                enemies.Add(enemy.gameObject);
            }       
        }

        //setup grid points and side walls
        private void BoardSetup()
        {
            //boardHolder = new GameObject("Board").transform;
            m_GridPointList = new List<GridPoint[]>();
            for (var i = 0; i < k_Rows; i++)
            {
                m_GridPointList.Add(new GridPoint[k_Columns]);
            }

            for (var column = 0; column < k_Columns; column++)
            {
                for (var row = 0; row < k_Rows; row++)
                {

                    {
                        GameObject toInstantiate = floorTiles;

                        m_GridPointList[row][column] = new GridPoint()
                        {
                            row = row,
                            column = column,
                            gridPointObject = GridPointObject.Empty
                        };
                        // If a wall
                        if ((row == 0 || column == k_Columns - 1 || column == 0 || row == k_Rows - 1))
                        {
                            toInstantiate = wallTiles;
                            m_GridPointList[row][column].gridPointObject = GridPointObject.Wall;
                        }

                        var instance =
                            Instantiate(toInstantiate, new Vector3(column*1.5f - 1.25f, row*1.5f - 4.8f, 0f),
                                Quaternion.identity) as GameObject;
                        boxesFloor.Add(instance);
                        if (instance.GetComponent<floor>() != null)
                        {
                            instance.GetComponent<floor>().gridRow = row;
                            instance.GetComponent<floor>().gridCol = column;
                        }
                        m_GridPointList[row][column].gameObject = instance;
                        instance.transform.SetParent(boardHolder);

                    }

                }

            }
        }

       
        public int[] RandomPosition()
        {
            int randomRowIndex = Random.Range(0, m_GridPointList.Count);
            int randomColumnIndex = Random.Range(0, m_GridPointList[0].Length);
            int[] array = new int[] {randomRowIndex, randomColumnIndex};
            return array;
        }

        public void LayoutObjectAtRandom(GameObject obj, GridPointObject gridPointObjectToAdd, int numberOfOccurences)
        {

            for (int i = 0; i < numberOfOccurences; i++)
            {
                //    var randomPosition = RandomPosition();
                int randomRowIndex = Random.Range(0, m_GridPointList.Count);
                int randomColumnIndex = Random.Range(0, m_GridPointList[0].Length);
                var gridPoint = m_GridPointList[randomRowIndex][randomColumnIndex];
                var gridPointObject = gridPoint.gridPointObject;
                if (gridPointObject == GridPointObject.Empty)
                {
                    gridPoint.gridPointObject = gridPointObjectToAdd;
                    if (gridPointObjectToAdd == GridPointObject.Enemy)
                    {
                        m_GridPointList[randomRowIndex][randomColumnIndex].gridPointObject = GridPointObject.Enemy;
                        var enemy =
                            Instantiate(obj, gridPoint.gameObject.transform.position, Quaternion.identity) as GameObject;
                        enemy.GetComponent<Enemy>().gridRow = randomRowIndex;
                        enemy.GetComponent<Enemy>().gridCol = randomColumnIndex;
                        enemy.GetComponent<Enemy>().setBoardListener(this.gameObject);
                        enemies.Add(enemy);
                        //new Vector3(gridPoint.gameObject.transform.position.x, gridPoint.gameObject.transform.position.y,-1)
                    }
                    else if (gridPointObjectToAdd == GridPointObject.Box)
                    {
                        m_GridPointList[randomRowIndex][randomColumnIndex].gridPointObject = GridPointObject.Box;
                        var box =
                            Instantiate(obj, gridPoint.gameObject.transform.position, Quaternion.identity) as GameObject;
                        box.GetComponent<Box>().gridRow = randomRowIndex;
                        box.GetComponent<Box>().gridCol = randomColumnIndex;
                        box.GetComponent<Box>().setBoardListener(this.gameObject);
                        boxesFloor.Add(box.gameObject);
                    }
                    else
                    {
                        var variable = Instantiate(obj, gridPoint.gameObject.transform.position, Quaternion.identity) as GameObject;
                        boxesFloor.Add(variable.gameObject);
                    }
                }
                else
                {
                    i--;
                }
            }
        }

        public void clearScene()
        {
            foreach(GameObject enemy in enemies)
            {
                if ( enemy != null)
                Destroy(enemy);
            }
            foreach(GameObject box in boxesFloor)
            {
                if (box != null)
                    Destroy(box);
            }
          //  String[] tagsToDelete = new[] {"Box", "EnemyBlue", "EnemyPurple", "EnemyPink", "Wall", "arrow"};

            /*for (int i = 0; i < tagsToDelete.Length; i++)
            {
                var clones = GameObject.FindGameObjectsWithTag(tagsToDelete[i].ToString());

                if (tagsToDelete[i].ToString() == "Box")
                {
                    foreach (var clone in clones)
                    {
                       
                        m_GridPointList[clone.GetComponent<Box>().gridRow][clone.GetComponent<Box>().gridCol].gridPointObject =
                            GridPointObject.Empty;
                        Destroy(clone);
                    }
                } else if (tagsToDelete[i].ToString() == "Wall")
                {
                    foreach (var clone in clones)
                    {
                        Destroy(clone);
                    }
                    for (int j = 0; j < wallPostions.Length; j++)
                    {
                        m_GridPointList[wallPostions[j]/10][wallPostions[j]%10].gridPointObject =
                            GridPointObject.Empty;
                    }
                }
                else if (tagsToDelete[i].ToString() == "arrow")
                {
                    foreach (var clone in clones)
                    {
                        Destroy(clone);
                    }
                } else
                {
                    foreach (var clone in clones)
                    {
                        Destroy(clone);
                        m_GridPointList[clone.GetComponent<Enemy>().gridRow][clone.GetComponent<Enemy>().gridCol].gridPointObject =
                            GridPointObject.Empty;
                    }
                }            
            }     */      
        }

        // This function should update the grid
        public void UpdateGridPointObject(int oldRow, int oldCol, int newRow, int newCol)
        {
            m_GridPointList[newRow][newCol].gridPointObject = m_GridPointList[oldRow][oldCol].gridPointObject;
            m_GridPointList[oldRow][oldCol].gridPointObject = GridPointObject.Empty;
        }

        // Checks if the next place is box or wall
        public bool CanMoveToGridPoint(int rowToMoveTo, int columnToMoveTo)
        {
            var gridPointObject = m_GridPointList[rowToMoveTo][columnToMoveTo].gridPointObject;
            return gridPointObject != GridPointObject.Wall &&
                   gridPointObject != GridPointObject.Box &&
                   gridPointObject != GridPointObject.Enemy &&
                   gridPointObject != GridPointObject.Bomb;
        }

        public GridPointObject GetGridPointObject(int row, int col)
        {
            return m_GridPointList[row][col].gridPointObject;
        }

        public GameObject GetGameObjectOnGridPoint(int gridRowBomb, int gridColBomb)
        {
            return m_GridPointList[gridRowBomb][gridColBomb].gameObject;
        }
        //generate boxes on random postion in board 
        public void GenerateBoxesInRandomPosition(int amountOfBoxes)
        {
            LayoutObjectAtRandom(boxTiles, GridPointObject.Box, amountOfBoxes);
        }

       
    }
}
