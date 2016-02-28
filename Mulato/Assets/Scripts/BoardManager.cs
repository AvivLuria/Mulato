using System;
using System.Collections.Generic;
using Assets.Scripts.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class BoardManager : SceneSingleton<BoardManager> {

        public enum GridPointObject {
            Empty,
            Wall,
            Box,
            Enemy,
            Player,
            PowerUp,
            Bomb
        }
		
        public class GridPoint {
            public int row;
            public int column;
            public GridPointObject gridPointObject;
            public Transform gridPointTransform;
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
		
        public int columns = 9;
        public int rows = 10;
        public Count wallCount = new Count (5, 9);
        public Count powerUpsCount = new Count (1, 5);
        public GameObject floorTiles;
        public GameObject wallTiles;
        public GameObject boxTiles;
        public GameObject powerUpsTiles;
        public GameObject enemyTiles;
        private List<GridPoint[]> m_board;

        private Transform boardHolder;
        private List <Vector3> gridPositions = new List<Vector3> ();

        public void InitializeList()
        {
            gridPositions.Clear ();

            for (var x = 1; x < columns - 1; x++) {
                for (var y = 1; y < rows - 1; y++) {
                    gridPositions.Add (new Vector3 (x, y, 0f));
                }
            }
        }

        private void BoardSetup () 
        {
            boardHolder = new GameObject ("Board").transform;
            m_board = new List<GridPoint[]>();

            for (var i = 0; i < rows; i++) {
                m_board.Add(new GridPoint[columns]);
            }

            m_board[1][1] = new GridPoint()
            {
                row = 1,
                column = 1,
                gridPointObject = GridPointObject.Player
            };

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

        public GridPoint RandomPosition()
        {
            var randomRowIndex = Random.Range (0, m_board.Count);
            var randomColumnIndex = Random.Range(0, m_board[0].Length);
            return new GridPoint {row = randomRowIndex, column = randomColumnIndex};
        }

        public void LayoutObjectAtRandom(GameObject obj, GridPointObject gridPointObjectToAdd, int minimum, int maximum)
        {
            var objectCount = Random.Range (minimum, maximum + 1);

            for (int i = 0; i < objectCount; i++) {
                var randomPosition = RandomPosition();
                var gridPoint = m_board[randomPosition.row][randomPosition.column];
                var gridPointObject = gridPoint.gridPointObject;
                if (gridPointObject != GridPointObject.Wall && gridPointObject != GridPointObject.Player)
                {
                    gridPoint.gridPointObject = gridPointObjectToAdd;
                    gridPoint.gameObject = Instantiate(obj, gridPoint.gameObject.transform.position, Quaternion.identity) as GameObject;
                }
            }
        }

        public void SetupScene(int level) {
            //Creates the outer walls and floor.
            BoardSetup ();

            //Reset our list of gridpositions.
            InitializeList ();

            //Instantiate a random number of boxes tiles based on minimum and maximum, at randomized positions.
            LayoutObjectAtRandom (boxTiles, GridPointObject.Box, wallCount.minimum, wallCount.maximum);

            //Instantiate a random number of powerUps tiles based on minimum and maximum, at randomized positions.
            LayoutObjectAtRandom (powerUpsTiles, GridPointObject.PowerUp, powerUpsCount.minimum, powerUpsCount.maximum);

            //Determine number of enemies based on current level number, based on a logarithmic progression
            var enemyCount = (int)Mathf.Log(level, 2f);

            //Instantiate a random number of enemies based on minimum and maximum, at randomized positions.
            LayoutObjectAtRandom (enemyTiles, GridPointObject.Enemy, enemyCount, enemyCount);
        }

        public bool CanMoveToGridPoint(int rowToMoveTo, int columnToMoveTo)
        {
            var gridPointObject = m_board[rowToMoveTo][columnToMoveTo].gridPointObject;
            return gridPointObject != GridPointObject.Wall && gridPointObject != GridPointObject.Box;
        }
    }
}
