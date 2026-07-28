using UnityEngine;
using UnityEngine.UI;

// Runs in edit mode too, so dragging the Slider's Value in the inspector
// updates the fill colour without having to enter play mode.
[ExecuteAlways]
public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetMaxHealth(int health)
    {
        if (slider == null)
            return;

        slider.maxValue = health;
        slider.value = health;

        ApplyGradient();
    }

    public void SetHealth(int health)
    {
        if (slider == null)
            return;

        slider.value = health;

        ApplyGradient();
    }

    void OnEnable()
    {
        ApplyGradient();
    }

    public void Update()
    {
        ApplyGradient();
    }

    // Null guards matter here: with [ExecuteAlways] this also runs in the editor,
    // where the references can be unassigned while the bar is being set up.
    void ApplyGradient()
    {
        if (fill == null || slider == null || gradient == null)
            return;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
