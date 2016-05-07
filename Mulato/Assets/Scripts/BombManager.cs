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

        public float explosionTime;
        public int powerOfExplosion;

        public LayerMask layerMask = ~(1 << 9 | 1 << 11); //all exept bomb and floor

        public GameObject bombBlue;
        public GameObject bombPink;
        public GameObject bombPurple;
        public GameObject SpecialBomb;
        public GameObject Combo;
        public Queue<GameObject> bombs;

        public ParticleSystem ExplodParticleSystem;

        public int currentBombColor;
        public int nextBombColor;
        public int thirdBombColor;
        public int forthBombColor;
        private int numOfColors;

        // for missions
        public bool onMission = false;
        public bool missionMultipleKilled = false;
        public bool missionSurvival = false;
        public bool wonMissionMultipleKilled;
        public int numOfKillesToWinComboMission;
        private int numOfKillesForCombo = 2;
        public int comboCounterKill;
        public int startIndexBombColor = 0;

        // for bouns bomb
        private float gridRowBomb;
        private float gridColBomb;


        void Start()
        {
            bombs = new Queue<GameObject>();

            currentBombColor = drawNextBomb();
            bombs.Enqueue(colorManager.main.currentColorPosibilities[currentBombColor]);
            nextBombColor = drawNextBomb();
            bombs.Enqueue(colorManager.main.currentColorPosibilities[nextBombColor]);
            thirdBombColor = drawNextBomb();
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
            int randomColorNumber = UnityEngine.Random.Range(startIndexBombColor, numOfColors);

            if (!onMission && GameManager.main.enemiesOnTheBoard[randomColorNumber] == 0)
            {
                randomColorNumber = drawNextBomb();
            }

            return randomColorNumber;
        }

        public void DeployBomb(float row, float column, int gridRow, int gridColumn)
        {

            var curBomb = Instantiate(bombs.Dequeue(), new Vector3(row, column, -1), Quaternion.identity) as GameObject;
            currentBombColor = nextBombColor;
            nextBombColor = thirdBombColor;
            thirdBombColor = forthBombColor;
            forthBombColor = drawNextBomb();

            if (thirdBombColor == colorManager.colorsOptions.Special)
            {
                bombs.Enqueue(SpecialBomb);
            }
            else
            {
                bombs.Enqueue(colorManager.main.currentColorPosibilities[thirdBombColor]);
            }

            colorManager.main.changeColors();
            BoardManager.main.setBombOnGrid(gridRow, gridColumn);
            StartCoroutine(DelayedExecution(curBomb, gridRow, gridColumn));
        }

        private void Explode(GameObject curBomb, int row, int column)
        {
            //raycast from bomb to right,left,up,down
            RaycastHit2D[] colliderHitsRight = Physics2D.RaycastAll(new Vector3(0.6f, 0, 0) + curBomb.transform.position, Vector2.right,
                powerOfExplosion, layerMask);
            RaycastHit2D[] colliderHitsLeft = Physics2D.RaycastAll(new Vector3(-0.6f, 0, 0) + curBomb.transform.position, Vector2.left,
                powerOfExplosion, layerMask);
            RaycastHit2D[] colliderHitsUp = Physics2D.RaycastAll(curBomb.transform.position, Vector2.up,
                powerOfExplosion, layerMask);
            RaycastHit2D[] colliderHitsDown = Physics2D.RaycastAll(new Vector3(0, -0.6f, 0) + curBomb.transform.position, Vector2.down,
                powerOfExplosion, layerMask);

            comboCounterKill = missionMultipleKilled ? numOfKillesToWinComboMission : numOfKillesForCombo;

            colliderHitsAction(colliderHitsDown, curBomb.tag);
            colliderHitsAction(colliderHitsRight, curBomb.tag);
            colliderHitsAction(colliderHitsUp, curBomb.tag);
            colliderHitsAction(colliderHitsLeft, curBomb.tag);

            //check if multiple enemies killed 
            if (comboCounterKill <= 0)
            {
                StartCoroutine(slowMotion());
               // comboCounterKill = missionMultipleKilled ? numOfKillesToWinComboMission : numOfKillesForCombo;
                if (missionMultipleKilled)
                {
                    wonMissionMultipleKilled = true;
                    Missions.main.checkMissionStatus();
                }
                else
                {
                    Instantiate(Combo, new Vector3((int)column, (int)row, 0), Quaternion.identity);
                    deploySpecialBomb();
                }
            }
          

            BoardManager.main.updateGridPointObject(row, column, row, column);
            //BoardManager.main.setFireOff(row, column, powerOfExplosion);
        }

        //handle raycasting colliders
        private void colliderHitsAction(RaycastHit2D[] colliderHits, String bombTag)
        {
            Rigidbody2D currCollider = null;

            for (int i = 0; i < colliderHits.Length; i++)
            {
                currCollider = colliderHits[i].rigidbody;
                if (currCollider.tag == "Wall")
                {
                    break;
                }
                else if (currCollider.tag == "EnemyBlue" && (bombTag == "BombBlue" || bombTag == "SpecialBomb"))
                {
                    comboCounterKill--;
                    GameManager.main.enemiesOnTheBoard[colorManager.colorsOptions.Blue]--;
                    currCollider.GetComponent<Enemy>().activeDelay();
                    GameManager.main.EnemyKilled();
                    if (missionSurvival)
                    {
                        Timer.main.setTimerMission((int)Timer.main.myTimer + 5);
                    }
                    //colliderHits [i].rigidbody.gameObject.GetComponent<Enemy> ().animator.SetBool ("die", true);

                }
                else if (currCollider.tag == "EnemyPink" && (bombTag == "BombPink" || bombTag == "SpecialBomb"))
                {
                    comboCounterKill--;
                    GameManager.main.enemiesOnTheBoard[colorManager.colorsOptions.Pink]--;
                    currCollider.GetComponent<Enemy>().activeDelay();
                    GameManager.main.EnemyKilled();
                    if (missionSurvival)
                    {
                        Timer.main.setTimerMission((int)Timer.main.myTimer + 5);
                    }
                    //colliderHits [i].rigidbody.gameObject.GetComponent<Enemy> ().animator.SetBool ("die", true);

                }
                else if (currCollider.tag == "EnemyPurple" && (bombTag == "BombPurple" || bombTag == "SpecialBomb"))
                {
                    comboCounterKill--;
                    GameManager.main.enemiesOnTheBoard[colorManager.colorsOptions.Purple]--;
                    currCollider.GetComponent<Enemy>().activeDelay();
                    GameManager.main.EnemyKilled();
                    if (missionSurvival)
                    {
                        Timer.main.setTimerMission((int)Timer.main.myTimer + 5);
                    }
                    //colliderHits [i].rigidbody.gameObject.GetComponent<Enemy> ().animator.SetBool ("die", true);

                }
                else if (currCollider.tag == "Box")
                {
                    currCollider.GetComponent<Box>().DestroyMe();
                    break;
                }
                else
                {
                    GameManager.main.GameOver(1);
                }
            }

           

        }

    
        //for bouns combo
        private void deploySpecialBomb()
        {
            gridColBomb = UnityEngine.Random.Range(0, BoardManager.main.columns);
            gridRowBomb = UnityEngine.Random.Range(0, BoardManager.main.rows);
            GameObject floor = BoardManager.main.getGameObjectOnGridPoint((int)gridRowBomb, (int)gridColBomb);

            if (BoardManager.main.getGridPointObject((int)gridRowBomb, (int)gridColBomb) == BoardManager.GridPointObject.Empty)
            {
                
                var curBomb =
                    Instantiate(SpecialBomb, new Vector3(floor.transform.position.x, floor.transform.position.y, -1), Quaternion.identity) as GameObject;
                StartCoroutine(DelayedExecution(curBomb, (int)gridRowBomb, (int)gridColBomb));
                BoardManager.main.setBombOnGrid((int)gridRowBomb, (int)gridColBomb);
            }
            else
            {
                deploySpecialBomb();
            }
        }
        //delay bomb action
        IEnumerator DelayedExecution(GameObject curBomb, int row, int column)
        {
            yield return new WaitForSeconds(2.8f);
            Explode(curBomb, row, column);
            yield return new WaitForSeconds(0.2f);
            Destroy(curBomb);


        }

        IEnumerator slowMotion()
        {
            Time.timeScale = 0.2F;
            yield return new WaitForSeconds(0.2f);
            Time.timeScale = 1F;

        }

    }
}

