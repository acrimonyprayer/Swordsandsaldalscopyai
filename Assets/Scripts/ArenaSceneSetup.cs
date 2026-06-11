using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ArenaSceneSetup : MonoBehaviour
{
    void Awake()
    {
        Camera cam = SetupCamera();
        EnsureEventSystem();
        CreateArena(cam);
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
        cam.backgroundColor = new Color(0.12f, 0.08f, 0.06f);
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

    void CreateArena(Camera cam)
    {
        UIHelper.CreateArenaBackground(cam);

        // Player
        GameObject playerGo = new GameObject("Player");
        SpriteRenderer playerSr = playerGo.AddComponent<SpriteRenderer>();
        Sprite playerSprite = UIHelper.LoadSprite("pngtree-gladiator-cartoon-fighting-png-image_6671245");
        playerSr.sprite = playerSprite != null ? playerSprite : UIHelper.CreateSprite(new Color(0.3f, 0.5f, 0.9f));
        playerSr.sortingOrder = 10;
        PlayerGladiator player = playerGo.AddComponent<PlayerGladiator>();

        // Enemy
        GameObject enemyGo = new GameObject("Enemy");
        SpriteRenderer enemySr = enemyGo.AddComponent<SpriteRenderer>();
        Sprite enemySprite = UIHelper.LoadSprite("gladiator-clipart-xl");
        enemySr.sprite = enemySprite != null ? enemySprite : UIHelper.CreateSprite(new Color(0.9f, 0.25f, 0.2f));
        enemySr.sortingOrder = 10;
        EnemyGladiator enemy = enemyGo.AddComponent<EnemyGladiator>();

        player.enemyGladiator = enemy;
        enemy.playerGladiator = player;

        Canvas canvas = UIHelper.CreateCanvas();
        BuildCombatUI(canvas.transform, player, enemy);
    }

    void BuildCombatUI(Transform ct, PlayerGladiator player, EnemyGladiator enemy)
    {
        // Top bar - texts with dark backgrounds
        TextMeshProUGUI waveText = UIHelper.CreateTextWithBG(ct, "WaveText", "",
            new Vector2(0, 500), 36, new Color(1f, 0.85f, 0.2f),
            TextAlignmentOptions.Center, 500, 50, 0.92f);

        TextMeshProUGUI playerHp = UIHelper.CreateTextWithBG(ct, "PlayerHP", "",
            new Vector2(-700, 450), 28, new Color(0.2f, 0.9f, 0.2f),
            TextAlignmentOptions.Left, 380, 40, 0.9f);

        TextMeshProUGUI playerEn = UIHelper.CreateTextWithBG(ct, "PlayerEnergy", "",
            new Vector2(-700, 405), 24, new Color(0.2f, 0.9f, 0.9f),
            TextAlignmentOptions.Left, 380, 40, 0.9f);

        TextMeshProUGUI enemyName = UIHelper.CreateTextWithBG(ct, "EnemyName", "",
            new Vector2(700, 450), 28, Color.white,
            TextAlignmentOptions.Right, 420, 40, 0.9f);

        TextMeshProUGUI enemyHp = UIHelper.CreateTextWithBG(ct, "EnemyHP", "",
            new Vector2(700, 405), 24, new Color(1f, 0.3f, 0.3f),
            TextAlignmentOptions.Right, 420, 40, 0.9f);

        TextMeshProUGUI distText = UIHelper.CreateTextWithBG(ct, "Distance", "",
            new Vector2(0, 440), 22, new Color(0.9f, 0.9f, 0.9f),
            TextAlignmentOptions.Center, 300, 40, 0.9f);

        // Action log - moved above gladiators
        TextMeshProUGUI actionLog = UIHelper.CreateTextWithBG(ct, "ActionLog", "",
            new Vector2(0, 340), 28, new Color(1f, 0.9f, 0.3f),
            TextAlignmentOptions.Center, 900, 55, 0.92f);

        // Game over
        TextMeshProUGUI gameOver = UIHelper.CreateTextWithBG(ct, "GameOver", "",
            new Vector2(0, 50), 58, Color.red,
            TextAlignmentOptions.Center, 800, 80, 0.92f);

        // Button area background
        GameObject btnBgGo = new GameObject("ButtonAreaBG");
        btnBgGo.transform.SetParent(ct, false);
        RectTransform btnBgRect = btnBgGo.AddComponent<RectTransform>();
        btnBgRect.anchoredPosition = new Vector2(0, -375);
        btnBgRect.sizeDelta = new Vector2(1920, 220);
        Image btnBgImg = btnBgGo.AddComponent<Image>();
        btnBgImg.color = new Color(0, 0, 0, 0.88f);
        btnBgImg.raycastTarget = false;

        // Action buttons - Row 1 (moved lower)
        Color red = new Color(0.75f, 0.12f, 0.12f);
        Color darkRed = new Color(0.55f, 0.08f, 0.08f);
        Color blue = new Color(0.15f, 0.35f, 0.75f);
        Color purple = new Color(0.55f, 0.15f, 0.75f);
        Color green = new Color(0.15f, 0.6f, 0.25f);
        Color teal = new Color(0.15f, 0.5f, 0.55f);
        Color yellow = new Color(0.95f, 0.85f, 0.15f);
        Color orange = new Color(0.95f, 0.5f, 0.05f);

        RectTransform atkBtn = UIHelper.CreateButton(ct, "AttackBtn", "ATTACK",
            new Vector2(-250, -340), new Vector2(155, 62), red);
        RectTransform hvyBtn = UIHelper.CreateButton(ct, "HeavyBtn", "HEAVY ATK",
            new Vector2(-85, -340), new Vector2(155, 62), darkRed);
        RectTransform tntBtn = UIHelper.CreateButton(ct, "TauntBtn", "TAUNT",
            new Vector2(85, -340), new Vector2(155, 62), purple);
        RectTransform defBtn = UIHelper.CreateButton(ct, "DefendBtn", "DEFEND",
            new Vector2(250, -340), new Vector2(155, 62), blue);

        // Row 2
        RectTransform mlBtn = UIHelper.CreateButton(ct, "MoveLeftBtn", "< MOVE",
            new Vector2(-250, -415), new Vector2(155, 62), teal);
        RectTransform rstBtn = UIHelper.CreateButton(ct, "RestBtn", "REST",
            new Vector2(-85, -415), new Vector2(155, 62), green);
        RectTransform potBtn = UIHelper.CreateButton(ct, "PotionBtn", "POTION",
            new Vector2(85, -415), new Vector2(155, 62), green);
        RectTransform mrBtn = UIHelper.CreateButton(ct, "MoveRightBtn", "MOVE >",
            new Vector2(250, -415), new Vector2(155, 62), teal);

        // End buttons
        RectTransform restartBtn = UIHelper.CreateButton(ct, "RestartBtn", "RESTART",
            new Vector2(-130, -200), new Vector2(220, 65), yellow, 24);
        RectTransform nextBtn = UIHelper.CreateButton(ct, "NextWaveBtn", "NEXT WAVE",
            new Vector2(130, -200), new Vector2(250, 65), orange, 24);

        // Wire player
        player.playerHealthText = playerHp;
        player.playerEnergyText = playerEn;
        player.actionLogText = actionLog;
        player.gameOverText = gameOver;
        player.waveText = waveText;
        player.distanceText = distText;
        player.attackButtonTransform = atkBtn;
        player.heavyAttackButtonTransform = hvyBtn;
        player.tauntButtonTransform = tntBtn;
        player.defendButtonTransform = defBtn;
        player.moveLeftButtonTransform = mlBtn;
        player.moveRightButtonTransform = mrBtn;
        player.restButtonTransform = rstBtn;
        player.usePotionButtonTransform = potBtn;
        player.restartButtonTransform = restartBtn;
        player.nextWaveButtonTransform = nextBtn;

        // Wire enemy
        enemy.enemyHealthText = enemyHp;
        enemy.enemyNameText = enemyName;
    }
}
