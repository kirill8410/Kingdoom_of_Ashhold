using UnityEngine;
using UnityEngine.SceneManagement;

public class Difficulty : MonoBehaviour // Сохранение сложности игры
{
    [SerializeField] int difficulty; // 0.5, 1, 2

    public void SetDifficulty()
    {
        PlayerPrefs.SetInt("Difficulty", difficulty); 
        PlayerPrefs.Save();
        SceneManager.LoadScene("GameLobby");
    }
}
