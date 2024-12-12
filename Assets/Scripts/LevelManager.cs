using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int coins;
    public int HP = 10;
    public int wave;
    [SerializeField] int MaxWave;
    [SerializeField] int numberLevel;
    [SerializeField] GameObject UI;
    [SerializeField] GameObject enemySpawn;
    [SerializeField] GameObject[] points;
    [SerializeField] 

    public void Win()
    {
        if (PlayerPrefs.GetInt("Level") < numberLevel)
        {
            PlayerPrefs.SetInt("Level", numberLevel);
            PlayerPrefs.Save();
        }
        UI.SetActive(true);
    }

    public void Lose()
    {
        UI.SetActive(true);
    }

    public void ReturtToLobby()
    {
        SceneManager.LoadScene("GameLobby");
    }

    public void StartWave()
    {

    }

}
