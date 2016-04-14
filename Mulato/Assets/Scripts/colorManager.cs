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
            colors[1] = blue;
            colors[2] = pink;
            colors[3] = yellow;
            colors[4] = green;
            colors[5] = yellow;
            numOfColors = GameManager.main.levelNumColors;
            
			curColor = BombManager.main.cur;
            cur.sprite = colors[curColor];
			nextColor1 = BombManager.main.next1;
            next1.sprite = colors[nextColor1];
       
			nextColor2 = BombManager.main.next2;
			next2.sprite = colors[nextColor2];

        }

        // Update is called once per frame
        void Update()
        {
           
                curColor = nextColor1;
                
                cur.sprite = colors[curColor];
                nextColor1 = nextColor2;
                next1.sprite = colors[nextColor1];
				nextColor2 = BombManager.main.next2;
				next2.sprite = colors [nextColor2];
               
              
            

        }
    }
}
