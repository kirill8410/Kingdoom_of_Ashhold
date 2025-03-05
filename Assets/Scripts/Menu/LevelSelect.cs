using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] int numberLevel;

    private void Start()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = numberLevel.ToString();
        if (PlayerPrefs.GetInt("Level") < 1)
        {
            PlayerPrefs.SetInt("Level", 1);
            PlayerPrefs.Save();
        }
    }

    private void Update()
    {
        if (PlayerPrefs.GetInt("Level") >= numberLevel)
        {
            GetComponentInChildren<Canvas>().enabled = true;
        }
        else
        {
            GetComponentInChildren<Canvas>().enabled = false;
        }
    }

    public void StartLevel()
    {
        PlayerPrefs.SetInt("Level", 6);
        SceneManager.LoadScene($"Level_{numberLevel}");
    }
}
