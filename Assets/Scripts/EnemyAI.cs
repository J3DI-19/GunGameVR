using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent agent;

    [Header("Combat")]
    public float attackDistance = 10f;
    public float fireRate = 1f;
    public float damage = 20f;
    public float range = 100f;

    [Header("Accuracy")]
    public float baseSpread = 1.2f;
    public float hitChance = 0.2f;

    [Header("Difficulty Tuning")]
    [Range(0f, 1f)]
    public float accuracyMultiplier = 0.25f; // 🔥 MAIN CONTROL

    [Header("Timing")]
    public float extraDelayChance = 0.3f;
    public float extraDelayTime = 1.5f;

    public Transform firePoint;
    public GameObject muzzleFlashPrefab;

    private float nextFireTime = 0f;

    private int layerMask;

    private int missStreak = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = Camera.main.transform;

        layerMask = ~LayerMask.GetMask("Enemy");
    }

    void Update()
    {
        if (WaveManager.instance != null)
        {
            int wave = WaveManager.instance.GetCurrentWave();

            float t = Mathf.Clamp01(wave / 15f); // slower scaling

            // 🎯 Spread scaling
            baseSpread = Mathf.Lerp(1.5f, 0.08f, t);

            // 🎲 Hit chance scaling (then nerfed by multiplier)
            float rawHitChance = Mathf.Lerp(0.15f, 0.85f, t);
            hitChance = rawHitChance * accuracyMultiplier;
        }

        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
        {
            agent.isStopped = true;
            FacePlayer();
            Shoot();
        }
    }

    void FacePlayer()
    {
        Vector3 dir = (player.position - transform.position);
        dir.y = 0;

        if (dir != Vector3.zero)
        {
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                Quaternion.LookRotation(dir),
                Time.deltaTime * 5f
            );
        }
    }

    void Shoot()
    {
        if (Time.time < nextFireTime) return;

        nextFireTime = Time.time + 1f / fireRate;

        if (firePoint == null || player == null) return;

        // 🎲 RANDOM EXTRA DELAY
        if (Random.value < extraDelayChance)
        {
            nextFireTime += extraDelayTime;
        }

        // 🔥 muzzle flash
        if (muzzleFlashPrefab != null)
        {
            GameObject flash = Instantiate(
                muzzleFlashPrefab,
                firePoint.position,
                firePoint.rotation
            );

            Destroy(flash, 0.2f);
        }

        // 🎯 TARGET POINT
        CharacterController cc = player.GetComponentInParent<CharacterController>();
        Vector3 targetPoint = (cc != null) ? cc.bounds.center : player.position;

        // 🎲 MISS STREAK LOGIC
        bool willHit;

        if (missStreak >= 3)
        {
            willHit = true;
            missStreak = 0;
        }
        else
        {
            willHit = Random.value < hitChance;

            if (!willHit)
                missStreak++;
            else
                missStreak = 0;
        }

        // 🔥 EXTRA MISS NERF (IMPORTANT)
        if (willHit && Random.value < 0.3f)
        {
            willHit = false;
        }

        Vector3 direction;

        if (willHit)
        {
            direction = (targetPoint - firePoint.position).normalized;
        }
        else
        {
            // 🎲 STRONGER MISS SPREAD
            float spreadAmount = Random.Range(baseSpread * 0.7f, baseSpread * 1.2f);

            Vector3 randomOffset = new Vector3(
                Random.Range(-spreadAmount, spreadAmount),
                Random.Range(-spreadAmount * 0.3f, spreadAmount * 0.3f),
                Random.Range(-spreadAmount, spreadAmount)
            );

            direction = ((targetPoint + randomOffset) - firePoint.position).normalized;
        }

        // 🔥 HIT THICKNESS (scaled)
        float wave = WaveManager.instance != null ? WaveManager.instance.GetCurrentWave() : 1;
        float hitRadius = Mathf.Lerp(0.12f, 0.22f, wave / 10f);

        if (Physics.SphereCast(firePoint.position, hitRadius, direction, out RaycastHit hit, range, layerMask))
        {
            PlayerHealth playerHealth = hit.collider.GetComponentInParent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage((int)damage);
                return;
            }

            HeadshotTarget headshot = hit.collider.GetComponent<HeadshotTarget>();
            if (headshot != null)
            {
                headshot.TakeHeadshot((int)damage);
                return;
            }

            Target target = hit.collider.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage((int)damage);
                return;
            }
        }

        Debug.DrawRay(firePoint.position, direction * 10f, Color.red, 1f);
    }

    public void Die()
    {
        if (WaveManager.instance != null)
        {
            WaveManager.instance.OnEnemyKilled();
        }

        Destroy(gameObject);
    }
}