using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int _level;
    public int _score;
    public int _balls;
    public int _coins;
    public int _highestLevel;
    public int _highscore;
    public float[] position;

    //Mess with in the future; figure out how to save them!!
    public int ItemID;
    public int[,] shopItems;
    public int levelsUnlocked;
    public int[,] levelID;
    public int[,] amID;
    public int[,] nBLID;


    public SaveData (GameManager gamemanager)
    {
        _level = gamemanager._level;
        _score = gamemanager._score;
        _balls = gamemanager._balls;
        _coins = gamemanager._coins;
        ItemID = gamemanager.ItemID;
        shopItems = gamemanager.shopItems;
        levelID = gamemanager.levelID;
        amID = gamemanager.amID;
        nBLID = gamemanager.nBLID;
        levelsUnlocked = gamemanager.levelsUnlocked;
        _highestLevel = PlayerPrefs.GetInt("highestLevel");
        _highscore = PlayerPrefs.GetInt("highscore");
    }
}
