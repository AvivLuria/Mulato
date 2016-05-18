using UnityEngine;
using System.Collections;
using Assets.Scripts;
using System;
using Assets.Scripts.Utils;

public class Box : MonoBehaviour
{
    //public Canvas heartBouns;

    private float enemySpeed;
    public int gridRow;
    public int gridCol;
    public bool isSpecialColorBombBox = false;
    public bool isMoreLifeBox = false;
    public bool isEnemyFreezeBox = false;
    private Animator animator;

    public void DestroyMe()
    {
        animator = GetComponent<Animator>();
        BoardManager.main.updateGridPointObject(gridRow,gridCol,gridRow,gridCol);
        if (isSpecialColorBombBox)
        {
            // TODO: add special effect here
            addSpecialColorBombToTheStack();
        } else if (isMoreLifeBox)
        {
            addMoreLife();
        } else if (isEnemyFreezeBox)
        {
            freezeEnemies();
        } else
        {
            animator.SetBool("empty", true);
        }

        StartCoroutine(delayedDestory());
    }

    private void freezeEnemies()
    {
        Enemy[] enemies = UnityEngine.Object.FindObjectsOfType<Enemy>();
        enemySpeed = 0;
        foreach (Enemy enemy in enemies)
        {
            enemySpeed = enemy.gameObject.GetComponent<Enemy>().EnemySpeedSlow;
            enemy.EnemySpeedSlow = float.MaxValue;
            enemy.gameObject.GetComponent<Enemy>().activeDelay(enemySpeed);
        }
        
    }

    private void addMoreLife()
    {
        GameObject.FindGameObjectWithTag("bounsHeart").transform.position = new Vector3(this.transform.position.x * 7f, this.transform.position.y * 7f, 0);
        GameObject.FindGameObjectWithTag("bounsHeart").GetComponent<Indicators>().moveHeart();
        GameManager.main.life++;
    }

    private void addSpecialColorBombToTheStack()
    {
        
        BombManager.main.forthBombColor = colorManager.colorsOptions.Special;
        colorManager.main.changeColors();
        
    }

   
    IEnumerator delayedDestory()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
    }

}
