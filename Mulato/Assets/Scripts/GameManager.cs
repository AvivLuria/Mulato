using System;
using System.Collections;
using Assets.Scripts.Utils;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class GameManager : SceneSingleton<GameManager> {
        public int level;
        public int life = 3;
        public int numberOFEnemiesInTheLevel;
        public int[] enemiesOnTheBoard;
        public int numberOfColors;
        private bool onAMission = false;
        public int difficulty;
        public GameObject Memory;
		public bool timer = true;
		public bool nextLevel = false;
		public Transform mainMenu, exitMenu, pauseMenu, startLevel2, startLevel3, startLevel4, startLevel5;
		public Transform[] startLevel;


        public override void Awake()
        {
            base.Awake();
            //DontDestroyOnLoad (gameObject);
            InitGame (numberOFEnemiesInTheLevel);         
        }
        void Start()
        {
            life = Memory.GetComponent<GameMemory>().returnLives();
			startLevel = new Transform[4];
			startLevel [0] = startLevel2;
			startLevel [1] = startLevel3;
			startLevel [2] = startLevel4;
			startLevel [3] = startLevel5;
        }

        private void InitGame (int numOfEnemies) {
            //TODO : Fix start level
            enemiesOnTheBoard = new int[numberOfColors];
            if (level == 1)
            {
                colorManager.main.init(numberOfColors);
                BoardManager.main.setNumberOfEnemies(numberOFEnemiesInTheLevel);
                BoardManager.main.setNumberOfColors(numberOfColors);
                BombManager.main.setNumberOfColors(numberOfColors);
                BoardManager.main.SetupScene(level);
            }
            else if (level != 0)
            {
                onAMission = true;
                Missions.main.initMission(level, numOfEnemies, difficulty);
            } else
            {
                colorManager.main.init(numberOfColors);
                BoardManager.main.setNumberOfEnemies(numberOFEnemiesInTheLevel);
                BoardManager.main.setNumberOfColors(numberOfColors);
                BombManager.main.setNumberOfColors(numberOfColors);
            }
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
            level++;
			nextLevel = true;
			startLevel [level - 1].gameObject.SetActive (nextLevel);

            
          
        }
		public void StartLevel2(bool clicked){
			if (clicked == true) {
				
				SceneManager.LoadScene ("Scene" + level, LoadSceneMode.Single);
			}
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
				
			
    }

}
