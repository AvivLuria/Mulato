using System;
using System.Collections;
using System.Diagnostics;
using Assets.Scripts.Utils;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class GameManager : SceneSingleton<GameManager>
    {
        private int missionNum;
        public int difficulty;
        public int level;
        public int life = 3;
        public int numberOFEnemiesInTheLevel;
        public int[] enemiesOnTheBoard;
        public int numberOfColors;
         
        public GameObject Memory;
        private bool onAMission = false;
        public bool timer = true;
		public bool nextLevel = false;
		public Transform mainMenu, exitMenu, pauseMenu, startLevel2, startLevel3, startLevel4, startLevel5;
		public Transform[] startLevel;


        public override void Awake()
        {
            base.Awake();
            //DontDestroyOnLoad (gameObject);
            InitGame (level = 3);         
        }
        void Start()
        {
            
			startLevel = new Transform[4];
			startLevel [0] = startLevel2;
			startLevel [1] = startLevel3;
			startLevel [2] = startLevel4;
			startLevel [3] = startLevel5;
        }

        public void InitGame (int level) {
          
            
            BoardManager.main.clearScene();
            switch (level)
            {
                case (1):
                {
                    #region hard_coding_scene

                    numberOfColors = 1;
                    numberOFEnemiesInTheLevel = 3;
                    BoardManager.main.wallPostions = new int[]
                    {92, 93, 94, 95, 83, 84, 63, 64, 52, 53, 54, 55, 43, 44, 12, 13, 14, 15, 23, 24};
                    BoardManager.main.numOfLifeBoxes = 0;
                    BoardManager.main.numOfFreezeBoxes = 0;
                    BoardManager.main.numOfSpecialBombBoxes = 0;
                    BoardManager.main.numOfNomralBoxes = 0;

                    Timer.main.setTimerMission(180);
                        #endregion
                        break;
                }
                //classic play
                case (2):
                {
                    #region hard_coding_scene

                    numberOfColors = 2;
                    numberOFEnemiesInTheLevel = 4;
                    BoardManager.main.wallPostions = new int[]
                    {92, 93, 94, 95, 83, 84, 63, 64, 43, 44, 12, 13, 14, 15, 23, 24};
                    BoardManager.main.numOfLifeBoxes = 0;
                    BoardManager.main.numOfFreezeBoxes = 0;
                    BoardManager.main.numOfSpecialBombBoxes = 0;
                    BoardManager.main.numOfNomralBoxes = 0;

                    Timer.main.setTimerMission(180);
                    #endregion

                    break;
                }
                //classic play - robot 
                case (3):
                {
                    #region hard_coding_scene

                    numberOfColors = 2;
                    numberOFEnemiesInTheLevel = 6;
                    BoardManager.main.wallPostions = new int[]
                    {93, 94, 83, 84, 63, 64, 43, 44, 13, 14, 23, 24, 66, 56, 46, 61, 41, 51};
                    BoardManager.main.numOfLifeBoxes = 0;
                    BoardManager.main.numOfFreezeBoxes = 0;
                    BoardManager.main.numOfSpecialBombBoxes = 0;
                    BoardManager.main.numOfNomralBoxes = 0;

                    Timer.main.setTimerMission(180);
  
                    #endregion

                    break;
                }
                    //combo kill - 2
                case (4):
                {
                    #region hard_coding_scene

                    
                    numberOfColors = 1;
                    numberOFEnemiesInTheLevel = 6;
                    BoardManager.main.wallPostions = new int[]
                    {93, 94, 83, 84, 63, 64, 43, 44, 13, 14, 23, 24, 66, 56, 46, 61, 41, 51};
                    BoardManager.main.numOfLifeBoxes = 0;
                    BoardManager.main.numOfFreezeBoxes = 0;
                    BoardManager.main.numOfSpecialBombBoxes = 0;
                    BoardManager.main.numOfNomralBoxes = 0;

                    difficulty = 2;
                    missionNum = 2;
                        #endregion
                        //sets up the color array to choose from
                        BoardManager.main.setNumberOfColors(numberOfColors);
                        colorManager.main.init(numberOfColors);  
                        BombManager.main.setNumberOfColors(numberOfColors);
                        
                        Timer.main.setTimerMission(120);
                        break;
                }
                    //classic play - 2 colors squares
                case (5):
                {
                    #region hard_coding_scene

                    numberOfColors = 2;
                    numberOFEnemiesInTheLevel = 8;
                    BoardManager.main.wallPostions = new int[]
                    {11, 12, 13, 22, 34, 35, 36, 45, 51, 52, 53, 62, 74, 75, 76, 85};
                    BoardManager.main.numOfLifeBoxes = 1;
                    BoardManager.main.numOfFreezeBoxes = 0;
                    BoardManager.main.numOfSpecialBombBoxes = 0;
                    BoardManager.main.numOfNomralBoxes = 2;

                    Timer.main.setTimerMission(180);


                        #endregion
                    onAMission = false;
                    break;
                }
                    //classic play - 3 colors
                case (6):
                {
                    #region hard_coding_scene

                    numberOfColors = 3;
                    numberOFEnemiesInTheLevel = 6;
                    BoardManager.main.wallPostions = new int[] {92,93,82,83,64,65,54,55,32,33,22,23,25,26,15,16};
                    BoardManager.main.numOfLifeBoxes = 1;
                    BoardManager.main.numOfFreezeBoxes = 0;
                    BoardManager.main.numOfSpecialBombBoxes = 2;
                    BoardManager.main.numOfNomralBoxes = 2;

                    Timer.main.setTimerMission(180);

                    #endregion

                    onAMission = false;
                    break;

                }
                // kill the same color - lucky number 7 
                case (7):
                    {
                        #region hard_coding_scene

                        numberOfColors = 2;
                        numberOFEnemiesInTheLevel = 7;
                        BoardManager.main.wallPostions = new int[] {73,72,74,64,54,44,34,52,53,55};
                        BoardManager.main.numOfLifeBoxes = 1;
                        BoardManager.main.numOfFreezeBoxes = 0;
                        BoardManager.main.numOfSpecialBombBoxes = 1;
                        BoardManager.main.numOfNomralBoxes = 2;

                       
                        difficulty = 2;
                        missionNum = 3;
                        #endregion

                        BoardManager.main.setNumberOfColors(numberOfColors);
                        onAMission = true;
                        break;

                    }
                //disappearing enemies - 2 colors
                case (8):
                    {
                        #region hard_coding_scene

                        numberOfColors = 2;
                        numberOFEnemiesInTheLevel = 7;
                        BoardManager.main.wallPostions = new int[] { 82,72,62,52,42,32,22,83,74,65,45,34,23 };
                        BoardManager.main.numOfLifeBoxes = 1;
                        BoardManager.main.numOfFreezeBoxes = 0;
                        BoardManager.main.numOfSpecialBombBoxes = 1;
                        BoardManager.main.numOfNomralBoxes = 2;
                      
                   
                        missionNum = 5;
                        #endregion

                        BoardManager.main.setNumberOfColors(numberOfColors);
                        onAMission = true;
                        break;

                    }
                    //kill the same color - 2 colors
                case (9):
                    {
                        #region hard_coding_scene

                        numberOfColors = 2;
                        numberOFEnemiesInTheLevel = 7;
                        BoardManager.main.wallPostions = new int[] { 72,83,84,85,75,65,54,53,43,23};
                        BoardManager.main.numOfLifeBoxes = 1;
                        BoardManager.main.numOfFreezeBoxes = 0;
                        BoardManager.main.numOfSpecialBombBoxes = 1;
                        BoardManager.main.numOfNomralBoxes = 2;
                      
                        Missions.main.difficulty = 1;

                        missionNum = 3;
                        #endregion

                        
                        onAMission = true;
                        break;

                    }
            }
            enemiesOnTheBoard = new int[numberOfColors];
            BoardManager.main.setNumberOfEnemies(numberOFEnemiesInTheLevel);
          
            if (onAMission)
            {
                Missions.main.initMission(missionNum, numberOFEnemiesInTheLevel,difficulty);
            }
            else
            {

                colorManager.main.init(numberOfColors);            
                BoardManager.main.setNumberOfColors(numberOfColors);              
                BombManager.main.setNumberOfColors(numberOfColors);
            }

            BoardManager.main.SetupScene(level);
        }

        public void GameOver(int damage)
        {
            life -= damage;
            if (life <= 0)
            {
                SceneManager.LoadScene("StartScene", LoadSceneMode.Single);
            }
        }

        public void EnemyKilled()
        {
            numberOFEnemiesInTheLevel--;
            if (numberOFEnemiesInTheLevel == 0 && !onAMission)
            {
                changeLevel();
            }
            else
            {
                Missions.main.checkMissionStatus();
            } 
        }
   
        public void changeLevel()
        {
            Timer.main.setTimerMission(5);
            level++;    
            nextLevel = true;
            //startLevel [level - 2].gameObject.SetActive (nextLevel);
            StartCoroutine(delayLoadLevel());

        }
		public void StartLevel2(bool clicked){
          
        }




        public void StartGame()
        {
            level = 1;
            SceneManager.LoadScene("Scene1", LoadSceneMode.Single);

        }
		public void ExitMenu(bool clicked){
			if (clicked == true) {
				exitMenu.gameObject.SetActive (clicked);
				mainMenu.gameObject.SetActive (false);
			} else {
				exitMenu.gameObject.SetActive (clicked);
				mainMenu.gameObject.SetActive (true);
			}

		}

		public void MainMenu(){
			SceneManager.LoadScene ("StartScene", LoadSceneMode.Single);
		}

		public void PauseMenu(bool clicked){
			if (clicked == true) {
				pauseMenu.gameObject.SetActive (clicked);
				timer = false;

			} else {
				pauseMenu.gameObject.SetActive (clicked);
				timer = true;
			}

		}

		public void Exit(){

		}

        IEnumerator delayLoadLevel()
        {
            
            yield return new WaitForSeconds(3f);
            InitGame(level);
            yield return new WaitForSeconds(0.5f);
            BombManager.main.DeployBomb(50, 50, 50, 50);
            BombManager.main.DeployBomb(50, 50, 50, 50);
            BombManager.main.DeployBomb(50, 50, 50, 50);
   

        }
    }

}
