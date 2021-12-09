using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
   public GameManager gameManager;
   public static bool GameIsPaused = false;
   public GameObject pauseMenuUI;
   public GameObject shopUI;
   public GameObject panelMenu;
   public GameObject panelPlay;
   public GameObject controlsUI;
   public GameObject creditsUI;
   public GameObject image1x;
   public GameObject image2x;
   public Text text2xSpeed;

   public GameManager gamemanager;



    //Pause Menu UI is automatically set to false when the game starts
    void Start()
    {
        pauseMenuUI.SetActive(false);
    }

    //When the space bar is pressed, this code determines what happens
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (shopUI.activeSelf)
            {
                return;
            }
            else
            {
                if (GameIsPaused)
                {
                    Resume();
                    Cursor.visible = false;
                }
                else
                {
                    Pause();
                    Cursor.visible = true;
                }
            }    
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (shopUI.activeSelf)
            {
                return;
            }
            else
            {
                if (GameIsPaused)
                {
                    Resume();
                    Cursor.visible = false;
                }
                else
                {
                    Pause();
                    Cursor.visible = true;
                }
            }    
        }
    }

    //Resumes the game when this void is used
    public void Resume()
    {
        image1x.SetActive(true);
        image2x.SetActive(false);
        text2xSpeed.enabled = false;
        Cursor.visible = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    
    //Pauses the game when this void is used
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    //Loads the shop menu when this public void is called, or when applied to a button such as the 'Shop' button in the Pause Menu
    public void LoadShop()
    {
        FindObjectOfType<AudioManager>().Play("ShopMenuTheme"); 
        FindObjectOfType<AudioManager>().Stop("MenuTheme");
        FindObjectOfType<AudioManager>().SilenceVolume("Levels1-5Theme");
        pauseMenuUI.SetActive(false);
        shopUI.SetActive(true);
        panelPlay.SetActive(false);
    }

    //Closes the shop menu when this public void is called, or applied to a button such as the 'Leave Shop' button
    public void LeaveShopMenu()
    {
        FindObjectOfType<AudioManager>().Stop("ShopMenuTheme");
        shopUI.SetActive(false);
        if (gameManager.Level >= 0)
        {
            if (gameManager.Level <= 5)
            {
                FindObjectOfType<AudioManager>().NormalizeVolume("Levels1-5Theme");
            }
        }
        if (gameManager.Level >= 1)
        {
            panelPlay.SetActive(true);
            pauseMenuUI.SetActive(true);
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("MenuTheme");
        }

    }

    //Loads the Main Menu when this public void is called
    public void LoadMenu()
    {
        FindObjectOfType<AudioManager>().Play("MenuTheme");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        Debug.Log("Loading menu...");

    }

    //Quits the game when this public void is called, or when it is added to a button such as the 'Quit' button in the Pause Menu
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    public void LoadControlsMenu()
    {
        pauseMenuUI.SetActive(false);
        controlsUI.SetActive(true);
    }

    public void LeaveControlsMenu()
    {
        pauseMenuUI.SetActive(true);
        controlsUI.SetActive(false);
    }


    public void LoadCredits()
    {
        creditsUI.SetActive(true);
    }

    public void ExitCredits()
    {
        creditsUI.SetActive(false);
    }
}
