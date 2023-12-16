using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
    public static string directory = "/Saves/";
    public static void SavePlayer(PlayerData player)
    {
        string path = Application.persistentDataPath + directory;

        string fileName = "player" + ".punk";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        string json = JsonUtility.ToJson(player);
        File.WriteAllText(path + fileName, json);

    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + directory + "player" + ".punk";


        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            //Debug.Log("Save file FOUND in " + path);
            return data;
        }
        else
        {
            //Debug.Log("Save file NOT in " + path);
            return null;
        }
    }

    public static bool GetSave()
    {
        bool state = false;
        string path = Application.persistentDataPath + directory + "player" + ".punk";

        if (File.Exists(path))
        {
            state = true;
        }
        else
        {
            state = false;
        }

        return state;
    }
}
