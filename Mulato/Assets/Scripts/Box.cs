using UnityEngine;
using System.Collections;
using Assets.Scripts;
using System;

public class Box : MonoBehaviour {

    public int gridRow;
    public int gridCol;
    public bool isSpecialColorBombBox = false;
    public bool isMoreLifeBox = false;
    public bool isEnemyFreezeBox = false;

    public void DestroyMe()
    {
        BoardManager.main.updateMovementPosition(gridRow,gridCol,gridRow,gridCol);
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
        }

        Destroy(gameObject);
    }

    private void freezeEnemies()
    {
        Enemy[] enemies = UnityEngine.Object.FindObjectsOfType<Enemy>();
        float temp = 0;
        foreach (Enemy enemy in enemies)
        {
            temp = enemy.gameObject.GetComponent<Enemy>().EnemySpeedSlow;
            enemy.EnemySpeedSlow = float.MaxValue;
            
        }

        StartCoroutine(DelayedExecution(temp));

        
    }

    private void returnFromFreezing(float enemiesMovement)
    {
        Enemy[] enemies = UnityEngine.Object.FindObjectsOfType<Enemy>();

        foreach (Enemy enemy in enemies)
        {

            enemy.EnemySpeedSlow = enemiesMovement;

        }
    }

    private void addMoreLife()
    {
        GameManager.main.life++;
    }

    private void addSpecialColorBombToTheStack()
    {
        
        BombManager.main.forthBombColor = colorManager.colorsOptions.Special;
        
    }

    IEnumerator DelayedExecution(float enemiesMovement)
    {
        yield return new WaitForSeconds(3f);
        returnFromFreezing(enemiesMovement);
        yield return new WaitForSeconds(1f);
    }
}
