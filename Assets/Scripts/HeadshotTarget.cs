using UnityEngine;

public class HeadshotTarget : MonoBehaviour
{
    public Target bodyTarget;
    public int headshotMultiplier = 4;
    public int bonusScore = 20;

    public void TakeHeadshot(int damage)
    {
        if (bodyTarget != null)
        {
            bodyTarget.TakeDamage(damage * headshotMultiplier);
        }

        // Bonus score
        if (GameManager.instance != null)
            GameManager.instance.AddScore(15); // bonus
    }
}