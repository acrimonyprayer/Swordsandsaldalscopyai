using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerGladiator : MonoBehaviour
{
    [Header("Player Stats")]
    public int maxHealth;
    public int health;
    public int maxEnergy;
    public int energy;
    public int attackDamage;
    public int defense;
    public int blockChance;
    public int dodgeChance;
    public int critChance;
    public int tauntChance;
    public float moveDistance;
    public int healthPotions;
    public bool isDefending = false;
    public bool isPlayerTurn = true;

    [Header("References")]
    public EnemyGladiator enemyGladiator;

    [Header("UI Texts")]
    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI playerEnergyText;
    public TextMeshProUGUI actionLogText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI distanceText;

    [Header("Action Buttons")]
    public RectTransform attackButtonTransform;
    public RectTransform heavyAttackButtonTransform;
    public RectTransform tauntButtonTransform;
    public RectTransform defendButtonTransform;
    public RectTransform moveLeftButtonTransform;
    public RectTransform moveRightButtonTransform;
    public RectTransform restButtonTransform;
    public RectTransform usePotionButtonTransform;

    [Header("End Buttons")]
    public RectTransform restartButtonTransform;
    public RectTransform nextWaveButtonTransform;

    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = Color.white;
            sr.sortingOrder = 10;
        }

        transform.localScale = new Vector3(1.5f, 2f, 1f);
        transform.position = new Vector3(-3f, -2f + transform.localScale.y / 2f, 0f);

        InitializeStats();
        SetupUI();
        UpdateUI();

        int wave = GameManager.instance != null ? GameManager.instance.currentWave : 1;
        if (waveText != null) waveText.text = "WAVE " + wave;
        LogAction("WAVE " + wave + "! FIGHT!");
    }

    void InitializeStats()
    {
        if (GameManager.instance != null)
        {
            var gm = GameManager.instance;
            maxHealth = gm.GetMaxHealth();
            health = maxHealth;
            attackDamage = gm.GetAttackDamage();
            maxEnergy = gm.GetMaxEnergy();
            energy = maxEnergy;
            defense = gm.GetTotalDefense();
            blockChance = gm.GetBlockChance();
            dodgeChance = gm.GetDodgeChance();
            critChance = gm.GetCritChance();
            tauntChance = gm.GetTauntChance();
            moveDistance = gm.GetMoveDistance();
            healthPotions = gm.healthPotions;
        }
        else
        {
            maxHealth = 100; health = 100;
            maxEnergy = 50; energy = 50;
            attackDamage = 15; defense = 0;
            blockChance = 15; dodgeChance = 10;
            critChance = 10; tauntChance = 15;
            moveDistance = 1.5f; healthPotions = 0;
        }
    }

    void SetupUI()
    {
        SetupButton(attackButtonTransform, PlayerAttack);
        SetupButton(heavyAttackButtonTransform, PlayerHeavyAttack);
        SetupButton(tauntButtonTransform, PlayerTaunt);
        SetupButton(defendButtonTransform, PlayerDefend);

        SetupButton(moveLeftButtonTransform, MoveLeft);
        SetupButton(restButtonTransform, PlayerRest);
        SetupButton(usePotionButtonTransform, UsePotion);
        SetupButton(moveRightButtonTransform, MoveRight);

        if (playerHealthText != null) playerHealthText.color = new Color(0.2f, 0.9f, 0.2f);
        if (playerEnergyText != null) playerEnergyText.color = new Color(0.2f, 0.9f, 0.9f);
        if (actionLogText != null) actionLogText.color = new Color(1f, 0.85f, 0.3f);
        if (distanceText != null) distanceText.color = Color.white;

        if (gameOverText != null)
            gameOverText.gameObject.SetActive(false);

        if (restartButtonTransform != null)
        {
            restartButtonTransform.gameObject.SetActive(false);
            Button restartBtn = restartButtonTransform.GetComponent<Button>();
            if (restartBtn != null) restartBtn.onClick.AddListener(RestartGame);
        }

        if (nextWaveButtonTransform != null)
        {
            nextWaveButtonTransform.gameObject.SetActive(false);
            Button nextBtn = nextWaveButtonTransform.GetComponent<Button>();
            if (nextBtn != null) nextBtn.onClick.AddListener(StartNextWave);
        }
    }

    void SetupButton(RectTransform rect, UnityEngine.Events.UnityAction action)
    {
        if (rect == null) return;
        Button btn = rect.GetComponent<Button>();
        if (btn != null) btn.onClick.AddListener(action);
    }

    public void PlayerAttack()
    {
        if (!CanAct()) return;

        float dist = Vector3.Distance(transform.position, enemyGladiator.transform.position);
        if (dist > 3.5f)
        {
            LogAction("TOO FAR! MOVE CLOSER!");
            return;
        }

        if (energy < 10)
        {
            LogAction("NOT ENOUGH ENERGY! REST!");
            return;
        }

        energy -= 10;
        isDefending = false;

        int damage = attackDamage;
        bool isCrit = Random.Range(0, 100) < critChance;
        if (isCrit)
            damage = Mathf.RoundToInt(damage * 1.5f);

        enemyGladiator.TakeDamage(damage, isCrit);
        EndTurn();
    }

    public void PlayerHeavyAttack()
    {
        if (!CanAct()) return;

        float dist = Vector3.Distance(transform.position, enemyGladiator.transform.position);
        if (dist > 3.5f)
        {
            LogAction("TOO FAR! MOVE CLOSER!");
            return;
        }

        if (energy < 20)
        {
            LogAction("NOT ENOUGH ENERGY! REST!");
            return;
        }

        energy -= 20;
        isDefending = false;

        int hitChance = 35 + (GameManager.instance != null ? GameManager.instance.agility * 2 : 0);
        if (Random.Range(0, 100) < hitChance)
        {
            int damage = attackDamage * 2;
            bool isCrit = Random.Range(0, 100) < critChance;
            if (isCrit)
                damage = Mathf.RoundToInt(damage * 1.5f);

            LogAction("HEAVY ATTACK CONNECTS!");
            enemyGladiator.TakeDamage(damage, isCrit);
        }
        else
        {
            LogAction("HEAVY SWING MISSES! THE CROWD BOOS!");
        }
        EndTurn();
    }

    public void PlayerTaunt()
    {
        if (!CanAct()) return;

        if (energy < 5)
        {
            LogAction("NOT ENOUGH ENERGY!");
            return;
        }

        energy -= 5;
        isDefending = false;

        if (Random.Range(0, 100) < tauntChance)
        {
            enemyGladiator.isStunned = true;
            LogAction("THE CROWD LAUGHS! " + enemyGladiator.enemyName + " IS STUNNED!");
        }
        else
        {
            LogAction(enemyGladiator.enemyName + " IS UNIMPRESSED.");
        }
        EndTurn();
    }

    public void PlayerDefend()
    {
        if (!CanAct()) return;

        if (energy < 10)
        {
            LogAction("NOT ENOUGH ENERGY!");
            return;
        }

        isDefending = true;
        energy -= 10;
        LogAction("PLAYER TAKES A DEFENSIVE STANCE!");
        EndTurn();
    }

    public void PlayerRest()
    {
        if (!CanAct()) return;

        int healAmount = 5 + (GameManager.instance != null ? GameManager.instance.vitality : 0);
        health = Mathf.Min(health + healAmount, maxHealth);

        int energyGain = 20 + (GameManager.instance != null ? GameManager.instance.endurance * 2 : 0);
        energy = Mathf.Min(energy + energyGain, maxEnergy);

        isDefending = false;
        LogAction("PLAYER RESTS. +" + healAmount + " HP, +" + energyGain + " EN");
        EndTurn();
    }

    public void UsePotion()
    {
        if (!CanAct()) return;

        if (healthPotions <= 0)
        {
            LogAction("NO POTIONS LEFT!");
            return;
        }

        healthPotions--;
        int heal = GameManager.PotionHealAmount;
        health = Mathf.Min(health + heal, maxHealth);
        isDefending = false;
        LogAction("DRINKS POTION! +" + heal + " HP!");
        UpdateUI();
        EndTurn();
    }

    public void MoveLeft()
    {
        if (!CanAct()) return;

        float newX = transform.position.x - moveDistance;
        if (newX < -8f)
        {
            LogAction("ARENA WALL!");
            return;
        }

        transform.position += new Vector3(-moveDistance, 0, 0);
        isDefending = false;
        LogAction("PLAYER RETREATS.");
        EndTurn();
    }

    public void MoveRight()
    {
        if (!CanAct()) return;

        float distToEnemy = Vector3.Distance(
            transform.position + new Vector3(moveDistance, 0, 0),
            enemyGladiator.transform.position
        );

        if (distToEnemy < 1.0f)
        {
            LogAction("TOO CLOSE!");
            return;
        }

        transform.position += new Vector3(moveDistance, 0, 0);
        isDefending = false;
        LogAction("PLAYER ADVANCES.");
        EndTurn();
    }

    bool CanAct()
    {
        return isPlayerTurn && health > 0 && enemyGladiator != null && enemyGladiator.health > 0;
    }

    void EndTurn()
    {
        if (enemyGladiator.health <= 0) return;

        isPlayerTurn = false;
        UpdateUI();
        SetButtonsInteractable(false);
        Invoke("StartEnemyTurn", 1.2f);
    }

    void StartEnemyTurn()
    {
        if (enemyGladiator != null && enemyGladiator.health > 0)
            enemyGladiator.MakeMove();
    }

    public void OnEnemyTurnEnd()
    {
        if (health > 0)
        {
            isPlayerTurn = true;
            SetButtonsInteractable(true);
            LogAction("YOUR TURN.");
            UpdateUI();
        }
    }

    public void TakeDamage(int damage, bool wasHeavy)
    {
        int effectiveDodge = isDefending ? dodgeChance : dodgeChance / 3;
        if (Random.Range(0, 100) < effectiveDodge)
        {
            isDefending = false;
            LogAction("PLAYER DODGES!");
            UpdateUI();
            return;
        }

        if (isDefending && Random.Range(0, 100) < blockChance)
        {
            damage /= 2;
            LogAction("PLAYER BLOCKS!");
        }
        isDefending = false;

        damage = Mathf.Max(1, damage - defense);
        health -= damage;

        if (health <= 0)
        {
            health = 0;
            LogAction("FATAL BLOW! -" + damage + " HP!");
            ShowGameOver("YOU LOST!", Color.red);
        }
        else
        {
            string msg = wasHeavy ? "BRUTAL HIT! -" : "HIT! -";
            LogAction(msg + damage + " HP!");
        }
        UpdateUI();
    }

    public void WinWave()
    {
        if (GameManager.instance != null)
        {
            var gm = GameManager.instance;
            gm.healthPotions = healthPotions;
            gm.currentWave++;
            gm.statPoints += 3;

            int rewardGold = 50 * gm.currentWave;
            gm.gold += rewardGold;
            LogAction("VICTORY! +3 STAT POINTS, +" + rewardGold + " GOLD!");
        }
        else
        {
            LogAction("VICTORY!");
        }

        SetButtonsActive(false);
        if (nextWaveButtonTransform != null)
            nextWaveButtonTransform.gameObject.SetActive(true);
        if (restartButtonTransform != null)
            restartButtonTransform.gameObject.SetActive(true);
    }

    public void StartNextWave()
    {
        SceneManager.LoadScene("StatScene");
    }

    public void LogAction(string message)
    {
        if (actionLogText != null)
            actionLogText.text = message;
    }

    public void ShowGameOver(string message, Color textColor)
    {
        if (gameOverText != null)
        {
            gameOverText.text = message;
            gameOverText.color = textColor;
            gameOverText.gameObject.SetActive(true);
        }
        if (restartButtonTransform != null)
            restartButtonTransform.gameObject.SetActive(true);
        SetButtonsActive(false);
    }

    void SetButtonsActive(bool active)
    {
        if (attackButtonTransform != null) attackButtonTransform.gameObject.SetActive(active);
        if (heavyAttackButtonTransform != null) heavyAttackButtonTransform.gameObject.SetActive(active);
        if (tauntButtonTransform != null) tauntButtonTransform.gameObject.SetActive(active);
        if (defendButtonTransform != null) defendButtonTransform.gameObject.SetActive(active);
        if (moveLeftButtonTransform != null) moveLeftButtonTransform.gameObject.SetActive(active);
        if (moveRightButtonTransform != null) moveRightButtonTransform.gameObject.SetActive(active);
        if (restButtonTransform != null) restButtonTransform.gameObject.SetActive(active);
        if (usePotionButtonTransform != null) usePotionButtonTransform.gameObject.SetActive(active);
    }

    void SetButtonsInteractable(bool interactable)
    {
        SetInteractable(attackButtonTransform, interactable);
        SetInteractable(heavyAttackButtonTransform, interactable);
        SetInteractable(tauntButtonTransform, interactable);
        SetInteractable(defendButtonTransform, interactable);
        SetInteractable(moveLeftButtonTransform, interactable);
        SetInteractable(moveRightButtonTransform, interactable);
        SetInteractable(restButtonTransform, interactable);
        SetInteractable(usePotionButtonTransform, interactable);
    }

    void SetInteractable(RectTransform rect, bool val)
    {
        if (rect == null) return;
        Button btn = rect.GetComponent<Button>();
        if (btn != null) btn.interactable = val;
    }

    public void RestartGame()
    {
        if (GameManager.instance != null)
            Destroy(GameManager.instance.gameObject);
        SceneManager.LoadScene("StatScene");
    }

    public void UpdateUI()
    {
        if (playerHealthText != null)
            playerHealthText.text = "HP: " + health + "/" + maxHealth;

        if (playerEnergyText != null)
            playerEnergyText.text = "EN: " + energy + "/" + maxEnergy;

        if (usePotionButtonTransform != null)
        {
            TextMeshProUGUI t = usePotionButtonTransform.GetComponentInChildren<TextMeshProUGUI>();
            if (t != null) t.text = "POTION x" + healthPotions;
        }

        if (distanceText != null && enemyGladiator != null)
        {
            float dist = Vector3.Distance(transform.position, enemyGladiator.transform.position);
            distanceText.text = "DISTANCE: " + dist.ToString("F1");
        }
    }
}