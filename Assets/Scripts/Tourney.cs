using NUnit.Framework.Constraints;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Tourney : MonoBehaviour
{
    private SaveDataTourney _saveData;
    private SaveDataTourneyGame _saveDataGame;

    private void Awake()
    {
        _saveData = LoadTourney();
        if (_saveData == null || _saveData.Names == null || _saveData.Records == null)
        {
            SaveDataTourney data = new SaveDataTourney();
            _saveData = data;
            for (int i = 0; i < 10; i++)
            {
                _saveData.Records[i] = 0;
                _saveData.Names[i] = "None";
            }
            SaveTourney(_saveData);
        }

        SaveDataTourney sd = LoadTourney();
        sd.Names[1] = "kirill";
        sd.Records[1] = 123456;
        SaveTourney(sd);
    }
    
    #region Get

    public SaveDataTourney GetTourney()
    {
        _saveData = LoadTourney();
        return _saveData;
    }

    public SaveDataTourney GetTourney(string fileName)
    {
        _saveData = LoadTourney(fileName);
        return _saveData;
    }

    public SaveDataTourneyGame GetTourneyGame()
    {
        _saveDataGame = LoadTourneyGame();
        return _saveDataGame;
    }

    public SaveDataTourneyGame GetTourneyGame(string fileName)
    {
        _saveDataGame = LoadTourneyGame(fileName);
        return _saveDataGame;
    }

    #endregion

    public static Tourney CreateTourney()
    {
        if (FindFirstObjectByType<Tourney>() == null)
        {
            GameObject tourney = new GameObject();
            tourney.name = "Tourney";
            tourney.transform.position = Vector3.zero;
            tourney.AddComponent<Tourney>();
            return tourney.GetComponent<Tourney>();
        }
        return null;
    }

    #region Сохранение файлов

    public void SaveTourney(SaveDataTourney saveData)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/Tourney.dat");
        SaveDataTourney data = saveData;
        bf.Serialize(file, data);
        file.Close();;
    }
    public void SaveTourney(SaveDataTourney saveData, string fileName)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + $"/{fileName}.dat");
        SaveDataTourney data = saveData;
        bf.Serialize(file, data);
        file.Close();
    }

    public void SaveTourneyGame(SaveDataTourneyGame saveData)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/TourneyGame.dat");
        SaveDataTourneyGame data = saveData;
        bf.Serialize(file, data);
        file.Close(); ;
    }
    public void SaveTourneyGame(SaveDataTourneyGame saveData, string fileName)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + $"/{fileName}.dat");
        SaveDataTourneyGame data = saveData;
        bf.Serialize(file, data);
        file.Close();
    }

    #endregion

    #region Загрузка файлов

    public SaveDataTourney LoadTourney()
    {
        if (File.Exists(Application.persistentDataPath + "/Tourney.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/Tourney.dat", FileMode.Open);
            SaveDataTourney data = (SaveDataTourney)bf.Deserialize(file);
            file.Close();
            return data;
        }
        else
        {
            return null;
        }
    }
    public SaveDataTourney LoadTourney(string fileName)
    {
        if (File.Exists(Application.persistentDataPath + $"/{fileName}.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + $"/{fileName}.dat", FileMode.Open);
            SaveDataTourney data = (SaveDataTourney)bf.Deserialize(file);
            file.Close();
            return data;
        }
        else
        {
            return null;
        }
    }

    public SaveDataTourneyGame LoadTourneyGame()
    {
        if (File.Exists(Application.persistentDataPath + "/TourneyGame.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/TourneyGame.dat", FileMode.Open);
            SaveDataTourneyGame data = (SaveDataTourneyGame)bf.Deserialize(file);
            file.Close();
            return data;
        }
        else
        {
            return null;
        }
    }
    public SaveDataTourneyGame LoadTourneyGame(string fileName)
    {
        if (File.Exists(Application.persistentDataPath + $"/{fileName}.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + $"/{fileName}.dat", FileMode.Open);
            SaveDataTourneyGame data = (SaveDataTourneyGame)bf.Deserialize(file);
            file.Close();
            return data;
        }
        else
        {
            return null;
        }
    }

    #endregion

    public void DeleteTourneyGame()
    {
        if (File.Exists(Application.persistentDataPath + "/TourneyGame.dat"))
        {
            File.Delete(Application.persistentDataPath + "/TourneyGame.dat");
        }
    }
}
[System.Serializable]
public class SaveDataTourney
{
    public string[] Names = new string[10];
    public int[] Records = new int[10];
}

[System.Serializable]
public class SaveDataTourneyGame
{
    public string Name;

    public float HP;
    public int Wave;
    public int Points;
    public int Coins;

    public Tower.TowerTypes[] Towers = new Tower.TowerTypes[10];
    public int[] TowerLevel = new int[10];
}
