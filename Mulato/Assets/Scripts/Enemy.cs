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

	public Animator animator;
     
	void Update() {
		m_timeToMove += Time.deltaTime;
		if (m_timeToMove >= EnemySpeedSlow) {
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
        float random = Random.value;

        if (random < 0.25)
        {
            xDir = 1;

        }
        else if (random >= 0.25 && random < 0.5)
        {
            xDir = -1;
        }
        else if (random >= 0.5 && random < 0.75)
        {
            yDir = 1;
        }
        else
        {
            yDir = -1;
        }

		//animator.SetInteger ("X", xDir);
		//animator.SetInteger ("Y", yDir);	

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
    }

    public void activeDelay(float speed)
	{
        StartCoroutine(delayedExecution(speed));
    }

    IEnumerator delayedExecution(float speed)
    {
        yield return new WaitForSeconds(3f);
        EnemySpeedSlow = speed;
    }
}

