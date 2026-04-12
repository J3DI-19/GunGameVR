using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [Header("UI")]
    public TMP_InputField nameInput;

    private void Awake()
    {
        // Singleton
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // 🟢 CREATE NEW PLAYER
    public void CreatePlayer()
    {
        string playerName = nameInput.text.Trim();

        if (string.IsNullOrEmpty(playerName))
        {
            Debug.Log("Name is empty!");
            return;
        }

        // Save current player
        PlayerPrefs.SetString("CurrentPlayer", playerName);

        // Add to player list if not exists
        string players = PlayerPrefs.GetString("PlayerList", "");

        if (!players.Contains(playerName + ","))
        {
            players += playerName + ",";
            PlayerPrefs.SetString("PlayerList", players);
        }

        PlayerPrefs.Save();

        Debug.Log("Player Created: " + playerName);

        // Load game
        SceneManager.LoadScene("GameScene");
    }

    // 🟡 SELECT EXISTING PLAYER
    public void SelectPlayer(string playerName)
    {
        PlayerPrefs.SetString("CurrentPlayer", playerName);
        PlayerPrefs.Save();

        Debug.Log("Selected Player: " + playerName);

        SceneManager.LoadScene("GameScene");
    }

    // 🔴 SAVE SCORE
    public void SaveScore(int score)
    {
        string player = PlayerPrefs.GetString("CurrentPlayer", "");

        if (string.IsNullOrEmpty(player))
            return;

        int bestScore = PlayerPrefs.GetInt(player + "_score", 0);

        if (score > bestScore)
        {
            PlayerPrefs.SetInt(player + "_score", score);
            PlayerPrefs.Save();
        }
    }

    // 🔵 GET SCORE
    public int GetScore(string playerName)
    {
        return PlayerPrefs.GetInt(playerName + "_score", 0);
    }

    // 🧠 GET ALL PLAYERS
    public string[] GetAllPlayers()
    {
        string players = PlayerPrefs.GetString("PlayerList", "");

        if (string.IsNullOrEmpty(players))
            return new string[0];

        return players.Split(',');
    }

    // 🧹 (OPTIONAL) CLEAR DATA
    public void ClearAllData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}