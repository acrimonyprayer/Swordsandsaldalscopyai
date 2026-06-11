using UnityEngine;
using UnityEngine.UI;
using TMPro;

public static class UIHelper
{
    public static Canvas CreateCanvas()
    {
        GameObject go = new GameObject("Canvas");
        Canvas canvas = go.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = go.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 0.5f;

        go.AddComponent<GraphicRaycaster>();

        return canvas;
    }

    public static TextMeshProUGUI CreateText(Transform parent, string name, string content,
        Vector2 pos, float fontSize, Color color,
        TextAlignmentOptions align = TextAlignmentOptions.Center,
        float width = 400, float height = 50)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent, false);

        RectTransform rect = go.AddComponent<RectTransform>();
        rect.anchoredPosition = pos;
        rect.sizeDelta = new Vector2(width, height);

        TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.text = content;
        tmp.fontSize = fontSize;
        tmp.color = color;
        tmp.alignment = align;
        tmp.fontStyle = FontStyles.Bold;

        return tmp;
    }

    public static RectTransform CreateButton(Transform parent, string name, string label,
        Vector2 pos, Vector2 size, Color bgColor, float fontSize = 20)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent, false);

        RectTransform rect = go.AddComponent<RectTransform>();
        rect.anchoredPosition = pos;
        rect.sizeDelta = size;

        Image img = go.AddComponent<Image>();
        img.color = bgColor;

        go.AddComponent<Button>();

        GameObject textGo = new GameObject("Text");
        textGo.transform.SetParent(go.transform, false);

        RectTransform textRect = textGo.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        TextMeshProUGUI tmp = textGo.AddComponent<TextMeshProUGUI>();
        tmp.text = label;
        tmp.fontSize = fontSize;
        tmp.color = Color.white;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontStyle = FontStyles.Bold;

        return rect;
    }

    public static void CreatePanel(Transform parent, Color color)
    {
        GameObject go = new GameObject("Background");
        go.transform.SetParent(parent, false);
        go.transform.SetAsFirstSibling();

        RectTransform rect = go.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Image img = go.AddComponent<Image>();
        img.color = color;
    }

    public static TextMeshProUGUI CreateTextWithBG(Transform parent, string name, string content,
        Vector2 pos, float fontSize, Color color,
        TextAlignmentOptions align = TextAlignmentOptions.Center,
        float width = 400, float height = 50, float bgAlpha = 0.55f)
    {
        GameObject bgGo = new GameObject(name + "BG");
        bgGo.transform.SetParent(parent, false);
        RectTransform bgRect = bgGo.AddComponent<RectTransform>();
        bgRect.anchoredPosition = pos;
        bgRect.sizeDelta = new Vector2(width + 24, height + 8);
        Image bgImg = bgGo.AddComponent<Image>();
        bgImg.color = new Color(0, 0, 0, bgAlpha);
        bgImg.raycastTarget = false;

        return CreateText(parent, name, content, pos, fontSize, color, align, width, height);
    }

    public static Sprite CreateSprite(Color color)
    {
        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, color);
        tex.Apply();
        tex.filterMode = FilterMode.Point;
        return Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1);
    }

    public static Sprite LoadSprite(string resourceName)
    {
        Texture2D tex = Resources.Load<Texture2D>(resourceName);
        if (tex == null) return null;
        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
            new Vector2(0.5f, 0.5f), tex.height);
    }

    public static void CreateArenaBackground(Camera cam, float dimAmount = 1f)
    {
        Sprite bgSprite = LoadSprite("collasium");
        if (bgSprite == null) return;

        GameObject bgGo = new GameObject("ArenaBackground");
        SpriteRenderer sr = bgGo.AddComponent<SpriteRenderer>();
        sr.sprite = bgSprite;
        sr.sortingOrder = -10;
        sr.color = new Color(dimAmount, dimAmount, dimAmount);

        float scale = cam.orthographicSize * 2;
        bgGo.transform.position = new Vector3(0, 0, 5);
        bgGo.transform.localScale = new Vector3(scale, scale, 1);
    }
}
