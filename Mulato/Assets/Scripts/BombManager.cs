using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts
{
    public class BombManager : SceneSingleton<BombManager> {
        public float explosionTime = 3;
        public int power = 1;

        public LayerMask wallLayer;

        public Player player;
        public GameObject bomb;

        void Start() {
           // SetTheBomb (explosionTime, power);
            player.onPlayerDeployedBomb.AddListener(DeployBomb);
        }

        public void DeployBomb(int row, int column)
        {
            var curBomb = Instantiate(bomb, player.transform.position, Quaternion.identity) as GameObject;
            BoardManager.main.setFireOn(row, column, player.power);
        }

        private void Explode(GameObject curBomb, int row, int column)
        {
            var hit1 = Physics2D.Linecast(curBomb.transform.position,
                new Vector2(curBomb.transform.position.x + player.power, curBomb.transform.position.y));
            BoardManager.main.setFireOff(row, column, player.power);
        }

    
    }
}
