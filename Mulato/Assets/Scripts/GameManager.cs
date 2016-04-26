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

        public override void Awake()
        {
            base.Awake();
            //DontDestroyOnLoad (gameObject);
            InitGame (numberOFEnemiesInTheLevel, 0);         
        }
        void Start()
        {
            life = Memory.GetComponent<GameMemory>().returnLives();
        }

        private void InitGame (int numOfEnemies, int color) {
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
            else
            {
                onAMission = true;
                Missions.main.initMission(level, numOfEnemies, difficulty);
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
            SceneManager.LoadScene("Scene" + level);
          
        }



        public void StartGame()
        {
            level = 1;
            SceneManager.LoadScene("Scene1", LoadSceneMode.Single);

        }
    }

}
