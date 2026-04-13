using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SelectPlayerUI : MonoBehaviour
{
    public Transform container;
    public GameObject buttonPrefab;

    private void OnEnable()
    {
        PopulateList();
    }

    void PopulateList()
    {
        // Clear old buttons
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        string[] players = PlayerManager.instance.GetAllPlayers();

        foreach (string p in players)
        {
            if (string.IsNullOrEmpty(p)) continue;

            GameObject btn = Instantiate(buttonPrefab, container);

            btn.GetComponentInChildren<TMP_Text>().text = p;

            btn.GetComponent<Button>().onClick.AddListener(() =>
            {
                PlayerManager.instance.SelectPlayer(p);

                FindObjectOfType<LobbyManager>().StartSelectedPlayer();
            });
        }
    }
}