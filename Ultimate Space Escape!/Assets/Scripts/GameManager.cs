using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class GameManager : MonoBehaviour
{
    [Header("Particle Systems")]
    public ParticleSystem starField;
    public ParticleSystem questionMarks;

    [Header("Prefabs")]
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject fakeballPrefab;
    [SerializeField] private GameObject brickSavingPrefab;
    [SerializeField] private GameObject buttonFireToggle;
    [SerializeField] private GameObject buttonGoopToggle;

    [Header("Texts")]
    [SerializeField] private Text scoreText;
    [SerializeField] private Text ballsText;
    [SerializeField] private Text levelText;
    public Text highscoreText;
    public Text highestLevelText;
    public Text coinsText;
    public Text coinsShopText;
    public Text text2xSpeed;
    public Text fireToggleOnText;
    public Text fireToggleOffText;
    public Text fireBallPriceText;
    public Text goopToggleOnText;
    public Text goopToggleOffText;
    public Text goopBallPriceText;
    public Text unlockPageText;
    public Text AmOnText;
    public Text AmOffText;

    [Header("Panels")]
    [SerializeField] private GameObject panelMenu;
    [SerializeField] private GameObject panelPlay;
    [SerializeField] private GameObject panelLevelCompleted;
    [SerializeField] private GameObject panelGameOver;
    [SerializeField] private GameObject panelGameOverTips;
    [SerializeField] private GameObject panelGameCompleted;
    [SerializeField] private GameObject panelTwoTimeSpeedTutorial;
    [SerializeField] private GameObject panelSaveFiles;
    [SerializeField] private GameObject panelLoadFiles;
    [SerializeField] private GameObject panelDeleteFiles;
    [SerializeField] private GameObject panelLevelSelect;
    [SerializeField] private GameObject panelLaserCannon;
    [SerializeField] private GameObject panelBestiary;
    [SerializeField] private GameObject panelCharacterBoxes;
    [SerializeField] private GameObject canvasPauseMenu;

    [Header("Buttons/UI")]
    [SerializeField] private GameObject button2xSpeed;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject MenuButton;
    [SerializeField] private GameObject image1x;
    [SerializeField] private GameObject image2x;
    public GameObject imageSoldOutFire;
    public GameObject imageSoldOutGoop;
    [SerializeField] private GameObject imageLaserActivated;

    [Header("Character Art")]
    [SerializeField] private GameObject imageAiLarge;
    [SerializeField] private GameObject imageAiSmall;
    [SerializeField] private GameObject imageMCLarge;
    [SerializeField] private GameObject imageMCSmall;

    [Header("Level Select")]
    [SerializeField] private GameObject textComeBackLater;
    [SerializeField] private GameObject[] buttonsLevels = new GameObject[20];
    private int buttonLevelCheck = 0;

    [Header("Bestiary")]
    [SerializeField] private int bPrefabs;
    [SerializeField] private int bPanels;
    [SerializeField] private GameObject buttonBestiaryForward;
    [SerializeField] private GameObject buttonBestiaryBackward;
    [SerializeField] private int[] bLevUnlock = new int[14];
    [SerializeField] private GameObject[] bestiaryPrefabs = new GameObject[28];
    [SerializeField] private GameObject[] bestiaryPanels = new GameObject[14];
   
    [Header("Script References")]
    public Currency currency;
    public Player player;
    public Ball ball;
    public FakeBall fakeBall;
    public ButtonInfoLevelSelect buttonInfoLevelSelect;



    private bool AutoSaveFile1 = true;
    private bool AutoSaveFile2 = false;
    private bool AutoSaveFile3 = false;


    [Header("Shop Ints")]
    public int[,] shopItems = new int[7, 7];
    public int ItemID;

   


    [Header("Special Level Ints")]
    public int levelsUnlocked;
    public int[,] levelID = new int[3, 22];
    public int[,] amID = new int[3, 22];
    public int[,] nBLID = new int[3, 22];

    //Is altered by the FakeBall Upgrade PV. 
    //Larger its value, the more likely a fakeball will spawn along a normal.
    private int fakeBallSpawnChance = 0;
    private int ballsLost;
    private int brickzBeforeLaser;

    private bool isMenu; //Checks if player is in the menu state.

    private bool GameObjectsDestroyed = false;

    public GameObject[] levels;

    [Header("Level Part Two0ers")]
    public GameObject level10Part2;
    public GameObject level20Part2;

    [Header("Boss Health Bars")]
    [SerializeField] private GameObject[] healthBars = new GameObject[2];

    private GameObject _currentBall;
    private GameObject _currentLevel;

    public static GameManager Instance { get; private set; }


    public enum State { MENU, INIT, PLAY, LEVELCOMPLETED, LOADLEVEL, GAMEOVER, GAMECOMPLETED }
    public State _state;

    public static bool GameIsPaused = false;
    bool _isSwitchingState;

    public int _score;
    public int Score
    {
        get { return _score; }
        set { _score = value;
            scoreText.text = "SCORE: " + _score;
        }
    }

    public int _level;
    public int Level
    {
        get { return _level; }
        set { _level = value;
            levelText.text = "LEVEL: " + _level;
        }
    }

    public int _balls;
    public int Balls
    {
        get { return _balls; }
        set { _balls = value;
            ballsText.text = "BALLS: " + _balls;
        }
    }

    public int _coins;
    public int Coins
    {
        get { return _coins; }
        set
        {
            _coins = value;
            coinsText.text = "COINS: " + _coins;
            coinsShopText.text = "COINS: " + _coins;
        }
    }

    public int _highscore;
    public int Highscore
    {
        get { return _highscore; }
        set
        {
            _highscore = value;
        }

    }


    private Vector3 Vector3(float v1, float v2, float v3) // Don't need to copy.
    {
        throw new NotImplementedException();
    }



    
    public void SaveDataTings()
    {
        panelMenu.SetActive(true);
        canvasPauseMenu.SetActive(true);
        panelSaveFiles.SetActive(false);
    }

    //When called, allows player to save their game via the SaveData and SaveSystem scripts respectively
    public void SaveData1 ()
    {
        SaveSystem.SaveGame1(this);
        SaveDataTings();

        AutoSaveFile1 = true;
        AutoSaveFile2 = false;
        AutoSaveFile3 = false;
    }

    public void SaveData2 ()
    {
        SaveSystem.SaveGame2(this);
        SaveDataTings();
        AutoSaveFile1 = false;
        AutoSaveFile2 = true;
        AutoSaveFile3 = false;
    }

    public void SaveData3 ()
    {
        SaveSystem.SaveGame3(this);
        SaveDataTings();
        AutoSaveFile1 = false;
        AutoSaveFile2 = false;
        AutoSaveFile3 = true;
    }

    public void AutoSave ()
    {
        if (AutoSaveFile1 == true)
        {
            SaveSystem.SaveGame1(this);
        }
        else if (AutoSaveFile2 == true)
        {
            SaveSystem.SaveGame2(this);
        }
        else if (AutoSaveFile3 == true)
        {
            SaveSystem.SaveGame3(this);
        }
    }


   
    //Allows for saved game to be loaded in
    public void LoadSave ()
    {
        if (SaveSystem.LoadSave1() != null)
        {
            SaveData data = SaveSystem.LoadSave1();

            _level = data._level;
            _coins = data._coins;
            ItemID = data.ItemID;
            shopItems = data.shopItems;
            _highscore = data._highscore;
            levelsUnlocked = data.levelsUnlocked;
            levelID = data.levelID;
            amID = data.amID;
            nBLID = data.nBLID;

            coinsText.text = "COINS: " + _coins;
            levelText.text = "LEVEL: " + _level;

            PlayerPrefs.SetInt("highscore", data._highscore);
            PlayerPrefs.SetInt("levelsUnlocked", data.levelsUnlocked);

            canvasPauseMenu.SetActive(true);
            panelMenu.SetActive(true);
            panelLoadFiles.SetActive(false);

            AutoSaveFile1 = true;
            AutoSaveFile2 = false;
            AutoSaveFile3 = false;

            if (Level > 0)
            {
                _score = data._score;
                _balls = data._balls;
                ballsText.text = "BALLS: " + _balls;
                scoreText.text = "SCORE: " + _score;
                FindObjectOfType<AudioManager>().Stop("MenuTheme");
                SwitchState(State.INIT);
            }

            if (data.levelsUnlocked >= 10)
            {
                player.LaserUpgrade = true;
                player.LaserCannonWait = false;
                player.NormalLaser = true;
                panelLaserCannon.SetActive(true);
            }

            LoadSaveShop();
        }
    }

    public void LoadSave2()
    {
        if (SaveSystem.LoadSave2() != null)
        {
            SaveData data2 = SaveSystem.LoadSave2();

            _level = data2._level;
            _coins = data2._coins;
            ItemID = data2.ItemID;
            shopItems = data2.shopItems;
            _highscore = data2._highscore;
            levelsUnlocked = data2.levelsUnlocked;
            levelID = data2.levelID;
            amID = data2.amID;
            nBLID = data2.nBLID;


            coinsText.text = "COINS: " + _coins;
            levelText.text = "LEVEL: " + _level;


            PlayerPrefs.SetInt("highscore", data2._highscore);
            PlayerPrefs.SetInt("levelsUnlocked", data2.levelsUnlocked);

            canvasPauseMenu.SetActive(true);
            panelMenu.SetActive(true);
            panelLoadFiles.SetActive(false);


            AutoSaveFile1 = false;
            AutoSaveFile2 = true;
            AutoSaveFile3 = false;

            if (Level >= 0)
            {
                _score = data2._score;
                _balls = data2._balls;
                ballsText.text = "BALLS: " + _balls;
                scoreText.text = "SCORE: " + _score;
                FindObjectOfType<AudioManager>().Stop("MenuTheme");
                SwitchState(State.INIT);
            }

            if (data2.levelsUnlocked >= 10)
            {
                player.LaserUpgrade = true;
                player.LaserCannonWait = false;
                player.NormalLaser = true;
                panelLaserCannon.SetActive(true);
            }
            LoadSaveShop();
        } 
    }

    public void LoadSave3()
    {
        if (SaveSystem.LoadSave3() != null)
        {
            SaveData data3 = SaveSystem.LoadSave3();

            _level = data3._level;
            _coins = data3._coins;
            ItemID = data3.ItemID;
            shopItems = data3.shopItems;
            _highscore = data3._highscore;
            levelsUnlocked = data3.levelsUnlocked;
            levelID = data3.levelID;
            amID = data3.amID;
            nBLID = data3.nBLID;


            coinsText.text = "COINS: " + _coins;
            levelText.text = "LEVEL: " + _level;

            PlayerPrefs.SetInt("highscore", data3._highscore);
            PlayerPrefs.SetInt("levelsUnlocked", data3.levelsUnlocked);

            canvasPauseMenu.SetActive(true);
            panelLoadFiles.SetActive(false);
            panelMenu.SetActive(true);


            AutoSaveFile1 = false;
            AutoSaveFile2 = false;
            AutoSaveFile3 = true;

            if (Level >= 0)
            {
                _score = data3._score;
                _balls = data3._balls;
                ballsText.text = "BALLS: " + _balls;
                scoreText.text = "SCORE: " + _score;
                FindObjectOfType<AudioManager>().Stop("MenuTheme");
                SwitchState(State.INIT);
            }

            if (data3.levelsUnlocked >= 10)
            {
                player.LaserUpgrade = true;
                player.LaserCannonWait = false;
                player.NormalLaser = true;
                panelLaserCannon.SetActive(true);
            }

            LoadSaveShop();
        }
    }

    //Allows shop upgrades to be loaded in...?
    public void LoadSaveShop()
    {
        if (shopItems[2, 3] == 3)
        {
            fireToggleOnText.enabled = true;
            buttonFireToggle.SetActive(true);
            imageSoldOutFire.SetActive(true);
            fireBallPriceText.enabled = false;
            ball.FireEffectOn();
        }

        if (shopItems[2, 5] == 3)
        {
            goopToggleOnText.enabled = true;
            buttonGoopToggle.SetActive(true);
            imageSoldOutGoop.SetActive(true);
            goopBallPriceText.enabled = false;
            ball.GoopEffectOn();
        }


        //Detects at what level the magnet upgrade was upgraded at in the save data, and restores said upgrade level.
        if (shopItems[3, 2] > 0)
        {
            currency.MagnetOn = true;

            if (shopItems[3, 2] == 1)
            {
                shopItems[2, 2] = 15;
            }
            else if (shopItems[3, 2] == 2)
            {
                player.myCollider.radius += 2f;
                shopItems[2, 2] = 25;
            }
            else if (shopItems[3, 2] == 3)
            {
                currency.step = currency.step * 2f; // calculate distance to move
                player.myCollider.radius += 1f;
                shopItems[2, 2] = 40;
            }
            else if (shopItems[3, 2] == 4)
            {
                currency.step = currency.step * 1.5f; // calculate distance to move
                player.myCollider.radius += 1f;
                shopItems[2, 2] = 0;
            }      
        }

        //Detects at what level paddle size was upgraded at in the save data, and restores said upgrade accordingly.
        if (shopItems[3, 1] > 0)
        {
            if (shopItems[3, 1] == 1)
            {
                player._rigidbody.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                player.myCollider.radius -= 1.77777777f;
                shopItems[2, 1] = 20;
            }
            else
            {
                if (shopItems[3, 1] == 2)
                {
                    player._rigidbody.transform.localScale = new Vector3(1.45f, 1.45f, 1.45f);
                    player.myCollider.radius -= 1.436781603f;
                    shopItems[2, 1] = 40;
                }
                else
                {
                    if (shopItems[3, 1] == 3)
                    {
                        player._rigidbody.transform.localScale = new Vector3(1.75f, 1.75f, 1.75f);
                        player.myCollider.radius -= 1.182266012f;
                        shopItems[2, 1] = 60;
                    }
                    else
                    {
                        if (shopItems[3, 1] == 4)
                        {
                            player._rigidbody.transform.localScale = new Vector3(2.15f, 2.15f, 2.15f);
                            player.myCollider.radius -= 1.063122923f;
                            shopItems[2, 1] = 80;
                        }
                        else
                        {
                            if (shopItems[3, 1] == 5)
                            {
                                player._rigidbody.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
                                player.myCollider.radius -= .651162792f;
                                shopItems[2, 1] = 0;
                            }
                        }
                    }
                }
            }
        }

        if (shopItems[3, 4] > 0)
        {
            if (shopItems[3, 4] == 1)
            {
                player.lr.SetWidth(3f / 4f, 3f / 4f);  //Maybe update, since code is 'obsolete'?
                player.lr.enabled = false;
                player.Laseroff = true;
                player.LaserCannonWait = false;
                player.NormalLaser = true;
                player.BetterLaser = true;
                player.EvenBetterLaser = false;
                player.TheBestLaser = false;
                shopItems[2, 4] = 30;
            }
            else
            {
                if (shopItems[3, 4] == 2)
                {
                    player.lr.SetWidth(1, 1);
                    player.lr.enabled = false;
                    player.Laseroff = true;
                    player.LaserCannonWait = false;
                    player.NormalLaser = true;
                    player.BetterLaser = true;
                    player.EvenBetterLaser = true;
                    player.TheBestLaser = false;
                    shopItems[2, 4] = 45;
                }
                else
                {
                    if (shopItems[3, 4] == 3)
                    {
                        player.NormalLaser = true;
                        player.BetterLaser = true;
                        player.EvenBetterLaser = true;
                        player.TheBestLaser = true;
                        shopItems[2, 4] = 0;
                    }
                }
            }
        }

        if (shopItems[3, 6] > 0)
        {
            if (shopItems[3, 6] == 1)
            {
                fakeBallSpawnChance = 2;
                shopItems[2, 6] = 20;
            }
            else if (shopItems[3, 6] == 2)
            {
                fakeBallSpawnChance = 3;
                shopItems[2, 6] = 30;
            }
            else if (shopItems[3, 6] == 3)
            {
                fakeBallSpawnChance = 4;
                shopItems[2, 6] = 50;
            }
            else if (shopItems[3, 6] == 4)
            {
                fakeBallSpawnChance = 6;
                shopItems[2, 6] = 75;
            }
            else if (shopItems[3, 6] == 5)
            {
                fakeBallSpawnChance = 8;
            }
        }
        MakePaddleInvisible();

    }



    //To toggle the Attract mode... Maybe put into an 'Options' Menu later down the line?
    public void AttractModeToggle()
    {
        if (ball.EasyOn == true)
        {
            ball.EasyOn = false;
            fakeBall.EasyOn = false;
            AmOnText.enabled = false;
            AmOffText.enabled = true;
        }
        else
        {
            AmOnText.enabled = true;
            AmOffText.enabled = false;
            ball.EasyOn = true;
            fakeBall.EasyOn = true;
        }
    }

    //Is called upon opening the level select menu, and regulates which level select buttons are to be visible.
    public void LevelSelectMenu()
    {
        canvasPauseMenu.SetActive(false);
        panelMenu.SetActive(false);
        panelLevelSelect.SetActive(true);
        MakePaddleInvisible();
        Level = 0;
        if (_currentLevel != null)
        {
            Destroy(_currentLevel);
        }
        if (levelsUnlocked == 0)
        {
            textComeBackLater.SetActive(true);
        }
        else
        {
            textComeBackLater.SetActive(false);
            foreach (GameObject level in buttonsLevels)
            {
                if (levelsUnlocked > buttonLevelCheck)
                {
                    level.SetActive(true);
                    buttonLevelCheck++;
                }
                else
                {
                    buttonLevelCheck = 0;
                    return;
                }
            }
        }     
    }

    //Is called when any of the level select buttons are clicked.
    //Regulates which level is being selected, and begins to load said level accordingly.
    public void StartSelectedLevel()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;

        if (levelsUnlocked >= levelID[1, ButtonRef.GetComponent<ButtonInfoLevelSelect>().LevelID])
        {
            int LevelLoad = ButtonRef.GetComponent<ButtonInfoLevelSelect>().LevelID;

            canvasPauseMenu.SetActive(true);
            panelLevelSelect.SetActive(false);

            Level = LevelLoad;
            Score = 0;
            Balls = 4;
            SwitchState(State.INIT);

        }
        else
        {
            return;
        }
    }

    //Void that's called when Bestiary button is pressed on main menu.
    //Controls and regulates everything that happens open opening.
    //All of the 'GoTo' public voids below are attached to the buttons; they make sure the screen of its item is set up right.
    public void BestiaryMenu ()
    {
        if (_currentLevel != null)
        {
            Destroy(_currentLevel);
        }
        starField.Stop();
        bPrefabs = 0; //0 would be the locked prefab of blue brickz
        bPanels = 0; // 0 would be a nothing panel.

        canvasPauseMenu.SetActive(false);
        panelMenu.SetActive(false);
        panelBestiary.SetActive(true);

        foreach (GameObject panel in bestiaryPanels)
        {
            panel.SetActive(false);
        }
        GetRidOfBestiary();
        MakePaddleInvisible();
        if (levelsUnlocked >= bLevUnlock[bPanels])
        {
            questionMarks.gameObject.SetActive(true);
            bestiaryPanels[bPanels].SetActive(true);
            Instantiate(bestiaryPrefabs[bPrefabs+1]);
            unlockPageText.enabled = false;
            buttonBestiaryBackward.SetActive(false);
            buttonBestiaryForward.SetActive(true);
            bPrefabs++;
        }
        else
        {
            Instantiate(bestiaryPrefabs[bPrefabs]);
            unlockPageText.enabled = true;
        }
    }

    public void GoForwardInBestiary()
    {
        if (bPanels >= bestiaryPanels.Length)
        {
            return;
        }
        else
        {
            GetRidOfBestiary();
            bestiaryPanels[bPanels].SetActive(false);
            if (levelsUnlocked >= bLevUnlock[(bPanels + 1)])
            {
                Instantiate(bestiaryPrefabs[bPrefabs + 2]);
                bestiaryPanels[bPanels + 1].SetActive(true);
                unlockPageText.enabled = false;
            }
            else
            {
                Instantiate(bestiaryPrefabs[bPrefabs + 1]);
                unlockPageText.enabled = true;
                unlockPageText.text = "Beat Level " + (bLevUnlock[(bPanels + 1)] - 1) + " to Unlock This Page!";
            }
            bPanels += 1;
            bPrefabs += 2;
            if (bPanels == bestiaryPanels.Length - 1)
            {
                buttonBestiaryForward.SetActive(false);
            }
            buttonBestiaryBackward.SetActive(true);
        }
    }

    public void GoBackwardInBestiary()
    {
        if (bPanels == 0)
        {
            return;
        }    
        else
        {
            GetRidOfBestiary();
            bestiaryPanels[bPanels].SetActive(false);
            if (levelsUnlocked >= bLevUnlock[(bPanels - 1)])
            {
                Instantiate(bestiaryPrefabs[bPrefabs - 2]);
                bestiaryPanels[bPanels - 1].SetActive(true);
                unlockPageText.enabled = false;
            }
            else
            {
                Instantiate(bestiaryPrefabs[bPrefabs - 1]);
                unlockPageText.enabled = true;
                unlockPageText.text = "Beat Level " + (bLevUnlock[(bPanels - 1)] -1) + " to Unlock This Page!";
            }
            bPanels -= 1;
            bPrefabs -= 2;
            if (bPanels == 0)
            {
                buttonBestiaryBackward.SetActive(false);
            }
            buttonBestiaryForward.SetActive(true);
        }
    }

    public void SaveFileMenu ()
    {
        MakePaddleInvisible();
        if (_currentLevel != null)
        {
            Destroy(_currentLevel);
        }
        canvasPauseMenu.SetActive(false);
        panelMenu.SetActive(false);
        panelSaveFiles.SetActive(true);

    }

    public void LoadFileMenu ()
    {
        MakePaddleInvisible();
        if (_currentLevel != null)
        {
            Destroy(_currentLevel);
        }
        canvasPauseMenu.SetActive(false);
        pauseMenuUI.SetActive(false);
        panelMenu.SetActive(false);
        panelLoadFiles.SetActive(true);

    }

    public void DeleteFileMenu ()
    {
        canvasPauseMenu.SetActive(false);
        panelMenu.SetActive(false);
        panelSaveFiles.SetActive(false);
        panelLoadFiles.SetActive(false);
        panelDeleteFiles.SetActive(true);
    }

    public void GoFromDeleteFileMenuToLoadFileMenu ()
    {
        canvasPauseMenu.SetActive(false);
        panelMenu.SetActive(false);
        panelLoadFiles.SetActive(true);
        panelDeleteFiles.SetActive(false);
        panelLevelSelect.SetActive(false);
    }

    public void GoFromDeleteFileMenuToSaveFileMenu()
    {
        canvasPauseMenu.SetActive(false);
        panelMenu.SetActive(false);
        panelSaveFiles.SetActive(true);
        panelDeleteFiles.SetActive(false);
        panelLevelSelect.SetActive(false);
    }

    public void GoToMainMenu ()
    {
        MakePaddleInvisible();
        starField.Play();
        questionMarks.gameObject.SetActive(false);
        questionMarks.Stop();
        canvasPauseMenu.SetActive(false);
        panelLoadFiles.SetActive(false);
        panelDeleteFiles.SetActive(false);
        panelSaveFiles.SetActive(false);
        panelBestiary.SetActive(false);
        GetRidOfBestiary();
        panelMenu.SetActive(true);
        buttonBestiaryBackward.SetActive(false);
        buttonBestiaryForward.SetActive(false);
    }

    public void GoToMainMenuFromLevelSelect()
    {
        canvasPauseMenu.SetActive(false);
        panelMenu.SetActive(true);
        panelLevelSelect.SetActive(false);
    }


    //Allows Files to Be Deleted, resetting all progress in said save file. After deleting 1 save file, it takes user back to load menu.
    public void DeleteFile1 ()
    {
        string path = Application.persistentDataPath + "/savefile1.fun";
        if (File.Exists(path))
        {
            File.Delete(path);
            panelLoadFiles.SetActive(true);
            panelDeleteFiles.SetActive(false);
        }
        else
        {
            return;
        }
    }
    public void DeleteFile2 ()
    {
        string path2 = Application.persistentDataPath + "/savefile2.fun";
        if (File.Exists(path2))
        {
            File.Delete(path2);
            panelLoadFiles.SetActive(true);
            panelDeleteFiles.SetActive(false);
        }
        else
        {
            return;
        }
    }
    public void DeleteFile3()
    {
        string path3 = Application.persistentDataPath + "/savefile3.fun";
        if (File.Exists(path3))
        {
            File.Delete(path3);
            panelLoadFiles.SetActive(true);
            panelDeleteFiles.SetActive(false);
        }
        else
        {
            return;
        }
    }


    //Public void that can be attached to button, but is instead used as part of another public void
    public void Resume()
    {
        image1x.SetActive(true);
        Cursor.visible = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    //Publio void that can be attached to a button, so that INIT gamestate can be activated
    public void PlayClicked()
    {
        SwitchState(State.INIT);

    }

    //Publio void that can be attached to a button, so that MENU gamestate can be activated and the game can be resumed
    public void MenuButtonClicked()
    {
        Resume();
        SwitchState(State.GAMEOVER);
    }

    //When the game starts, this code lets the game start on the Menu state
    void Start()
    {
        player._rigidbody.transform.localScale = new Vector3(1f, 1f, 1f);
        FindObjectOfType<AudioManager>().Play("MenuTheme");
        Instance = this;
        PlayerPrefs.DeleteKey("highscore");
        levelsUnlocked = 0;
        questionMarks.gameObject.SetActive(false);
        player.lr.SetWidth(1f/2f, 1f/2f); //Maybe update later, since code is 'obsolete'?

        canvasPauseMenu.SetActive(false);
        // ballPrefab.transform.localScale += new Vector3(0f, 0f, 0f);
        coinsText.text = "COINS" + _coins.ToString();
        coinsShopText.text = "COINS" + _coins.ToString();
        currency.MagnetOn = false;
        ball.FireEffectOff();
        ball.GoopEffectOff();
        ball.EasyOn = false;
        fakeBall.EasyOn = false;
        fireToggleOnText.enabled = true;
        fireToggleOffText.enabled = false;
        goopToggleOnText.enabled = true;
        goopToggleOffText.enabled = false;
        AmOnText.enabled = false;
        AmOffText.enabled = true;
        imageLaserActivated.SetActive(false);
        currency.step = 5f * 2f * Time.deltaTime;
        player.myCollider.radius = 10f;


        //These 6 lines ensure that the laser upgrade is turned off. However, if it has been unlocked in a save, laser upgrade will be unlocked upon loading.
        player.NormalLaser = true;
        player.BetterLaser = false;
        player.EvenBetterLaser = false;
        player.TheBestLaser = false;
        player.LaserUpgrade = false;
        brickzBeforeLaser = 10;
        panelLaserCannon.SetActive(false);


        //Shop ID's
        shopItems[1, 1] = 1;
        shopItems[1, 2] = 2;
        shopItems[1, 3] = 3;
        shopItems[1, 4] = 4;
        shopItems[1, 5] = 5;
        shopItems[1, 6] = 6;

        //Shop Price
        shopItems[2, 1] = 10;
        shopItems[2, 2] = 5;
        shopItems[2, 3] = 16;
        shopItems[2, 4] = 20;
        shopItems[2, 5] = 8;
        shopItems[2, 6] = 10;

        //Shop Quantity
        shopItems[3, 1] = 0;
        shopItems[3, 2] = 0;
        shopItems[3, 3] = 0;
        shopItems[3, 4] = 0;
        shopItems[3, 5] = 0;
        shopItems[3, 6] = 0;
        //If save file exist, automatically load save file when game starts. May remove, and instead allow three save files. 

        panelLevelSelect.SetActive(false);

        //Level Select Button IDS
        levelID[1, 1] = 1;
        levelID[1, 2] = 2;
        levelID[1, 3] = 3;
        levelID[1, 4] = 4;
        levelID[1, 5] = 5;
        levelID[1, 6] = 6;
        levelID[1, 7] = 7;
        levelID[1, 8] = 8;
        levelID[1, 9] = 9;
        levelID[1, 10] = 10;
        levelID[1, 11] = 11;
        levelID[1, 12] = 12;
        levelID[1, 13] = 13;
        levelID[1, 14] = 14;
        levelID[1, 15] = 15;
        levelID[1, 16] = 16;
        levelID[1, 17] = 17;
        levelID[1, 18] = 18;
        levelID[1, 19] = 19;
        levelID[1, 20] = 20;
        levelID[1, 21] = 21;

        //Attract Mode Level Pin IDs
        amID[1, 1] = 0;
        amID[1, 2] = 0;
        amID[1, 3] = 0;
        amID[1, 4] = 0;
        amID[1, 5] = 0;
        amID[1, 6] = 0;
        amID[1, 7] = 0;
        amID[1, 8] = 0;
        amID[1, 9] = 0;
        amID[1, 10] = 0;
        amID[1, 11] = 0;
        amID[1, 12] = 0;
        amID[1, 13] = 0;
        amID[1, 14] = 0;
        amID[1, 15] = 0;
        amID[1, 16] = 0;
        amID[1, 17] = 0;
        amID[1, 18] = 0;
        amID[1, 19] = 0;
        amID[1, 20] = 0;
        amID[1, 21] = 0;

        //
        nBLID[1, 1] = 0;
        nBLID[1, 2] = 0;
        nBLID[1, 3] = 0;
        nBLID[1, 4] = 0;
        nBLID[1, 5] = 0;
        nBLID[1, 6] = 0;
        nBLID[1, 7] = 0;
        nBLID[1, 8] = 0;
        nBLID[1, 9] = 0;
        nBLID[1, 10] = 0;
        nBLID[1, 11] = 0;
        nBLID[1, 12] = 0;
        nBLID[1, 13] = 0;
        nBLID[1, 14] = 0;
        nBLID[1, 15] = 0;
        nBLID[1, 16] = 0;
        nBLID[1, 17] = 0;
        nBLID[1, 18] = 0;
        nBLID[1, 19] = 0;
        nBLID[1, 20] = 0;
        nBLID[1, 21] = 0;

        Instantiate(playerPrefab);
        StartCoroutine(CheckForSaveFiles());

    }

    IEnumerator CheckForSaveFiles()
    {
        yield return new WaitForSeconds(1f/10f);
        string path = Application.persistentDataPath + "/savefile1.fun";
        string path2 = Application.persistentDataPath + "/savefile2.fun";
        string path3 = Application.persistentDataPath + "/savefile3.fun";
        if (File.Exists(path))
        {
            LoadFileMenu();
        }
        else
        {
            if (File.Exists(path2))
            {
                LoadFileMenu();
            }
            else
            {
                if (File.Exists(path3))
                {
                    LoadFileMenu();
                }
                else
                {
                    SwitchState(State.MENU);
                }
            }
        }
        yield return null;
    }

    //When this is called, the game state can be switched
    public void SwitchState(State newState, float delay = 0)
    {
        StartCoroutine(SwitchDelay(newState, delay));
    }

    
    //Allows for a delay inbetween gamestates
    IEnumerator SwitchDelay(State newState, float delay)
    {
        _isSwitchingState = true;
        yield return new WaitForSeconds(delay);
        EndState();
        _state = newState;
        BeginState(newState);
        _isSwitchingState = false;
    }

    void BeginState(State newState)
    {
        switch (newState)
        {
            case State.MENU:
                MakePaddleInvisible();
                isMenu = true;
                highscoreText.text = "HIGHSCORE: " + PlayerPrefs.GetInt("highscore");
                highestLevelText.text = "HIGHEST LEVEL REACHED: " + levelsUnlocked;
                panelTwoTimeSpeedTutorial.SetActive(false);
                FindObjectOfType<AudioManager>().Play("MenuTheme");
                Cursor.visible = true;
                panelMenu.SetActive(true);
                Time.timeScale = 1f;
                image2x.SetActive(false);
                image1x.SetActive(true);
                panelPlay.SetActive(false);
                panelGameOver.SetActive(false);
                Level = 0;
                Score = 0;
                ballsLost = 0;
                break;
            case State.INIT:
                isMenu = false;
                canvasPauseMenu.SetActive(true);
                Cursor.visible = false;
                panelPlay.SetActive(true);
                //If Level is 1 or less, change Level to be 1
                if (Level <= 1)
                {
                    Level = 1;
                }
                //Allows tutorial panel to only show on first level
                if (Level == 1)
                {
                    Balls = 5;
                    Score = 0;
                }
                button2xSpeed.SetActive(false);
                //Allows for level progression
                if (_currentLevel != null)
                {
                    Destroy(_currentLevel);
                }
                DestroyCurrentPaddle();
                SwitchState(State.LOADLEVEL);
                break;
            case State.PLAY:
                isMenu = false;
                if (Level >= 1)
                {
                    if (Level <= 5)
                    {
                        FindObjectOfType<AudioManager>().NormalizeVolume("Levels1-5Theme");
                        FindObjectOfType<AudioManager>().Play("Levels1-5Theme");
                    }  
                }
                button2xSpeed.SetActive(true);
                ballsLost = 0;
                _currentBall = Instantiate(ballPrefab);
                player.Laseroff = true;

                //Allows text describing what the 2x Speed button does, to only show on Level 1
                if (Level == 1)
                {
                    panelTwoTimeSpeedTutorial.SetActive(true);
                }
                break;
            case State.LEVELCOMPLETED:
                canvasPauseMenu.SetActive(false);
                DestroyCurrentPaddle();
                panelTwoTimeSpeedTutorial.SetActive(false);
                panelLevelCompleted.SetActive(true);
                Instantiate(brickSavingPrefab);
                image1x.SetActive(true);
                image2x.SetActive(false);
                text2xSpeed.enabled = false;
                button2xSpeed.SetActive(false);
                imageLaserActivated.SetActive(false);
                Destroy(_currentBall);
                Destroy(_currentLevel);
                FindObjectOfType<AudioManager>().Play("LevelCompleted!");
                if (ballsLost == 0)
                {
                    nBLID[1, Level] = 1;
                }
                if (ball.EasyOn == true)
                {
                    if (amID[1, Level] < 2)
                    {
                        amID[1, Level] = 1;
                    }
                }
                else
                {
                    amID[1, Level] = 2;
                }
                if (Level == 20)
                {
                    healthBars[1].SetActive(false);
                }
                Level++;
                Time.timeScale = 1f;
                //Saves game data after every level completed.
                AutoSave();
                if (Level >= 0)
                {
                    if (Level <= 6)
                    {
                        FindObjectOfType<AudioManager>().Stop("Levels1-5Theme");
                    }
                    if (Level > 10)
                    {
                        if (player.LaserUpgrade != true)
                        {
                            DestroyCurrentPaddle();
                            panelLaserCannon.SetActive(true);
                        }
                    }
                }
                GetRidOfSpecialGameObjects();
                SwitchState(State.LOADLEVEL, 3f);
                break;
            case State.LOADLEVEL:
                //Detects when the player has completed the game
                if (Level >= levels.Length)
                {
                    SwitchState(State.GAMECOMPLETED);
                    button2xSpeed.SetActive(false);
                }
                else
                {
                    _currentLevel = Instantiate(levels[Level]);
                    GameObjectsDestroyed = true;
                    SwitchState(State.PLAY);
                }
                break;
            case State.GAMEOVER:
                canvasPauseMenu.SetActive(false);
                imageLaserActivated.SetActive(false);
                FindObjectOfType<AudioManager>().Stop("Levels1-5Theme");
                FindObjectOfType<AudioManager>().Play("GameEnd");
                if (Level == 10)
                {
                    Destroy(GameObject.Find("LBB 1st(Clone)"));
                    Destroy(GameObject.Find("LBB 2nd(Clone)"));
                    Destroy(GameObject.Find("LBB 3rd(Clone)"));
                }
                //Updates highscore in Menu screen if the Player has reached a new highscore
                if (Score > PlayerPrefs.GetInt("highscore"))
                {
                    PlayerPrefs.SetInt("highscore", Score);
                }
                if (Level > levelsUnlocked)
                {
                    levelsUnlocked = Level;
                }
                if (Level == 20)
                {
                    Destroy(GameObject.FindGameObjectWithTag("Shockwave"));
                }
                panelGameOver.SetActive(true);
                panelGameOverTips.SetActive(false);
                Destroy(_currentBall);
                Time.timeScale = 1f;
                GetRidOfSpecialGameObjects();
                break;
            case State.GAMECOMPLETED:
                FindObjectOfType<AudioManager>().Play("");
                if (Score > PlayerPrefs.GetInt("highscore"))
                {
                    PlayerPrefs.SetInt("highscore", Score);
                }
                if (Level > levelsUnlocked)
                {
                    levelsUnlocked = Level;
                }
                panelGameCompleted.SetActive(true);
                GetRidOfSpecialGameObjects();
                break;
        }
    }
    private void FixedUpdate()
    {
        switch (_state)
        {
            case State.PLAY:

               
                //Detects when the player can use the laser
                if (_currentLevel.transform.childCount <= brickzBeforeLaser)
                {
                    if (GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().LaserCannonWait == true)
                    {
                        imageLaserActivated.SetActive(false);
                    }
                    else
                    {
                        imageLaserActivated.SetActive(true);
                    }
                    if (player.LaserUpgrade == false)
                    {
                        player.Laseroff = true;
                        player.LaserUpgrade = true;
                        player.LaserCannonWait = false;
                        DestroyCurrentPaddle();
                        imageLaserActivated.SetActive(true);
                    }
                }
                else
                {
                    if (player.LaserUpgrade == true)
                    {
                        player.LaserUpgrade = false;
                        player.Laseroff = false;
                        player.LaserCannonWait = true;
                        DestroyCurrentPaddle();
                        imageLaserActivated.SetActive(false);
                    }
                }
                break;
        }
       
    }
    void Update()
    {
        coinsText.text = "COINS: " + _coins;
        coinsShopText.text = "COINS: " + _coins;

        switch (_state)
            {
            case State.MENU:
                break;
            case State.INIT:
                break;
            case State.PLAY:
                

               
                //Tells the game what to do when the current ball is deleted
                if (_currentBall == null)
                {
                    if (Balls > 0)
                    {
                        //Simulates a randomizer that determines whether a FakeBall will spawn
                        //alongside normal one. Only does anything after its upgrade it bought.
                        BallSpawn();
                    }
                    //Makes the game end if the player runs out of balls
                    else
                    {
                        SwitchState(State.GAMEOVER);
                    }
                }

                //Detects when the player completes the level, switching the gamestate accordingly
                if (_currentLevel != null && _currentLevel.transform.childCount == 0 && !_isSwitchingState)
                {
                    if (Level == 10)
                    {
                        //May remove, if so, switch back to Switch Gamestate to Level Complete, or game complete if the #of levels have ran out.
                        StartCoroutine(WaitingForBoss());

                    }
                    else if (Level == 20 && _currentLevel.name == "Level 20(Clone)")
                    {
                        Destroy(_currentLevel);
                        Instantiate(level20Part2);
                        _currentLevel = GameObject.Find("Level 20 Part 2(Clone)");
                        FindObjectOfType<AudioManager>().Play("");
                        healthBars[1].SetActive(true);
                    }
                    else
                    {
                        SwitchState(State.LEVELCOMPLETED);
                    }
                }
                break;
            case State.LEVELCOMPLETED:               
                break;
            case State.LOADLEVEL:
                break;
            case State.GAMEOVER:
                if (GameObjectsDestroyed == true)
                {
                    panelGameOverTips.SetActive(true);
                    if (Input.anyKeyDown)
                    {
                        SwitchState(State.MENU);
                    }
                }
                break;
            case State.GAMECOMPLETED:
                if (GameObjectsDestroyed == true)
                {
                    if (Input.anyKeyDown)
                    {
                        SwitchState(State.MENU);
                    }
                }
                break;
            }   
    }

    void EndState()
    {
            switch (_state)
            {
                case State.MENU:
                canvasPauseMenu.SetActive(true);
                panelMenu.SetActive(false);
                FindObjectOfType<AudioManager>().Stop("MenuTheme");
                GameObject.FindGameObjectWithTag("Player").GetComponent<MeshRenderer>().enabled = true;
                isMenu = false;
                break;
                case State.INIT:
                    break;
                case State.PLAY:
                break;
                case State.LEVELCOMPLETED:
                button2xSpeed.SetActive(true);
                canvasPauseMenu.SetActive(true);
                panelLevelCompleted.SetActive(false);
                Destroy(GameObject.Find("BrickSaving(Clone)"));
                break;
                case State.LOADLEVEL:
                    break;
                case State.GAMEOVER:
                panelPlay.SetActive(false);
                panelGameOver.SetActive(false);
                if (Level == 10)
                {
                    Destroy(GameObject.Find("LBB 2nd(Clone)"));
                    Destroy(GameObject.Find("LBB 3rd(Clone)"));
                }
                Destroy(_currentBall);
                    break;
                case State.GAMECOMPLETED:
                panelGameCompleted.SetActive(false);
                break;
            }
    }




    //Waiting for the Boss... (May Remove; Experimental)
    public IEnumerator WaitingForBoss()
    {
        var LBB2 = GameObject.Find("LBB 2nd(Clone)");
        var LBB3 = GameObject.Find("LBB 3rd(Clone)");


        yield return new WaitForSeconds(2);
        if (_currentLevel == null)
        {
            yield return null;
        }
        else
        {
            if (_currentLevel.transform.childCount == 0 && !_isSwitchingState)
            {
                if (LBB2)
                {
                    if (LBB3)
                    {
                        if (!LBB3)
                        {
                            SwitchState(State.LEVELCOMPLETED);
                            yield return null;
                        }
                    }
                }
                else
                {
                    SwitchState(State.LEVELCOMPLETED);
                    yield return null;
                }
            }
        }
    }

    // Public void that, when attached to buttons, allows for the Paddle Size upgrade to be 'bought'
    public void BuyPaddleSizeIncrease()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;

        if (_coins >= shopItems[2, ButtonRef.GetComponent<ButtonInfo>().ItemID])
        {
            // System to increase upgrade cost for Paddle Size Increase after each upgrade, may change into increase system for all using 3, ItemID
            if (shopItems[3, 1] == 0)
            {
                _coins -= shopItems[2, ButtonRef.GetComponent<ButtonInfo>().ItemID];
                player._rigidbody.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                player.myCollider.radius -= 1.77777777f;
                shopItems[2, 1] = 20;
                DestroyCurrentPaddle();
                StartCoroutine(WaiterShipSizeVA());


            }
            else if (shopItems[3, 1] == 1)
            {
                _coins -= shopItems[2, ButtonRef.GetComponent<ButtonInfo>().ItemID];
                player._rigidbody.transform.localScale = new Vector3(1.45f, 1.45f, 1.45f);
                player.myCollider.radius -= 1.436781603f;
                shopItems[2, 1] = 40;
                DestroyCurrentPaddle();
            }
            else if (shopItems[3, 1] == 2)
            {
                _coins -= shopItems[2, ButtonRef.GetComponent<ButtonInfo>().ItemID];
                player._rigidbody.transform.localScale = new Vector3(1.75f, 1.75f, 1.75f);
                player.myCollider.radius -= 1.182266012f;
                shopItems[2, 1] = 60;
                DestroyCurrentPaddle();
            }
            else if (shopItems[3, 1] == 3)
            {
                _coins -= shopItems[2, ButtonRef.GetComponent<ButtonInfo>().ItemID];
                player._rigidbody.transform.localScale = new Vector3(2.15f, 2.15f, 2.15f);
                player.myCollider.radius -= 1.063122923f;
                shopItems[2, 1] = 80;
                DestroyCurrentPaddle();
            }
            else if (shopItems[3, 1] == 4)
            {
                _coins -= shopItems[2, ButtonRef.GetComponent<ButtonInfo>().ItemID];
                player._rigidbody.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
                player.myCollider.radius -= .651162792f;
                DestroyCurrentPaddle();
                shopItems[3, ButtonRef.GetComponent<ButtonInfo>().ItemID]++;
                ButtonRef.GetComponent<ButtonInfo>().QuantityText.text = shopItems[3, ButtonRef.GetComponent<ButtonInfo>().ItemID].ToString();
                shopItems[2, 1] = 0;
                AutoSave();
            }
            if (shopItems[2, 1] == 0)
            {
                return;
            }


            //Updates #of items the player has, or level of upgrade
            ButtonRef.GetComponent<ButtonInfo>().QuantityText.text = shopItems[3, ButtonRef.GetComponent<ButtonInfo>().ItemID].ToString();
            shopItems[3, ButtonRef.GetComponent<ButtonInfo>().ItemID]++;
            AutoSave();

        }
        else
        {
            return;
        }
    }

    //Destroys the current paddle whenever an upgrade is bought that involves the paddle model needing to be updated with a new version of the prefab
    public void DestroyCurrentPaddle()
    {
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        if (isMenu == true)
        {
            player.MeshInvisible();
            Instantiate(playerPrefab); //MeshInvisible()n and MeshVisible() may not be necessary?
            player.MeshVisible();
        }
        else
        {
            Instantiate(playerPrefab);
        }
    }

    //Makes the current paddle on the screen invisible, to make ui look cleaner.
    public void MakePaddleInvisible()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<MeshRenderer>().enabled = false;
        }
    }


    //Allows the Magnet Uograde in the Shop to activate the sphere collider, as wall as increase its radius (and hopefully attraction force as well)
    public void BuyMagnetSizeIncrease()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;

        if (_coins >= shopItems[2, ButtonRef.GetComponent<ButtonInfo>().ItemID])
        {

            // System to increase upgrade cost for Paddle Size Increase after each upgrade, may change into increase system for all using 3, ItemID
            if (shopItems[3, 2] == 0)
            {
                _coins -= shopItems[2, ButtonRef.GetComponent<ButtonInfo>().ItemID];
                currency.MagnetOn = true;
                shopItems[2, 2] = 15;
            }
            else if (shopItems[3, 2] == 1)
            {
                _coins -= shopItems[2, ButtonRef.GetComponent<ButtonInfo>().ItemID];
                currency.MagnetOn = true;
                player.myCollider.radius += 2f;
                shopItems[2, 2] = 25;
                DestroyCurrentPaddle();
            }
            else if (shopItems[3, 2] == 2)
            {
                _coins -= shopItems[2, ButtonRef.GetComponent<ButtonInfo>().ItemID];
                currency.step = currency.step * 2f; // calculate distance to move
                player.myCollider.radius += 1f;
                currency.MagnetOn = true;
                shopItems[2, 2] = 40;
                DestroyCurrentPaddle();
            }
            else if (shopItems[3, 2] == 3)
            {
                _coins -= shopItems[2, ButtonRef.GetComponent<ButtonInfo>().ItemID];
                currency.step = currency.step * 1.5f; // calculate distance to move.
                player.myCollider.radius += 1f;
                DestroyCurrentPaddle();
                ButtonRef.GetComponent<ButtonInfo>().QuantityText.text = shopItems[3, ButtonRef.GetComponent<ButtonInfo>().ItemID].ToString();
                shopItems[3, ButtonRef.GetComponent<ButtonInfo>().ItemID]++;
                shopItems[2, 2] = 0;
                AutoSave();
            }

            if (shopItems[2, 2] == 0)
            {
                return;
            }

            //Updates #of items the player has, or level of upgrade.
            ButtonRef.GetComponent<ButtonInfo>().QuantityText.text = shopItems[3, ButtonRef.GetComponent<ButtonInfo>().ItemID].ToString();
            shopItems[3, ButtonRef.GetComponent<ButtonInfo>().ItemID]++;
            AutoSave();
        }
        else
        {
            return;
        }
    }


    //Allows Fire Ball Particle Effect to be Bought At the Shop.
    public void BuyFireBallEffect()
    {
        if (shopItems[2, 3] == 3)
        {
            return;
        }
        else if (_coins >= shopItems[2, 3])
        {
            _coins -= shopItems[2, 3];
            fireToggleOnText.enabled = true;
            ball.FireEffectOn();
            buttonFireToggle.SetActive(true);
            imageSoldOutFire.SetActive(true);
            fireBallPriceText.enabled = false;
            StartCoroutine(WaiterFireBallVA());
            shopItems[2, 3] = 3;
            AutoSave();
        }
    }

    //Allows Goop Ball Particle Effect to be Bought At the Shop.
    public void BuyGoopBallEffect()
    {
        if (shopItems[2, 5] == 3)
        {
            return;
        }
        else if (_coins >= shopItems[2, 5])
        {
            _coins -= shopItems[2, 5];
            goopToggleOnText.enabled = true;
            ball.GoopEffectOn();
            buttonGoopToggle.SetActive(true);
            imageSoldOutGoop.SetActive(true);
            goopBallPriceText.enabled = false;
            // StartCoroutine(WaiterGoopBallVA());
            shopItems[2, 5] = 3;
            AutoSave();
        }
    }

    //Where all of the cool things happen when you upgrade the laser cannon!
    public void BuyLaserCannon()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;

        if (player.LaserUpgrade == true)
        {
            if (_coins >= shopItems[2, ButtonRef.GetComponent<ButtonInfo>().ItemID])
            {
                if (shopItems[3, 4] == 0)
                {
                    brickzBeforeLaser = 12;
                    _coins -= shopItems[2, ButtonRef.GetComponent<ButtonInfo>().ItemID];
                    player.lr.SetWidth(3f/4f,3f/4f);  //Maybe update, since code is 'obsolete'?
                    player.lr.enabled = false;
                    player.Laseroff = true;
                    player.LaserCannonWait = false;
                    player.NormalLaser = true;
                    player.BetterLaser = true;
                    player.EvenBetterLaser = false;
                    player.TheBestLaser = false;
                    DestroyCurrentPaddle();
                    shopItems[2, 4] = 30;
                }
                else if (shopItems[3,4] == 1)
                {
                    brickzBeforeLaser = 15;
                    _coins -= shopItems[2, ButtonRef.GetComponent<ButtonInfo>().ItemID];
                    player.lr.SetWidth(1, 1);
                    player.lr.enabled = false;
                    player.Laseroff = true;
                    player.LaserCannonWait = false;
                    player.NormalLaser = true;
                    player.BetterLaser = true;
                    player.EvenBetterLaser = true;
                    player.TheBestLaser = false;
                    DestroyCurrentPaddle();
                    shopItems[2, 4] = 45;
                }
                else if (shopItems[3,4] == 2)
                {
                    brickzBeforeLaser = 17;
                    _coins -= shopItems[2, ButtonRef.GetComponent<ButtonInfo>().ItemID];
                    player.lr.SetWidth(5f/4f,5f /4f);
                    DestroyCurrentPaddle();
                    player.lr.enabled = false;
                    player.Laseroff = true;
                    player.LaserCannonWait = false;
                    shopItems[2, 4] = 60;
                }
                else if (shopItems[3,4] == 3)
                {
                    brickzBeforeLaser = 20;
                    _coins -= shopItems[2, ButtonRef.GetComponent<ButtonInfo>().ItemID];
                    player.NormalLaser = true;
                    player.BetterLaser = true;
                    player.EvenBetterLaser = true;
                    player.TheBestLaser = true;
                    ButtonRef.GetComponent<ButtonInfo>().QuantityText.text = shopItems[3, ButtonRef.GetComponent<ButtonInfo>().ItemID].ToString();
                    shopItems[3, ButtonRef.GetComponent<ButtonInfo>().ItemID]++;
                    shopItems[2, 4] = 0;
                    AutoSave();
                }

                if (shopItems[2,4] == 0)
                {
                    return;
                }

                //Updates #of items the player has, or level of upgrade
                ButtonRef.GetComponent<ButtonInfo>().QuantityText.text = shopItems[3, ButtonRef.GetComponent<ButtonInfo>().ItemID].ToString();
                shopItems[3, ButtonRef.GetComponent<ButtonInfo>().ItemID]++;
                AutoSave();
            }
        }
        else
        {
            return;
        }
        


    }

    public void BuyFakeBallSpawn()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
       
        if (_coins >= shopItems[2, ButtonRef.GetComponent<ButtonInfo>().ItemID])
        {
            if (shopItems[3, 6] == 0)
            {
                _coins -= shopItems[2, ButtonRef.GetComponent<ButtonInfo>().ItemID];
                fakeBallSpawnChance = 2;
                shopItems[2, 6] = 20;
            }
            else if (shopItems[3, 6] == 1)
            {
                _coins -= shopItems[2, ButtonRef.GetComponent<ButtonInfo>().ItemID];
                fakeBallSpawnChance = 3;
                shopItems[2, 6] = 30;
            }
            else if (shopItems[3, 6] == 2)
            {
                _coins -= shopItems[2, ButtonRef.GetComponent<ButtonInfo>().ItemID];
                fakeBallSpawnChance = 4;
                shopItems[2, 6] = 50;
            }
            else if (shopItems[3, 6] == 3)
            {
                _coins -= shopItems[2, ButtonRef.GetComponent<ButtonInfo>().ItemID];
                fakeBallSpawnChance = 6;
                shopItems[2, 6] = 75;
            }
            else if (shopItems[3, 6] == 4)
            {
                _coins -= shopItems[2, ButtonRef.GetComponent<ButtonInfo>().ItemID];
                fakeBallSpawnChance = 8;
                ButtonRef.GetComponent<ButtonInfo>().QuantityText.text = shopItems[3, ButtonRef.GetComponent<ButtonInfo>().ItemID].ToString();
                shopItems[3, ButtonRef.GetComponent<ButtonInfo>().ItemID]++;
                shopItems[2, 6] = 0;
                AutoSave();
            }
            else if (shopItems[2, 6] == 0)
            {
                return;
            }

            //Updates #of items the player has, or level of upgrade
            ButtonRef.GetComponent<ButtonInfo>().QuantityText.text = shopItems[3, ButtonRef.GetComponent<ButtonInfo>().ItemID].ToString();
            shopItems[3, ButtonRef.GetComponent<ButtonInfo>().ItemID]++;
            AutoSave();
        }
    }


    //Allows for Voice Acting to be played after the FireBall Cosmetic is purchased.
    IEnumerator WaiterFireBallVA()
    {
        FindObjectOfType<AudioManager>().LowerVolume("ShopMenuTheme");
        panelCharacterBoxes.SetActive(true);
        imageAiLarge.SetActive(false);
        imageAiSmall.SetActive(true);
        imageMCLarge.SetActive(true);
        imageMCSmall.SetActive(false);
        FindObjectOfType<AudioManager>().Play("FireInSpace");
        yield return new WaitForSecondsRealtime(3);
        imageAiLarge.SetActive(true);
        imageAiSmall.SetActive(false);
        imageMCLarge.SetActive(false);
        imageMCSmall.SetActive(true);
        FindObjectOfType<AudioManager>().Play("Don'tQuestionIt");
        yield return new WaitForSecondsRealtime(2);
        panelCharacterBoxes.SetActive(false);
        FindObjectOfType<AudioManager>().NormalizeVolume("ShopMenuTheme");
        yield return null;
    }

    //Allows for this cheesy Dialogue to be played after you first upgrade the Paddle Size.
    IEnumerator WaiterShipSizeVA()
    {
        FindObjectOfType<AudioManager>().LowerVolume("ShopMenuTheme");
        panelCharacterBoxes.SetActive(true);
        imageAiLarge.SetActive(true);
        imageAiSmall.SetActive(false);
        imageMCLarge.SetActive(false);
        imageMCSmall.SetActive(true);
        FindObjectOfType<AudioManager>().Play("ShipSizeIncreased");
        yield return new WaitForSecondsRealtime(2);
        imageAiLarge.SetActive(false);
        imageAiSmall.SetActive(true);
        imageMCLarge.SetActive(true);
        imageMCSmall.SetActive(false);
        FindObjectOfType<AudioManager>().Play("What?!");
        yield return new WaitForSecondsRealtime(4);
        panelCharacterBoxes.SetActive(false);
        FindObjectOfType<AudioManager>().NormalizeVolume("ShopMenuTheme");
        yield return null;
    }



    //Is attached to the button above the fire ball particle effect item in the shop. 
    //Allows for the player to toggle said effect on and off.
    public void ToggleFireEffect()
    {
        if (fireToggleOffText.enabled == true)
        {
            ball.FireEffectOn();
            fireToggleOnText.enabled = true;
            fireToggleOffText.enabled = false;


        }
        else
        {
            ball.FireEffectOff();
            fireToggleOffText.enabled = true;
            fireToggleOnText.enabled = false;
        }
    }

    //Toggles the goop effect. Crazy, right?
    public void ToggleGoopEffect()
    {
        if (goopToggleOffText.enabled == true)
        {
            ball.GoopEffectOn();
            goopToggleOnText.enabled = true;
            goopToggleOffText.enabled = false;


        }
        else
        {
            ball.GoopEffectOff();
            goopToggleOffText.enabled = true;
            goopToggleOnText.enabled = false;
        }
    }

    //Calculates whether a fakeball should spawn when a new ball is spawned.
    //Should only be used when the corresponding upgrade has been purchased.
    public void BallSpawn()
    {
        int chance = UnityEngine.Random.Range(1, 11);

        if (chance < fakeBallSpawnChance)
        {
            Instantiate(fakeballPrefab);
        }

        ballsLost++;
        _currentBall = Instantiate(ballPrefab);
    }


    //Self explanatory.
    public void UpdateCoinTexts()
    {
        coinsText.text = "COINS" + _coins.ToString();
        coinsShopText.text = "COINS" + _coins.ToString();
    }

    //To clean up those pesky gameobjects! 
    void GetRidOfSpecialGameObjects()
    {
        var fakeBalls = GameObject.FindGameObjectsWithTag("Ball");
        var ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        var weirds = GameObject.FindGameObjectsWithTag("Weird");
        var explosives = GameObject.FindGameObjectsWithTag("Explosive");
        var powerUps = GameObject.FindGameObjectsWithTag("PowerUp");
        var multiBalls = GameObject.FindGameObjectsWithTag("Multiball");

        if (fakeBalls != null)
        {
            foreach (var fakeBall in fakeBalls)
            {
                Destroy(fakeBall);
            }
        }
        
        if (ghosts != null)
        {
            foreach (var ghost in ghosts)
            {
                Destroy(ghost);
            }
        }
        if (weirds != null)
        {
            foreach (var weird in weirds)
            {
                Destroy(weird);
            }
        }
        if (explosives != null)
        {
            foreach (var explosive in explosives)
            {
                Destroy(explosive);
            }
        }
        if (powerUps != null)
        {
            foreach (var powerUp in powerUps)
            {
                Destroy(powerUp);
            }
        }
        if (multiBalls != null)
        {
            foreach (var multiBall in multiBalls)
            {
                Destroy(multiBall);
            }
        }

        if (fakeBalls == null && ghosts == null && weirds == null && explosives == null && powerUps == null && multiBalls == null)
        {
            GameObjectsDestroyed = true;
        }
    }

    void GetRidOfBestiary()
    {
        var bestiaries = GameObject.FindGameObjectsWithTag("Bestiary");

        if (bestiaries != null)
        {
            foreach (var bestiary in bestiaries)
            {
                Destroy(bestiary);
            }
        }
    }
}