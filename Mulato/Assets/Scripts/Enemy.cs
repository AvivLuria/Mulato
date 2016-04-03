using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class Enemy : MovingObject {

	private float m_delaytime = 1.0f;
	private float m_timeToMove = 1.0f;
    public int gridRow;
    public int gridCol;

	void Update() {
		m_timeToMove += Time.deltaTime;
		if (m_timeToMove >= m_delaytime) {
			MoveEnemey ();
			m_timeToMove = 0f;
		}
	}		
    // TODO: Needs to be change
	public void MoveEnemey()
	{
        float randomX = Random.Range(0, BoardManager.main.rows);
        float randomY = Random.Range(0, BoardManager.main.columns);
        Vector2 randomXY = new Vector2(randomX, randomY);
        //Declare variables for X and Y axis move directions, these range from -1 to 1.
        //These values allow us to choose between the cardinal directions: up, down, left and right.
        int xDir = 0;
		int yDir = 0;
		//If the difference in positions is approximately zero (Epsilon) do the following:
		if (Mathf.Abs (randomXY.x -  transform.position.x) < float.Epsilon) {
			//If the column coordinate of the target's (player) position is greater than the column coordinate of this enemy's position set column direction 1 (to move up). If not, set it to -1 (to move down).
			yDir = randomXY.y > transform.position.y ? 1 : -1;
		}
		//If the difference in positions is not approximately zero (Epsilon) do the following:
		else {
			//Check if target row position is greater than enemy's row position, if so set row direction to 1 (move right), if not set to -1 (move left).
			xDir = randomXY.x > transform.position.x ? 1 : -1;
		}
		//Call the AttemptMove function and pass in the generic parameter Player,
		// because Enemy is moving and expecting to potentially encounter a Player		
	   
        if(AttemptMove(xDir, yDir, gridRow + yDir, gridCol + xDir,0))
        {           
            gridRow += yDir;
            gridCol += xDir;
        }
        // TODO: add avoiding the bomb
    }
}
