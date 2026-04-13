using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LobbyManager : MonoBehaviour
{
    public TMP_InputField nameInput;
    public string gameSceneName = "GameScene";

    // 🟢 NEW PLAYER
    public void StartNewPlayer()
    {
        string playerName = nameInput.text;

        if (string.IsNullOrWhiteSpace(playerName))
        {
            Debug.Log("Enter a name!");
            return;
        }

        PlayerManager.instance.SavePlayer(playerName);

        SceneManager.LoadScene(gameSceneName);
    }

    // 🔵 EXISTING PLAYER
    public void StartSelectedPlayer()
    {
        SceneManager.LoadScene(gameSceneName);
    }
}