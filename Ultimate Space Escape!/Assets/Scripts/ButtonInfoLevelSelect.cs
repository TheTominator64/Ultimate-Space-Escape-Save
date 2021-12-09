using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonInfoLevelSelect : MonoBehaviour
{
    public int LevelID;


    public GameObject aMPin;
    public GameObject nBLPin;
    public GameObject nBLPinLocked;


    public Text LevelNumberText;
    public GameObject GameManager;
    public GameManager gameManager;
    public GameObject levelButton;

    // Start is called before the first frame update
    void Start()
    {
        levelButton.SetActive(false);
        if (aMPin != null)
        {
            aMPin.SetActive(false);
        }
        if (nBLPinLocked != null)
        {
            nBLPinLocked.SetActive(true);
        }
    }

    private void OnEnable()
    {
        if (levelButton.activeInHierarchy == true)
        {
            LevelNumberText.text = GameManager.GetComponent<GameManager>().levelID[1, LevelID].ToString();
            if (aMPin != null)
            {
                if (GameManager.GetComponent<GameManager>().amID[1, LevelID] == 1)
                {
                    aMPin.SetActive(true);
                }
                else
                {
                    aMPin.SetActive(false);
                }
            }
            if (nBLPin != null)
            {
                if (GameManager.GetComponent<GameManager>().nBLID[1, LevelID] == 1)
                {
                    nBLPin.SetActive(true);
                    nBLPinLocked.SetActive(false);
                }
                else
                {
                    nBLPin.SetActive(false);
                }
            }
        }
    }
}
