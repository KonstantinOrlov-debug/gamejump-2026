using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 3;

    [Header("Shooting")]
    public GameObject projectilePrefab;  // Assign a prefab that has Projectile.cs
    public Transform firePoint;          // Empty child Transform at the barrel/mouth
    public float fireRate = 2f;          // Shots per second
    public float detectionRange = 20f;   // Only shoots while player is in range

    // ---- private ----
    Transform player;
    int currentHealth;
    float fireTimer;

    void Awake()
    {
        currentHealth = maxHealth;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        // If no firePoint is set, shoot from this transform
        if (firePoint == null)
            firePoint = transform;
    }

    void Update()
    {
        if (player == null || projectilePrefab == null) return;

        float dist = Vector3.Distance(transform.position, player.position);
        if (dist > detectionRange) return;

        // Always face the player (horizontal only — remove y-lock if needed)
        Vector3 dir = player.position - transform.position;
        dir.y = 0;
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(dir);

        // Shoot on cooldown
        fireTimer += Time.deltaTime;
        if (fireTimer >= 1f / fireRate)
        {
            fireTimer = 0f;
            Shoot();
        }
    }

    void Shoot()
    {
        // Aim toward player's center (adjust y-offset if the pivot is off)
        Vector3 targetPos = player.position + Vector3.up * 1f;
        Vector3 shootDir = (targetPos - firePoint.position).normalized;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(shootDir));

        if (proj.TryGetComponent<Projectile>(out Projectile p))
            p.direction = shootDir;
    }

    // -------------------------------------------------- //
    // Receive damage (called by player attack logic)      //
    // -------------------------------------------------- //
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
#endif
}