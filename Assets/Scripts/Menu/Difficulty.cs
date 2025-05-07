using UnityEngine;
using UnityEngine.SceneManagement;

public class Difficulty : MonoBehaviour // Сохранение сложности игры
{
    [SerializeField] float difficulty; // 1, 1.5, 2

    public void SetDifficulty()
    {
        PlayerPrefs.SetFloat("Difficulty", difficulty); 

        PlayerPrefs.Save();
        SceneManager.LoadScene("GameLobby");
    }
}
