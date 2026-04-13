using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI")]
    public GameObject gameOverUI;
    public TMP_Text scoreText; // Game Over screen score
    public TMP_Text hudScoreText;
    public GameObject hud;

    private bool isGameOver = false;
    private int score = 0;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (gameOverUI != null)
            gameOverUI.SetActive(false);

        score = 0;
        isGameOver = false;

        // Initialize HUD score
        if (hudScoreText != null)
            hudScoreText.text = "Score: 0";
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    // 🟢 ADD SCORE
    public void AddScore(int amount)
    {
        if (isGameOver) return;

        score += amount;

        if (hudScoreText != null)
            hudScoreText.text = "Score: " + score;
    }

    public int GetScore()
    {
        return score;
    }

    // 🔴 PLAYER DIED
    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        Debug.Log("GAME OVER");

        // 🛑 Stop waves
        if (WaveManager.instance != null)
            WaveManager.instance.isGameOver = true;

        // 💾 SAVE SCORE (NO PlayerManager dependency)
        string player = PlayerPrefs.GetString("CurrentPlayer", "");

        int bestScore = 0;

        if (!string.IsNullOrEmpty(player))
        {
            string key = player + "_score";

            bestScore = PlayerPrefs.GetInt(key, 0);

            if (score > bestScore)
            {
                PlayerPrefs.SetInt(key, score);
                PlayerPrefs.Save();

                bestScore = score;

                Debug.Log("NEW HIGH SCORE: " + score);
            }
            else
            {
                Debug.Log("Score not higher than best");
            }
        }

        // 🖥 Show Game Over UI
        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        // ❌ Disable HUD
        if (hud != null)
            hud.SetActive(false);

        // 🧾 Update score display
        if (scoreText != null)
        {
            if (!string.IsNullOrEmpty(player))
                scoreText.text = "Score: " + score + "\nBest: " + bestScore;
            else
                scoreText.text = "Score: " + score;
        }

        Time.timeScale = 1f;
    }

    // 🔁 RESTART
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 🔙 BACK TO MENU
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("LobbyScene");
    }
}