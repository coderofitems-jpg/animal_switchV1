using UnityEditor;
using UnityEngine;

/// <summary>
/// Haengt Testbuttons unter den Tier-Slider, damit sich die Gegner-Leiste in
/// Play Mode ausprobieren laesst, solange es noch kein Angriffssystem gibt.
/// </summary>
[CustomEditor(typeof(Enemy))]
public class EnemyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Enemy enemy = target as Enemy;

        // Nach dem letzten Treffer zerstoert sich der Gegner selbst. Der Inspector wird
        // danach noch einmal gezeichnet, und jeder Zugriff auf target wuerde werfen.
        if (enemy == null)
        {
            EditorGUILayout.HelpBox("Gegner wurde besiegt.", MessageType.Info);
            return;
        }

        // Tier-Slider und alles andere ganz normal zeichnen.
        DrawDefaultInspector();

        EditorGUILayout.Space();

        using (new EditorGUI.DisabledScope(!Application.isPlaying))
        {
            EditorGUILayout.LabelField("Test Controls", EditorStyles.boldLabel);

            if (Application.isPlaying)
                EditorGUILayout.LabelField("HP", enemy.CurrentHealth + " / " + enemy.MaxHealth);

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Damage 5"))
                    enemy.TakeDamage(5);

                if (GUILayout.Button("Damage 20"))
                    enemy.TakeDamage(20);

                if (GUILayout.Button("Reset"))
                    enemy.SetHealth(enemy.MaxHealth);
            }
        }

        if (!Application.isPlaying)
            EditorGUILayout.HelpBox(
                "Play Mode starten, dann wirken die Buttons. Die Leiste erscheint erst nach dem ersten Treffer.",
                MessageType.Info);
        else
            Repaint();   // damit die HP-Anzeige mitlaeuft
    }
}
