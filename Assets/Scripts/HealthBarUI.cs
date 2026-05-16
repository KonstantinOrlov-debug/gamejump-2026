using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [Header("References")]
    public Image healthFill;            // The "Filled" Image that shrinks/grows
    public PlayerHealth playerHealth;   // Optional: auto-assign if left empty

    [Header("Colours (optional gradient)")]
    public Color highHealthColor = Color.green;
    public Color lowHealthColor = Color.red;
    public bool useColorGradient = true;

    void Awake()
    {
        // Auto-find if not assigned
        if (playerHealth == null)
            playerHealth = FindObjectOfType<PlayerHealth>();

        if (playerHealth != null)
            playerHealth.onHealthChanged.AddListener(SetHealth);
    }

    void Start()
    {
        // Initialise to full health on scene start
        if (playerHealth != null)
            SetHealth(playerHealth.NormalisedHealth);
    }

    void OnDestroy()
    {
        if (playerHealth != null)
            playerHealth.onHealthChanged.RemoveListener(SetHealth);
    }

    // Called by PlayerHealth.onHealthChanged (normalised 0–1)
    public void SetHealth(float normalised)
    {
        if (healthFill == null) return;

        healthFill.fillAmount = normalised;

        if (useColorGradient)
            healthFill.color = Color.Lerp(lowHealthColor, highHealthColor, normalised);
    }
}