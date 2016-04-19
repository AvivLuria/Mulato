using System.Collections;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts
{
    public class Missions : SceneSingleton<Missions>
    {
        public int winColor;
        private int currMission;
        private int numberOfEnemies;
        public int color;
        public int timeToSet;
        public int difficulty;


        public void setTimerMission(int time)
        {
            Timer.main.myTimer = time;
        }

        public void setTimerMission()
        {
            Timer.main.myTimer = timeToSet;
        }

        public void initMission(int input_level, int input_numberOfEnemies, int input_difficulty)
        {
            difficulty = input_difficulty;
            //if (difficulty <= 0 || difficulty > 3) difficulty = 2;
            currMission = input_level;
            numberOfEnemies = input_numberOfEnemies;
            switch (input_level)
            {
                //kill up to number
                case (2):
                    {
                        //sets up the color array to choose from
                        colorManager.main.init(difficulty);
                        //sets how much to kill at once
                        BombManager.main.combo = difficulty;                        
                        BombManager.main.setNumberOfColors(difficulty);
                        //for the draw next bomb, to select all types of bomb
                        BombManager.main.onMission = true;
                        //sets up mode to check win condition
                        BombManager.main.missionMultipleKilled = true;
                        setTimerMission();
                        break;
                    }
                //kill the same color
                case (3):
                    {     
                        //choose which color to destory
                        color = UnityEngine.Random.Range(0, difficulty);
                        colorManager.main.init(difficulty);
                        BombManager.main.onMission = true;
                        BombManager.main.setNumberOfColors(difficulty);
                        setTimerMission();
                        break;
                    }
                 //Timer
                case (4):
                    {                      
                        if (difficulty > 3)
                        {
                            colorManager.main.init(2);
                            BombManager.main.setNumberOfColors(2);
                        }
                        else
                        {
                            colorManager.main.init(1);
                            BombManager.main.setNumberOfColors(1);
                        }

                        setTimerMission(60 - difficulty * 5);
                        break;
                    }
            }
            BombManager.main.onMission = true;
            BoardManager.main.setNumberOfEnemies(numberOfEnemies);
            BoardManager.main.SetupScene(input_level);
        }

        public void checkMissionStatus()
        {
            switch (currMission)
            {
                case (2):
                    {
                        if (BombManager.main.wonMissionMultipleKilled)
                        {
                            BombManager.main.missionMultipleKilled = false;
                            GameManager.main.changeLevel();
                        }
                        else if (GameManager.main.numberOFEnemiesInTheLevel == 0)
                        {
                            initMission(currMission, numberOfEnemies, color);
                        }
                        break;
                    }

                case (3):
                    {
                        if (GameManager.main.enemiesOnTheBoard[color] == 0)
                        {
                            GameManager.main.changeLevel();
                        }
                        break;
                    }
                case (4):
                    {
                        if (GameManager.main.numberOFEnemiesInTheLevel == 0)
                        {
                            setTimerMission(5);
                            StartCoroutine("DelayedExecution");
                        }
                        break;
                    }

            }
        }

        IEnumerator DelayedExecution()
        {
            yield return new WaitForSeconds(3f);
            BoardManager.main.clearScene();
            GameManager.main.numberOFEnemiesInTheLevel = numberOfEnemies;
            initMission(currMission, numberOfEnemies, difficulty + 1);
        }

    }
}
