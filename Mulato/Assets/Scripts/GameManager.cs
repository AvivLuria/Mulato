using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public BoardManager boardScript;
	public int playerLife = 1;
	public int playerPoints = 0;
	private int level = 3;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);

		InitGame ();
	}

	void InitGame () {
		BoardManager.main.SetupScene(level);
	}

	public void GameOver()
	{
		enabled = false;
	}
		
	
	// Update is called once per frame
	void Update () {
	
	}
}
