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
        UIHelper.CreateArenaBackground(cam, 0.3f);
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
        cam.backgroundColor = new Color(0.06f, 0.04f, 0.03f);
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

    void BuildUI()
    {
        Canvas canvas = UIHelper.CreateCanvas();
        Transform ct = canvas.transform;

        UIHelper.CreatePanel(ct, new Color(0, 0, 0, 0.7f));

        // Title
        UIHelper.CreateText(ct, "Title", "THE ARMORY",
            new Vector2(0, 480), 42, new Color(1f, 0.85f, 0.2f),
            TextAlignmentOptions.Center, 600, 55);

        // Gold
        TextMeshProUGUI goldText = UIHelper.CreateText(ct, "GoldText", "",
            new Vector2(0, 420), 34, new Color(1f, 0.85f, 0.2f),
            TextAlignmentOptions.Center, 400, 45);

        // Message
        TextMeshProUGUI messageText = UIHelper.CreateText(ct, "MessageText", "",
            new Vector2(0, 370), 24, new Color(0.9f, 0.9f, 0.9f),
            TextAlignmentOptions.Center, 700, 40);

        // Category layout
        float infoX = -230f;
        float btnX = 340f;
        float startY = 290f;
        float gap = 72f;

        Color headerWeapon = new Color(1f, 0.4f, 0.35f);
        Color headerArmor = new Color(0.4f, 0.65f, 1f);
        Color headerHelmet = new Color(0.4f, 0.9f, 0.9f);
        Color headerShield = new Color(0.4f, 0.9f, 0.4f);
        Color headerPotion = new Color(0.85f, 0.4f, 0.85f);
        Color infoCol = new Color(0.85f, 0.85f, 0.85f);

        // Weapon
        UIHelper.CreateText(ct, "WeaponHeader", "WEAPON",
            new Vector2(-420, startY + 12), 16, headerWeapon,
            TextAlignmentOptions.Left, 120, 25);
        TextMeshProUGUI weaponText = UIHelper.CreateText(ct, "WeaponText", "",
            new Vector2(infoX, startY), 21, infoCol, TextAlignmentOptions.Left, 500, 38);
        RectTransform buyWeaponBtn = UIHelper.CreateButton(ct, "BuyWeaponBtn", "",
            new Vector2(btnX, startY), new Vector2(320, 52), new Color(0.55f, 0.18f, 0.15f), 15);

        // Armor
        UIHelper.CreateText(ct, "ArmorHeader", "ARMOR",
            new Vector2(-420, startY - gap + 12), 16, headerArmor,
            TextAlignmentOptions.Left, 120, 25);
        TextMeshProUGUI armorText = UIHelper.CreateText(ct, "ArmorText", "",
            new Vector2(infoX, startY - gap), 21, infoCol, TextAlignmentOptions.Left, 500, 38);
        RectTransform buyArmorBtn = UIHelper.CreateButton(ct, "BuyArmorBtn", "",
            new Vector2(btnX, startY - gap), new Vector2(320, 52), new Color(0.15f, 0.3f, 0.55f), 15);

        // Helmet
        UIHelper.CreateText(ct, "HelmetHeader", "HELMET",
            new Vector2(-420, startY - gap * 2 + 12), 16, headerHelmet,
            TextAlignmentOptions.Left, 120, 25);
        TextMeshProUGUI helmetText = UIHelper.CreateText(ct, "HelmetText", "",
            new Vector2(infoX, startY - gap * 2), 21, infoCol, TextAlignmentOptions.Left, 500, 38);
        RectTransform buyHelmetBtn = UIHelper.CreateButton(ct, "BuyHelmetBtn", "",
            new Vector2(btnX, startY - gap * 2), new Vector2(320, 52), new Color(0.15f, 0.45f, 0.5f), 15);

        // Shield
        UIHelper.CreateText(ct, "ShieldHeader", "SHIELD",
            new Vector2(-420, startY - gap * 3 + 12), 16, headerShield,
            TextAlignmentOptions.Left, 120, 25);
        TextMeshProUGUI shieldText = UIHelper.CreateText(ct, "ShieldText", "",
            new Vector2(infoX, startY - gap * 3), 21, infoCol, TextAlignmentOptions.Left, 500, 38);
        RectTransform buyShieldBtn = UIHelper.CreateButton(ct, "BuyShieldBtn", "",
            new Vector2(btnX, startY - gap * 3), new Vector2(320, 52), new Color(0.15f, 0.45f, 0.2f), 15);

        // Potion
        UIHelper.CreateText(ct, "PotionHeader", "POTION",
            new Vector2(-420, startY - gap * 4 + 12), 16, headerPotion,
            TextAlignmentOptions.Left, 120, 25);
        TextMeshProUGUI potionText = UIHelper.CreateText(ct, "PotionText", "",
            new Vector2(infoX, startY - gap * 4), 21, infoCol, TextAlignmentOptions.Left, 500, 38);
        RectTransform buyPotionBtn = UIHelper.CreateButton(ct, "BuyPotionBtn", "",
            new Vector2(btnX, startY - gap * 4), new Vector2(320, 52), new Color(0.45f, 0.15f, 0.45f), 15);

        // Stats preview
        TextMeshProUGUI statsPreviewText = UIHelper.CreateText(ct, "StatsPreview", "",
            new Vector2(0, -140), 20, new Color(0.7f, 0.7f, 0.7f),
            TextAlignmentOptions.Center, 900, 40);

        // Fight button
        RectTransform fightBtn = UIHelper.CreateButton(ct, "FightBtn", "GO TO ARENA!",
            new Vector2(0, -230), new Vector2(320, 75), new Color(0.75f, 0.12f, 0.12f), 28);

        // ShopManager
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
