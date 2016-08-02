using System;
using System.Collections;
using System.Diagnostics;
using Assets.Scripts.Utils;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameManager : SceneSingleton<GameManager>
    {
        private bool onAMission = false;
        public bool timer = true;
        public bool nextLevel = false;

        private int missionNum;
        public int difficulty;
        private int currLevel = -2;
        public int life = 3;
        public int numberOFEnemiesInTheLevel;
        public int[] enemiesOnTheBoard;
        public int numberOfColors;

        public GameObject m_BombManager;
        public GameObject m_CurrentBoard = null;
        public GameObject m_BoardManager;
        public GameObject arrowIndicator;
        public GameObject arrowIndicator_2;
        public GameObject Memory;
        public Sprite redBall;
        public Sprite greenBall;

        public Canvas fader;
        public Canvas gameCanvas;
        public Canvas mapCanvas;

        public Sprite eggiconRight;
        public Sprite eggiconLeft;

        public Transform  mainMenu, exitMenu,gameOver, pauseMenu, startLevel0, startLevel2, startLevel3, startLevel4, startLevel5, startLevel1, startLevel6, startLevel7, startLevel8, startLevel9, startLevel10, startLevel11,startLevel12, startLevel13, startLevel14, gameWon;
		public Transform[] startLevel;
        public GameObject eggIcon;
        public Image[] levelBalls;
        public bool stillPlaying = true;
        private bool firstTimePlaying = true;
        public override void Awake()
        {
            base.Awake();
            //DontDestroyOnLoad (gameObject);

        }
        void Start()
        {
           
            startLevel = new Transform[16];
            startLevel[0] = startLevel0;
            startLevel [1] = startLevel1;
			startLevel [2] = startLevel2;
			startLevel [3] = startLevel3;
			startLevel [4] = startLevel4;
			startLevel [5] = startLevel5;
			startLevel [6] = startLevel6;
			startLevel [7] = startLevel7;
			startLevel [8] = startLevel8;
			startLevel [9] = startLevel9;
			startLevel [10] = startLevel10;
			startLevel [11] = startLevel11;
            startLevel[12] = startLevel12;
            startLevel[13] = startLevel13;
            startLevel[14] = startLevel14;
            startLevel[15] = gameWon;

            nextLevel = true;
            if (PlayerPrefs.HasKey("isFirstTime") || PlayerPrefs.GetInt("isFirstTime") == 1)
            {
                currLevel = -2;
                startLevel[currLevel + 2].gameObject.SetActive(nextLevel);
                startLevel[currLevel + 2].gameObject.GetComponentInChildren<Button>().image.enabled = false;
                StartCoroutine(delayShowButton(currLevel + 2));
                PlayerPrefs.SetInt("isFirstTime", 1);
                PlayerPrefs.Save();
            } else
            {
                currLevel = 0;
                startLevel[currLevel + 3].gameObject.SetActive(nextLevel);
                startLevel[currLevel + 3].gameObject.GetComponentInChildren<Button>().image.enabled = false;
             //   firstTimePlaying = false;
                StartCoroutine(delayShowButton(currLevel + 3));
            }
           
            // InitGame (currLevel = -1);         
        }		

        public void InitGame (int currLevel) {

            if (m_CurrentBoard != null)
            {
                m_CurrentBoard.GetComponent<BoardManager>().clearScene();            
                Destroy(m_CurrentBoard.gameObject);
            }
            #region setting up for all levels
            stillPlaying = true;
            m_CurrentBoard = Instantiate(m_BoardManager, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            m_BombManager.GetComponent<BombManager>().m_updateBombGrid = new BombManagerBoxMovingObjectsUpdateBombGridPoint(m_CurrentBoard.GetComponent<BoardManager>().UpdateGridPointObject);
            m_BombManager.GetComponent<BombManager>().m_setBombOnBoard = new BombManagersetBombOnBoard(m_CurrentBoard.GetComponent<BoardManager>().SetBombOnGrid);
            m_BombManager.GetComponent<BombManager>().m_GetGameObjectOnBoard = new BombManagerGetGameObjectFromBoard(m_CurrentBoard.GetComponent<BoardManager>().GetGameObjectOnGridPoint);
            m_BombManager.GetComponent<BombManager>().m_GetGridPointObjectOnBoard = new BombManagerGetGridPointObjectFromBoard(m_CurrentBoard.GetComponent<BoardManager>().GetGridPointObject);
            m_CurrentBoard.GetComponent<BoardManager>().m_Level = currLevel;
            #endregion
            switch (currLevel)
            {
                case (-2):
                    {
                        numberOfColors = 1;
                        numberOFEnemiesInTheLevel = 1;
                        
                        m_CurrentBoard.GetComponent<BoardManager>().wallPostions = new int[]{
                            11 ,12, 13 ,14 ,15, 16, 21, 22, 23, 24, 25, 26, 31, 32, 33, 34 , 35, 36, 41, 42, 44, 45
                           ,46 ,51 ,52, 56, 66, 71 ,72, 76, 81, 82,84 ,85 ,86, 91, 92, 93, 94, 95, 96, 54,55,74,75 };
                        //Instantiate(arrowIndicator, new Vector3(6.27f, 8.6f, 0), Quaternion.Euler(0, 0, 50));
                        m_BombManager.GetComponent<BombManager>().explainBombTime = true;
                        StartCoroutine(delayedMove(arrowIndicator, 4.3f, 5.6f, 3.8f, 5, 3));
                        StartCoroutine(delayedMove(arrowIndicator, -10, -10, -10, -10, 4f));
                        onAMission = false;
                        Timer.main.setTimerMission(1800);
                        Timer.main.timerText.enabled = false;
                    break;
                }
                case (-1):
                    {
                        #region hard_coding_scene
                        m_BombManager.GetComponent<BombManager>().explainBombTime = false;
                        numberOfColors = 1;
                        numberOFEnemiesInTheLevel = 4;
                        m_CurrentBoard.GetComponent<BoardManager>().m_Level = currLevel;
                        m_CurrentBoard.GetComponent<BoardManager>().wallPostions = new int[]
                          {13,14,23,24,33,34,43,44,53,54,63,64,73,74,83,84,93,94,51,52,55,56};                  
                        BombManager.main.setNumberOfColors(numberOfColors);
                        Timer.main.setTimerMission(1800);
                        //    Instantiate(arrowIndicator, new Vector3(3.3f, 9, 0), Quaternion.Euler(0, 0, 300));
                        #endregion

                        Timer.main.timerText.enabled = false;
                        onAMission = false;
                        break;
                    }
                    
                case (0):
                    {
                        #region hard_coding_scene

                        numberOfColors = 2;
                        numberOFEnemiesInTheLevel = 4;
                        m_CurrentBoard.GetComponent<BoardManager>().wallPostions = new int[]
                          {13,14,23,24,33,34,43,44,53,54,63,64,73,74,83,84,93,94,51,52,55,56};
                        BombManager.main.setNumberOfColors(numberOfColors);
                        Timer.main.setTimerMission(1800);
                       // Instantiate(arrowIndicator, new Vector3(3.3f, 9, 0), Quaternion.Euler(0, 0, 300));
                        #endregion

                        Timer.main.timerText.enabled = false;
                        onAMission = false;
                        break;
                    }
                    
                case (1):
                    {
                        #region hard_coding_scene

                        numberOfColors = 1;
                        numberOFEnemiesInTheLevel = 3;
                        m_CurrentBoard.GetComponent<BoardManager>().wallPostions = new int[]
                          {92, 93, 94, 95, 83, 84, 63, 64, 52, 53, 54, 55, 43, 44, 12, 13, 14, 15, 23, 24};
                        Timer.main.timerText.enabled = true;
                        Timer.main.setTimerMission(119);
                        #endregion
                        onAMission = false;
                        break;
                    }
                //classic play
                case (2):
                {
                    #region hard_coding_scene

                    numberOfColors = 2;
                    numberOFEnemiesInTheLevel = 4;
                        m_CurrentBoard.GetComponent<BoardManager>().wallPostions = new int[]
                    {92, 93, 94, 95, 83, 84, 63, 64, 43, 44, 12, 13, 14, 15, 23, 24};
                    m_CurrentBoard.GetComponent<BoardManager>().m_NumOfFreezeBoxes = 1;
                    onAMission = false;
                    Timer.main.setTimerMission(119);
                    #endregion
                    break;
                }
                //classic play - robot 
                case (3):
                {
                    #region hard_coding_scene

                    numberOfColors = 2;
                    numberOFEnemiesInTheLevel = 6;
                    m_CurrentBoard.GetComponent<BoardManager>().wallPostions = new int[]
                    {93, 94, 83, 84, 63, 64, 43, 44, 13, 14, 23, 24, 66, 56, 46, 61, 41, 51};
                    Timer.main.setTimerMission(90);
                    onAMission = false;
                    #endregion
                    break;
                }
                    //combo kill - 2
                case (4):
                {
                    #region hard_coding_scene

                    
                    numberOfColors = 1;
                    numberOFEnemiesInTheLevel = 6;
                    m_CurrentBoard.GetComponent<BoardManager>().wallPostions = new int[]
                    {93, 94, 83, 84, 63, 64, 43, 44, 13, 14, 23, 24, 66, 56, 46, 61, 41, 51};

                    difficulty = 2;
                    missionNum = 2;
                        #endregion
                        //sets up the color array to choose from
                        m_CurrentBoard.GetComponent<BoardManager>().m_NumOfColors = (numberOfColors);                       
                        BombManager.main.setNumberOfColors(numberOfColors);                       
                        
                        Timer.main.setTimerMission(90);
                        onAMission = true;
                        break;
                }
                    //classic play - 2 colors squares
                case (5):
                {
                    #region hard_coding_scene

                    numberOfColors = 2;
                    numberOFEnemiesInTheLevel = 8;
                    m_CurrentBoard.GetComponent<BoardManager>().wallPostions = new int[]
                    {11, 12, 13, 22, 34, 35, 36, 45, 51, 52, 53, 62, 74, 75, 76, 85};
                    m_CurrentBoard.GetComponent<BoardManager>().m_NumOfFreezeBoxes = 1;
                    m_CurrentBoard.GetComponent<BoardManager>().m_NumOfNomralBoxes = 2;

                    Timer.main.setTimerMission(119);

                        #endregion
                    onAMission = false;
                    break;
                }
                    //classic play - 3 colors
                case (6):
                {
                    #region hard_coding_scene

                    numberOfColors = 3;
                    numberOFEnemiesInTheLevel = 6;
                    m_CurrentBoard.GetComponent<BoardManager>().wallPostions = new int[] {92,93,82,83,64,65,54,55,32,33,22,23,25,26,15,16};
                    m_CurrentBoard.GetComponent<BoardManager>().m_NumOfLifeBoxes = 1;
                    m_CurrentBoard.GetComponent<BoardManager>().m_NumOfSpecialBombBoxes = 1;
                    m_CurrentBoard.GetComponent<BoardManager>().m_NumOfNomralBoxes = 2;
                    m_CurrentBoard.GetComponent<BoardManager>().m_NumOfFreezeBoxes = 1;
                    Timer.main.setTimerMission(90);

                    #endregion
                    onAMission = false;
                    break;

                }
                // kill the same color - lucky number 7 
                case (7):
                {
                        #region hard_coding_scene
                        difficulty = 2;
                        missionNum = 3;
                        numberOfColors = 2;
                        numberOFEnemiesInTheLevel = 7;
                        m_CurrentBoard.GetComponent<BoardManager>().wallPostions = new int[] {73,72,74,64,54,44,34,52,53,55};
                        m_CurrentBoard.GetComponent<BoardManager>().m_NumOfLifeBoxes = 1;
                        m_CurrentBoard.GetComponent<BoardManager>().m_NumOfSpecialBombBoxes = 1;
                        m_CurrentBoard.GetComponent<BoardManager>().m_NumOfNomralBoxes = 2;

                        Timer.main.setTimerMission(90);                    
                        #endregion               
                    onAMission = true;
                    break;

                }
                //classic play - 2 color / 30 sec
                case (8):
                {
                    #region hard_coding_scene

                        numberOfColors = 2;
                        difficulty = numberOfColors;
                        numberOFEnemiesInTheLevel = 5;
                        m_CurrentBoard.GetComponent<BoardManager>().wallPostions = new int[] { 82,72,62,52,42,32,22,83,74,65,45,34,23 };
                        m_CurrentBoard.GetComponent<BoardManager>().m_NumOfLifeBoxes = 1;
                        m_CurrentBoard.GetComponent<BoardManager>().m_NumOfSpecialBombBoxes = 1;
                        m_CurrentBoard.GetComponent<BoardManager>().m_NumOfNomralBoxes = 1;
                        m_CurrentBoard.GetComponent<BoardManager>().m_NumOfFreezeBoxes = 1;
                        Timer.main.setTimerMission(30);
                        #endregion
                    onAMission = false;
                    break;                   
                }
                //disappearing enemies - 2 colors
                case (9):
                {
                    #region hard_coding_scene

                        missionNum = 5;
                        numberOfColors = 2;
                        difficulty = numberOfColors;
                        numberOFEnemiesInTheLevel = 7;
                        m_CurrentBoard.GetComponent<BoardManager>().wallPostions = new int[] { 72,83,84,85,75,65,54,53,43,23};
                        m_CurrentBoard.GetComponent<BoardManager>().m_NumOfLifeBoxes = 1;
                        m_CurrentBoard.GetComponent<BoardManager>().m_NumOfSpecialBombBoxes = 1;
                        m_CurrentBoard.GetComponent<BoardManager>().m_NumOfNomralBoxes = 2;
                        Timer.main.setTimerMission(90);
                        #endregion
                        onAMission = true;
                    break;
                }
                    
                //combo kill - 2
                case (10):
                {
                        #region hard_coding_scene                   
                        missionNum = 2;
                        numberOfColors = 2;
                        difficulty = 2;
                        numberOFEnemiesInTheLevel = 8;
                        m_CurrentBoard.GetComponent<BoardManager>().wallPostions = new int[] {33, 23, 25, 35, 43, 45, 53, 55, 63, 65, 62, 64, 73, 75 };
                        m_CurrentBoard.GetComponent<BoardManager>().m_NumOfLifeBoxes = 1;
                        m_CurrentBoard.GetComponent<BoardManager>().m_NumOfFreezeBoxes = 1;
                        m_CurrentBoard.GetComponent<BoardManager>().m_NumOfSpecialBombBoxes = 1;
                        m_CurrentBoard.GetComponent<BoardManager>().m_NumOfNomralBoxes = 3;

                        Timer.main.setTimerMission(90);

                        #endregion
                        m_CurrentBoard.GetComponent<BoardManager>().m_NumOfColors = (numberOfColors);
                        BombManager.main.setNumberOfColors(numberOfColors);
                        onAMission = true;
                    break;
                }
                    
                // kill the same color
                case (11):
                {
                        #region hard_coding_scene
                        difficulty = 2;
                        missionNum = 3;
                        numberOfColors = 2;
                        numberOFEnemiesInTheLevel = 9;
                        m_CurrentBoard.GetComponent<BoardManager>().wallPostions = new int[] {33,23,25,35,43,45,53,55,63,65,62,64,73,75 };
                        m_CurrentBoard.GetComponent<BoardManager>().m_NumOfLifeBoxes = 1;
                        m_CurrentBoard.GetComponent<BoardManager>().m_NumOfSpecialBombBoxes = 1;
                        m_CurrentBoard.GetComponent<BoardManager>().m_NumOfNomralBoxes = 2;
                        m_CurrentBoard.GetComponent<BoardManager>().m_NumOfFreezeBoxes = 1;
                        Timer.main.setTimerMission(60);
                        #endregion
                    onAMission = true;
                    break;
                }

                // closed levels
                //disappearing enemies - 3 colors
                case (12):
                {
                        #region hard_coding_scene

                        missionNum = 5;
                        numberOfColors = 3;
                        difficulty = numberOfColors;
                        numberOFEnemiesInTheLevel = 9;
                        m_CurrentBoard.GetComponent<BoardManager>().wallPostions = new int[] { 11,12,13,14,15,16,91,92,93,94,95,96,51,52,55,56};
                        m_CurrentBoard.GetComponent<BoardManager>().m_NumOfLifeBoxes = 1;
                        m_CurrentBoard.GetComponent<BoardManager>().m_NumOfSpecialBombBoxes = 1;
                        m_CurrentBoard.GetComponent<BoardManager>().m_NumOfNomralBoxes = 2;
                        Timer.main.setTimerMission(90);
                  
                        #endregion                    
                    onAMission = true;
                    break;
                }
                //like bomber man
                /*
                case (13):
                {
                    #region hard_coding_scene

                        numberOfColors = 3;
                        numberOFEnemiesInTheLevel = 9;
                        BoardManager.main.wallPostions = new int[] {81,83,85,62,64,66,41,43,45,22,24,26 };
                        BoardManager.main.numOfLifeBoxes = 1;
                        BoardManager.main.numOfFreezeBoxes = 1;
                        BoardManager.main.numOfSpecialBombBoxes = 2;
                        BoardManager.main.numOfNomralBoxes = 3;

                        Timer.main.setTimerMission(120);
                        #endregion                    
                    onAMission = false;
                    break;
                }
                // lamed
                case (14):
                {
                    #region hard_coding_scene
                        missionNum = 7;
                        numberOfColors = 3;
                        numberOFEnemiesInTheLevel = 9;
                        BoardManager.main.wallPostions = new int[] { 26,35,44,53,62,52,42,32,22,33,55,66,56,46,36,45,43};
                        BoardManager.main.numOfLifeBoxes = 1;
                        BoardManager.main.numOfFreezeBoxes = 1;
                        BoardManager.main.numOfSpecialBombBoxes = 2;
                        BoardManager.main.numOfNomralBoxes = 3;

                        Timer.main.setTimerMission(120);
                        #endregion
                    onAMission = true;
                    break;
                }
                case (15):
                {
                    #region hard_coding_scene

                        numberOfColors = 3;
                        numberOFEnemiesInTheLevel = 9;
                        BoardManager.main.wallPostions = new int[] { 22, 25, 32, 35, 42, 45, 52, 55, 62, 65, 72, 75, 53, 54, 82, 85 };
                        BoardManager.main.numOfLifeBoxes = 1;
                        BoardManager.main.numOfFreezeBoxes = 1;
                        BoardManager.main.numOfSpecialBombBoxes = 2;
                        BoardManager.main.numOfNomralBoxes = 3;

                        Timer.main.setTimerMission(120);
                        #endregion
                    onAMission = false;
                    break;
                }
                case (16):
                {
                    #region hard_coding_scene

                        numberOfColors = 3;
                        numberOFEnemiesInTheLevel = 9;
                        BoardManager.main.wallPostions = new int[] { 83,84,72,73,62,63,52,53,42,43,32,33,23,24,55};
                        BoardManager.main.numOfLifeBoxes = 1;
                        BoardManager.main.numOfFreezeBoxes = 1;
                        BoardManager.main.numOfSpecialBombBoxes = 2;
                        BoardManager.main.numOfNomralBoxes = 3;

                        Timer.main.setTimerMission(120);
                        #endregion
                    onAMission = false;
                    break;
                }*/
            }

            enemiesOnTheBoard = new int[3];
            m_CurrentBoard.GetComponent<BoardManager>().m_NumOfEnemies = numberOFEnemiesInTheLevel;
            m_CurrentBoard.GetComponent<BoardManager>().m_NumOfColors = numberOfColors;
            if (onAMission)
            {
                Missions.main.initMission(missionNum, numberOFEnemiesInTheLevel, difficulty);
            }
            else
            {                                           
                BombManager.main.setNumberOfColors(numberOfColors);
                BombManager.main.onMission = false;              
            }
            timer = true;
            m_CurrentBoard.GetComponent<BoardManager>().StartScene();
            if ( currLevel == -1)
            {
                Vector2 enemyOne = m_CurrentBoard.GetComponent<BoardManager>().enemies[0].transform.position;
                Vector2 enemyTwo = m_CurrentBoard.GetComponent<BoardManager>().enemies[1].transform.position;
                StartCoroutine(delayedMove(arrowIndicator, enemyOne.x + 2.5f, enemyOne.y + 1.8f, enemyOne.x + 1.8f, enemyOne.y + 0.8f, 0f));               
                StartCoroutine(delayedMove(arrowIndicator_2, enemyOne.x - 1f, enemyOne.y + 2.8f, enemyOne.x, enemyOne.y + 2f, 0f));
                StartCoroutine(delayedMove(arrowIndicator, enemyTwo.x + 2.5f, enemyTwo.y + 1.8f, enemyTwo.x + 1.8f, enemyTwo.y + 0.8f, 4f));
                StartCoroutine(delayedMove(arrowIndicator_2, enemyTwo.x - 1f, enemyTwo.y + 2.8f, enemyTwo.x, enemyTwo.y + 2f, 4f));
                StartCoroutine(delayedMove(arrowIndicator, -10, -10, -10, -10, 7f));
                StartCoroutine(delayedMove(arrowIndicator_2, -10, -10, -10, -10, 7f));
            }
            BombManager.main.reDrawBombs();
        }

        public void EnemyKilled(int enemiesKilled)
        {
            numberOFEnemiesInTheLevel -= enemiesKilled;
            if (stillPlaying)
            {
                if (numberOFEnemiesInTheLevel <= 0 && !onAMission)
                {
                    stillPlaying = false;
                    timer = false;
                    changeLevel(3);
                }
                else if (onAMission)
                {
                    Missions.main.checkMissionStatus();
                }
            }
        }

        public void GameOver(int damage)
        {
            if (stillPlaying)
            {
                life -= damage;
                if (life <= 0)
                {
                    stillPlaying = false;
                    life = 0;
                    Ui.activeUI(false);
                    fader.GetComponentInChildren<canvasFader>().OnFade();
                    StartCoroutine(delayLost());
                }
            }
        }

        public void OkFirstLevel()
        {
            stillPlaying = true;
            nextLevel = false;
            InitGame(currLevel);
            startLevel[currLevel + 2].gameObject.SetActive(false);
        }

        public void OkNextLevel()
        {
            if (firstTimePlaying == false)
            {
                changeLevel(3);

            } else
            {
                nextLevel = false;
                currLevel++;
                Ui.activeUI(false);

                startLevel[currLevel + 2].gameObject.SetActive(false);
                InitGame(currLevel);
                Ui.activeUI(true);
            }
            
        }
        public void Restart(){
            stillPlaying = true;
            Ui.activeUI(false);
            life = 3;
            // InitGame(currLevel);           
            currLevel--;
            changeLevel(0);
            gameOver.gameObject.SetActive(false);
            // Ui.activeUI(true);
        }

        public void changeLevel(float delayTime)
        {
			StartCoroutine(delayLoadLevel(delayTime));         
        }

		

        public void StartGame()
        {
            currLevel = -2;
            SceneManager.LoadScene("Scene", LoadSceneMode.Single);
        }

        public void ExitMenu(bool clicked){
			if (clicked == true) {
				exitMenu.gameObject.SetActive (clicked);
				mainMenu.gameObject.SetActive (false);
			} else {
				exitMenu.gameObject.SetActive (clicked);
				mainMenu.gameObject.SetActive (true);
			}

		}

		public void MainMenu(){
			SceneManager.LoadScene ("StartScene", LoadSceneMode.Single);
            Time.timeScale = 1f;
            nextLevel = false;
            Ui.activeUI(true);
        }

		public void PauseMenu(bool clicked){
			if (clicked == true) {
                Ui.activeUI(false);
                Time.timeScale = 0f;
				pauseMenu.gameObject.SetActive (clicked);
				timer = false;
			}
            else
            {
                Ui.activeUI(true);
                Time.timeScale = 1f;
                pauseMenu.gameObject.SetActive (clicked);
				timer = true;
			}
		}

        IEnumerator delayStartingLevelLoad()
        {
            Ui.activeUI(false);
            yield return new WaitForSeconds(3f);
            nextLevel = true;
            startLevel[currLevel + 2].gameObject.SetActive(nextLevel);
            startLevel[currLevel + 2].gameObject.GetComponentInChildren<Button>().image.enabled = false;
            StartCoroutine(delayShowButton(currLevel + 2));
        }

        IEnumerator delayLoadLevel(float timeDelay)
        {
            Ui.activeUI(false);
            yield return new WaitForSeconds(timeDelay);
            if (currLevel >= 0)
            {
              
                if (currLevel < 4 || currLevel > 8 && currLevel < 12)
                {
                    eggIcon.GetComponent<SpriteRenderer>().sprite = eggiconRight;                 
                } else
                {
                    eggIcon.GetComponent<SpriteRenderer>().sprite = eggiconLeft;
                }
                
               StartCoroutine(blinkBlink(levelBalls[currLevel].GetComponent<Image>().sprite));
            }

            nextLevel = true;
            startLevel [currLevel + 3].gameObject.SetActive (nextLevel);
            startLevel[currLevel + 3].gameObject.GetComponentInChildren<Button>().image.enabled = false;
            StartCoroutine(delayShowButton(currLevel + 3));
        }

        IEnumerator delayShowButton(int currLevelt)
        {
            yield return new WaitForSeconds(1f);
            startLevel[currLevelt].gameObject.GetComponentInChildren<Button>().image.enabled = true;
        }

        IEnumerator delayLost()
        {
            nextLevel = true;          
            yield return new WaitForSeconds(3f);
            gameOver.gameObject.SetActive(nextLevel);
            fader.GetComponentInChildren<canvasFader>().OffFade();
        }

        IEnumerator delayedMove(GameObject objectToMove, float XStartPostion, float YStartPostion, float XEndPostion, float YEndPostion, float timeToWait)
        {
            yield return new WaitForSeconds(timeToWait);
            objectToMove.GetComponent<TweenTransforms>().startingVector = new Vector3(XStartPostion, YStartPostion, 0);
            objectToMove.GetComponent<TweenTransforms>().endVector = new Vector3(XEndPostion, YEndPostion, 0);
        }

        IEnumerator blinkBlink(Sprite ball)
        {
            mapCanvas.gameObject.SetActive(true);
            gameCanvas.gameObject.SetActive(false);
            iTween.MoveTo(eggIcon, levelBalls[currLevel].transform.position + new Vector3(0,0.5f,0),3f);
            yield return new WaitForSeconds(0.8f);     
            levelBalls[currLevel].GetComponent<Image>().sprite = greenBall;
            yield return new WaitForSeconds(0.8f);
            levelBalls[currLevel].GetComponent<Image>().sprite = redBall;
            yield return new WaitForSeconds(0.8f);
            levelBalls[currLevel].GetComponent<Image>().sprite = greenBall;
            yield return new WaitForSeconds(0.8f);
            levelBalls[currLevel].GetComponent<Image>().sprite = redBall;
            yield return new WaitForSeconds(0.8f);
            levelBalls[currLevel].GetComponent<Image>().sprite = greenBall;
            mapCanvas.gameObject.SetActive(false);
            gameCanvas.gameObject.SetActive(true);
        }
    }
}
