using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int coins;
    public int HP = 10;
    public int wave = 0;
    private bool isWave = false;
    private bool isSpawn = false;
    [SerializeField] int numberLevel;
    [SerializeField] GameObject UI;
    [SerializeField] Canvas waveButton;
    public Transform enemySpawn;
    public GameObject[] points;
    public Wave[] waves;

    private void Start()
    {
        if (PlayerPrefs.GetInt("Difficulty") == 0)
        {
            PlayerPrefs.SetInt("Difficulty", 2);
            PlayerPrefs.Save();
        }
        if (PlayerPrefs.GetString("Music") != "true" && PlayerPrefs.GetString("Music") != "false")
        {
            PlayerPrefs.SetString("Music", "true");
            PlayerPrefs.Save();
        }
    }
    private void Update()
    {
        if (wave >= waves.Length)
        {
            Win();
        }
        if (HP <= 0)
        {
            Lose();
        }
        if (isSpawn)
        {
            if (isWave)
            {
                Enemy[] e = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
                if (e.Length == 0)
                {
                    isWave = false;
                    wave += 1;
                    if (wave < waves.Length)
                    waveButton.enabled = true;
                }
            }
        }
    }

    public void Win()
    {
        if (PlayerPrefs.GetInt("Level") < numberLevel + 1)
        {
            PlayerPrefs.SetInt("Level", numberLevel + 1);
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
        isWave = true;
        isSpawn = false;
        StartCoroutine(waves[wave].SpawnEnemies(this));
        waveButton.enabled = false;
    }

    public void StopSpawn()
    {
        isSpawn = true;
    }
}
