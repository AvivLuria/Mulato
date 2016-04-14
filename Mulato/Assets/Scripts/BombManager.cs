using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class BombManager : SceneSingleton<BombManager>
    {
		
        public float explosionTime = 3;
        public int powerOfExplosion;
        public LayerMask layerMask = ~(1 << 9 | 1 << 11); //all exept bomb and floor
		public GameObject bombBlue;
		public GameObject bombPink;
		public GameObject bombYellow;
        private GameObject[] bombsPosibilities;
		public Queue<GameObject> bombs;       
        public ParticleSystem ExplodParticleSystem;
		public int cur;
		public int next1;
		public int next2;


        void Start()
        {
            
            bombs = new Queue<GameObject>();
            bombsPosibilities = new GameObject[3];
            bombsPosibilities = getBombPosibilities(bombsPosibilities);
			cur = drawNextBomb ();
			bombs.Enqueue(bombsPosibilities[cur]);
			next1 = drawNextBomb ();
			bombs.Enqueue(bombsPosibilities[next1]);
			next2 = drawNextBomb ();
			bombs.Enqueue(bombsPosibilities[next2]);

        }

        private int drawNextBomb()
        {
            int randomColorNumber = UnityEngine.Random.Range(0, 3);
            return randomColorNumber;
        }

        
        private GameObject[] getBombPosibilities(GameObject[] bombsPosibilities)
        {
            // TODO: fix the 3
            bombsPosibilities = new GameObject[3];
            bombsPosibilities[0] = bombBlue;
            bombsPosibilities[1] = bombPink;
            bombsPosibilities[2] = bombYellow;
            return bombsPosibilities;
        }

        public void DeployBomb(int row, int column, int gridRow, int gridColumn)
        {
            Instantiate(ExplodParticleSystem, new Vector3(row, column, 0), ExplodParticleSystem.transform.rotation);
            var curBomb = Instantiate(bombs.Dequeue(), new Vector3(row, column, 0), Quaternion.identity) as GameObject;
			cur = next1;
			next1 = next2;
			next2 = drawNextBomb ();
			bombs.Enqueue(bombsPosibilities[next2]);

            
            BoardManager.main.setBombPosition(gridRow, gridColumn);
            //BoardManager.main.setFireOn(row, column, powerOfExplosion);
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
           
            colliderHitsAction(colliderHitsDown, curBomb.tag);
            colliderHitsAction(colliderHitsRight, curBomb.tag);
            colliderHitsAction(colliderHitsUp, curBomb.tag);
            colliderHitsAction(colliderHitsLeft, curBomb.tag);

            Destroy(curBomb);         
            BoardManager.main.setFireOff(row, column, powerOfExplosion);
        }
        //TODO : destroy for bomb , need to delete later
        //handle raycasting colliders
        private void colliderHitsAction(RaycastHit2D[] colliderHits, String bombTag)
        {
            for (int i = 0; i < colliderHits.Length; i++)
            {
                if (colliderHits[i].rigidbody != null && colliderHits[i].rigidbody.tag == "Wall")
                {
                    break;
                }
                else if (colliderHits[i].rigidbody != null && colliderHits[i].rigidbody.tag == "EnemyBlue" && bombTag == "BombBlue")
                {
                    Debug.Log("enemy blue");
                    Destroy(colliderHits[i].rigidbody.gameObject);
                }
                else if (colliderHits[i].rigidbody != null && colliderHits[i].rigidbody.tag == "EnemyPink" && bombTag == "BombPink")
                {
                    Debug.Log("enemy pink");
                    Destroy(colliderHits[i].rigidbody.gameObject);
                }
                else if (colliderHits[i].rigidbody != null && colliderHits[i].rigidbody.tag == "EnemyYellow" && bombTag == "BombYellow")
                {
                    Debug.Log("enemy yellow");
                    Destroy(colliderHits[i].rigidbody.gameObject);
                }
                else if (colliderHits[i].rigidbody != null && colliderHits[i].rigidbody.tag == "Box")
                {
                    colliderHits[i].rigidbody.GetComponent<Box>().DestroyMe();
                    break;
                }
                else
                {
                    Debug.Log("You Lost!");
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
