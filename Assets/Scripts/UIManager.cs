using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject newPlayer;
    public GameObject selectPlayer;
    public GameObject scoreboard;

    void Start()
    {
        ShowMainMenu(); // ensure clean start
    }

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        newPlayer.SetActive(false);
        selectPlayer.SetActive(false);
        scoreboard.SetActive(false);
    }

    public void ShowNewPlayer()
    {
        mainMenu.SetActive(false);
        newPlayer.SetActive(true);
        selectPlayer.SetActive(false);
        scoreboard.SetActive(false);
    }

    public void ShowSelectPlayer()
    {
        mainMenu.SetActive(false);
        newPlayer.SetActive(false);
        selectPlayer.SetActive(true);
        scoreboard.SetActive(false);
    }

    public void ShowScoreboard()
    {
        mainMenu.SetActive(false);
        newPlayer.SetActive(false);
        selectPlayer.SetActive(false);
        scoreboard.SetActive(true);
    }
}