using UnityEngine;
using UnityEngine.SceneManagement;

public class Difficulty : MonoBehaviour // Сохранение сложности игры
{
    [SerializeField] float difficulty; // 0.5, 1, 2

    public void SetDifficulty()
    {
        PlayerPrefs.SetFloat("Difficulty", difficulty); 
        PlayerPrefs.Save();
        SceneManager.LoadScene("GameLobby");
    }
}
