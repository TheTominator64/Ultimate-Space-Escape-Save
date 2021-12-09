using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SaveDataButtonInfo3 : MonoBehaviour
{
    public Text coinCount;
    public Text highestLevelBeaten;



    private void OnEnable()
    {
        SaveData data3 = SaveSystem.LoadSave3();
        string path = Application.persistentDataPath + "/savefile3.fun";
        if (File.Exists(path))
        {
            highestLevelBeaten.text = "LEVELS BEATEN\n" + data3.levelsUnlocked;
            coinCount.text = "COINS: " + data3._coins;
        }
        else
        {
            highestLevelBeaten.text = "LEVELS BEATEN\nNONE";
            coinCount.text = "COINS: NONE";
        }
    }
}
