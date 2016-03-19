using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class Box : MonoBehaviour {

    public int gridRow;
    public int gridCol;

    public void DestroyMe()
    {
            BoardManager.main.updateMovementPosition(gridRow,gridCol,gridRow,gridCol);
            Destroy(gameObject);
    }
}
