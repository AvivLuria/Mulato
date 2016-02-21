using UnityEngine;
using System.Collections;

public class TileMap : MonoBehaviour {
	public TileType[] tileTypes;
	int[,] tiles;

	public int mapSizeX = 10;
	public int mapSizeY = 10;

	void Start() {

		GenerateMapData ();

		// Spawn the prefabs
		GenerateMapVisual();
	}

	void GenerateMapData() {
 		// Allocate our map tiles
		tiles = new int[mapSizeX, mapSizeY];

		// Initialize our map tiles floor
		for (int i = 0; i < mapSizeX; i++) {
			for (int j = 0; j < mapSizeY; j++) {
				tiles [i, j] = 0;
			}
		}

		// Making some blocks
		tiles [2, 2] = 1;
		tiles [3, 3] = 1;
		tiles [4, 4] = 1;
		tiles [5, 5] = 1;
		tiles [6, 6] = 1;
	}

	// Generate the map
	void GenerateMapVisual() {
		for (int i = 0; i < mapSizeX; i++) {
			for (int j = 0; j < mapSizeY; j++) {
				TileType tt = tileTypes [tiles [i, j]];
				Instantiate (tt.tileVisualPrefab, new Vector3 (i, j, 0), Quaternion.identity);
			}
		}
	}
}
