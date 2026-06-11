using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int statPoints = 10;
    public int currentWave = 1;
    public int gold = 0;

    public int vitality = 1;
    public int strength = 1;
    public int endurance = 1;
    public int agility = 1;
    public int dexterity = 1;

    public int weaponTier = 0;
    public int armorTier = 0;
    public int helmetTier = 0;
    public int shieldTier = 0;

    public int healthPotions = 0;

    public static readonly string[] WeaponNames = { "Fists", "Rusty Sword", "Iron Sword", "Steel Blade", "War Axe", "Obsidian Edge", "Legendary Blade" };
    public static readonly int[] WeaponDamages = { 0, 5, 12, 20, 30, 42, 55 };
    public static readonly int[] WeaponPrices = { 0, 40, 100, 200, 350, 550, 800 };

    public static readonly string[] ArmorNames = { "Rags", "Leather Armor", "Chain Mail", "Iron Plate", "Steel Armor", "Dragon Plate" };
    public static readonly int[] ArmorDefenses = { 0, 3, 7, 12, 18, 25 };
    public static readonly int[] ArmorPrices = { 0, 35, 90, 180, 300, 500 };

    public static readonly string[] HelmetNames = { "None", "Leather Cap", "Iron Helm", "Steel Helm", "War Helmet", "Dragon Crown" };
    public static readonly int[] HelmetDefenses = { 0, 2, 5, 8, 12, 18 };
    public static readonly int[] HelmetPrices = { 0, 25, 60, 130, 220, 380 };

    public static readonly string[] ShieldNames = { "None", "Wooden Shield", "Iron Shield", "Steel Shield", "Tower Shield", "Dragon Shield" };
    public static readonly int[] ShieldBlocks = { 0, 5, 10, 16, 22, 30 };
    public static readonly int[] ShieldPrices = { 0, 30, 70, 150, 250, 420 };

    public static readonly int PotionPrice = 15;
    public static readonly int PotionHealAmount = 30;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    public int GetAttackDamage()
    {
        return 5 + (strength * 3) + WeaponDamages[weaponTier];
    }

    public int GetMaxHealth()
    {
        return 50 + (vitality * 10);
    }

    public int GetMaxEnergy()
    {
        return 30 + (endurance * 5);
    }

    public int GetTotalDefense()
    {
        return ArmorDefenses[armorTier] + HelmetDefenses[helmetTier];
    }

    public int GetBlockChance()
    {
        return 10 + (endurance * 2) + ShieldBlocks[shieldTier];
    }

    public int GetDodgeChance()
    {
        return 5 + (agility * 3);
    }

    public float GetMoveDistance()
    {
        return 1.0f + (dexterity * 0.15f);
    }

    public int GetCritChance()
    {
        return 5 + (dexterity * 2);
    }

    public int GetTauntChance()
    {
        return 10 + (agility * 2) + (dexterity * 2);
    }
}
