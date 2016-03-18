using UnityEngine;
using System.Collections;
using Assets.Scripts;
using UnityEngine.Events;

public class Player : MovingObject {

	public int bombs = 1;
	public int life = 1;
	public int power = 3;
	public int speed = 1;
	public int points = 0;
    public int gridRow;
    public int gridCol;
	private float playerMovementDeylay = 0.175f;
	private float playerMovementTimeToMove = 1.0f;
	public BombManager bombPrefab;

    [System.Serializable]
    public class PlayerDeployBombEvent : UnityEvent<int, int> { }
    public PlayerDeployBombEvent onPlayerDeployedBomb = new PlayerDeployBombEvent();

	// Use this for initialization
	protected override void Start () {
		// TODO: we need to add animator here
		// We take this parameters from the gameManager and store them in the end of the level
		points = GameManager.main.playerPoints;
		life = GameManager.main.playerLife;
	    gridRow = BoardManager.main.playerPositionRow;
	    gridCol = BoardManager.main.playerPositionCol;
		base.Start ();
        /////////// ADDED TODO
        onPlayerDeployedBomb.AddListener(BombManager.main.DeployBomb);
    }


	private void OnDisable()
	{
		// Store the parameters in the gameManager
		GameManager.main.playerLife = life;
		GameManager.main.playerPoints = points;
	}
	
	// Update is called once per frame
	void Update () {
		MovePlayer();
		if (Input.GetButtonDown("Fire1")) {
			SetBomb ();
		}
	}

    private void MovePlayer()
    {
        playerMovementTimeToMove += Time.deltaTime;
        if (playerMovementTimeToMove >= playerMovementDeylay)
        {
            playerMovementTimeToMove = 0;
            int horizontal = 0;
            int vertical = 0;

            if (Mathf.Abs(JoyStick.main.inputVector.x) >= Mathf.Abs(JoyStick.main.inputVector.z))
            {
                if (JoyStick.main.inputVector.x > 0)
                {
                    horizontal = 1;
                }
                else if (JoyStick.main.inputVector.x < 0)
                {
                    horizontal = -1;
                }
            }
            else {
                if (JoyStick.main.inputVector.z > 0)
                {
                    vertical = 1;
                }
                else if (JoyStick.main.inputVector.z < 0)
                {
                    vertical = -1;
                }
            }

            if (horizontal != 0 || vertical != 0)
                if (AttemptMove(horizontal, vertical, gridRow + vertical, gridCol + horizontal,1))
                {
                    
                    gridRow += vertical;
                    gridCol += horizontal;
                    /////////// TODO: added this
                    BoardManager.main.playerPositionRow = gridRow;
                    BoardManager.main.playerPositionCol = gridCol;
                }
        }
    }

	private void SetBomb() {
        //bombPrefab.GetComponent<BombManager> ().power = power;
        //Instantiate (bombPrefab, transform.position, Quaternion.identity);
        ///////////TODO : change invoke
        onPlayerDeployedBomb.Invoke(BoardManager.main.playerPositionCol, BoardManager.main.playerPositionRow);
    }

	private void OnTriggerEnter2D(Collider2D other) {
		// TODO: we need to do player hit from the bomb
		if (other.tag == "Enemy") {
			LoseLife (1);
		} else if (other.tag == "PowerUp") {
			// TODO: pickup PowerUp
			other.gameObject.SetActive(false);
		}
	}

	private void CheckIfGameOver()
	{
		if (life <= 0)
			GameManager.main.GameOver ();
	}

	public void LoseLife (int loss) {
		// TODO: add animation of player got hit here
		life -= loss;
		CheckIfGameOver ();
	}

}
