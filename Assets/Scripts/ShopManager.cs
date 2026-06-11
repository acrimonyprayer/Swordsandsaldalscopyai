using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    [Header("Info Texts")]
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI messageText;
    public TextMeshProUGUI weaponText;
    public TextMeshProUGUI armorText;
    public TextMeshProUGUI helmetText;
    public TextMeshProUGUI shieldText;
    public TextMeshProUGUI potionText;
    public TextMeshProUGUI statsPreviewText;

    [Header("Buttons")]
    public Button buyWeaponButton;
    public Button buyArmorButton;
    public Button buyHelmetButton;
    public Button buyShieldButton;
    public Button buyPotionButton;
    public Button fightButton;

    void Start()
    {
        if (buyWeaponButton != null) buyWeaponButton.onClick.AddListener(BuyWeapon);
        if (buyArmorButton != null) buyArmorButton.onClick.AddListener(BuyArmor);
        if (buyHelmetButton != null) buyHelmetButton.onClick.AddListener(BuyHelmet);
        if (buyShieldButton != null) buyShieldButton.onClick.AddListener(BuyShield);
        if (buyPotionButton != null) buyPotionButton.onClick.AddListener(BuyPotion);
        if (fightButton != null) fightButton.onClick.AddListener(GoToFight);

        if (messageText != null) messageText.text = "WELCOME TO THE ARMORY!";
        UpdateUI();
    }

    public void BuyWeapon()
    {
        if (GameManager.instance == null) return;
        int next = GameManager.instance.weaponTier + 1;
        if (next >= GameManager.WeaponNames.Length)
        {
            SetMessage("MAX WEAPON TIER!");
            return;
        }
        if (GameManager.instance.gold >= GameManager.WeaponPrices[next])
        {
            GameManager.instance.gold -= GameManager.WeaponPrices[next];
            GameManager.instance.weaponTier = next;
            SetMessage("BOUGHT " + GameManager.WeaponNames[next] + "!");
            UpdateUI();
        }
        else
        {
            SetMessage("NOT ENOUGH GOLD! NEED " + GameManager.WeaponPrices[next] + "G");
        }
    }

    public void BuyArmor()
    {
        if (GameManager.instance == null) return;
        int next = GameManager.instance.armorTier + 1;
        if (next >= GameManager.ArmorNames.Length)
        {
            SetMessage("MAX ARMOR TIER!");
            return;
        }
        if (GameManager.instance.gold >= GameManager.ArmorPrices[next])
        {
            GameManager.instance.gold -= GameManager.ArmorPrices[next];
            GameManager.instance.armorTier = next;
            SetMessage("BOUGHT " + GameManager.ArmorNames[next] + "!");
            UpdateUI();
        }
        else
        {
            SetMessage("NOT ENOUGH GOLD! NEED " + GameManager.ArmorPrices[next] + "G");
        }
    }

    public void BuyHelmet()
    {
        if (GameManager.instance == null) return;
        int next = GameManager.instance.helmetTier + 1;
        if (next >= GameManager.HelmetNames.Length)
        {
            SetMessage("MAX HELMET TIER!");
            return;
        }
        if (GameManager.instance.gold >= GameManager.HelmetPrices[next])
        {
            GameManager.instance.gold -= GameManager.HelmetPrices[next];
            GameManager.instance.helmetTier = next;
            SetMessage("BOUGHT " + GameManager.HelmetNames[next] + "!");
            UpdateUI();
        }
        else
        {
            SetMessage("NOT ENOUGH GOLD! NEED " + GameManager.HelmetPrices[next] + "G");
        }
    }

    public void BuyShield()
    {
        if (GameManager.instance == null) return;
        int next = GameManager.instance.shieldTier + 1;
        if (next >= GameManager.ShieldNames.Length)
        {
            SetMessage("MAX SHIELD TIER!");
            return;
        }
        if (GameManager.instance.gold >= GameManager.ShieldPrices[next])
        {
            GameManager.instance.gold -= GameManager.ShieldPrices[next];
            GameManager.instance.shieldTier = next;
            SetMessage("BOUGHT " + GameManager.ShieldNames[next] + "!");
            UpdateUI();
        }
        else
        {
            SetMessage("NOT ENOUGH GOLD! NEED " + GameManager.ShieldPrices[next] + "G");
        }
    }

    public void BuyPotion()
    {
        if (GameManager.instance == null) return;
        if (GameManager.instance.gold >= GameManager.PotionPrice)
        {
            GameManager.instance.gold -= GameManager.PotionPrice;
            GameManager.instance.healthPotions++;
            SetMessage("BOUGHT HEALTH POTION!");
            UpdateUI();
        }
        else
        {
            SetMessage("NOT ENOUGH GOLD! NEED " + GameManager.PotionPrice + "G");
        }
    }

    public void GoToFight()
    {
        SceneManager.LoadScene("SampleScene");
    }

    void SetMessage(string msg)
    {
        if (messageText != null) messageText.text = msg;
    }

    void UpdateUI()
    {
        var gm = GameManager.instance;
        if (gm == null) return;

        if (goldText != null)
            goldText.text = "GOLD: " + gm.gold;

        UpdateEquipmentText(weaponText, "WEAPON", GameManager.WeaponNames, GameManager.WeaponDamages, gm.weaponTier, "DMG");
        UpdateEquipmentText(armorText, "ARMOR", GameManager.ArmorNames, GameManager.ArmorDefenses, gm.armorTier, "DEF");
        UpdateEquipmentText(helmetText, "HELMET", GameManager.HelmetNames, GameManager.HelmetDefenses, gm.helmetTier, "DEF");
        UpdateEquipmentText(shieldText, "SHIELD", GameManager.ShieldNames, GameManager.ShieldBlocks, gm.shieldTier, "BLOCK");

        UpdateBuyButton(buyWeaponButton, GameManager.WeaponNames, GameManager.WeaponDamages, GameManager.WeaponPrices, gm.weaponTier, "DMG");
        UpdateBuyButton(buyArmorButton, GameManager.ArmorNames, GameManager.ArmorDefenses, GameManager.ArmorPrices, gm.armorTier, "DEF");
        UpdateBuyButton(buyHelmetButton, GameManager.HelmetNames, GameManager.HelmetDefenses, GameManager.HelmetPrices, gm.helmetTier, "DEF");
        UpdateBuyButton(buyShieldButton, GameManager.ShieldNames, GameManager.ShieldBlocks, GameManager.ShieldPrices, gm.shieldTier, "BLOCK");

        if (potionText != null)
            potionText.text = "POTIONS: " + gm.healthPotions + " | HEALS " + GameManager.PotionHealAmount + " HP";

        if (buyPotionButton != null)
        {
            var t = buyPotionButton.GetComponentInChildren<TextMeshProUGUI>();
            if (t != null) t.text = "BUY POTION\n" + GameManager.PotionPrice + "G";
        }

        if (statsPreviewText != null)
        {
            statsPreviewText.text = "YOUR STATS  |  ATK: " + gm.GetAttackDamage() +
                "  |  DEF: " + gm.GetTotalDefense() +
                "  |  HP: " + gm.GetMaxHealth() +
                "  |  BLOCK: " + gm.GetBlockChance() + "%";
        }
    }

    void UpdateEquipmentText(TextMeshProUGUI text, string label, string[] names, int[] values, int tier, string stat)
    {
        if (text == null) return;
        text.text = label + ": " + names[tier] + " (+" + values[tier] + " " + stat + ")";
    }

    void UpdateBuyButton(Button btn, string[] names, int[] values, int[] prices, int tier, string stat)
    {
        if (btn == null) return;
        var t = btn.GetComponentInChildren<TextMeshProUGUI>();
        int next = tier + 1;
        if (next < names.Length)
        {
            if (t != null) t.text = names[next] + "\n+" + values[next] + " " + stat + " | " + prices[next] + "G";
            btn.interactable = true;
        }
        else
        {
            if (t != null) t.text = "MAX TIER";
            btn.interactable = false;
        }
    }
}
