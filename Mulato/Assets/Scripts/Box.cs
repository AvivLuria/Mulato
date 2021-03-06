﻿using UnityEngine;
using System.Collections;
using Assets.Scripts;
using System;
using Assets.Scripts.Utils;
using UnityEngine.UI;

public class Box : MonoBehaviour
{
    public BombManagerBoxMovingObjectsUpdateBombGridPoint m_UpdateBombGridPointBox;
    //public Canvas heartBouns;
    private bool active = true;
    private float enemySpeed;
    public int gridRow;
    public int gridCol;
    public bool isSpecialColorBombBox = false;
    public bool isMoreLifeBox = false;
    public bool isEnemyFreezeBox = false;
    private Animator animator;
    private AudioSource source;
    public AudioClip BoxSound;
    public GameObject heart;
    public GameObject special;
    public GameObject m_curBoard;
    public GameObject roniSnowFlakes;

    public void setBoardListener(GameObject i_curBoard)
    {
        m_curBoard = i_curBoard;
    }
    

    public void DestroyMe()
    {
        if (active)
        {
            BombManagerBoxMovingObjectsUpdateBombGridPoint m_UpdateBombGridPointBox = new BombManagerBoxMovingObjectsUpdateBombGridPoint(m_curBoard.GetComponent<BoardManager>().UpdateGridPointObject);
            
            active = !active;
            animator = GetComponent<Animator>();       
            m_UpdateBombGridPointBox(gridRow, gridCol, gridRow, gridCol);
            if (isSpecialColorBombBox)
            {
                // TODO: add special effect here
                animator.SetBool("multyBomb", true);
                addSpecialColorBombToTheStack();
            }
            else if (isMoreLifeBox)
            {
                animator.SetBool("heart", true);
                addMoreLife();
            }
            else if (isEnemyFreezeBox)
            {
                
                animator.SetBool("freeze", true);
                freezeEnemies();
            }
            else
            {
                animator.SetBool("empty", true);
            }

            StartCoroutine(delayedDestory());
        }
    }

    private void freezeEnemies()
    {
        Enemy[] enemies = UnityEngine.Object.FindObjectsOfType<Enemy>();
        GameObject roni = Instantiate(roniSnowFlakes, new Vector3(5f, -5f, 0), Quaternion.identity) as GameObject;
        
        enemySpeed = 0;
        foreach (Enemy enemy in enemies)
        {
            enemySpeed = enemy.gameObject.GetComponent<Enemy>().EnemySpeedSlow;
            enemy.EnemySpeedSlow = float.MaxValue;
            enemy.gameObject.GetComponent<Enemy>().activeDelay(enemySpeed);
        }
        Destroy(roni,2f);
    }

    private void addMoreLife()
    {
        //  GameObject.FindGameObjectWithTag("bounsHeart").transform.position = bar.transform.position;
        Instantiate(heart, this.transform.position, Quaternion.identity);
     //   GameObject.FindGameObjectWithTag("bounsHeart").GetComponent<Indicators>().moveHeart();
        GameManager.main.life++;
    }

    private void addSpecialColorBombToTheStack()
    {
        Instantiate(special, this.transform.position, Quaternion.identity);
        BombManager.main.forthBombColor = colorManager.colorsOptions.Special;
        colorManager.main.changeColors();
        
    }

        IEnumerator delayedDestory()
    {
        yield return new WaitForSeconds(0.5f);
        source = GetComponent<AudioSource>();
        source.PlayOneShot(BoxSound);
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }

}
