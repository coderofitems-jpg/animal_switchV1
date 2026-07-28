using UnityEditor;
using UnityEngine;

/// <summary>
/// Adds a health slider and damage/heal buttons to the PlayerHealth inspector so the
/// bar can be exercised in play mode without any gameplay hooked up.
/// </summary>
[CustomEditor(typeof(PlayerHealth))]
public class PlayerHealthEditor : Editor
{
    SerializedProperty maxHealth;
    SerializedProperty healthBar;
    SerializedProperty currentHealth;

    void OnEnable()
    {
        maxHealth = serializedObject.FindProperty("maxHealth");
        healthBar = serializedObject.FindProperty("healthBar");
        currentHealth = serializedObject.FindProperty("currentHealth");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(maxHealth);
        EditorGUILayout.PropertyField(healthBar);

        // A slider rather than a plain int field, bounded by maxHealth.
        currentHealth.intValue = EditorGUILayout.IntSlider(
            "Current Health", currentHealth.intValue, 0, Mathf.Max(1, maxHealth.intValue));

        // Applies the edit and fires OnValidate, which pushes the value to the bar.
        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space();

        PlayerHealth player = (PlayerHealth)target;

        using (new EditorGUI.DisabledScope(!Application.isPlaying))
        {
            EditorGUILayout.LabelField("Test Controls", EditorStyles.boldLabel);

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Damage 10"))
                    player.TakeDamage(10);

                if (GUILayout.Button("Heal 10"))
                    player.Heal(10);

                if (GUILayout.Button("Reset"))
                    player.SetHealth(player.maxHealth);
            }

            if (!Application.isPlaying)
                EditorGUILayout.HelpBox(
                    "Enter play mode to use the buttons. The slider above works in play mode too.",
                    MessageType.Info);
        }
    }
}
