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
            public const int Special = -1;
        }

        public int currentBombColor;
        public int nextBombColor;
        public int thirdBombColor;
        private int numOfColors;
      
        public Image currentBombColorImage;
        public Image nextBombColorImage;
        public Image thirdBombColorImage;


        public GameObject blue;
        public GameObject pink;
        public GameObject purple;

        public GameObject[] currentColorPosibilities;

        // Use this for initialization
        public void init(int levelNumberOfColors)
        {
            numOfColors = levelNumberOfColors;
            currentColorPosibilities = new GameObject[levelNumberOfColors];
            setBombColorPosibilities();
        }

        public void changeColors()
        {
            currentBombColor = BombManager.main.currentBombColor;
            nextBombColor = BombManager.main.nextBombColor;
            thirdBombColor = BombManager.main.thirdBombColor;
            
            if (currentBombColor == colorsOptions.Special)
            {
                currentBombColorImage.sprite = BombManager.main.SpecialBomb.GetComponent<SpriteRenderer>().sprite;
            } else
            {
                currentBombColorImage.sprite = currentColorPosibilities[BombManager.main.currentBombColor].GetComponent<SpriteRenderer>().sprite;
            }
            if (nextBombColor == colorsOptions.Special)
            {
                nextBombColorImage.sprite = BombManager.main.SpecialBomb.GetComponent<SpriteRenderer>().sprite;
            } else
            {
                nextBombColorImage.sprite = currentColorPosibilities[BombManager.main.nextBombColor].GetComponent<SpriteRenderer>().sprite;
            }
            if (thirdBombColor == colorsOptions.Special)
            {
                thirdBombColorImage.sprite = BombManager.main.SpecialBomb.GetComponent<SpriteRenderer>().sprite;
            } else
            {
                thirdBombColorImage.sprite = currentColorPosibilities[BombManager.main.thirdBombColor].GetComponent<SpriteRenderer>().sprite;
            }

        }

        public void setBombColorPosibilities()
        {
            for (int i = 0; i < numOfColors; i++)
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
