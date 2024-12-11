using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] int numberLevel;

    private void Update()
    {
        if (PlayerPrefs.GetInt("Difficulty") >= numberLevel)
        {
            GetComponent<Canvas>().enabled = true;
        }
        else
        {
            GetComponent<Canvas>().enabled = false;
        }
    }

    public void StartLevel()
    {
        SceneManager.LoadScene($"Level_{numberLevel}");
    }
}
