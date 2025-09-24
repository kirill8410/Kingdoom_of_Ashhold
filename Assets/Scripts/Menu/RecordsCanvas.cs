using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

public class RecordsCanvas : MonoBehaviour
{
    private GameObject _prefabPlace;
    private Tourney _tourney;
    private GameObject[] places = new GameObject[10];

    private void Awake()
    {
        _tourney = FindFirstObjectByType<Tourney>();
        if (_tourney == null)// Создаём и получаем Tourney если его нет
        {
            _tourney = Tourney.CreateTourney();
        }
    }

    private void Start()
    {
        _prefabPlace = Resources.Load<GameObject>("Prefabs/TourneyMenu/Place");

        
        UpdateRecords();
    }

    private void UpdateRecords()
    {
        if (_tourney.GetTourney() != null)
        {
            for (int i = 0; i < 10; i++)
            {
                CreatePlace(i);
            }
        }
    }

    private void CreatePlace(int i)
    {
        if (places[i] != null)
        {
            Destroy(places[i]);
            places[i] = null;
        }
        GameObject place = Instantiate(_prefabPlace, gameObject.GetNamedChild("Texts").transform);
        place.transform.localPosition = new Vector3(0, 100 - 20 * i, 0);
        TextMeshProUGUI name = place.GetNamedChild("Name").GetComponent<TextMeshProUGUI>();
        name.text = _tourney.GetTourney().Names[i];
        TextMeshProUGUI record = place.GetNamedChild("Record").GetComponent<TextMeshProUGUI>();
        record.text = _tourney.GetTourney().Records[i].ToString();
        places[i] = place;
    }
}
