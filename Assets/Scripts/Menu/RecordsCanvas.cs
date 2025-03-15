using System;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

public class RecordsCanvas : MonoBehaviour
{
    private GameObject _prefabPlace;
    private Tourney _tourney;
    private GameObject[] places = new GameObject[10];
    private enum RecordsCanvasTypes
    {
        Public, Local
    }
    [SerializeField] RecordsCanvasTypes _recordsCanvasType;
    
    private void Start()
    {
        _prefabPlace = Resources.Load<GameObject>("Prefabs/TourneyMenu/Place");

        _tourney = FindFirstObjectByType<Tourney>();
        if ( _tourney == null)// Создаём и получаем Tourney если его нет
        {
            _tourney = Tourney.CreateTourney();
        }
        UpdateRecords();
    }

    private void Update()
    {
        UpdateRecords();
    }

    private void UpdateRecords()
    {
        if (_recordsCanvasType == RecordsCanvasTypes.Public)
        {
            int[] records = new int[10];
            
            foreach (string key in _tourney.GetSaveData().PublicRecords.Keys)
            {
                int[] recs = records;
                foreach (int rec in recs)
                {
                    if (rec == 0 || _tourney.GetSaveData().PublicRecords[key] > rec)
                    {
                        int recI = Array.IndexOf(recs, rec);
                        CreatePlace(records, key, recI);
                        break;
                    }
                }
            }
        }
        else
        {
            int[] records = new int[10];

            foreach (string key in _tourney.GetSaveData().LocalRecords.Keys)
            {
                int[] recs = records;
                foreach (int rec in recs)
                {
                    if (rec == 0 || _tourney.GetSaveData().LocalRecords[key] > rec)
                    {
                        int recI = Array.IndexOf(recs, rec);
                        CreatePlace(records, key, recI);
                        break;
                    }
                }
            }
        }
    }

    private void CreatePlace(int[] records, string key, int recI)
    {
        if (places[recI] != null)
        {
            Destroy(places[recI]);
            places[recI] = null;
        }
        GameObject place = Instantiate(_prefabPlace, gameObject.GetNamedChild("Texts").transform);
        place.transform.localPosition = new Vector3(0, 100 - 20 * recI, 0);
        TextMeshProUGUI name = place.GetNamedChild("Name").GetComponent<TextMeshProUGUI>();
        name.text = key;
        TextMeshProUGUI record = place.GetNamedChild("Record").GetComponent<TextMeshProUGUI>();
        record.text = _tourney.GetSaveData().PublicRecords[key].ToString();
        records[recI] = _tourney.GetSaveData().PublicRecords[key];
        places[recI] = place;
    }
}
