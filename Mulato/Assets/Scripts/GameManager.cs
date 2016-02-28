﻿using System;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : SceneSingleton<GameManager> {
        public int playerLife = 1;
        public int playerPoints = 0;
        private int level = 3;

        public override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad (gameObject);

            InitGame ();
        }

        private void InitGame () {
            try
            {
                BoardManager.main.SetupScene(level);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        public void GameOver()
        {
            enabled = false;
        }
    }
}
