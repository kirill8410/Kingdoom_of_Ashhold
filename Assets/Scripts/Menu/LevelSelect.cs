using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] int numberLevel;

    private void Update()
    {
        if (PlayerPrefs.GetInt("Level") >= numberLevel)
        {
            GetComponent<Canvas>().enabled = true;
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
