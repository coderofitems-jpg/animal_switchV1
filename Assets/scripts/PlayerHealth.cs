using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public HealthBar healthBar;

    // Exposed so it can be driven from the inspector slider while testing.
    public int currentHealth = 100;

    void Start()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetHealth(currentHealth);
        }
    }

    public void TakeDamage(int amount)
    {
        SetHealth(currentHealth - amount);
    }

    public void Heal(int amount)
    {
        SetHealth(currentHealth + amount);
    }

    public void SetHealth(int health)
    {
        currentHealth = Mathf.Clamp(health, 0, maxHealth);

        if (healthBar != null)
            healthBar.SetHealth(currentHealth);
    }

    // Keeps the bar in sync when the value is changed from the inspector.
    void OnValidate()
    {
        maxHealth = Mathf.Max(1, maxHealth);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (Application.isPlaying && healthBar != null)
            healthBar.SetHealth(currentHealth);
    }
}
