using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public delegate void BombManagerBoxMovingObjectsUpdateBombGridPoint(int oldRow, int oldCol, int newRow, int newCol);
    public delegate void BombManagersetBombOnBoard(int row, int col);
    public delegate GameObject BombManagerGetGameObjectFromBoard(int row, int col);
    public delegate BoardManager.GridPointObject BombManagerGetGridPointObjectFromBoard(int row, int col);

    public class BombManager : SceneSingleton<BombManager>
    {
        public BombManagerBoxMovingObjectsUpdateBombGridPoint m_updateBombGrid;
        public BombManagersetBombOnBoard m_setBombOnBoard;
        public BombManagerGetGameObjectFromBoard m_GetGameObjectOnBoard;
        public BombManagerGetGridPointObjectFromBoard m_GetGridPointObjectOnBoard;
        public float explosionTime;
        public int powerOfExplosion;

        public LayerMask layerMask = ~(1 << 9 | 1 << 11); //all exept bomb and floor

       // public GameObject m_currBoard;
        public GameObject heart;
        public GameObject SpecialBomb;
        public GameObject Combo;
        public Queue<GameObject> bombs;
        public AudioClip bombSound;
        
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

        /*public void setBoardAsListener(GameObject i_CurBoard)
        {
            m_currBoard = i_CurBoard;
        }*/

        public void reDrawBombs()
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
            colorManager.main.init(numberOfColors);
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

            var curBomb = Instantiate(bombs.Dequeue(), new Vector3(3f, 10.5f, -1), Quaternion.identity) as GameObject;
            iTween.MoveTo(curBomb, new Vector3(row, column, -1), 2f);
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
           
            StartCoroutine(DelayedExplode(curBomb, gridRow, gridColumn));
        }

        private void Explode(GameObject curBomb, int row, int column)
        {
            //raycast from bomb to right,left,up,down
          // Instantiate(test, new Vector3(1.2f, 0, 0) + curBomb.transform.position, Quaternion.identity);
            RaycastHit2D[] colliderHitsRight = Physics2D.RaycastAll(new Vector3(1.2f, 0, 0) + curBomb.transform.position, Vector2.right,
                powerOfExplosion - 0.3f, layerMask);
         //   Instantiate(test, new Vector3(3f, 0, 0) + curBomb.transform.position, Quaternion.identity);

          //  Instantiate(test, new Vector3(-1.2f, 0, 0) + curBomb.transform.position, Quaternion.identity);
            RaycastHit2D[] colliderHitsLeft = Physics2D.RaycastAll(new Vector3(-1.2f, 0, 0) + curBomb.transform.position, Vector2.left,
                powerOfExplosion - 0.3f, layerMask);
          //  Instantiate(test, new Vector3(-3f, 0, 0) + curBomb.transform.position, Quaternion.identity);

          //  Instantiate(test, new Vector3(0, 0, 0) + curBomb.transform.position, Quaternion.identity);
            RaycastHit2D[] colliderHitsUp = Physics2D.RaycastAll(curBomb.transform.position, Vector2.up,
                powerOfExplosion +0.5f, layerMask);
         //  Instantiate(test, new Vector3(0, 2.5f, 0) + curBomb.transform.position, Quaternion.identity);

         //   Instantiate(test, new Vector3(0, -1.2f, 0) + curBomb.transform.position, Quaternion.identity);
            RaycastHit2D[] colliderHitsDown = Physics2D.RaycastAll(new Vector3(0, -1.2f, 0) + curBomb.transform.position, Vector2.down,
                powerOfExplosion -0.3f, layerMask);
        //    Instantiate(test, new Vector3(0, -3.2f, 0) + curBomb.transform.position, Quaternion.identity);
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
                    //create "Combo" on last bomb position
                    Instantiate(Combo, new Vector3((int)column, (int)row, 0), Quaternion.identity);
                    deploySpecialBomb();
                }
            }

            // m_currBoard.GetComponent<BoardManager>().UpdateGridPointObject (row, column, row, column);
            m_updateBombGrid(row, column, row, column);
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
                    Instantiate(heart, currCollider.transform.position, Quaternion.identity);
                    heart.GetComponent<Heart>().SetAnimation();
                }
            }

           

        }
    
        //for bouns combo
        private void deploySpecialBomb()
        {
            gridColBomb = UnityEngine.Random.Range(0, BoardManager.k_Columns);
            gridRowBomb = UnityEngine.Random.Range(0, BoardManager.k_Rows);
            //get game object on random postion choosen
            GameObject floor = m_GetGameObjectOnBoard((int)gridRowBomb, (int)gridColBomb);
            //check if random game object is empty to deploy special bomb
            if (m_GetGridPointObjectOnBoard((int)gridRowBomb, (int)gridColBomb) == 0)
            {
                //creates special bomb on choosen point
                var curBomb =
                    Instantiate(SpecialBomb, new Vector3(floor.transform.position.x, floor.transform.position.y, -1), Quaternion.identity) as GameObject;
                StartCoroutine(DelayedExplode(curBomb, (int)gridRowBomb, (int)gridColBomb));
                m_setBombOnBoard((int)gridRowBomb, (int)gridColBomb);
            }
            //if game object isnt empty recall function
            else
            {
                deploySpecialBomb();
            }
        }
        //delay bomb action for slow motion
        IEnumerator DelayedExplode(GameObject curBomb, int row, int column)
        {
            yield return new WaitForSeconds(0.3f);
            m_setBombOnBoard(row, column);
            yield return new WaitForSeconds(2.5f);
            curBomb.GetComponent<AudioSource>().PlayOneShot(bombSound, 0.5f);
            yield return new WaitForSeconds(0.2f);
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

