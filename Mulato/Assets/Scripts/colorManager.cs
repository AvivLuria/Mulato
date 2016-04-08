using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts
{
    public class colorManager : SceneSingleton<colorManager>
    {
        public int curColor;
        public int nextColor1;
        public int nextColor2;
        public int numOfColors;

        public Image cur;
        public Image next1;
        public Image next2;

        public Sprite red;
        public Sprite yellow;
        public Sprite blue;
        public Sprite orange;
        public Sprite pink;
        public Sprite green;

        public Sprite[] colors;


        // Use this for initialization
        void Start()
        {
            colors = new Sprite[6];
            colors[0] = red;
            colors[1] = yellow;
            colors[2] = blue;
            colors[3] = orange;
            colors[4] = pink;
            colors[5] = green;
            numOfColors = GameManager.main.levelNumColors;
            curColor = chooseColor(numOfColors);
            cur.Sprite = colors[curColor];
            nextColor1 = chooseColor(numOfColors);
            next1.Sprite = colors[nextColor1];
            nextColor2 = chooseColor(numOfColors);
            next2.Sprite = colors[nextColor2];

        }

        // Update is called once per frame
        void Update()
        {
            if (BombScript.main.flag == 0)
            {
                curColor = nextColor1;
                cur.Sprite = colors[curColor];
                nextColor1 = nextColor2;
                next1.Sprite = colors[nextColor1];
                nextColor2 = chooseColor(numOfColors);
                next2.Sprite = colors[nextColor2];
                BombScript.main.flag = 1;

            }

        }
    }
}
