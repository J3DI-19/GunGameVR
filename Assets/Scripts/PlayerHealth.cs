using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 150;
    public int currentHealth;
    private PlayerDamageFeedback feedback;

    [Header("References")]
    public GameObject locomotionObject;
    public GameObject xrSimulator;
    public GameObject gunObject;
    public TMP_Text healthText;

    CharacterController controller;
    bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        controller = GetComponent<CharacterController>();
        feedback = GetComponent<PlayerDamageFeedback>();

        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0); // ✅ Clamp

        if (feedback != null)
        {
            feedback.PlayDamageFeedback(); // ✅ Slightly earlier feel
        }

        UpdateHealthUI();

        Debug.Log("Player Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // ✅ KEEP ONLY THIS ONE
    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + currentHealth;

            if (currentHealth > 80)
                healthText.color = Color.green;
            else if (currentHealth > 40)
                healthText.color = Color.yellow;
            else
                healthText.color = Color.red;
        }
    }

    void Die()
    {
        isDead = true;
        currentHealth = 0;

        Debug.Log("Player Died");

        // 💀 DESTROY ALL ENEMIES
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        if (locomotionObject != null)
            locomotionObject.SetActive(false);

        if (gunObject != null)
            gunObject.SetActive(false);

        if (GameManager.instance != null)
            GameManager.instance.GameOver();
    }
}