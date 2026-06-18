using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ShopSceneSetup : MonoBehaviour
{
    void Awake()
    {
        Camera cam = SetupCamera();
        EnsureEventSystem();
        UIHelper.CreateArenaBackground(cam, 0.25f);
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
        cam.backgroundColor = new Color(0.05f, 0.05f, 0.08f);
        return cam;
    }

    void EnsureEventSystem()
    {
        if (FindAnyObjectByType<EventSystem>() == null)
        {
            GameObject go = new GameObject("EventSystem");
            go.AddComponent<EventSystem>();
            go.AddComponent<StandaloneInputModule>();
        }
    }

    void BuildUI()
    {
        Canvas canvas = UIHelper.CreateCanvas();
        Transform ct = canvas.transform;

        UIHelper.CreatePanel(ct, new Color(0.02f, 0.02f, 0.02f, 0.85f));

        // Sol Ust Title
        UIHelper.CreateText(ct, "Title", "THE ARMORY",
            new Vector2(-550, 450), 50, new Color(0.9f, 0.9f, 0.9f),
            TextAlignmentOptions.Left, 600, 60);

        // Sag Ust Gold (Sari arkaplan ile)
        TextMeshProUGUI goldText = UIHelper.CreateTextWithBG(ct, "GoldText", "",
            new Vector2(650, 450), 32, new Color(1f, 0.85f, 0.2f),
            TextAlignmentOptions.Right, 350, 50, 0.4f);

        // Merkez Message
        TextMeshProUGUI messageText = UIHelper.CreateText(ct, "MessageText", "",
            new Vector2(0, 360), 28, new Color(1f, 0.4f, 0.4f),
            TextAlignmentOptions.Center, 900, 40);

        // Liste hizzalari
        float headerX = -550f;
        float infoX = -100f;
        float btnX = 550f;
        float startY = 240f;
        float gap = 90f;

        Color headerWeapon = new Color(1f, 0.4f, 0.35f);
        Color headerArmor = new Color(0.4f, 0.65f, 1f);
        Color headerHelmet = new Color(0.4f, 0.9f, 0.9f);
        Color headerShield = new Color(0.4f, 0.9f, 0.4f);
        Color headerPotion = new Color(0.85f, 0.4f, 0.85f);
        Color infoCol = new Color(0.9f, 0.9f, 0.9f);
        Color btnCol = new Color(0.15f, 0.15f, 0.2f); // Koyu butonlar

        // Weapon
        UIHelper.CreateTextWithBG(ct, "Bg1", "", new Vector2(0, startY), 10, Color.clear, TextAlignmentOptions.Center, 1400, 75, 0.3f);
        UIHelper.CreateText(ct, "WeaponHeader", "WEAPON", new Vector2(headerX, startY), 28, headerWeapon, TextAlignmentOptions.Left, 200, 40);
        TextMeshProUGUI weaponText = UIHelper.CreateText(ct, "WeaponText", "", new Vector2(infoX, startY), 26, infoCol, TextAlignmentOptions.Left, 600, 40);
        RectTransform buyWeaponBtn = UIHelper.CreateButton(ct, "BuyWeaponBtn", "", new Vector2(btnX, startY), new Vector2(250, 60), btnCol, 22);

        // Armor
        UIHelper.CreateTextWithBG(ct, "Bg2", "", new Vector2(0, startY - gap), 10, Color.clear, TextAlignmentOptions.Center, 1400, 75, 0.3f);
        UIHelper.CreateText(ct, "ArmorHeader", "ARMOR", new Vector2(headerX, startY - gap), 28, headerArmor, TextAlignmentOptions.Left, 200, 40);
        TextMeshProUGUI armorText = UIHelper.CreateText(ct, "ArmorText", "", new Vector2(infoX, startY - gap), 26, infoCol, TextAlignmentOptions.Left, 600, 40);
        RectTransform buyArmorBtn = UIHelper.CreateButton(ct, "BuyArmorBtn", "", new Vector2(btnX, startY - gap), new Vector2(250, 60), btnCol, 22);

        // Helmet
        UIHelper.CreateTextWithBG(ct, "Bg3", "", new Vector2(0, startY - gap * 2), 10, Color.clear, TextAlignmentOptions.Center, 1400, 75, 0.3f);
        UIHelper.CreateText(ct, "HelmetHeader", "HELMET", new Vector2(headerX, startY - gap * 2), 28, headerHelmet, TextAlignmentOptions.Left, 200, 40);
        TextMeshProUGUI helmetText = UIHelper.CreateText(ct, "HelmetText", "", new Vector2(infoX, startY - gap * 2), 26, infoCol, TextAlignmentOptions.Left, 600, 40);
        RectTransform buyHelmetBtn = UIHelper.CreateButton(ct, "BuyHelmetBtn", "", new Vector2(btnX, startY - gap * 2), new Vector2(250, 60), btnCol, 22);

        // Shield
        UIHelper.CreateTextWithBG(ct, "Bg4", "", new Vector2(0, startY - gap * 3), 10, Color.clear, TextAlignmentOptions.Center, 1400, 75, 0.3f);
        UIHelper.CreateText(ct, "ShieldHeader", "SHIELD", new Vector2(headerX, startY - gap * 3), 28, headerShield, TextAlignmentOptions.Left, 200, 40);
        TextMeshProUGUI shieldText = UIHelper.CreateText(ct, "ShieldText", "", new Vector2(infoX, startY - gap * 3), 26, infoCol, TextAlignmentOptions.Left, 600, 40);
        RectTransform buyShieldBtn = UIHelper.CreateButton(ct, "BuyShieldBtn", "", new Vector2(btnX, startY - gap * 3), new Vector2(250, 60), btnCol, 22);

        // Potion
        UIHelper.CreateTextWithBG(ct, "Bg5", "", new Vector2(0, startY - gap * 4), 10, Color.clear, TextAlignmentOptions.Center, 1400, 75, 0.3f);
        UIHelper.CreateText(ct, "PotionHeader", "POTION", new Vector2(headerX, startY - gap * 4), 28, headerPotion, TextAlignmentOptions.Left, 200, 40);
        TextMeshProUGUI potionText = UIHelper.CreateText(ct, "PotionText", "", new Vector2(infoX, startY - gap * 4), 26, infoCol, TextAlignmentOptions.Left, 600, 40);
        RectTransform buyPotionBtn = UIHelper.CreateButton(ct, "BuyPotionBtn", "", new Vector2(btnX, startY - gap * 4), new Vector2(250, 60), btnCol, 22);

        // Stats preview
        TextMeshProUGUI statsPreviewText = UIHelper.CreateTextWithBG(ct, "StatsPreview", "",
            new Vector2(0, -220), 24, new Color(0.8f, 0.8f, 0.8f),
            TextAlignmentOptions.Center, 1200, 60, 0.6f);

        // Fight button (En altta dev gibi)
        RectTransform fightBtn = UIHelper.CreateButton(ct, "FightBtn", "ENTER ARENA",
            new Vector2(0, -350), new Vector2(400, 80), new Color(0.8f, 0.1f, 0.1f), 32);

        // ShopManager ayarlamalari
        GameObject shopGo = new GameObject("ShopManager");
        ShopManager shop = shopGo.AddComponent<ShopManager>();
        shop.goldText = goldText;
        shop.messageText = messageText;
        shop.weaponText = weaponText;
        shop.armorText = armorText;
        shop.helmetText = helmetText;
        shop.shieldText = shieldText;
        shop.potionText = potionText;
        shop.statsPreviewText = statsPreviewText;
        shop.buyWeaponButton = buyWeaponBtn.GetComponent<Button>();
        shop.buyArmorButton = buyArmorBtn.GetComponent<Button>();
        shop.buyHelmetButton = buyHelmetBtn.GetComponent<Button>();
        shop.buyShieldButton = buyShieldBtn.GetComponent<Button>();
        shop.buyPotionButton = buyPotionBtn.GetComponent<Button>();
        shop.fightButton = fightBtn.GetComponent<Button>();
    }
}