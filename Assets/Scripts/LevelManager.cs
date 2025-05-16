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

    private bool _isWaveContinues = false;
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
        if (_isWaveContinues)
        {
            Enemy[] e = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
            if (e.Length == 0 && !_lose)
            {
                _numberWave += 1;
                _isWaveContinues = false;

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

    public bool GetWaveContinues()
    {
        return _isWaveContinues;
    }

    public float GetHP()
    {
        return _HP;
    }
    
    public int GetWave()
    {
        return _numberWave;
    }

    public Wave[] GetWaves()
    {
        return _waves;
    }

    public void ReduceHP(float hp)
    {
        _HP -= hp;
        if (hp < 0)
        {
            _HP = 0;
        }
    }

    public void SkipWave()
    {
        Enemy[] enemyes = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy enemy in enemyes)
        {
            enemy.Finish();
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
        if (!_isWaveContinues)
        {
            if (_freeze != null)
            {
                FreezeTower();
            }
            if (_enemySpawn != null)
            {
                StartCoroutine(SpawnEnemies());
            }
            else
            {
                Debug.LogError("Укажите на каком-нибудь объекте тег EnemySpawnPoint чтобы враги могли спавниться в его позиции.");
            }
            
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
            if (wave.Enemies[i] != null)
            {
                if (wave.NumberOfEnemies.Length > i)
                {
                    for (int j = 0; j < wave.NumberOfEnemies[i]; j++)
                    {
                        GameObject enemy = Instantiate(wave.Enemies[i], _enemySpawn.position, _enemySpawn.rotation);
                        if (enemy.GetComponent<Enemy>())
                        {
                            enemy.GetComponent<Enemy>().points = points;
                        }
                        _isWaveContinues = true;
                        yield return new WaitForSeconds(0.5f);
                    }
                }
                else
                {
                    Debug.LogError($"В Wave {wave} необходимо добавить {wave.NumberOfEnemies.Length - i - 1} элементов NumberOfEnemyes");
                }
            }
            else
            {
                Debug.LogErrorFormat("В Wave {0} элемент Enemy под номером {1} является пустым.", wave, i);
            }
        }
    }
}
