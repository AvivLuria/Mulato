using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {
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


	public int columns = 8;
	public int rows = 8;
	public Count wallCount = new Count (5, 9);
	public Count powerUpsCount = new Count (1, 5);
	public GameObject floorTiles;
	public GameObject wallTiles;
	public GameObject boxTiles;
	public GameObject powerUpsTiles;
	public GameObject enemyTiles;

	private Transform boardHolder;
	private List <Vector3> gridPositions = new List<Vector3> ();

	void InitializeList()
	{
		gridPositions.Clear ();

		for (int x = 1; x < columns - 1; x++) {
			for (int y = 1; y < rows - 1; y++) {
				gridPositions.Add (new Vector3 (x, y, 0f));
			}
		}
	}

	void BoardSetup () 
	{
		boardHolder = new GameObject ("Board").transform;

		for (int x = -1; x < columns + 1; x++) {
			for (int y = -1; y < rows + 1; y++) {
				
				GameObject toInstantiate = floorTiles;
				// If a wall
				if (x == -1 || x == columns || y == -1 || y == rows) {
					toInstantiate = wallTiles;
				}

				GameObject instance = Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;

				instance.transform.SetParent (boardHolder);

			}
		}
	}

	Vector3 RandomPosition()
	{
		int randomIndex = Random.Range (0, gridPositions.Count);
		Vector3 randomPosition = gridPositions [randomIndex];
		gridPositions.RemoveAt (randomIndex);
		return randomPosition;
	}

	void LayoutObjectAtRandom(GameObject obj, int minimum, int maximum)
	{
		int objectCount = Random.Range (minimum, maximum + 1);

		for (int i = 0; i < objectCount; i++) {
			Vector3 randomPosition = RandomPosition ();
			Instantiate (obj, randomPosition, Quaternion.identity);
		}
	}

	public void SetupScene(int level) {
		//Creates the outer walls and floor.
		BoardSetup ();

		//Reset our list of gridpositions.
		InitializeList ();

		//Instantiate a random number of boxes tiles based on minimum and maximum, at randomized positions.
		LayoutObjectAtRandom (boxTiles, wallCount.minimum, wallCount.maximum);

		//Instantiate a random number of powerUps tiles based on minimum and maximum, at randomized positions.
		LayoutObjectAtRandom (powerUpsTiles, powerUpsCount.minimum, powerUpsCount.maximum);

		//Determine number of enemies based on current level number, based on a logarithmic progression
		int enemyCount = (int)Mathf.Log(level, 2f);

		//Instantiate a random number of enemies based on minimum and maximum, at randomized positions.
		LayoutObjectAtRandom (enemyTiles, enemyCount, enemyCount);
	}
}
