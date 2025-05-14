using System.Collections;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int _coins;
    private float _HP = 1000;
    private int _numberWave = 0;
    private int _maxWave;

    private bool _isSpawn = true;
    private bool _lose = false;

    private TowerFunctions _frozenTower;

    public GameObject[] points;
    [SerializeField] int _numberLevel;
    [SerializeField] GameObject _freeze;
    [SerializeField] bool _isTestMode = false;

    private Transform _enemySpawn;
    private Wave[] _waves;
    
    private bool _isTourney = false;

    private void Awake()
    {
        if (!_isTourney)
        {
            if (_numberLevel == 0)
            {
                _waves = Resources.LoadAll<Wave>($"ScriptableObject/Wave/Level_{_numberLevel + 1}");
            }
            else
            {
                _waves = Resources.LoadAll<Wave>($"ScriptableObject/Wave/Level_{_numberLevel}");
            }

            _maxWave = _waves.Length;

            if (GameObject.FindGameObjectWithTag("EnemySpawnPoint") != null)
            {
                _enemySpawn = GameObject.FindGameObjectWithTag("EnemySpawnPoint").transform;
            }
        }
    }

    private void Start()
    {
        if (PlayerPrefs.GetFloat("Difficulty") == 0)
        {
            PlayerPrefs.SetFloat("Difficulty", 1);
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
        if (_numberWave >= _waves.Length)
        {
            Win();
        }
        if (_HP <= 0)
        {
            _HP = 0;
            Lose();
        }
        if (!_isSpawn)
        {
            Enemy[] e = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
            if (e.Length == 0 && !_lose)
            {
                _numberWave += 1;
                _isSpawn = true;

                if (_freeze != null)
                {
                    _frozenTower.isAttack = true;
                    _freeze.SetActive(false);
                }
            }
        }

        #region TEST MODE

        if (_isTestMode)
        {
            if (_coins < 10000)
            {
                _coins = 10000;
            }
            if (_HP < 100000)
            {
                _HP = 100000;
            }
        }

        #endregion
    }

    public void ReduceHP(float hp)
    {
        _HP -= hp;
        if (hp < 0)
        {
            _HP = 0;
        }
    }

    public void Win()
    {
        if (PlayerPrefs.GetInt("Level") < _numberLevel + 1)
        {
            PlayerPrefs.SetInt("Level", _numberLevel + 1);
            PlayerPrefs.Save();
        }
    }

    public void Lose()
    {
        Enemy[] enemies = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
    }

    public void ReturtToLobby()
    {
        if (_isTourney)
        {
            SceneManager.LoadSceneAsync("TourneyLobby");
        }
        else
        {
            SceneManager.LoadSceneAsync("GameLobby");
        }   
    }

    public void StartWave()
    {
        if (_isSpawn)
        {
            if (_freeze != null)
            {
                FreezeTower();
            }
            StartCoroutine(SpawnEnemies());
        }
        
    }

    private void FreezeTower()
    {
        TowerFunctions[] t = FindObjectsOfType<MonoBehaviour>().OfType<TowerFunctions>().ToArray();
        int r = Random.Range(0, t.Length);

        if (t.Length != 0 && _numberWave != 0)
        {  
            _frozenTower = t[r];
            t[r].isAttack = false;
            _freeze.transform.position = t[r].gm.transform.position;
            _freeze.SetActive(true);
        }
    }

    public IEnumerator SpawnEnemies()
    {
        Wave wave = _waves[_numberWave];
        
        for (int i = 0; i < wave.Enemies.Length; i++)
        {
            for (int j = 0; j < wave.NumberOfEnemies[i]; j++)
            {
                print(wave.Enemies[i].gameObject);
                GameObject enemy = Instantiate(wave.Enemies[i].gameObject, _enemySpawn.position, _enemySpawn.rotation);
                enemy.GetComponent<Enemy>().points = points;
                _isSpawn = false;
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
