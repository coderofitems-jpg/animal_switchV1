using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public HealthBar healthBar;

    // Exposed so it can be driven from the inspector slider while testing.
    public int currentHealth = 100;

    // Hook up whatever death should do (respawn, game over screen, ...) here.
    // Fires once when health reaches 0, not on every hit afterwards.
    public UnityEvent onDeath;

    public bool IsDead => isDead;

    bool isDead;

    void Start()
    {
        if (healthBar != null)
            healthBar.SetMaxHealth(maxHealth);

        // Through SetHealth so a scene that starts the player at 0 also fires onDeath.
        SetHealth(currentHealth);
    }

    public void TakeDamage(int amount)
    {
        // Without this an enemy standing on the corpse keeps hitting on every cooldown.
        if (isDead)
            return;

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

        // Healing back up (or the inspector Reset button) makes the player alive again.
        if (currentHealth > 0)
        {
            isDead = false;
            return;
        }

        if (isDead || !Application.isPlaying)
            return;

        isDead = true;

        // Null when the component was added at runtime instead of being serialized.
        if (onDeath != null)
            onDeath.Invoke();
    }

    // Keeps the bar in sync when the value is changed from the inspector.
    void OnValidate()
    {
        maxHealth = Mathf.Max(1, maxHealth);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (!Application.isPlaying)
            return;

        // Dragging the slider back up counts as reviving, same as Heal().
        if (currentHealth > 0)
            isDead = false;

        if (healthBar != null)
            healthBar.SetHealth(currentHealth);
    }
}
