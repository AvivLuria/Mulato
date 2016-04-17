using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class Enemy : MovingObject {

	public float EnemySpeedSlow = 1.0f;
	private float m_timeToMove = 1.0f;
    public int gridRow;
    public int gridCol;
    private int countTryTimes = 10;

	void Update() {
		m_timeToMove += Time.deltaTime;
		if (m_timeToMove >= EnemySpeedSlow) {
			MoveEnemey ();
			m_timeToMove = 0f;
		}
	}		
    // TODO: Needs to be change
	public void MoveEnemey()
	{
        try {
        countTryTimes--;
        int xDir = 0;
        int yDir = 0;
	    float random = Random.value;

	    if (random < 0.25)
	    {
	        xDir = 1;
	    } else if (random >= 0.25 && random < 0.5)
	    {
	        xDir = -1;
	    } else if (random >= 0.5 && random < 0.75)
	    {
	        yDir = 1;
	    }
	    else
	    {
	        yDir = -1;
	    }
        if (AttemptMove(xDir, yDir, gridRow + yDir, gridCol + xDir, 0))
	    {
            countTryTimes = 10;
	        gridRow += yDir;
	        gridCol += xDir;
	    }
	    else
	    {
            if (countTryTimes >= 0)
                MoveEnemey();
	    }
        } catch (System.Exception e)
        {
            Debug.Log("Attemps number: " + countTryTimes);
        }
        // TODO: add avoiding the bomb
    }
}
