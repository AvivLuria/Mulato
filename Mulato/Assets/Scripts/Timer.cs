using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.Utils;

namespace Assets.Scripts { 
    public class Timer : SceneSingleton<Timer> {

        public float myTimer = 200;
        public Text timerText;

	    // Use this for initialization
	    void Start () {
            timerText = GetComponent<Text>();
	    }
	
	    // Update is called once per frame
	    void Update () {
            myTimer -= Time.deltaTime;
            float minute = Mathf.Floor(myTimer / 60);
            float seconds = Mathf.RoundToInt(myTimer % 60);
        
            timerText.text = minute.ToString() + ":" + (seconds < 10 ? "0" + seconds.ToString() : seconds.ToString());
            if (myTimer <= 0)
            {
                myTimer = 0;
                GameManager.main.GameOver(int.MaxValue);
            }
	    }
    }
}