using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // 🟢 SAVE / CREATE PLAYER
    public void SavePlayer(string playerName)
    {
        playerName = playerName.Trim();

        if (string.IsNullOrEmpty(playerName))
        {
            Debug.Log("Name is empty!");
            return;
        }

        // Set current player
        PlayerPrefs.SetString("CurrentPlayer", playerName);

        // Add to player list if not exists
        string players = PlayerPrefs.GetString("PlayerList", "");

        if (!players.Contains(playerName + ","))
        {
            players += playerName + ",";
            PlayerPrefs.SetString("PlayerList", players);
        }

        PlayerPrefs.Save();

        Debug.Log("Player Saved: " + playerName);
    }

    // 🟡 SELECT EXISTING PLAYER
    public void SelectPlayer(string playerName)
    {
        PlayerPrefs.SetString("CurrentPlayer", playerName);
        PlayerPrefs.Save();

        Debug.Log("Selected Player: " + playerName);
    }

    // 🎯 GET CURRENT PLAYER
    public string GetCurrentPlayer()
    {
        return PlayerPrefs.GetString("CurrentPlayer", "Player");
    }

    // 🔴 SAVE SCORE
    public void SaveScore(int score)
    {
        string player = GetCurrentPlayer();

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

    // 🧹 OPTIONAL DEBUG
    public void ClearAllData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}