using UnityEditor;
using UnityEngine;

/// <summary>
/// Setzt einen fertig verkabelten Gegner in die offene Szene, damit man ihn nicht
/// jedes Mal von Hand zusammenklicken muss.
/// Menu: Tools > Create Enemy
/// </summary>
public static class EnemySetup
{
    [MenuItem("Tools/Create Enemy")]
    static void CreateEnemy()
    {
        GameObject go = new GameObject("Enemy");
        Undo.RegisterCreatedObjectUndo(go, "Create Enemy");

        // Platzhalter-Grafik, damit ueberhaupt etwas zu sehen ist.
        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
        sr.color = new Color(0.8f, 0.25f, 0.25f);
        sr.drawMode = SpriteDrawMode.Sliced;
        sr.size = Vector2.one;

        go.AddComponent<BoxCollider2D>();

        // Kinematic: der Gegner bewegt sich per transform.Translate, soll aber
        // trotzdem Kollisionen mit dem Spieler melden.
        Rigidbody2D body = go.AddComponent<Rigidbody2D>();
        body.bodyType = RigidbodyType2D.Kinematic;
        body.useFullKinematicContacts = true;

        go.AddComponent<Enemy>();

        // Vor der Kamera absetzen, nicht irgendwo im Nichts.
        Camera cam = Camera.main;
        Vector3 pos = cam != null ? cam.transform.position : Vector3.zero;
        pos.z = 0f;
        go.transform.position = pos;

        Selection.activeGameObject = go;
    }
}
