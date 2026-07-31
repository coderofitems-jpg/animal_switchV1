using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image fill;
    [SerializeField] private Gradient gradient;

    public void SetHealth(float currentHealth, float maxHealth)
    {
        if (fill == null)
            return;

        // maxHealth 0 waere sonst eine Division durch null und damit NaN.
        float percent = maxHealth > 0f ? Mathf.Clamp01(currentHealth / maxHealth) : 0f;

        fill.fillAmount = percent;

        if (gradient != null)
        {
            fill.color = gradient.Evaluate(percent);
        }
    }
}
