using UnityEngine;
using TMPro;
using System;

public class ScoreboardUI : MonoBehaviour
{
    public Transform container;
    public GameObject entryPrefab;

    private void OnEnable()
    {
        PopulateScores();
    }

    void PopulateScores()
    {
        // Clear old entries
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        string[] players = PlayerManager.instance.GetAllPlayers();

        // 🔥 SORT PLAYERS BY SCORE (DESCENDING)
        Array.Sort(players, (a, b) =>
        {
            if (string.IsNullOrEmpty(a)) return 1;
            if (string.IsNullOrEmpty(b)) return -1;

            int scoreA = PlayerManager.instance.GetScore(a);
            int scoreB = PlayerManager.instance.GetScore(b);

            return scoreB.CompareTo(scoreA); // DESCENDING
        });

        // Create UI entries
        foreach (string p in players)
        {
            if (string.IsNullOrEmpty(p)) continue;

            int score = PlayerManager.instance.GetScore(p);

            GameObject entry = Instantiate(entryPrefab, container);

            entry.GetComponent<TMP_Text>().text = p + "  -  " + score;
        }
    }
}