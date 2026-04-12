using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[AddComponentMenu("Nokobot/Modern Guns/Simple Shoot")]
public class SimpleShoot : MonoBehaviour
{
    [Header("Prefab References")]
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;
    public GameObject bulletHolePrefab;

    [Header("Hit Feedback")]
    public AudioClip hitSound;
    public AudioClip headshotSound;
    public HitmarkerPopup hitmarker;

    [Header("Audio")]
    public AudioClip gunSound;
    public AudioClip reloadSound;
    private AudioSource audioSource;

    [Header("Location References")]
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;

    [Header("Settings")]
    [SerializeField] private float destroyTimer = 2f;
    [SerializeField] private float shotPower = 500f;
    [SerializeField] private float ejectPower = 150f;

    [Header("Ammo")]
    public int magazineSize = 7;
    public int currentAmmo = 7;
    public int damage = 40;
    private bool isReloading = false;
    [SerializeField] private float reloadTime = 4f;
    public GameObject reloadUI;

    [Header("Fire Settings")]
    private float nextFireTime = 0f;
    private float fireCooldown = 0.25f; // adjust if needed

    [Header("Input")]
    public InputActionProperty fireAction;
    public InputActionProperty reloadAction;

    [Header("Recoil")]
    [SerializeField] private float recoilKickBack = 0.05f;
    [SerializeField] private float recoilUp = 2f;
    [SerializeField] private float recoilSide = 1f;
    [SerializeField] private float recoilReturnSpeed = 10f;

    private Vector3 originalLocalPosition;
    private Quaternion originalLocalRotation;

    void Start()
    {
        if (barrelLocation == null)
            barrelLocation = transform;

        if (gunAnimator == null)
            gunAnimator = GetComponentInChildren<Animator>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        originalLocalPosition = transform.localPosition;
        originalLocalRotation = transform.localRotation;
    }

    void Update()
    {
        if (GameManager.instance != null && GameManager.instance.IsGameOver())
            return;

        // ===== FIRE (INSPECTOR-BASED INPUT) =====
        if (fireAction.action != null && fireAction.action.WasPressedThisFrame())
        {
            if (Time.time < nextFireTime) return;

            nextFireTime = Time.time + fireCooldown;

            TryShoot();
        }

        // ===== RELOAD =====
        if (reloadAction.action != null && reloadAction.action.WasPressedThisFrame())
        {
            Reload();
        }

        // ===== RECOIL RESET =====
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            originalLocalPosition,
            Time.deltaTime * recoilReturnSpeed
        );

        transform.localRotation = Quaternion.Lerp(
            transform.localRotation,
            originalLocalRotation,
            Time.deltaTime * recoilReturnSpeed
        );
    }

    void TryShoot()
    {
        if (isReloading) return;

        if (currentAmmo <= 0)
        {
            Debug.Log("Out of ammo!");
            return;
        }

        if (gunAnimator != null)
            gunAnimator.SetTrigger("Fire");
    }

    void Shoot()
    {
        Debug.Log("SHOT FIRED");

        currentAmmo--;
        Debug.Log("Ammo: " + currentAmmo + " / " + magazineSize);

        ApplyRecoil();

        // sound
        if (gunSound != null)
            audioSource.PlayOneShot(gunSound);

        // muzzle flash
        if (muzzleFlashPrefab)
        {
            GameObject flash = Instantiate(
                muzzleFlashPrefab,
                barrelLocation.position,
                barrelLocation.rotation
            );

            Destroy(flash, destroyTimer);
        }

        // raycast
        if (Physics.Raycast(barrelLocation.position, barrelLocation.forward, out RaycastHit hit, 100f))
        {
            Debug.Log("Hit: " + hit.collider.name);

            HeadshotTarget headshot = hit.collider.GetComponent<HeadshotTarget>();
            Target target = hit.collider.GetComponent<Target>();

            if (headshot != null)
            {
                headshot.TakeHeadshot(damage);
                hitmarker?.ShowHitmarker();
                if (headshotSound) audioSource.PlayOneShot(headshotSound);
            }
            else if (target != null)
            {
                target.TakeDamage(damage);
                hitmarker?.ShowHitmarker();
                if (hitSound) audioSource.PlayOneShot(hitSound);
            }

            // bullet hole
            if (bulletHolePrefab)
            {
                GameObject hole = Instantiate(
                    bulletHolePrefab,
                    hit.point + hit.normal * 0.02f,
                    Quaternion.LookRotation(-hit.normal)
                );

                hole.transform.Rotate(0, 0, Random.Range(0, 360));
                hole.transform.Rotate(Random.Range(-8f, 8f), Random.Range(-8f, 8f), 0);

                float size = Random.Range(0.2f, 0.5f);
                hole.transform.localScale = Vector3.one * size;

                hole.transform.SetParent(hit.collider.transform);

                Destroy(hole, 30f);
            }
        }

        // projectile (optional)
        if (bulletPrefab)
        {
            Rigidbody rb = Instantiate(
                bulletPrefab,
                barrelLocation.position,
                barrelLocation.rotation
            ).GetComponent<Rigidbody>();

            rb.AddForce(barrelLocation.forward * shotPower);

            Destroy(rb.gameObject, 3f);
        }

        // casing
        CasingRelease();
    }

    void Reload()
    {
        if (isReloading || currentAmmo == magazineSize) return;

        StartCoroutine(ReloadRoutine());
    }

    IEnumerator ReloadRoutine()
    {
        isReloading = true;

        if (reloadUI != null)
            reloadUI.SetActive(true);

        if (reloadSound != null)
            audioSource.PlayOneShot(reloadSound);

        if (gunAnimator != null)
            gunAnimator.SetTrigger("Reload");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = magazineSize;

        if (reloadUI != null)
            reloadUI.SetActive(false);

        isReloading = false;

        Debug.Log("Reloaded!");
    }

    void CasingRelease()
    {
        if (!casingExitLocation || !casingPrefab)
            return;

        GameObject casing = Instantiate(
            casingPrefab,
            casingExitLocation.position,
            casingExitLocation.rotation
        );

        Rigidbody rb = casing.GetComponent<Rigidbody>();

        rb.AddExplosionForce(
            Random.Range(ejectPower * 0.7f, ejectPower),
            casingExitLocation.position,
            1f
        );

        Destroy(casing, destroyTimer);
    }

    void ApplyRecoil()
    {
        transform.localPosition -= new Vector3(0, 0, recoilKickBack);

        float side = Random.Range(-recoilSide, recoilSide);
        transform.localRotation *= Quaternion.Euler(-recoilUp, side, 0);
    }
}