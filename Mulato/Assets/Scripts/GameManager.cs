using System;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : SceneSingleton<GameManager> {
        private int level = 1;

        public override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad (gameObject);
            InitGame ();
        }

        private void InitGame () {
             BoardManager.main.SetupScene(level);
        }

        public void GameOver()
        {
            enabled = false;
        }
    }
}
