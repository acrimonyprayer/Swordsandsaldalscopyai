using UnityEngine;
using TMPro;

public class EnemyGladiator : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int maxHealth;
    public int health;
    public int attackDamage;
    public int defense;
    public bool isSmallBoss = false;
    public bool isBigBoss = false;
    public bool isDefending = false;
    public bool isStunned = false;
    public string enemyName;

    [Header("References")]
    public PlayerGladiator playerGladiator;

    [Header("UI Texts")]
    public TextMeshProUGUI enemyHealthText;
    public TextMeshProUGUI enemyNameText;

    static readonly string[] prefixes = {
        "Ugly", "Mad", "Crazy", "Big", "Tiny", "Old", "Dark",
        "Brave", "Wild", "Sneaky", "Angry", "Rusty", "Iron",
        "Bloody", "Swift", "Lazy", "Fierce", "Smelly", "Bald"
    };

    static readonly string[] names = {
        "Bob", "Greg", "Thor", "Brutus", "Maximus", "Felix",
        "Ajax", "Nero", "Cassius", "Rex", "Goliath", "Blade",
        "Hammer", "Stone", "Spike", "Grog", "Flint", "Rook"
    };

    static readonly string[] bossTitles = {
        "The Destroyer", "The Butcher", "Death Bringer",
        "Skull Crusher", "The Executioner", "The Impaler",
        "Iron Fist", "Blood Storm"
    };

    static readonly string[] bigBossTitles = {
        "TITAN OF THE ARENA", "THE IMMORTAL", "GOD OF WAR",
        "THE ANNIHILATOR", "LORD OF BLADES", "THE COLOSSUS"
    };

    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = Color.white;
            sr.sortingOrder = 10;
        }

        int wave = GameManager.instance != null ? GameManager.instance.currentWave : 1;
        SpawnNewEnemy(wave);
    }

    public void SpawnNewEnemy(int wave)
    {
        isSmallBoss = false;
        isBigBoss = false;
        isDefending = false;
        isStunned = false;

        maxHealth = 50 + (wave * 15);
        attackDamage = 10 + (wave * 3);
        defense = wave;

        if (wave > 1 && wave % 15 == 0)
        {
            isBigBoss = true;
            maxHealth += 250;
            attackDamage += 25;
            defense += 10;
            transform.localScale = new Vector3(2.2f, 3f, 1f);
            enemyName = bigBossTitles[Random.Range(0, bigBossTitles.Length)];
        }
        else if (wave > 1 && wave % 5 == 0)
        {
            isSmallBoss = true;
            maxHealth += 100;
            attackDamage += 10;
            defense += 5;
            transform.localScale = new Vector3(1.8f, 2.4f, 1f);
            string n = names[Random.Range(0, names.Length)];
            string t = bossTitles[Random.Range(0, bossTitles.Length)];
            enemyName = n + " " + t;
        }
        else
        {
            transform.localScale = new Vector3(1.5f, 2f, 1f);
            string p = prefixes[Random.Range(0, prefixes.Length)];
            string n = names[Random.Range(0, names.Length)];
            enemyName = p + " " + n;
        }

        health = maxHealth;
        transform.position = new Vector3(3f, -2f + transform.localScale.y / 2f, 0f);

        if (enemyNameText != null)
        {
            enemyNameText.text = enemyName;
            if (isBigBoss)
                enemyNameText.color = new Color(1f, 0.2f, 0.2f);
            else if (isSmallBoss)
                enemyNameText.color = new Color(1f, 0.6f, 0.1f);
            else
                enemyNameText.color = Color.white;
        }

        UpdateUI();
    }

    public void MakeMove()
    {
        if (health <= 0 || playerGladiator.health <= 0) return;

        if (isStunned)
        {
            isStunned = false;
            playerGladiator.LogAction(enemyName + " IS STUNNED! SKIPS TURN!");
            Invoke("EndEnemyTurn", 1f);
            return;
        }

        float distance = Vector3.Distance(transform.position, playerGladiator.transform.position);

        if (distance > 3.5f)
        {
            float step = Mathf.Min(1.5f, distance - 2.5f);
            transform.position += new Vector3(-step, 0, 0);
            isDefending = false;
            playerGladiator.LogAction(enemyName + " ADVANCES!");
        }
        else
        {
            int roll = Random.Range(0, 100);

            if (health < maxHealth * 0.25f && roll < 15)
            {
                float retreatX = transform.position.x + 1.5f;
                if (retreatX < 8f)
                {
                    transform.position = new Vector3(retreatX, transform.position.y, 0);
                    isDefending = false;
                    playerGladiator.LogAction(enemyName + " RETREATS!");
                }
                else
                {
                    EnemyAttack();
                }
            }
            else if (roll < 60)
            {
                EnemyAttack();
            }
            else if (roll < 80)
            {
                EnemyHeavyAttack();
            }
            else
            {
                isDefending = true;
                playerGladiator.LogAction(enemyName + " TAKES A DEFENSIVE STANCE!");
            }
        }

        playerGladiator.UpdateUI();
        Invoke("EndEnemyTurn", 1f);
    }

    void EnemyAttack()
    {
        isDefending = false;
        playerGladiator.LogAction(enemyName + " ATTACKS!");
        playerGladiator.TakeDamage(attackDamage, false);
    }

    void EnemyHeavyAttack()
    {
        isDefending = false;
        int heavyDamage = Mathf.RoundToInt(attackDamage * 1.5f);

        if (Random.Range(0, 100) < 55)
        {
            playerGladiator.LogAction(enemyName + " LANDS A DEVASTATING BLOW!");
            playerGladiator.TakeDamage(heavyDamage, true);
        }
        else
        {
            playerGladiator.LogAction(enemyName + "'S HEAVY SWING MISSES!");
        }
    }

    void EndEnemyTurn()
    {
        if (playerGladiator.health > 0)
            playerGladiator.OnEnemyTurnEnd();
    }

    public void TakeDamage(int damage, bool wasCrit)
    {
        if (isDefending)
        {
            damage /= 2;
            isDefending = false;
        }

        damage = Mathf.Max(1, damage - defense);
        health -= damage;

        string msg = wasCrit ? "CRITICAL! " : "";
        msg += enemyName + " TAKES " + damage + " DAMAGE!";

        if (health <= 0)
        {
            health = 0;
            UpdateUI();
            playerGladiator.LogAction(enemyName + " IS DEFEATED! THE CROWD ROARS!");
            playerGladiator.WinWave();
            return;
        }

        playerGladiator.LogAction(msg);
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (enemyHealthText != null)
        {
            string label = isBigBoss ? "BIG BOSS" : (isSmallBoss ? "BOSS" : "ENEMY");
            enemyHealthText.text = label + " HP: " + health + "/" + maxHealth;
        }
    }
}
