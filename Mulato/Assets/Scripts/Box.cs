using UnityEngine;
using System.Collections;
using Assets.Scripts;
using System;

public class Box : MonoBehaviour {

    public int gridRow;
    public int gridCol;
    public bool isSpecialColorBombBox;
    

    public void DestroyMe()
    {
        BoardManager.main.updateMovementPosition(gridRow,gridCol,gridRow,gridCol);
        if (isSpecialColorBombBox)
        {
            // TODO: add special effect here
            addSpecialColorBombToTheStack();
        }
        Destroy(gameObject);
    }

    private void addSpecialColorBombToTheStack()
    {
        BombManager.main.bombs.Enqueue(BombManager.main.SpecialBomb);
        BombManager.main.thirdBombColor = colorManager.colorsOptions.Special;
    }
}
