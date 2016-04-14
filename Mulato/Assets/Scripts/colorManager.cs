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
            colors[3] = red;
            colors[0] = blue;
            colors[1] = pink;
            colors[2] = yellow;
            colors[4] = green;
            colors[5] = yellow;
            numOfColors = GameManager.main.levelNumColors;
            
			curColor = BombManager.main.cur;
			cur.sprite = colors[BombManager.main.cur];
			nextColor1 = BombManager.main.next1;
			next1.sprite = colors[BombManager.main.next1];

			nextColor2 = BombManager.main.next2;
			next2.sprite = colors[BombManager.main.next2];


        }

        // Update is called once per frame
        void Update()
        {
           
			curColor = BombManager.main.cur;
			cur.sprite = colors[BombManager.main.cur];
			nextColor1 = BombManager.main.next1;
			next1.sprite = colors[BombManager.main.next1];

			nextColor2 = BombManager.main.next2;
			next2.sprite = colors[BombManager.main.next2];
              
            

        }
    }
}
