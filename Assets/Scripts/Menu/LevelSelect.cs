using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] int numberLevel;

    private void Start()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = numberLevel.ToString();
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
        SceneManager.LoadScene($"Level_{numberLevel}");
    }
}
