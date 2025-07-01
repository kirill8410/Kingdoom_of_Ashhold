using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TourneyUI : MonoBehaviour
{
    public GameObject clava;
    public GameObject tourney;

    public GameObject nextButton;

    private Tourney _tourney;

    private void Start()
    {
        _tourney = FindFirstObjectByType<Tourney>();
        
        if (_tourney.LoadTourneyGame() != null)
        {
            nextButton.SetActive(true);
        }
        else
        {
            nextButton.SetActive(false);
        }
        tourney.SetActive(true);
        clava.SetActive(false);
    }

    public void Next()
    {
        SceneManager.LoadSceneAsync("Tourney");
    }

    public void StartNew()
    {
        Invoke("Exit", 3f);
        tourney.SetActive(false);
        clava.SetActive(true);
    }

    private void Exit()
    {
        if (clava.GetComponent<TourmeyName>().Name == "")
        {
            tourney.SetActive(true);
            clava.SetActive(false);
        }
        else
        {
            Invoke("Exit", 3f);
        }
    }

    public void StartTourney()
    {
        if (clava.GetComponent<TourmeyName>().Name.Length > 0)
        {
            SaveDataTourneyGame sd = new SaveDataTourneyGame();
            sd.Name = clava.GetComponent<TourmeyName>().Name;
            sd.HP = 1000;
            sd.Coins = 100;
            sd.Wave = 0;
            sd.Points = 0;
            for (int i = 0; i < 10; i++)
            {
                sd.Towers[i] = Tower.TowerTypes.Base;
            }
            _tourney.SaveTourneyGame(sd);
            SceneManager.LoadSceneAsync("Tourney");
        }
    }
}
