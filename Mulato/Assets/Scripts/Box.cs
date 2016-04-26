using UnityEngine;
using System.Collections;
using Assets.Scripts;
using System;
using Assets.Scripts.Utils;

public class Box : MonoBehaviour
{

    private float enemySpeed;
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

     //   Destroy(gameObject);
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
        GameManager.main.life++;
    }

    private void addSpecialColorBombToTheStack()
    {
        
        BombManager.main.forthBombColor = colorManager.colorsOptions.Special;
        
    }

    
}
