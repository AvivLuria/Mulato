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
		public int levelNumColors = 2;
        public int life = 3;
        public int numberOFEnemiesInTheLevel;

        public override void Awake()
        {
            base.Awake();
            //DontDestroyOnLoad (gameObject);
            InitGame ();
        }

        private void InitGame () {
            switch (level)
            {
                case 1:
                    {
                        BoardManager.main.setNumberOfEnemies(numberOFEnemiesInTheLevel);
                        BoardManager.main.SetupScene(level);
                        break;
                    }
                case 2:
                    {
                        BoardManager.main.setNumberOfEnemies(numberOFEnemiesInTheLevel);
                        BoardManager.main.SetupScene(level);
                        break;
                    }
                case 3:
                    {
                        BoardManager.main.setNumberOfEnemies(numberOFEnemiesInTheLevel);
                        BoardManager.main.SetupScene(level);
                        break;
                    }
                default:
                    {
                        break;
                    }
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
            if (numberOFEnemiesInTheLevel == 0)
            {
                changeLevel();
            }
        }

        public void changeLevel()
        {
            level++;
            SceneManager.LoadScene("Scene" + level, LoadSceneMode.Single);
          
        }

        public void StartGame()
        {
            level = 1;
            SceneManager.LoadScene("Scene1", LoadSceneMode.Single);

        }
    }

}
