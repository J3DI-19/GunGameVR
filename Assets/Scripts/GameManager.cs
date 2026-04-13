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
        // 🔥 Safe singleton
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
    }
    public bool IsGameOver()
    {
        return isGameOver;
    }

    // 🟢 ADD SCORE
    public void AddScore(int amount)
    {
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

        // 🛑 Stop waves (centralized)
        if (WaveManager.instance != null)
            WaveManager.instance.isGameOver = true;

        // Save score
        if (PlayerManager.instance != null)
            PlayerManager.instance.SaveScore(score);

        // Show UI
        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        // Disable HUD
        if (hud != null)
            hud.SetActive(false);

        // Update score text
        if (scoreText != null)
            scoreText.text = "Score: " + score;

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