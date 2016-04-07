using System;
using System.Collections;
using Assets.Scripts.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class GameManager : SceneSingleton<GameManager> {
        public int level = 1;
		public int levelNumColors = 2;

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
