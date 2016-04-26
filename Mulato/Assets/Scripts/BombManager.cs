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
		
        public float explosionTime = 2;
        public int powerOfExplosion;
        public LayerMask layerMask = ~(1 << 9 | 1 << 11); //all exept bomb and floor
		public GameObject bombBlue;
		public GameObject bombPink;
		public GameObject bombPurple;
        public GameObject SpecialBomb;
        public Queue<GameObject> bombs;       
        public ParticleSystem ExplodParticleSystem;
		public int currentBombColor;
		public int nextBombColor;
		public int thirdBombColor;
        public int forthBombColor;
        private int numOfColors;

        public bool onMission = false;
        public bool missionMultipleKilled = false;
        public bool wonMissionMultipleKilled;
        public int combo;
        public int comboCounter;


        void Start()
        {            
            bombs = new Queue<GameObject>();
                   
			currentBombColor = drawNextBomb ();
			bombs.Enqueue(colorManager.main.currentColorPosibilities[currentBombColor]);
			nextBombColor = drawNextBomb ();
			bombs.Enqueue(colorManager.main.currentColorPosibilities[nextBombColor]);
			thirdBombColor = drawNextBomb ();
			bombs.Enqueue(colorManager.main.currentColorPosibilities[thirdBombColor]);
            colorManager.main.changeColors();
            forthBombColor = drawNextBomb();
        }

        public void setNumberOfColors(int numberOfColors)
        {
            numOfColors = numberOfColors;
        }

        private int drawNextBomb()
        {
            int randomColorNumber = UnityEngine.Random.Range(0, numOfColors);
             
                 if (!onMission && GameManager.main.enemiesOnTheBoard[randomColorNumber] == 0)
                  {
                    randomColorNumber = drawNextBomb();
                  }         
            
            return randomColorNumber;
        }

        public void DeployBomb(int row, int column, int gridRow, int gridColumn)
        {
         //   var toDestroy = Instantiate(ExplodParticleSystem, new Vector3(row, column, 0), ExplodParticleSystem.transform.rotation) as GameObject;
            var curBomb = Instantiate(bombs.Dequeue(), new Vector3(row, column, -1), Quaternion.identity) as GameObject;
			currentBombColor = nextBombColor;
			nextBombColor = thirdBombColor;
            thirdBombColor = forthBombColor;
			forthBombColor = drawNextBomb ();
            if (thirdBombColor == colorManager.colorsOptions.Special)
            {
                bombs.Enqueue(SpecialBomb);
            } else
            {
			    bombs.Enqueue(colorManager.main.currentColorPosibilities[thirdBombColor]);
            }

            colorManager.main.changeColors();
            BoardManager.main.setBombPosition(gridRow, gridColumn);
            StartCoroutine(DelayedExecution(curBomb, gridRow, gridColumn));            
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

             comboCounter = combo;
            colliderHitsAction(colliderHitsDown, curBomb.tag);
            colliderHitsAction(colliderHitsRight, curBomb.tag);
            colliderHitsAction(colliderHitsUp, curBomb.tag);
            colliderHitsAction(colliderHitsLeft, curBomb.tag);

            Destroy(curBomb);
            BoardManager.main.updateMovementPosition(row, column, row, column);
            //BoardManager.main.setFireOff(row, column, powerOfExplosion);
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
                else if (colliderHits[i].rigidbody != null && colliderHits[i].rigidbody.tag == "EnemyBlue" && (bombTag == "BombBlue" || bombTag == "SpecialBomb"))
                {
                    comboCounter--;
                    Debug.Log("enemy blue");
                    GameManager.main.enemiesOnTheBoard[colorManager.colorsOptions.Blue]--;
                    GameManager.main.EnemyKilled();
					colliderHits [i].rigidbody.gameObject.GetComponent<Enemy> ().animator.SetBool ("die", true);
                    Destroy(colliderHits[i].rigidbody.gameObject);
                }
                else if (colliderHits[i].rigidbody != null && colliderHits[i].rigidbody.tag == "EnemyPink" && (bombTag == "BombPink" || bombTag == "SpecialBomb"))
                {
                    comboCounter--;
                    GameManager.main.enemiesOnTheBoard[colorManager.colorsOptions.Pink]--;
                    Debug.Log("enemy pink");
                    GameManager.main.EnemyKilled();
					colliderHits [i].rigidbody.gameObject.GetComponent<Enemy> ().animator.SetBool ("die", true);
                    Destroy(colliderHits[i].rigidbody.gameObject);
                }
                else if (colliderHits[i].rigidbody != null && colliderHits[i].rigidbody.tag == "EnemyPurple" && (bombTag == "BombPurple" || bombTag == "SpecialBomb"))
                {
                    comboCounter--;
                    Debug.Log("enemy Purple");
                    GameManager.main.enemiesOnTheBoard[colorManager.colorsOptions.Purple]--;
                    GameManager.main.EnemyKilled();
					colliderHits [i].rigidbody.gameObject.GetComponent<Enemy> ().animator.SetBool ("die", true);
                    Destroy(colliderHits[i].rigidbody.gameObject);
                }
                else if (colliderHits[i].rigidbody != null && colliderHits[i].rigidbody.tag == "Box")
                {

                    colliderHits[i].rigidbody.GetComponent<Box>().DestroyMe();
                    hit(colliderHits[i].rigidbody.gameObject);
                    break;
                }
                else
                {
                    GameManager.main.GameOver(1);
                }
            }
            //check if two enemies killed mission
            if (comboCounter <= 0 && missionMultipleKilled)
            {
                GameManager.main.EnemyKilled();
                wonMissionMultipleKilled = true;
            }
        }

        //TODO : delete this later
        //delay bomb action
        IEnumerator DelayedExecution(GameObject curBomb,int row, int column)
        {
            yield return new WaitForSeconds(3f);
            Explode(curBomb, row, column);
            yield return new WaitForSeconds(1f);
           
        }
        
        public void hit( GameObject obj)
        {
            Debug.Log("destroy");
            Destroy(obj.gameObject);
        }
    }
}
