using UnityEngine;
using System.Collections;

public class GameMemory : MonoBehaviour {
    public static int lives = 3;

    public int returnLives()
    {
        return lives;
    }
}
