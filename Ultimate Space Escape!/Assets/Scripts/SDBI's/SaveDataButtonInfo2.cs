using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems; 

public class SaveDataButtonInfo2 : MonoBehaviour
{
    public Text coinCount;
    public Text highestLevelBeaten;


    private void OnEnable()
    {
        SaveData data2 = SaveSystem.LoadSave2();
        string path = Application.persistentDataPath + "/savefile2.fun";
        if (File.Exists(path))
        {
            highestLevelBeaten.text = "LEVELS BEATEN\n" + data2.levelsUnlocked;
            coinCount.text = "COINS: " + data2._coins;
        }
        else
        {
            highestLevelBeaten.text = "LEVELS BEATEN\nNONE";
            coinCount.text = "COINS: NONE";
        }
    }
}
