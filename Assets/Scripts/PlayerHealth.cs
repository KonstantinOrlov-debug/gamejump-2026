using UnityEngine;
using UnityEngine.Events;

// Attach this to the Player GameObject alongside PlayerController.
public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 10;

    [Header("Invincibility Frames")]
    public float iFrameDuration = 0.5f;  // Brief invincibility after taking a hit

    [Header("Events")]
    public UnityEvent<float> onHealthChanged;   // Sends normalised value (0-1) for UI
    public UnityEvent onDeath;

    int currentHealth;
    float lastHitTime = -Mathf.Infinity;

    public int CurrentHealth => currentHealth;
    public float NormalisedHealth => (float)currentHealth / maxHealth;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        // Respect iFrames
        if (Time.time - lastHitTime < iFrameDuration) return;

        lastHitTime = Time.time;
        currentHealth = Mathf.Max(currentHealth - amount, 0);

        onHealthChanged?.Invoke(NormalisedHealth);

        if (currentHealth <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        onHealthChanged?.Invoke(NormalisedHealth);
    }

    void Die()
    {
        onDeath?.Invoke();

        // Default behaviour — replace with game-over screen / respawn logic
        Debug.Log("Player died.");
        gameObject.SetActive(false);
    }
}