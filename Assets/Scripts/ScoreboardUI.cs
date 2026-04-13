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
        // 🧹 Clear old entries
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        string[] players = PlayerManager.instance.GetAllPlayers();

        // 🔥 Sort by score (descending)
        Array.Sort(players, (a, b) =>
        {
            if (string.IsNullOrEmpty(a)) return 1;
            if (string.IsNullOrEmpty(b)) return -1;

            int scoreA = PlayerManager.instance.GetScore(a);
            int scoreB = PlayerManager.instance.GetScore(b);

            return scoreB.CompareTo(scoreA);
        });

        // 🧾 Create entries
        foreach (string p in players)
        {
            if (string.IsNullOrEmpty(p)) continue;

            int score = PlayerManager.instance.GetScore(p);

            GameObject entry = Instantiate(entryPrefab, container);

            TMP_Text text = entry.GetComponentInChildren<TMP_Text>();

            text.text = $"{p}   -   {score}";
        }
    }
}