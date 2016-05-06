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
        public bool disappearingMission = false;


       

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
                        colorManager.main.init(GameManager.main.numberOfColors);
                        //sets how much to kill at once
                        BombManager.main.comboMission = difficulty;                        
                        BombManager.main.setNumberOfColors(GameManager.main.numberOfColors);
                        //for the draw next bomb, to select all types of bomb
                        BombManager.main.onMission = true;
                        //sets up mode to check win condition
                        BombManager.main.missionMultipleKilled = true;
                        Timer.main.setTimerMission(timeToSet);
                      
                        break;
                    }
                //kill the same color
                case (3):
                    {
                        //choose which color to destory

                        color = UnityEngine.Random.Range(0, difficulty);
                        colorManager.main.init(difficulty + 1);
                        BombManager.main.startIndexBombColor = color;
                        BombManager.main.setNumberOfColors(difficulty + 1);
                        BombManager.main.onMission = true;
                        Timer.main.setTimerMission(timeToSet);
                        BoardManager.main.setNumberOfColors(difficulty);
                        
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
                        BombManager.main.onMission = true;
                        BoardManager.main.setNumberOfEnemies(numberOfEnemies);
                        BoardManager.main.SetupScene(input_level);
                        Timer.main.setTimerMission(60 - difficulty * 5);
                        break;
                    }
                //disappearing enemies
                case (5):
                    {
                        if (difficulty <= 0 || difficulty > 3) difficulty = 2;
                        colorManager.main.init(difficulty);
                        BoardManager.main.setNumberOfColors(difficulty);
                        BombManager.main.setNumberOfColors(difficulty);
                        disappearingMission = true;
                        Timer.main.setTimerMission(timeToSet);
                        BombManager.main.onMission = true;
                        BoardManager.main.setNumberOfEnemies(numberOfEnemies);
                        BoardManager.main.SetupScene(input_level);
                        break;
                    }
                //survival
                case (6):
                    {                        
                        colorManager.main.init(difficulty);
                        BombManager.main.missionSurvival = true;
                        BombManager.main.setNumberOfColors(difficulty);                 
                        BombManager.main.onMission = true;
                        BoardManager.main.setNumberOfColors(difficulty);
                        BoardManager.main.setNumberOfEnemies(numberOfEnemies);
                        BoardManager.main.SetupScene(input_level);
                        Timer.main.setTimerMission(35 - difficulty * 5);
                        break;
                    }
            }
        
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
                        else if (GameManager.main.numberOFEnemiesInTheLevel < difficulty)
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
                            Timer.main.setTimerMission(5);
                            StartCoroutine("DelayedExecution");
                        }
                        break;
                    }
                case (5):
                    {
                        if (GameManager.main.numberOFEnemiesInTheLevel == 0)
                        {
                            disappearingMission = false;
                            GameManager.main.changeLevel();
                        }
                        break;
                    }
                case (6):
                    {
                        if (GameManager.main.numberOFEnemiesInTheLevel <= 1)
                        {
                            GameManager.main.numberOFEnemiesInTheLevel = numberOfEnemies + 1;
                            GameManager.main.life += 1;
                            BoardManager.main.setNumberOfEnemies(numberOfEnemies);
                            BoardManager.main.setEnemiesOnTheBoard();
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
