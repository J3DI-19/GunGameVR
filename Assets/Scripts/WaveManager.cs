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
    public float spawnDelay = 1.5f;

    private int currentWave = 0;
    private int enemiesAlive = 0;
    private bool isWaveSpawning = false;

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
        // 🛑 Prevent double triggering
        if (isWaveSpawning) return;

        currentWave++;
        isWaveSpawning = true;

        if (waveText != null)
            waveText.text = "Wave: " + currentWave;

        Debug.Log("Starting Wave: " + currentWave);

        StartCoroutine(SpawnWave(currentWave)); // 1,2,3...
    }

    public int GetCurrentWave()
    {
        return currentWave;
    }

    IEnumerator SpawnWave(int enemyCount)
    {
        enemiesAlive = 0;

        for (int i = 0; i < enemyCount; i++)
        {
            if (GameManager.instance != null && GameManager.instance.IsGameOver())
                yield break;

            SpawnEnemy();

            yield return new WaitForSeconds(spawnDelay);
        }

        // ✅ Done spawning
        isWaveSpawning = false;

        // 🛟 Safety: if somehow no enemies registered
        if (enemiesAlive <= 0)
        {
            StartCoroutine(HandleWaveClear());
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0 || enemyPrefab == null)
        {
            Debug.LogError("SpawnPoints or EnemyPrefab missing!");
            return;
        }

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        // ✅ REGISTER IMMEDIATELY (NO TIMING ISSUES)
        enemiesAlive++;
        Debug.Log("Enemy Spawned. Total: " + enemiesAlive);
    }

    // 🔴 CALLED BY ENEMY ON DEATH
    public void OnEnemyKilled()
    {
        enemiesAlive--;

        // 🛟 Clamp (prevents negative bugs)
        if (enemiesAlive < 0)
            enemiesAlive = 0;

        Debug.Log("Enemies left: " + enemiesAlive);

        if (enemiesAlive == 0 && !isWaveSpawning)
        {
            StartCoroutine(HandleWaveClear());
        }
    }

    IEnumerator HandleWaveClear()
    {
        Debug.Log("Wave Cleared!");

        if (waveClearText != null)
            waveClearText.SetActive(true);

        yield return new WaitForSeconds(2f);

        if (waveClearText != null)
            waveClearText.SetActive(false);

        yield return new WaitForSeconds(1f);

        StartNextWave();
    }
}