using UnityEngine;

public class Target : MonoBehaviour
{
    public int health = 150;

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Health remaining: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Normal kill score
        if (GameManager.instance != null)
            GameManager.instance.AddScore(10);

        // Notify wave manager
        if (WaveManager.instance != null)
            WaveManager.instance.OnEnemyKilled();

        Destroy(gameObject);
    }
}