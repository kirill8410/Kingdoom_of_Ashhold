using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Tourney : MonoBehaviour
{
    private SaveDataTourney _saveData;

    private void Start()
    {
        _saveData = LoadTourney();
    }

    public SaveDataTourney GetSaveData()
    {
        _saveData = LoadTourney();
        return _saveData;
    }

    public SaveDataTourney GetSaveData(string fileName)
    {
        _saveData = LoadTourney(fileName);
        return _saveData;
    }

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

    private void StartTourney()
    {
        
    }

    #region Сохранение файлов

    private void SaveTourney(SaveDataTourney saveData)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/Tourney.dat");
        SaveDataTourney data = saveData;
        bf.Serialize(file, data);
        file.Close();;
    }
    private void SaveTourney(SaveDataTourney saveData, string fileName)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + $"/{fileName}.dat");
        SaveDataTourney data = saveData;
        bf.Serialize(file, data);
        file.Close();
    }

    #endregion

    #region Загрузка файлов

    private SaveDataTourney LoadTourney()
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
    private SaveDataTourney LoadTourney(string fileName)
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

    #endregion
}
[System.Serializable]
public class SaveDataTourney
{
    public Dictionary<string, int> LocalRecords;
    public Dictionary<string, int> PublicRecords;
    public int TourneyDifficulty;
    public int GameDifficulty;
}
