using UnityEngine;
using System.Collections;
using Assets.Scripts;
using JetBrains.Annotations;

public class Enemy : MovingObject {

	public float EnemySpeedSlow = 1f;
	private float m_timeToMove = 1.0f;
    private bool onDisappearingMission = false;
    private bool scaleUp = true;
    public bool onTargetMission = false;
    public int gridRow;
    public int gridCol;
    private int countTryTimes = 10;
    private bool dead;
   
    private AudioSource source;
	public Animator animator;
    public Sprite omletBlue;
    public Sprite omletPurple;
    public Sprite omletPink;

    GameObject m_curBoard;
    void Awake()
    {
        // SHIR!
       animator = GetComponent<Animator>();
        dead = false;
    }

    public void setBoardListener(GameObject i_curBoard)
    {
        m_curBoard = i_curBoard;
    }

	void Update() {
		m_timeToMove += Time.deltaTime;
		if (m_timeToMove >= EnemySpeedSlow && !dead) {
			MoveEnemey ();
		    if (Missions.main.disappearingMission)
		    {
		        disappearing();
		    } else if (onTargetMission)
		    {
		        scale();
		    }
			m_timeToMove = 0f;
		}

	}

    private void disappearing()
    {
        onDisappearingMission = !onDisappearingMission;
        this.GetComponent<Renderer>().enabled = onDisappearingMission;
      
    }

    private void scale()
    {
        
        if (scaleUp)
        {
            iTween.ScaleTo(this.gameObject, transform.localScale += new Vector3(1f, 1f, 0), 1f);
        }
        else
        {
            iTween.ScaleTo(this.gameObject, transform.localScale -= new Vector3(1f, 1f, 0), 1f);
        }
        scaleUp = !scaleUp;
    }

    // TODO: Needs to be change
    public void MoveEnemey()
    {
        countTryTimes--;
        int xDir = 0;
        int yDir = 0;
        int AnimDir;
        float random = Random.value;

        if (random < 0.25)
        {
            xDir = 1;
            AnimDir = 1;
        }
        else if (random >= 0.25 && random < 0.5)
        {
            xDir = -1;
            AnimDir = 2;
        }
        else if (random >= 0.5 && random < 0.75)
        {
            yDir = 1;
            AnimDir = 3;
        }
        else
        {
            yDir = -1;
            AnimDir = 4;
        }

		

        if (AttemptMove(xDir, yDir, gridRow + yDir, gridCol + xDir, 0, m_curBoard))
        {
            // SHIR!
           animator.SetInteger("Dir", AnimDir);
            
            countTryTimes = 10;
            gridRow += yDir;
            gridCol += xDir;
        }
        else
        {
            if (countTryTimes >= 0)
                MoveEnemey();
        }
    }

    public void activeDelay(float speed)
	{
        StartCoroutine(delayedExecution(speed));
    }

    public void activeDelay()
    {
        if (!dead)
        {
            StartCoroutine(delayedDestory());
        }
    }

    IEnumerator delayedExecution(float speed)
    {
        yield return new WaitForSeconds(3f);
        EnemySpeedSlow = speed;
    }

    IEnumerator delayedDestory()
    {
        // SHIR!
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<SpriteRenderer>().sortingLayerName = "bouns_indic";
        yield return new WaitForSeconds(0f);
        animator.SetBool("die", true);
        Destroy(gameObject.GetComponent<BoxCollider2D>());
        dead = true;      
        yield return new WaitForSeconds(1.3f);
        m_curBoard.GetComponent<BoardManager>().UpdateGridPointObject(gridRow, gridCol, gridRow, gridCol);
        Destroy(this.gameObject);
    }
}

