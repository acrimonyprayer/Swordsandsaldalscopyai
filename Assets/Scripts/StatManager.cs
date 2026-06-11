using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class StatManager : MonoBehaviour
{
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI vitalityText;
    public TextMeshProUGUI strengthText;
    public TextMeshProUGUI enduranceText;
    public TextMeshProUGUI agilityText;
    public TextMeshProUGUI dexterityText;
    public TextMeshProUGUI warningText;
    public TextMeshProUGUI waveTitle;

    public Button btnVitality;
    public Button btnStrength;
    public Button btnEndurance;
    public Button btnAgility;
    public Button btnDexterity;
    public Button btnNext;

    private Coroutine warningCoroutine;

    void Start()
    {
        if (warningText != null)
            warningText.gameObject.SetActive(false);

        if (btnVitality != null) btnVitality.onClick.AddListener(IncreaseVitality);
        if (btnStrength != null) btnStrength.onClick.AddListener(IncreaseStrength);
        if (btnEndurance != null) btnEndurance.onClick.AddListener(IncreaseEndurance);
        if (btnAgility != null) btnAgility.onClick.AddListener(IncreaseAgility);
        if (btnDexterity != null) btnDexterity.onClick.AddListener(IncreaseDexterity);
        if (btnNext != null) btnNext.onClick.AddListener(GoToNextScene);

        if (waveTitle != null && GameManager.instance != null)
        {
            if (GameManager.instance.currentWave == 1)
                waveTitle.text = "CREATE YOUR GLADIATOR";
            else
                waveTitle.text = "WAVE " + GameManager.instance.currentWave + " - LEVEL UP!";
        }

        UpdateUI();
    }

    public void IncreaseVitality()
    {
        if (GameManager.instance != null && GameManager.instance.statPoints > 0)
        {
            GameManager.instance.vitality++;
            GameManager.instance.statPoints--;
            UpdateUI();
        }
    }

    public void IncreaseStrength()
    {
        if (GameManager.instance != null && GameManager.instance.statPoints > 0)
        {
            GameManager.instance.strength++;
            GameManager.instance.statPoints--;
            UpdateUI();
        }
    }

    public void IncreaseEndurance()
    {
        if (GameManager.instance != null && GameManager.instance.statPoints > 0)
        {
            GameManager.instance.endurance++;
            GameManager.instance.statPoints--;
            UpdateUI();
        }
    }

    public void IncreaseAgility()
    {
        if (GameManager.instance != null && GameManager.instance.statPoints > 0)
        {
            GameManager.instance.agility++;
            GameManager.instance.statPoints--;
            UpdateUI();
        }
    }

    public void IncreaseDexterity()
    {
        if (GameManager.instance != null && GameManager.instance.statPoints > 0)
        {
            GameManager.instance.dexterity++;
            GameManager.instance.statPoints--;
            UpdateUI();
        }
    }

    public void GoToNextScene()
    {
        if (GameManager.instance != null && GameManager.instance.statPoints == 0)
        {
            if (GameManager.instance.currentWave == 1)
                SceneManager.LoadScene("SampleScene");
            else
                SceneManager.LoadScene("ShopScene");
        }
        else
        {
            if (warningCoroutine != null)
                StopCoroutine(warningCoroutine);
            if (warningText != null)
                warningCoroutine = StartCoroutine(ShowWarning());
        }
    }

    IEnumerator ShowWarning()
    {
        warningText.text = "SPEND ALL POINTS FIRST!";
        warningText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        warningText.gameObject.SetActive(false);
    }

    void UpdateUI()
    {
        if (GameManager.instance == null) return;

        var gm = GameManager.instance;
        if (pointsText != null) pointsText.text = "STAT POINTS: " + gm.statPoints;
        if (vitalityText != null) vitalityText.text = "VITALITY: " + gm.vitality + "  (HP: " + gm.GetMaxHealth() + ")";
        if (strengthText != null) strengthText.text = "STRENGTH: " + gm.strength + "  (ATK: " + gm.GetAttackDamage() + ")";
        if (enduranceText != null) enduranceText.text = "ENDURANCE: " + gm.endurance + "  (EN: " + gm.GetMaxEnergy() + ")";
        if (agilityText != null) agilityText.text = "AGILITY: " + gm.agility + "  (DODGE: " + gm.GetDodgeChance() + "%)";
        if (dexterityText != null) dexterityText.text = "DEXTERITY: " + gm.dexterity + "  (CRIT: " + gm.GetCritChance() + "%)";
    }
}
