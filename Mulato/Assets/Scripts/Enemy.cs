using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class Enemy : MovingObject {

	public int playerDamage = 1;
	private Transform target;
	public float delaytime = 1.0f;
	private float timeToMove = 1.0f;
    public int gridRow;
    public int gridCol;

	// Use this for initialization
	protected override void Start () {
		// TODO: add animator
		target = GameObject.FindGameObjectWithTag("Player").transform;
		base.Start ();
	}

	void Update() {
		timeToMove += Time.deltaTime;
		if (timeToMove >= delaytime) {
			MoveEnemey ();
			timeToMove = 0;
		}
	}
		

	public void MoveEnemey()
	{
		//Declare variables for X and Y axis move directions, these range from -1 to 1.
		//These values allow us to choose between the cardinal directions: up, down, left and right.
		int xDir = 0;
		int yDir = 0;

		//If the difference in positions is approximately zero (Epsilon) do the following:
		if (Mathf.Abs (target.position.x - transform.position.x) < float.Epsilon) {
			//If the column coordinate of the target's (player) position is greater than the column coordinate of this enemy's position set column direction 1 (to move up). If not, set it to -1 (to move down).
			yDir = target.position.y > transform.position.y ? 1 : -1;

		}
		//If the difference in positions is not approximately zero (Epsilon) do the following:
		else {
			//Check if target row position is greater than enemy's row position, if so set row direction to 1 (move right), if not set to -1 (move left).
			xDir = target.position.x > transform.position.x ? 1 : -1;

		}
		//Call the AttemptMove function and pass in the generic parameter Player,
		// because Enemy is moving and expecting to potentially encounter a Player
		
        if (AttemptMove(xDir, yDir, gridRow + yDir, gridCol + xDir,0))
        {
           
            gridRow += yDir;
            gridCol += xDir;
        }
        // TODO: add avoiding the bomb
    }
}
