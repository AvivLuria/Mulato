using System;
using System.Collections;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts
{
    public class BombManager : SceneSingleton<BombManager>
    {
        public float explosionTime = 3;
        public int powerOfExplosion;

        public LayerMask wallLayer;
        public LayerMask layerMask = ~(1 << 9 | 1 << 11);

        public Player player;
        public GameObject bomb;

        void Start()
        {
            // SetTheBomb (explosionTime, powerOfExplosion);
            // player.onPlayerDeployedBomb.AddListener(DeployBomb);
            // player.onPlayerDeployedBomb.AddListener(DeployBomb);
        }

        public void DeployBomb(int row, int column)
        {

            var curBomb = Instantiate(bomb, new Vector3(row, column, 0), Quaternion.identity) as GameObject;
            BoardManager.main.setFireOn(row, column, powerOfExplosion);
            StartCoroutine(DelayedExecution(curBomb,row,column));
            
        }
        //TODO : added raycasting + destory
        private void Explode(GameObject curBomb, int row, int column)
        {
            
            //raycast from bomb to right,left,up,down
            RaycastHit2D[] colliderHitsRight = Physics2D.RaycastAll(curBomb.transform.position, Vector2.right,
                powerOfExplosion, layerMask);
            RaycastHit2D[] colliderHitsLeft = Physics2D.RaycastAll(curBomb.transform.position, Vector2.left,
                powerOfExplosion, layerMask);
            RaycastHit2D[] colliderHitsUp = Physics2D.RaycastAll(curBomb.transform.position, Vector2.up,
                powerOfExplosion, layerMask);
            RaycastHit2D[] colliderHitsDown = Physics2D.RaycastAll(curBomb.transform.position, Vector2.down,
                powerOfExplosion, layerMask);

            
            colliderHitsAction(colliderHitsDown);
            colliderHitsAction(colliderHitsRight);
            colliderHitsAction(colliderHitsUp);
            colliderHitsAction(colliderHitsLeft);


            Destroy(curBomb);
            /* var hit1 = Physics2D.Linecast(curBomb.transform.position,
                 new Vector2(curBomb.transform.position.x + powerOfExplosion, curBomb.transform.position.y));
             if (hit1.collider.gameObject.tag == "Enemy")
             {
                 Debug.Log("i see you");
             }
             */
            BoardManager.main.setFireOff(row, column, powerOfExplosion);
        }
        //TODO : destroy for bomb , need to delete later
        //handle raycasting colliders
        private void colliderHitsAction(RaycastHit2D[] colliderHits)
        {
            for (int i = 0; i < colliderHits.Length; i++)
            {
                if (colliderHits[i].rigidbody != null && colliderHits[i].rigidbody.tag == "Wall")
                {

                    Debug.Log("Wall");
                    break;
                }
                if (colliderHits[i].rigidbody != null && colliderHits[i].rigidbody.tag == "Enemy")
                {

                    Debug.Log("enemy");
                    Destroy(colliderHits[i].rigidbody.gameObject);
                }


            }

        }
        //TODO : delete this later
        //delay bomb action
        IEnumerator DelayedExecution(GameObject curBomb,int row, int column)
        {
            yield return new WaitForSeconds(3f);
            Explode(curBomb, row, column);
        }
    }
}
