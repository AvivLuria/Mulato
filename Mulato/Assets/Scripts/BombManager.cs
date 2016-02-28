using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts
{
    public class BombManager : SceneSingleton<BombManager> {
        public float explosionTime = 3;
        public int power = 1;

        public LayerMask wallLayer;

        public Player player;

        void Start() {
           // SetTheBomb (explosionTime, power);
            player.onPlayerDeployedBomb.AddListener(DeployBomb);
        }

        public void SetTheBomb(float explosionTime, int power) {
            //GetComponent <BoxCollider2D> ().enabled = false;
            // TODO: Notify That the Bomb was set up
            this.ExecuteWithDelay(explosionTime, Explode);
		
        }

        private void Explode()
        {
            RaycastHit2D hitPositiveX = Physics2D.Raycast(transform.position,
                new Vector2(transform.position.x + power, transform.position.y), power, wallLayer.value);

            RaycastHit2D hitNegativeX = Physics2D.Raycast(transform.position,
                new Vector2(transform.position.x - power, transform.position.y), power, wallLayer.value);

            RaycastHit2D hitPositiveY = Physics2D.Raycast(transform.position,
                new Vector2(transform.position.x, transform.position.y + power), power, wallLayer.value);

            RaycastHit2D hitNegativeY = Physics2D.Raycast(transform.position,
                new Vector2(transform.position.x, transform.position.y - power), power, wallLayer.value);
            //GetComponent <BoxCollider2D> ().enabled = true;

            if (hitPositiveX != null)
            {
                Destroy(hitPositiveX.transform.gameObject);
            }
            if (hitNegativeX != null)
            {
                Destroy(hitNegativeX.transform.gameObject);
            }
            if (hitPositiveY != null)
            {
                Destroy(hitPositiveY.transform.gameObject);
            }
            if (hitNegativeY != null)
            {
                Destroy(hitNegativeY.transform.gameObject);
            }

            Debug.DrawLine(transform.position, new Vector3(transform.position.x + power, transform.position.y), Color.green);
            Debug.DrawLine(transform.position, new Vector3(transform.position.x - power, transform.position.y), Color.green);
            Debug.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y + power), Color.green);
            Debug.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - power), Color.green);
            /*
                Debug.Log ("The positive row hit is: " + hitPositiveX.transform.tag);
                Debug.Log ("The neagtive row hit is: " + hitNegativeX.transform.tag);
                Debug.Log ("The positive column hit is: " + hitPositiveY.transform.tag);
                Debug.Log ("The negative column hit is: " + hitNegativeY.transform.tag);
        */
            Destroy(gameObject, 2);
        }

        public void DeployBomb(int row, int column)
        {
        
        }
    }
}
