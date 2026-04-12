using System.Collections;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    [Header("Setup")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    [Header("Wave Settings")]
    public float spawnDelay = 5f;

    private int currentWave = 0;
    private int enemiesAlive = 0;

    public TMP_Text waveText;
    public GameObject waveClearText;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        StartNextWave();
    }

    void StartNextWave()
    {
        currentWave++;

        if (waveText != null)
            waveText.text = "Wave: " + currentWave;

        Debug.Log("Starting Wave: " + currentWave);

        StartCoroutine(SpawnWave(currentWave));
    }

    IEnumerator SpawnWave(int enemyCount)
    {
        int spawned = 0;

        while (spawned < enemyCount)
        {
            // Stop if game over
            if (GameManager.instance != null && GameManager.instance.IsGameOver())
                yield break;

            // Spawn up to max spawn points
            int batchSize = Mathf.Min(spawnPoints.Length, enemyCount - spawned);

            for (int i = 0; i < batchSize; i++)
            {
                SpawnEnemy(spawnPoints[i]);
                spawned++;
                enemiesAlive++;
            }

            // Wait before next batch
            if (spawned < enemyCount)
                yield return new WaitForSeconds(spawnDelay);
        }
    }

    void SpawnEnemy(Transform spawnPoint)
    {
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    // 🔴 CALLED WHEN ENEMY DIES
    public void OnEnemyKilled()
    {
        enemiesAlive--;

        if (enemiesAlive <= 0)
        {
            Debug.Log("Wave Cleared!");

            StartCoroutine(HandleWaveClear());
        }
    }
    IEnumerator HandleWaveClear()
    {
        if (waveClearText != null)
            waveClearText.SetActive(true);

        yield return new WaitForSeconds(2f);

        if (waveClearText != null)
            waveClearText.SetActive(false);

        StartNextWave();
    }

}