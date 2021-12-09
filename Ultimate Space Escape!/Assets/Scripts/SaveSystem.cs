using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public static class SaveSystem
{


    public static void SaveGame1 (GameManager gamemanager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/savefile1.fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(gamemanager);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void SaveGame2(GameManager gamemanager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/savefile2.fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data2 = new SaveData(gamemanager);

        formatter.Serialize(stream, data2);
        stream.Close();
    }

    public static void SaveGame3(GameManager gamemanager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/savefile3.fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data3 = new SaveData(gamemanager);

        formatter.Serialize(stream, data3);
        stream.Close();
    }

    public static SaveData LoadSave1 ()
    {
        string path = Application.persistentDataPath + "/savefile1.fun";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return data;
        }
        else
        {
            return null;
        }
    }

    public static SaveData LoadSave2()
    {
        string path = Application.persistentDataPath + "/savefile2.fun";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data2 = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return data2;
        }
        else
        {
            return null;
        }
    }

    public static SaveData LoadSave3()
    {
        string path = Application.persistentDataPath + "/savefile3.fun";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data3 = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return data3;
        }
        else
        {
            return null;
        }
    }
}
