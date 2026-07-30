using UnityEngine;

[DisallowMultipleComponent]
public class EnemyHealthBar : MonoBehaviour
{
    const float width = 1f;
    const float height = 0.12f;
    const float offsetY = 0.8f;
    const int sortingOrder = 100;

    // Ein 1x1 weißes Sprite reicht für alle Leisten im Level.
    static Sprite quad;

    Transform bar;
    SpriteRenderer background;
    SpriteRenderer fill;

    float percent = 1f;
    bool visible;   // erst ab dem ersten Treffer sichtbar

    void Start()
    {
        EnsureBuilt();
    }

    public void SetPercent(float value)
    {
        percent = Mathf.Clamp01(value);

        if (percent < 1f)
            visible = true;

        EnsureBuilt();
        Apply();
    }

    void Apply()
    {
        background.enabled = visible;
        fill.enabled = visible;

        fill.transform.localScale = new Vector3(width * percent, height, 1f);

        // Gruen -> Gelb -> Rot, ohne Gradient-Referenz im Inspector.
        fill.color = percent > 0.5f
            ? Color.Lerp(Color.yellow, Color.green, (percent - 0.5f) * 2f)
            : Color.Lerp(Color.red, Color.yellow, percent * 2f);
    }

    void EnsureBuilt()
    {
        if (bar != null)
            return;

        if (quad == null)
        {
            Texture2D tex = new Texture2D(1, 1);
            tex.filterMode = FilterMode.Point;
            tex.SetPixel(0, 0, Color.white);
            tex.Apply();

            // Pivot links: dann schrumpft der Balken beim Skalieren nach rechts weg.
            quad = Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0f, 0.5f), 1f);
        }

        bar = new GameObject("HealthBar").transform;
        bar.SetParent(transform, false);
        bar.localPosition = new Vector3(0f, offsetY, 0f);

        background = NewPart("Background", new Color(0.1f, 0.1f, 0.1f, 0.85f), sortingOrder);
        background.transform.localScale = new Vector3(width, height, 1f);

        fill = NewPart("Fill", Color.green, sortingOrder + 1);

        Apply();
    }

    SpriteRenderer NewPart(string name, Color color, int order)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(bar, false);

        // Pivot sitzt links, also nach links versetzen damit die Leiste mittig sitzt.
        go.transform.localPosition = new Vector3(-width * 0.5f, 0f, 0f);

        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = quad;
        sr.color = color;
        sr.sortingOrder = order;
        return sr;
    }
}
