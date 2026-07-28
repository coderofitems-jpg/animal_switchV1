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

    [MenuItem("Tools/UI/Create HP Bar (Upper Left)")]
    static void CreateHealthBar()
    {
        Canvas canvas = GetOrCreateCanvas();
        EnsureEventSystem();

        GameObject sliderGo = new GameObject("HealthBar", typeof(RectTransform), typeof(Slider));
        Undo.RegisterCreatedObjectUndo(sliderGo, "Create HP Bar");
        sliderGo.transform.SetParent(canvas.transform, false);

        // Top-left anchoring: anchor and pivot both in the upper-left corner, so the
        // bar keeps its offset from that corner at every resolution.
        RectTransform rect = sliderGo.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0f, 1f);
        rect.anchorMax = new Vector2(0f, 1f);
        rect.pivot = new Vector2(0f, 1f);
        rect.sizeDelta = new Vector2(BarWidth, BarHeight);
        rect.anchoredPosition = new Vector2(Margin, -Margin);

        Image background = CreateStretchedChild(rect, "Background",
            GetBuiltinSprite("UI/Skin/Background.psd"));
        background.type = Image.Type.Sliced;
        background.color = new Color(0.15f, 0.15f, 0.15f, 1f);

        RectTransform fillArea = new GameObject("Fill Area", typeof(RectTransform))
            .GetComponent<RectTransform>();
        fillArea.SetParent(rect, false);
        Stretch(fillArea);
        fillArea.offsetMin = new Vector2(2f, 2f);
        fillArea.offsetMax = new Vector2(-2f, -2f);

        Image fill = CreateStretchedChild(fillArea, "Fill",
            GetBuiltinSprite("UI/Skin/UISprite.psd"));
        fill.type = Image.Type.Sliced;

        Slider slider = sliderGo.GetComponent<Slider>();
        slider.transition = Selectable.Transition.None;
        slider.interactable = false;
        slider.fillRect = fill.rectTransform;
        slider.targetGraphic = fill;
        slider.wholeNumbers = true;
        slider.minValue = 0f;
        slider.maxValue = 100f;
        slider.value = 100f;

        HealthBar healthBar = sliderGo.AddComponent<HealthBar>();
        healthBar.slider = slider;
        healthBar.fill = fill;
        healthBar.gradient = BuildGradient();
        fill.color = healthBar.gradient.Evaluate(1f);

        // Attached to the bar itself so the health slider can be driven from the
        // inspector without a player in the scene. Safe to remove once a real
        // player object owns its own PlayerHealth.
        PlayerHealth tester = sliderGo.AddComponent<PlayerHealth>();
        tester.healthBar = healthBar;
        tester.maxHealth = 100;
        tester.currentHealth = 100;

        Selection.activeGameObject = sliderGo;
        EditorUtility.SetDirty(sliderGo);
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

    static Sprite GetBuiltinSprite(string path)
    {
        return AssetDatabase.GetBuiltinExtraResource<Sprite>(path);
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
