using System;
using System.Collections;
using Assets.Scripts.Utils;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class GameManager : SceneSingleton<GameManager> {
        public int level = 1;
		public int levelNumColors = 2;
        public int life = 3;

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

            }
        }

        public void changeLevel()
        {
            level++;
            InitGame();
        }

        public void StartGame()
        {
            level = 1;
            SceneManager.LoadScene("Scene1", LoadSceneMode.Single);
            
            //InitGame();
        }
    }

}
