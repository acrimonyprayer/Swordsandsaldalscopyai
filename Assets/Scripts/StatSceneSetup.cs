using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class StatSceneSetup : MonoBehaviour
{
    void Awake()
    {
        Camera cam = SetupCamera();
        EnsureEventSystem();
        EnsureGameManager();
        UIHelper.CreateArenaBackground(cam, 0.35f);
        BuildUI();
    }

    Camera SetupCamera()
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            GameObject go = new GameObject("Main Camera");
            go.tag = "MainCamera";
            cam = go.AddComponent<Camera>();
        }
        cam.orthographic = true;
        cam.orthographicSize = 5;
        cam.transform.position = new Vector3(0, 0, -10);
        cam.backgroundColor = new Color(0.05f, 0.04f, 0.08f);
        return cam;
    }

    void EnsureEventSystem()
    {
        if (FindObjectOfType<EventSystem>() == null)
        {
            GameObject go = new GameObject("EventSystem");
            go.AddComponent<EventSystem>();
            go.AddComponent<StandaloneInputModule>();
        }
    }

    void EnsureGameManager()
    {
        if (GameManager.instance == null)
        {
            GameObject go = new GameObject("GameManager");
            go.AddComponent<GameManager>();
        }
    }

    void BuildUI()
    {
        Canvas canvas = UIHelper.CreateCanvas();
        Transform ct = canvas.transform;

        UIHelper.CreatePanel(ct, new Color(0, 0, 0, 0.65f));

        // Title
        TextMeshProUGUI waveTitle = UIHelper.CreateText(ct, "WaveTitle", "",
            new Vector2(0, 430), 46, new Color(1f, 0.85f, 0.2f),
            TextAlignmentOptions.Center, 800, 65);

        // Points
        TextMeshProUGUI pointsText = UIHelper.CreateText(ct, "PointsText", "",
            new Vector2(0, 340), 36, Color.white,
            TextAlignmentOptions.Center, 600, 55);

        // Stats
        float statX = -130f;
        float btnX = 330f;
        float startY = 230f;
        float gap = 65f;

        Color vitCol = new Color(0.3f, 0.95f, 0.3f);
        Color strCol = new Color(1f, 0.4f, 0.4f);
        Color endCol = new Color(0.45f, 0.75f, 1f);
        Color agiCol = new Color(1f, 1f, 0.35f);
        Color dexCol = new Color(1f, 0.65f, 0.2f);

        TextMeshProUGUI vitalityText = UIHelper.CreateText(ct, "VitalityText", "",
            new Vector2(statX, startY), 27, vitCol, TextAlignmentOptions.Left, 520, 45);
        TextMeshProUGUI strengthText = UIHelper.CreateText(ct, "StrengthText", "",
            new Vector2(statX, startY - gap), 27, strCol, TextAlignmentOptions.Left, 520, 45);
        TextMeshProUGUI enduranceText = UIHelper.CreateText(ct, "EnduranceText", "",
            new Vector2(statX, startY - gap * 2), 27, endCol, TextAlignmentOptions.Left, 520, 45);
        TextMeshProUGUI agilityText = UIHelper.CreateText(ct, "AgilityText", "",
            new Vector2(statX, startY - gap * 3), 27, agiCol, TextAlignmentOptions.Left, 520, 45);
        TextMeshProUGUI dexterityText = UIHelper.CreateText(ct, "DexterityText", "",
            new Vector2(statX, startY - gap * 4), 27, dexCol, TextAlignmentOptions.Left, 520, 45);

        // Warning
        TextMeshProUGUI warningText = UIHelper.CreateText(ct, "WarningText", "",
            new Vector2(0, -130), 32, new Color(1f, 0.2f, 0.2f),
            TextAlignmentOptions.Center, 700, 50);

        // Stat buttons
        Color btnVitCol = new Color(0.18f, 0.5f, 0.18f);
        Color btnStrCol = new Color(0.6f, 0.15f, 0.15f);
        Color btnEndCol = new Color(0.15f, 0.3f, 0.6f);
        Color btnAgiCol = new Color(0.6f, 0.6f, 0.12f);
        Color btnDexCol = new Color(0.6f, 0.32f, 0f);

        RectTransform btnVit = UIHelper.CreateButton(ct, "BtnVit", "+ VIT",
            new Vector2(btnX, startY), new Vector2(140, 52), btnVitCol, 22);
        RectTransform btnStr = UIHelper.CreateButton(ct, "BtnStr", "+ STR",
            new Vector2(btnX, startY - gap), new Vector2(140, 52), btnStrCol, 22);
        RectTransform btnEnd = UIHelper.CreateButton(ct, "BtnEnd", "+ END",
            new Vector2(btnX, startY - gap * 2), new Vector2(140, 52), btnEndCol, 22);
        RectTransform btnAgi = UIHelper.CreateButton(ct, "BtnAgi", "+ AGI",
            new Vector2(btnX, startY - gap * 3), new Vector2(140, 52), btnAgiCol, 22);
        RectTransform btnDex = UIHelper.CreateButton(ct, "BtnDex", "+ DEX",
            new Vector2(btnX, startY - gap * 4), new Vector2(140, 52), btnDexCol, 22);

        // Fight button
        RectTransform btnNext = UIHelper.CreateButton(ct, "BtnNext", "FIGHT!",
            new Vector2(0, -230), new Vector2(300, 75), new Color(0.75f, 0.12f, 0.12f), 30);

        // StatManager
        GameObject smGo = new GameObject("StatManager");
        StatManager sm = smGo.AddComponent<StatManager>();
        sm.waveTitle = waveTitle;
        sm.pointsText = pointsText;
        sm.vitalityText = vitalityText;
        sm.strengthText = strengthText;
        sm.enduranceText = enduranceText;
        sm.agilityText = agilityText;
        sm.dexterityText = dexterityText;
        sm.warningText = warningText;
        sm.btnVitality = btnVit.GetComponent<Button>();
        sm.btnStrength = btnStr.GetComponent<Button>();
        sm.btnEndurance = btnEnd.GetComponent<Button>();
        sm.btnAgility = btnAgi.GetComponent<Button>();
        sm.btnDexterity = btnDex.GetComponent<Button>();
        sm.btnNext = btnNext.GetComponent<Button>();
    }
}
