using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.LookDev;
using UnityEngine.SceneManagement;

public class TourneyLevelManager : MonoBehaviour
{
    private Tourney _tourney;
    private SaveDataTourneyGame _saveData;

    public int _coins = 100;
    private float _HP = 1000;
    private int _numberWave = 0;
    private int _points = 0;

    private bool _isWaveContinues = false;
    private bool _lose = false;

    public GameObject[] Points1;
    public GameObject[] Points2;
    public GameObject[] Points3;

    [SerializeField] private GameObject[] _bases = new GameObject[10];

    [SerializeField] private Transform[] _enemySpawn = new Transform[3];

    private PlayerUI _playerUI;

    #region Towers

    private GameObject _base;

    private GameObject _simpleBallist;
    private GameObject _bigBallist;
    private GameObject _doubleBallist;
    private GameObject _poisonBallist;
    private GameObject _sniperBallist;

    private GameObject _simpleMage;
    private GameObject _fireMage;
    private GameObject _iceMage;
    private GameObject _deathMage;
    private GameObject _godMage;

    private GameObject _simpleMortar;
    private GameObject _fireMortar;
    private GameObject _roketMortar;
    private GameObject _shrapnelMortar;

    #endregion

    private void Awake()
    {
        _playerUI = FindFirstObjectByType<PlayerUI>();
        if (_playerUI == null)
        {
            _playerUI = Instantiate(Resources.Load<GameObject>("Prefabs/PlayerUI"), Vector3.zero, Quaternion.identity).GetComponent<PlayerUI>();
        }

        _tourney = Tourney.CreateTourney();

        #region Tower

        _base = GetTower("Prefabs/Tower/Base");

        _simpleBallist = GetTower("Prefabs/Tower/Ballists/Simple_turet");
        _bigBallist = GetTower("Prefabs/Tower/Ballists/big_turet");
        _doubleBallist = GetTower("Prefabs/Tower/Ballists/double_turet");
        _poisonBallist = GetTower("Prefabs/Tower/Ballists/poison_turet");
        _sniperBallist = GetTower("Prefabs/Tower/Ballists/sniper_turet");

        _simpleMage = GetTower("Prefabs/Tower/Mage/magican");
        _fireMage = GetTower("Prefabs/Tower/Mage/fire_magican");
        _iceMage = GetTower("Prefabs/Tower/Mage/ice_magican");
        _deathMage = GetTower("Prefabs/Tower/Mage/death_magican");
        _godMage = GetTower("Prefabs/Tower/Mage/god_magican");

        _simpleMortar = GetTower("Prefabs/Tower/Mortar/mortar");
        _shrapnelMortar = GetTower("Prefabs/Tower/Mortar/shrapnel_mortar");
        _fireMortar = GetTower("Prefabs/Tower/Mortar/fire_mortar");
        _roketMortar = GetTower("Prefabs/Tower/Mortar/roket");

        #endregion
    }

    private void Start()
    {
        _saveData = _tourney.LoadTourneyGame();
        if (_saveData == null)
        {
            _saveData = new SaveDataTourneyGame();
            _saveData.HP = 1000;
            _saveData.Coins = 100;
            _saveData.Wave = 0;
            _saveData.Points = 0;
            for (int i = 0; i < 10; i++)
            {
                _saveData.Towers[i] = Tower.TowerTypes.Base;
            }
            _tourney.SaveTourneyGame(_saveData);
        }
        PlayerPrefs.SetFloat("Difficulty", 1);
        PlayerPrefs.Save();
        if (PlayerPrefs.GetString("Music") != "true" && PlayerPrefs.GetString("Music") != "false")
        {
            PlayerPrefs.SetString("Music", "true");
            PlayerPrefs.Save();
        }

        Load();
        
    }

    private void Update()
    {
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
                Save();
                Points();
            }
        }
    }

    private void Load()
    {
        #region Load

        #region Tower

        for (int i = 0; i < 10; i++)
        {
            if (_saveData.Towers[i] == Tower.TowerTypes.Base)
            {
                GameObject t = Instantiate(_base, _bases[i].transform);
                t.transform.localPosition = Vector3.zero;
                t.transform.localRotation = Quaternion.identity;
            }
            else
            {
                GameObject tower = new GameObject();
                switch (_saveData.Towers[i])
                {
                    case Tower.TowerTypes.SimpleBallist:
                        tower = Instantiate(_simpleBallist, _bases[i].transform);
                        break;
                    case Tower.TowerTypes.SimpleMage:
                        tower = Instantiate(_simpleMage, _bases[i].transform);
                        break;
                    case Tower.TowerTypes.SimpleMortar:
                        tower = Instantiate(_simpleMortar, _bases[i].transform);
                        break;
                    case Tower.TowerTypes.BigBallist:
                        tower = Instantiate(_bigBallist, _bases[i].transform);
                        break;
                    case Tower.TowerTypes.PoisonBallist:
                        tower = Instantiate(_poisonBallist, _bases[i].transform);
                        break;
                    case Tower.TowerTypes.SniperBallist:
                        tower = Instantiate(_sniperBallist, _bases[i].transform);
                        break;
                    case Tower.TowerTypes.DoubleBallist:
                        tower = Instantiate(_doubleBallist, _bases[i].transform);
                        break;
                    case Tower.TowerTypes.IceMage:
                        tower = Instantiate(_iceMage, _bases[i].transform);
                        break;
                    case Tower.TowerTypes.FireMage:
                        tower = Instantiate(_fireMage, _bases[i].transform);
                        break;
                    case Tower.TowerTypes.DeathMage:
                        tower = Instantiate(_deathMage, _bases[i].transform);
                        break;
                    case Tower.TowerTypes.GodMage:
                        tower = Instantiate(_godMage, _bases[i].transform);
                        break;
                    case Tower.TowerTypes.RoketMortar:
                        tower = Instantiate(_roketMortar, _bases[i].transform);
                        break;
                    case Tower.TowerTypes.FireMortar:
                        tower = Instantiate(_fireMortar, _bases[i].transform);
                        break;
                    case Tower.TowerTypes.ShrapnelMortar:
                        tower = Instantiate(_shrapnelMortar, _bases[i].transform);
                        break;
                }
                for (int j = 1; j < _saveData.TowerLevel[i]; j++)
                {
                    tower.GetComponent<TowerFunctions>().LevelUp();
                    tower.transform.localPosition = Vector3.zero;
                    tower.transform.localRotation = Quaternion.identity;
                }
            }
        }

        #endregion

        _HP = _saveData.HP;
        _coins = _saveData.Coins;
        _points = _saveData.Points;
        _numberWave = _saveData.Wave;

        #endregion
    }

    private void Save()
    {
        _saveData.Wave = _numberWave;
        _saveData.Points = _points;
        _saveData.HP = _HP;
        _saveData.Coins = _coins;

        for (int i = 0; i < 10; i++)
        {
            if (_bases[i].GetComponentInChildren<TowerFunctions>() != null)
            {
                _saveData.Towers[i] = _bases[i].GetComponentInChildren<TowerFunctions>().Parameters.TowerType;
                _saveData.TowerLevel[i] = _bases[i].GetComponentInChildren<TowerFunctions>().TowerLevel;
            }
            else
            {
                _saveData.Towers[i] = Tower.TowerTypes.Base;
            }
        }
        _tourney.SaveTourneyGame(_saveData);
    }

    private void Points()
    {
        _points += ((int)_HP / 50 + _coins / 10) * (1 + _numberWave / 10);
        _saveData = _tourney.LoadTourneyGame();
        _saveData.Points = _points;
        _tourney.SaveTourneyGame(_saveData);
    }

    public Wave GetWave()
    {
        Wave wave = new Wave();
        int a;
        if (_numberWave < 30)
        {
            if (_numberWave % 10 < 2)
            {
                a = 3;
            }
            else if (_numberWave % 10 < 4)
            {
                a = 6;
            }
            else if (_numberWave % 10 < 5)
            {
                a = 9;
            }
            else if (_numberWave % 10 < 7)
            {
                a = 12;
            }
            else
            {
                a = 15;
            }
        }
        else
        {
            a = 15;
        }
        wave.Enemies = new GameObject[a];
        wave.NumberOfEnemies = new int[a];
        for (int i = 0; i < a; i++)
        {
            wave.Enemies[i] = GetGoblin("Simple_goblin");
            wave.NumberOfEnemies[i] = a;
        }
        return wave;
    }

    private GameObject GetTower(string name)
    {
        return Resources.Load<GameObject>(name);
    }

    private GameObject GetGoblin(string name)
    {
        return Resources.Load<GameObject>($"Prefabs/Enemy/Goblins/{name}");
    }

    public bool GetWaveContinues()
    {
        return _isWaveContinues;
    }

    public float GetHP()
    {
        return _HP;
    }

    public int GetNumberWave()
    {
        return _numberWave;
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

    public void Lose()
    {
        Enemy[] enemies = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
        _saveData = _tourney.LoadTourneyGame();
        _saveData.Points = _points;
        _tourney.SaveTourneyGame(_saveData);
        _playerUI.Lose();
        Save();
    }

    public void ReturtToLobby()
    {
        SceneManager.LoadSceneAsync("TourneyLobby");
    }

    public void StartWave()
    {
        if (!_isWaveContinues)
        {
            if (_enemySpawn != null)
            {
                if (_numberWave >= 0)
                {
                    _enemySpawn[0].gameObject.SetActive(true);
                }
                else if (_numberWave >= 10)
                {
                    _enemySpawn[1].gameObject.SetActive(true);
                }
                else
                {
                    _enemySpawn[2].gameObject.SetActive(true);
                }
                    StartCoroutine(SpawnEnemies());
            }
            else
            {
                Debug.LogError("”кажите на каком-нибудь объекте тег EnemySpawnPoint чтобы враги могли спавнитьс€ в его позиции.");
            }
        }

    }

    public IEnumerator SpawnEnemies()
    {
        Wave wave = GetWave();

        for (int i = 0; i < wave.Enemies.Length; i++)
        {
            for (int j = 0; j < wave.NumberOfEnemies[i]; j++)
            {
                int a = i + 1;
                while (a != 1 && a != 2 && a != 3)
                {
                    a -= 3;
                }
                GameObject enemy = Instantiate(wave.Enemies[i], _enemySpawn[a - 1].position, _enemySpawn[a - 1].rotation);
                if (enemy.GetComponent<Enemy>())
                {
                    if (a == 1)
                    {
                        enemy.GetComponent<Enemy>().SetPoints(Points1);
                    }
                    else if (a == 2)
                    {
                        enemy.GetComponent<Enemy>().SetPoints(Points2);
                    }
                    else
                    {
                        enemy.GetComponent<Enemy>().SetPoints(Points3);
                    }
                }
                _isWaveContinues = true;
                yield return new WaitForSeconds(0.5f);
            }

        }
    }
}
