using System.Linq;
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
    [SerializeField] GameObject Freeze;
    [SerializeField] TextMeshProUGUI Text;


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
                    {
                        waveButton.enabled = true;
                        TowerFunctions[] t = Object.FindObjectsOfType<MonoBehaviour>().OfType<TowerFunctions>().ToArray();
                        foreach (TowerFunctions tower in t)
                        {
                            tower.isAttack = true;
                            Freeze.SetActive(false);
                        }
                    }
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
        Text.text = "Победа";
        UI.SetActive(true);
    }

    public void Lose()
    {
        Text.text = "Поражение";
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

    public void FreezeTower()
    {
        TowerFunctions[] t = FindObjectsOfType<MonoBehaviour>().OfType<TowerFunctions>().ToArray();
        int r = Random.Range(0, t.Length);

        if (t.Length != 0 && wave != 0)
        {  
            t[r].isAttack = false;
            Freeze.transform.position = t[r].gm.transform.position;
            Freeze.SetActive(true);
        }
    }
}
