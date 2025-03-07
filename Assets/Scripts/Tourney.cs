using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor.Overlays;
using UnityEngine;

public class Tourney : MonoBehaviour
{
    private Dictionary<string, int> _localRecords = new Dictionary<string, int>();
    public Dictionary <string, int> LocalRecords {  get { return _localRecords; } }

    private Dictionary<string, int> _publicRecords = new Dictionary<string, int>();
    public Dictionary<string, int> PublicRecords { get { return _publicRecords; } }

    private int _tourneyDifficulty = 0;
    private int _gameDifficulty = 1;

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
        SaveTourney();
    }

    #region Сохранение файлов

    private void SaveTourney()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/Tourney.dat");
        SaveDataTourney data = new SaveDataTourney();
        data.LocalRecords = _localRecords;
        data.PublicRecords = _publicRecords;
        data.TourneyDifficulty = _tourneyDifficulty;
        bf.Serialize(file, data);
        file.Close();;
    }
    private void SaveTourney(string fileName)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + $"/{fileName}.dat");
        SaveDataTourney data = new SaveDataTourney();
        data.LocalRecords = _localRecords;
        data.PublicRecords = _publicRecords;
        data.TourneyDifficulty = _tourneyDifficulty;
        bf.Serialize(file, data);
        file.Close(); ;
    }

    #endregion

    #region Загрузка файлов

    private void LoadTourney()
    {
        if (File.Exists(Application.persistentDataPath + "/Tourney.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/Tourney.dat", FileMode.Open);
            SaveDataTourney data = (SaveDataTourney)bf.Deserialize(file);
            file.Close();
            _localRecords = data.LocalRecords;
            _publicRecords = data.PublicRecords;
            _tourneyDifficulty = data.TourneyDifficulty;
        }
    }
    private void LoadTourney(string fileName)
    {
        if (File.Exists(Application.persistentDataPath + $"/{fileName}.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + $"/{fileName}.dat", FileMode.Open);
            SaveDataTourney data = (SaveDataTourney)bf.Deserialize(file);
            file.Close();
            _localRecords = data.LocalRecords;
            _publicRecords = data.PublicRecords;
            _tourneyDifficulty = data.TourneyDifficulty;
            Debug.Log($"{_localRecords}, {_publicRecords}, {_tourneyDifficulty}");
        }
    }

    #endregion
}
[SerializeField]
class SaveDataTourney
{
    public Dictionary<string, int> LocalRecords;
    public Dictionary<string, int> PublicRecords;
    public int TourneyDifficulty;
}
