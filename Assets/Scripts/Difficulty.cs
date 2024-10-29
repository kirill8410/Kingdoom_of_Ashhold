using UnityEngine;
using UnityEngine.SceneManagement;

public class Difficulty : MonoBehaviour // Сохранение сложности игры
{
    [SerializeField] string difficulty; // Easy, Medium, Hard

    public void SetDifficulty()
    {
        PlayerPrefs.SetString("Difficulty", difficulty); 
        PlayerPrefs.Save();
        SceneManager.LoadScene("GameLobby");
    }
}
