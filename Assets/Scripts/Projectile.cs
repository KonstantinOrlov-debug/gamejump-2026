using UnityEngine;

// Attach to the projectile prefab.
// Requires a Collider (set to Trigger) and a Rigidbody (or use the built-in movement below).
public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float speed = 12f;
    public int damage = 1;
    public float lifetime = 5f;         // Auto-destroy after this many seconds
    public GameObject hitEffectPrefab;  // Optional VFX on impact

    [HideInInspector]
    public Vector3 direction;           // Set by RangedEnemy.Shoot()

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        // Ignore other enemy colliders
        if (other.TryGetComponent<MeleeEnemy>(out _)) return;
        if (other.TryGetComponent<RangedEnemy>(out _)) return;

        // Damage player
        if (other.TryGetComponent<PlayerHealth>(out PlayerHealth ph))
            ph.TakeDamage(damage);

        // Optional hit effect
        if (hitEffectPrefab != null)
        {
            GameObject fx = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            Destroy(fx, 2f);
        }

        Destroy(gameObject);
    }
}