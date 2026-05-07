using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerGladiator : MonoBehaviour
{
    // --- Oyun Mekanikleri ---
    [Header("Player Stats")]
    public int health = 100;
    public int energy = 50;
    public int attackDamage = 25;
    public bool isDefending = false;
    public bool isPlayerTurn = true;
    
    [Header("References")]
    public EnemyGladiator enemyGladiator;
    
    // --- UI Metinleri (TMP) ---
    [Header("UI Texts")]
    public TextMeshProUGUI playerHealthText;
    public TextMeshProUGUI playerEnergyText;
    public TextMeshProUGUI actionLogText;
    public TextMeshProUGUI gameOverText;
    
    // --- UI Butonları (RectTransform) ---
    [Header("UI Buttons")]
    public RectTransform attackButtonTransform;
    public RectTransform defendButtonTransform;
    public RectTransform leftButtonTransform;
    public RectTransform rightButtonTransform;
    public RectTransform restartButtonTransform;

    void Start()
    {
        // Oyuncuyu mavi bir kare yap ve konumlandır
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = Color.blue; // Tam Mavi
            sr.sortingOrder = 5; // Arka planın önünde olması için
        }

        // Kare boyutu ve başlangıç pozisyonu
        transform.localScale = new Vector3(2.5f, 2.5f, 1f); 
        transform.position = new Vector3(-6f, 0f, 0f); 

        // UI elemanlarını kod üzerinden şık bir şekilde ayarla
        SetupUI();
        UpdateUI();
        LogAction("GAME STARTED! PLAYER TURN.");
    }

    // UI tasarımı
    void SetupUI()
    {
        // Renk Paleti
        Color greenColor = new Color(0.2f, 0.9f, 0.2f); // Parlak Yeşil
        Color cyanColor = new Color(0.2f, 0.9f, 0.9f); // Parlak Camgöbeği
        Color orangeColor = new Color(1f, 0.6f, 0.2f); // Turuncu

        // Oyuncu Can ve Enerji Metinleri (Ss'teki yerleri)
        if (playerHealthText != null)
        {
            playerHealthText.rectTransform.anchoredPosition = new Vector2(-400, 250);
            playerHealthText.color = greenColor;
            playerHealthText.fontSize = 35;
            playerHealthText.fontStyle = FontStyles.Bold;
        }
        if (playerEnergyText != null)
        {
            playerEnergyText.rectTransform.anchoredPosition = new Vector2(-400, 180); // Aradaki boşluk
            playerEnergyText.color = cyanColor;
            playerEnergyText.fontSize = 35;
            playerEnergyText.fontStyle = FontStyles.Bold;
        }

        // Action Log (Ekranın ortasındaki metin)
        if (actionLogText != null)
        {
            actionLogText.rectTransform.anchoredPosition = new Vector2(0, 320);
            actionLogText.color = orangeColor;
            actionLogText.fontSize = 30;
            actionLogText.alignment = TextAlignmentOptions.Center;
        }

        // Butonların Boyutu ve Pozisyonu (Bold yapıldı)
        Vector2 buttonSize = new Vector2(180, 60);

        ConfigureButton(attackButtonTransform, "ATTACK", new Vector2(-100, -150), new Color(0.8f, 0.2f, 0.2f));
        ConfigureButton(defendButtonTransform, "DEFEND", new Vector2(100, -150), new Color(0.2f, 0.4f, 0.8f));
        ConfigureButton(leftButtonTransform, "MOVE LEFT", new Vector2(-100, -250), new Color(0.2f, 0.4f, 0.8f));
        ConfigureButton(rightButtonTransform, "MOVE RIGHT", new Vector2(100, -250), new Color(0.2f, 0.4f, 0.8f));

        // Oyun Bitti Ekranı (Gizli Başla)
        if (gameOverText != null)
        {
            gameOverText.rectTransform.anchoredPosition = new Vector2(0, 70);
            gameOverText.fontSize = 70;
            gameOverText.fontStyle = FontStyles.Bold;
            gameOverText.gameObject.SetActive(false); 
        }
        if (restartButtonTransform != null)
        {
            restartButtonTransform.anchoredPosition = new Vector2(0, -80);
            restartButtonTransform.sizeDelta = new Vector2(200, 60);
            restartButtonTransform.GetComponent<Image>().color = Color.yellow;
            restartButtonTransform.gameObject.SetActive(false); 
        }
    }

    void ConfigureButton(RectTransform rect, string label, Vector2 pos, Color col)
    {
        if (rect == null) return;
        rect.anchoredPosition = pos;
        rect.sizeDelta = new Vector2(180, 60);
        rect.GetComponent<Image>().color = col;
        TextMeshProUGUI t = rect.GetComponentInChildren<TextMeshProUGUI>();
        if (t != null)
        {
            t.text = label;
            t.fontSize = 22;
            t.fontStyle = FontStyles.Bold; // KALIN
            t.color = Color.white;
        }
    }

    // --- Oyuncu Hamleleri ---

    public void PlayerAttack()
    {
        if (!isPlayerTurn || health <= 0 || enemyGladiator.health <= 0) return;

        // Mesafe Kontrolü: Yakın Değilse Vuramaz
        float distance = Vector3.Distance(transform.position, enemyGladiator.transform.position);
        
        if (distance > 3.5f)
        {
            LogAction("ENEMY TOO FAR!");
            return;
        }

        if (energy >= 15)
        {
            energy -= 15;
            isDefending = false; 
            LogAction("PLAYER ATTACKED!");
            enemyGladiator.TakeDamage(attackDamage);
            EndTurn();
        }
        else
        {
            LogAction("NOT ENOUGH ENERGY!");
        }
    }

    public void PlayerDefend()
    {
        if (!isPlayerTurn || health <= 0 || enemyGladiator.health <= 0) return;

        isDefending = true;
        energy += 25; // Savunurken enerji yenilenir
        if (energy > 100) energy = 100;
        LogAction("PLAYER IS DEFENDING.");
        EndTurn();
    }

    public void MoveLeft()
    {
        if (!isPlayerTurn || health <= 0 || enemyGladiator.health <= 0) return;
        
        transform.position += new Vector3(-2, 0, 0);
        isDefending = false;
        LogAction("PLAYER MOVED LEFT.");
        EndTurn();
    }

    public void MoveRight()
    {
        if (!isPlayerTurn || health <= 0 || enemyGladiator.health <= 0) return;

        // Düşmanın İçine Girmeyi Engelle
        float distance = Vector3.Distance(transform.position + new Vector3(2, 0, 0), enemyGladiator.transform.position);
        if (distance < 1.5f)
        {
            LogAction("PATH BLOCKED!");
            return;
        }

        transform.position += new Vector3(2, 0, 0);
        isDefending = false;
        LogAction("PLAYER MOVED RIGHT.");
        EndTurn();
    }

    // --- Sıra Yönetimi ---

    void EndTurn()
    {
        isPlayerTurn = false;
        UpdateUI();
        // 1.2 saniye sonra düşmanın sırasını başlat
        Invoke("StartEnemyTurn", 1.2f); 
    }

    void StartEnemyTurn()
    {
        if (enemyGladiator != null && enemyGladiator.health > 0)
        {
            enemyGladiator.MakeMove();
        }
    }

    // --- Hasar Alınca ---

    public void TakeDamage(int damage)
    {
        // Savunma Durumu: Hasarı Yarıya İndirir
        if (isDefending)
        {
            damage = damage / 2; 
            LogAction("PLAYER DEFENDED!");
        }

        health -= damage;
        if (health <= 0)
        {
            health = 0;
            ShowGameOver("YOU LOST!", Color.red);
        }
        UpdateUI();
    }

    // --- UI Güncelleme ve Loglama ---

    public void LogAction(string message)
    {
        if (actionLogText != null)
        {
            actionLogText.text = message;
        }
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
        {
            restartButtonTransform.gameObject.SetActive(true);
        }

        // Oyun bitince butonları gizle
        SetButtonsActive(false);
    }

    void SetButtonsActive(bool active)
    {
        attackButtonTransform.gameObject.SetActive(active);
        defendButtonTransform.gameObject.SetActive(active);
        leftButtonTransform.gameObject.SetActive(active);
        rightButtonTransform.gameObject.SetActive(active);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void UpdateUI()
    {
        if (playerHealthText != null) playerHealthText.text = "PLAYER HP: " + health;
        if (playerEnergyText != null) playerEnergyText.text = "ENERGY: " + energy;
    }
}