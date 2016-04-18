using Assets.Scripts.Utils;
using System;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts
{
    public class colorManager : SceneSingleton<colorManager>
    {

        public static class colorsOptions
        {
            public const int Blue = 0;
            public const int Pink = 1;
            public const int Purple = 2;
        }

        public int currentBombColor;
        public int nextBombColor;
        public int thirdBombColor;

        public int levelNumberOfColors;

        public Image currentBombColorImage;
        public Image nextBombColorImage;
        public Image thirdBombColorImage;


        public GameObject blue;
        public GameObject pink;
        public GameObject purple;

        public GameObject[] currentColorPosibilities;

        // Use this for initialization
        void Start()
        {
            currentColorPosibilities = new GameObject[levelNumberOfColors];
            setBombColorPosibilities();
            
        }

        public void changeColors()
        {
            currentBombColor = BombManager.main.currentBombColor;
            currentBombColorImage.sprite = currentColorPosibilities[BombManager.main.currentBombColor].GetComponent<SpriteRenderer>().sprite;

            nextBombColor = BombManager.main.nextBombColor;
            nextBombColorImage.sprite = currentColorPosibilities[BombManager.main.nextBombColor].GetComponent<SpriteRenderer>().sprite;

            thirdBombColor = BombManager.main.thirdBombColor;
            thirdBombColorImage.sprite = currentColorPosibilities[BombManager.main.thirdBombColor].GetComponent<SpriteRenderer>().sprite;
        }

        public void setBombColorPosibilities()
        {
            for (int i = 0; i < colorManager.main.levelNumberOfColors; i++)
            {
                switch (i) {
                    case colorsOptions.Blue:
                        {
                            currentColorPosibilities[i] = blue;
                            break;
                        }
                    case colorsOptions.Pink:
                        {
                            currentColorPosibilities[i] = pink;
                            break;
                        }
                    case colorsOptions.Purple:
                        {
                            currentColorPosibilities[i] = purple;
                            break;
                        }
                }
            }
        }
    }
}
