using UnityEngine;
using TMPro;

public class EnemyGladiator : MonoBehaviour
{
    // --- Düşman Mekanikleri ---
    [Header("Enemy Stats")]
    public int health = 100;
    public int attackDamage = 20;
    public bool isDefending = false;
    
    [Header("References")]
    public PlayerGladiator playerGladiator;
    
    // --- UI Metinleri (TMP) ---
    [Header("UI Texts")]
    public TextMeshProUGUI enemyHealthText;

    void Start()
    {
        // Düşmanı kırmızı bir kare yap
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = Color.red; // Tam Kırmızı
            sr.sortingOrder = 5;
        }

        // Kare boyutu ve başlangıç pozisyonu
        transform.localScale = new Vector3(2.5f, 2.5f, 1f); 
        transform.position = new Vector3(6f, 0f, 0f); 

        // UI elemanlarını kod üzerinden şık bir şekilde ayarla
        SetupUI();
        UpdateUI();
    }

    // UI tasarımı
    void SetupUI()
    {
        // Düşman Can Metni (Ekranın sağ üst köşesi)
        if (enemyHealthText != null)
        {
            enemyHealthText.rectTransform.anchoredPosition = new Vector2(400, 250);
            enemyHealthText.color = new Color(1f, 0.3f, 0.3f); // Parlak Kırmızı
            enemyHealthText.fontSize = 35;
            enemyHealthText.fontStyle = FontStyles.Bold;
            enemyHealthText.alignment = TextAlignmentOptions.Right;
        }
    }

    // --- Düşman Yapay Zeka Hamlesi ---

    public void MakeMove()
    {
        if (health <= 0 || playerGladiator.health <= 0) return;

        // Oyuncu ile Mesafe Kontrolü
        float distance = Vector3.Distance(transform.position, playerGladiator.transform.position);

        if (distance > 3.5f)
        {
            // Uzaktaysa: Yaklaş
            transform.position += new Vector3(-2, 0, 0);
            isDefending = false;
            playerGladiator.LogAction("ENEMY MOVED FORWARD.");
        }
        else
        {
            // Yakındaysa: Rastgele Hamle (Saldır veya Savun)
            int choice = Random.Range(0, 10);
            if (health < 40 && choice < 5) // Canı azsa %50 ihtimalle iyileşir
            {
                isDefending = true;
                health += 15;
                if (health > 100) health = 100;
                playerGladiator.LogAction("ENEMY DEFENDING.");
            }
            else
            {
                // Saldır
                isDefending = false;
                playerGladiator.LogAction("ENEMY ATTACKED!");
                playerGladiator.TakeDamage(attackDamage);
            }
        }

        UpdateUI();
        // 1 saniye sonra düşmanın sırasını bitir ve oyuncuya devret
        Invoke("EndEnemyTurn", 1f);
    }

    void EndEnemyTurn()
    {
        playerGladiator.isPlayerTurn = true;
        playerGladiator.LogAction("PLAYER TURN.");
    }

    // --- Hasar Alınca ---

    public void TakeDamage(int damage)
    {
        // Savunma Durumu: Hasarı Yarıya İndirir
        if (isDefending)
        {
            damage = damage / 2;
        }

        health -= damage;
        
        if (health <= 0)
        {
            health = 0;
            UpdateUI();
            playerGladiator.LogAction("ENEMY DEFEATED!");
            playerGladiator.ShowGameOver("YOU WIN!", Color.green);
            return;
        }

        UpdateUI();
    }

    public void UpdateUI()
    {
        if (enemyHealthText != null)
        {
            enemyHealthText.text = "ENEMY HP: " + health;
        }
    }
}