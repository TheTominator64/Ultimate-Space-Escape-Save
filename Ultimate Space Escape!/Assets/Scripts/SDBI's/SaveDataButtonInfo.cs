using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SaveDataButtonInfo : MonoBehaviour
{
    public Text coinCount;
    public Text highestLevelBeaten;



    private void OnEnable()
    {
        SaveData data = SaveSystem.LoadSave1();
        string path = Application.persistentDataPath + "/savefile1.fun";
        if (File.Exists(path))
        {
            highestLevelBeaten.text = "LEVELS BEATEN\n" + data.levelsUnlocked;
            coinCount.text = "COINS: " + data._coins;
        }
        else
        {
            highestLevelBeaten.text = "LEVELS BEATEN\nNONE";
            coinCount.text = "COINS: NONE";
        }
    }
}
