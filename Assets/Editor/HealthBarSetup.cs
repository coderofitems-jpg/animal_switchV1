using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Builds the health bar UI (Brackeys style) anchored to the upper left of the screen.
/// Menu: Tools > UI > Create HP Bar (Upper Left)
/// </summary>
public static class HealthBarSetup
{
    const float Margin = 20f;
    const float BarWidth = 220f;
    const float BarHeight = 26f;

    const string FrameSpritePath = "Assets/HP Leiste.png";
    const string FillSpritePath = "Assets/HP Leiste Fill.png";

    // Die weisse Innenflaeche von "HP Leiste.png" (128x16) liegt bei x 4..123 und
    // y 3..12. Als Anker-Brueche ausgedrueckt sitzt die Fuellung damit bei jeder
    // Leistengroesse pixelgenau im Rahmen.
    static readonly Vector2 InteriorAnchorMin = new Vector2(4f / 128f, 3f / 16f);
    static readonly Vector2 InteriorAnchorMax = new Vector2(124f / 128f, 13f / 16f);

    [MenuItem("Tools/UI/Create HP Bar (Upper Left)")]
    static void CreateHealthBar()
    {
        Canvas canvas = GetOrCreateCanvas();
        EnsureEventSystem();

        GameObject barGo = new GameObject("HealthBar", typeof(RectTransform));
        Undo.RegisterCreatedObjectUndo(barGo, "Create HP Bar");
        barGo.transform.SetParent(canvas.transform, false);

        // Top-left anchoring: anchor and pivot both in the upper-left corner, so the
        // bar keeps its offset from that corner at every resolution.
        RectTransform rect = barGo.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0f, 1f);
        rect.anchorMax = new Vector2(0f, 1f);
        rect.pivot = new Vector2(0f, 1f);
        rect.sizeDelta = new Vector2(BarWidth, BarHeight);
        rect.anchoredPosition = new Vector2(Margin, -Margin);

        // Der Rahmen ist das Bild selbst, unveraendert und weiss getoent, damit es
        // seine eigenen Farben zeigt. Simple statt Sliced: so wird die Grafik als
        // Ganzes gestreckt und der Rand skaliert proportional mit.
        Image background = CreateStretchedChild(rect, "Background", LoadFrameSprite());
        background.type = Image.Type.Simple;
        background.color = Color.white;

        RectTransform fillArea = new GameObject("Fill Area", typeof(RectTransform))
            .GetComponent<RectTransform>();
        fillArea.SetParent(rect, false);
        fillArea.anchorMin = InteriorAnchorMin;
        fillArea.anchorMax = InteriorAnchorMax;
        fillArea.offsetMin = Vector2.zero;
        fillArea.offsetMax = Vector2.zero;

        // Das Fuell-Sprite hat exakt die Form der weissen Innenflaeche, samt der
        // abgeschraegten Enden. Filled/Horizontal deckt es von links auf, statt das
        // Rect zu skalieren - so bleiben beide Enden formtreu. HealthBar setzt
        // fillAmount direkt; ein Slider dazwischen wuerde den Wert ueberschreiben.
        Image fill = CreateStretchedChild(fillArea, "Fill", LoadFillSprite());
        fill.type = Image.Type.Filled;
        fill.fillMethod = Image.FillMethod.Horizontal;
        fill.fillOrigin = (int)Image.OriginHorizontal.Left;
        fill.fillAmount = 1f;

        Gradient gradient = BuildGradient();
        fill.color = gradient.Evaluate(1f);

        // fill und gradient sind [SerializeField] private, also ueber SerializedObject
        // statt direkt zuweisen.
        HealthBar healthBar = barGo.AddComponent<HealthBar>();
        SerializedObject so = new SerializedObject(healthBar);
        so.FindProperty("fill").objectReferenceValue = fill;
        so.FindProperty("gradient").gradientValue = gradient;
        so.ApplyModifiedPropertiesWithoutUndo();

        Selection.activeGameObject = barGo;
        EditorUtility.SetDirty(barGo);
    }

    static Canvas GetOrCreateCanvas()
    {
        Canvas existing = Object.FindFirstObjectByType<Canvas>();
        if (existing != null)
            return existing;

        GameObject canvasGo = new GameObject("Canvas",
            typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        Undo.RegisterCreatedObjectUndo(canvasGo, "Create Canvas");

        Canvas canvas = canvasGo.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = canvasGo.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.matchWidthOrHeight = 0.5f;

        return canvas;
    }

    static void EnsureEventSystem()
    {
        if (Object.FindFirstObjectByType<EventSystem>() != null)
            return;

        GameObject go = new GameObject("EventSystem", typeof(EventSystem));
        // The project uses the new Input System package, so the legacy
        // StandaloneInputModule would log errors at play time.
        go.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
        Undo.RegisterCreatedObjectUndo(go, "Create EventSystem");
    }

    static Image CreateStretchedChild(RectTransform parent, string name, Sprite sprite)
    {
        GameObject go = new GameObject(name, typeof(RectTransform), typeof(Image));
        RectTransform rect = go.GetComponent<RectTransform>();
        rect.SetParent(parent, false);
        Stretch(rect);

        Image image = go.GetComponent<Image>();
        image.sprite = sprite;
        return image;
    }

    static void Stretch(RectTransform rect)
    {
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    static Sprite LoadFrameSprite()
    {
        return LoadSprite(FrameSpritePath);
    }

    static Sprite LoadFillSprite()
    {
        return LoadSprite(FillSpritePath);
    }

    static Sprite LoadSprite(string path)
    {
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        if (sprite == null)
            Debug.LogWarning($"HP-Leisten-Sprite nicht gefunden: {path}");

        return sprite;
    }

    static Gradient BuildGradient()
    {
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new[]
            {
                new GradientColorKey(Color.red, 0f),
                new GradientColorKey(Color.yellow, 0.5f),
                new GradientColorKey(Color.green, 1f)
            },
            new[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f) });
        return gradient;
    }
}
