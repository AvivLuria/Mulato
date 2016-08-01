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
        private bool createBoxes = false;
        private bool missionWon = false;
        GameObject m_CurrBoard;

        public void initMission(int input_level, int input_numberOfEnemies, int input_difficulty)
        {
            m_CurrBoard = GameManager.main.m_BoardManager;
            difficulty = input_difficulty;
            currMission = input_level;
            numberOfEnemies = input_numberOfEnemies;
            switch (input_level)
            {
                //kill up to number
                case (2):
                    {
                        //2 <= difficulty < 4
                        //sets how much to kill at once
                        BombManager.main.numOfKillesToWinComboMission = difficulty;
                        missionWon = false;
                        //for the draw next bomb, to select all types of bomb
                        BombManager.main.onMission = true;
                        //sets up mode to check win condition
                        BombManager.main.missionMultipleKilled = true;                      
                      
                        break;
                    }
                //kill the same color
                case (3):
                    {
                        //choose which color to destory
                        color = UnityEngine.Random.Range(0, difficulty);

                        m_CurrBoard.GetComponent<BoardManager>(). m_NumOfColors = difficulty;                     
                        BombManager.main.startIndexBombColor = color;
                        BombManager.main.setNumberOfColors(color + 1);
                        BombManager.main.onMission = true;
                        
                        break;
                    }
                 //Timer
                /*case (4):
                    {
                        if (difficulty > 3)
                        {                           
                            BombManager.main.setNumberOfColors(2);
                        }
                        else
                        {                         
                            BombManager.main.setNumberOfColors(1);
                        }
                        BombManager.main.onMission = true;
                        BoardManager.main.setNumberOfEnemies(numberOfEnemies);
                        BoardManager.main.SetupScene(input_level);
                        Timer.main.setTimerMission(60 - difficulty * 5);
                        break;
                    }*/
                //disappearing enemies
                case (5):
                    {
                        if (difficulty <= 0 || difficulty > 3) difficulty = 2;

                        m_CurrBoard.GetComponent<BoardManager>().m_NumOfColors = (difficulty);
                        BombManager.main.setNumberOfColors(difficulty);                        
                        BombManager.main.onMission = true;
                        disappearingMission = true;
                        break;
                    }
                //survival
                case (6):
                    {                                            
                        BombManager.main.missionSurvival = true;
                        BombManager.main.setNumberOfColors(difficulty);                 
                        BombManager.main.onMission = true;
                       
                       // Timer.main.setTimerMission(35 - difficulty * 5);
                        break;
                    }
                    //boxes appears 
                case (7):
                    {                  
                        BombManager.main.setNumberOfColors(difficulty);
                        BombManager.main.onMission = true;
                        createBoxes = true;
                        StartCoroutine(DelayCreateBoxes(difficulty));
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
                            missionWon = true;
                            BombManager.main.missionMultipleKilled = false;
                            GameManager.main.stillPlaying = false;
                            GameManager.main.timer = false;
                            GameManager.main.changeLevel(3);
                        }
                        else if (GameManager.main.numberOFEnemiesInTheLevel < 2 && !missionWon)
                        {
                            GameManager.main.GameOver(20);
                        }

                        break;
                    }

                case (3):
                    {
                        if (GameManager.main.enemiesOnTheBoard[color] == 0)
                        {
                            GameManager.main.timer = false;
                            GameManager.main.stillPlaying = false;
                            BombManager.main.startIndexBombColor = 0;
                            GameManager.main.changeLevel(3);
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
                            GameManager.main.timer = false;
                            GameManager.main.stillPlaying = false;
                            disappearingMission = false;
                            GameManager.main.changeLevel(3);
                        }
                        break;
                    }
                case (6):
                    {
                        if (GameManager.main.numberOFEnemiesInTheLevel <= 1)
                        {
                            GameManager.main.numberOFEnemiesInTheLevel = numberOfEnemies + 1;
                            GameManager.main.life += 1;
                            m_CurrBoard.GetComponent<BoardManager>().m_NumOfEnemies = (numberOfEnemies);
                         //   m_CurrBoard.SetUpEnemiesOnTheBoard();
                        }
                        break;
                    }
                case (7):
                    {
                        if (GameManager.main.enemiesOnTheBoard[color] == 0)
                        {
                            createBoxes = false;
                            GameManager.main.changeLevel(3);
                        }
                        break;
                    }
            }
        }


        IEnumerator DelayedExecution()
        {
            yield return new WaitForSeconds(3f);
        //    BoardManager.main.clearScene();
            GameManager.main.numberOFEnemiesInTheLevel = numberOfEnemies;
            initMission(currMission, numberOfEnemies, difficulty + 1);
        }

        IEnumerator DelayCreateBoxes(int i_difficulty)
        {
            while (createBoxes)
            {
                yield return new WaitForSeconds(2.5f);
                m_CurrBoard.GetComponent<BoardManager>().GenerateBoxesInRandomPosition(i_difficulty);
            }
        }
    }
}
