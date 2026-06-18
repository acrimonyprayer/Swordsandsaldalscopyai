using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ArenaSceneSetup : MonoBehaviour
{
    void Awake()
    {
        CleanOldObjects();
        Camera mainCamera = SetupCamera();
        EnsureEventSystem();
        CreateArena(mainCamera);
    }

    void CleanOldObjects()
    {
        Canvas oldCanvas = FindAnyObjectByType<Canvas>();
        if (oldCanvas != null) Destroy(oldCanvas.gameObject);

        GameObject oldPlayer = GameObject.Find("Player");
        if (oldPlayer != null) Destroy(oldPlayer);

        GameObject oldEnemy = GameObject.Find("Enemy");
        if (oldEnemy != null) Destroy(oldEnemy);

        GameObject oldEventSystem = GameObject.Find("EventSystem");
        if (oldEventSystem != null) Destroy(oldEventSystem);
    }

    Camera SetupCamera()
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            GameObject camObject = new GameObject("Main Camera");
            camObject.tag = "MainCamera";
            cam = camObject.AddComponent<Camera>();
        }
        cam.orthographic = true;
        cam.orthographicSize = 5;
        cam.transform.position = new Vector3(0, 0, -10);
        cam.backgroundColor = new Color(0.12f, 0.08f, 0.06f);
        return cam;
    }

    void EnsureEventSystem()
    {
        if (FindAnyObjectByType<EventSystem>() == null)
        {
            GameObject eventSystemObject = new GameObject("EventSystem");
            eventSystemObject.AddComponent<EventSystem>();
            eventSystemObject.AddComponent<StandaloneInputModule>();
        }
    }

    void CreateArena(Camera cam)
    {
        UIHelper.CreateArenaBackground(cam);

        GameObject playerObject = new GameObject("Player");
        SpriteRenderer playerSpriteRenderer = playerObject.AddComponent<SpriteRenderer>();
        Sprite playerSprite = UIHelper.LoadSprite("pngtree-gladiator-cartoon-fighting-png-image_6671245");
        playerSpriteRenderer.sprite = playerSprite != null ? playerSprite : UIHelper.CreateSprite(new Color(0.3f, 0.5f, 0.9f));
        playerSpriteRenderer.sortingOrder = 10;
        PlayerGladiator player = playerObject.AddComponent<PlayerGladiator>();

        GameObject enemyObject = new GameObject("Enemy");
        SpriteRenderer enemySpriteRenderer = enemyObject.AddComponent<SpriteRenderer>();
        Sprite enemySprite = UIHelper.LoadSprite("gladiator-clipart-xl");
        enemySpriteRenderer.sprite = enemySprite != null ? enemySprite : UIHelper.CreateSprite(new Color(0.9f, 0.25f, 0.2f));
        enemySpriteRenderer.sortingOrder = 10;
        EnemyGladiator enemy = enemyObject.AddComponent<EnemyGladiator>();

        player.enemyGladiator = enemy;
        enemy.playerGladiator = player;

        Canvas uiCanvas = UIHelper.CreateCanvas();
        BuildCombatUI(uiCanvas.transform, player, enemy);
    }

    void BuildCombatUI(Transform canvasTransform, PlayerGladiator player, EnemyGladiator enemy)
    {
        TextMeshProUGUI waveText = UIHelper.CreateTextWithBG(canvasTransform, "WaveText", "",
            new Vector2(0, 500), 36, new Color(1f, 0.85f, 0.2f),
            TextAlignmentOptions.Center, 500, 50, 0.92f);

        TextMeshProUGUI playerHpText = UIHelper.CreateTextWithBG(canvasTransform, "PlayerHP", "",
            new Vector2(-700, 450), 28, new Color(0.2f, 0.9f, 0.2f),
            TextAlignmentOptions.Left, 380, 40, 0.9f);

        TextMeshProUGUI playerEnergyText = UIHelper.CreateTextWithBG(canvasTransform, "PlayerEnergy", "",
            new Vector2(-700, 405), 24, new Color(0.2f, 0.9f, 0.9f),
            TextAlignmentOptions.Left, 380, 40, 0.9f);

        TextMeshProUGUI enemyNameText = UIHelper.CreateTextWithBG(canvasTransform, "EnemyName", "",
            new Vector2(700, 450), 28, Color.white,
            TextAlignmentOptions.Right, 420, 40, 0.9f);

        TextMeshProUGUI enemyHpText = UIHelper.CreateTextWithBG(canvasTransform, "EnemyHP", "",
            new Vector2(700, 405), 24, new Color(1f, 0.3f, 0.3f),
            TextAlignmentOptions.Right, 420, 40, 0.9f);

        TextMeshProUGUI distanceText = UIHelper.CreateTextWithBG(canvasTransform, "Distance", "",
            new Vector2(0, 440), 22, new Color(0.9f, 0.9f, 0.9f),
            TextAlignmentOptions.Center, 300, 40, 0.9f);

        TextMeshProUGUI actionLogText = UIHelper.CreateTextWithBG(canvasTransform, "ActionLog", "",
            new Vector2(0, 340), 28, new Color(1f, 0.9f, 0.3f),
            TextAlignmentOptions.Center, 900, 55, 0.92f);

        TextMeshProUGUI gameOverText = UIHelper.CreateTextWithBG(canvasTransform, "GameOver", "",
            new Vector2(0, 160), 58, Color.red,
            TextAlignmentOptions.Center, 700, 100, 0.92f);

        GameObject buttonBgObject = new GameObject("ButtonAreaBG");
        buttonBgObject.transform.SetParent(canvasTransform, false);
        RectTransform buttonBgRect = buttonBgObject.AddComponent<RectTransform>();
        buttonBgRect.anchoredPosition = new Vector2(0, -380);
        buttonBgRect.sizeDelta = new Vector2(1920, 200);
        Image buttonBgImage = buttonBgObject.AddComponent<Image>();
        buttonBgImage.color = new Color(0, 0, 0, 0.88f);
        buttonBgImage.raycastTarget = false;

        Color redColor = new Color(0.75f, 0.12f, 0.12f);
        Color darkRedColor = new Color(0.55f, 0.08f, 0.08f);
        Color blueColor = new Color(0.15f, 0.35f, 0.75f);
        Color purpleColor = new Color(0.55f, 0.15f, 0.75f);
        Color greenColor = new Color(0.15f, 0.6f, 0.25f);
        Color tealColor = new Color(0.15f, 0.5f, 0.55f);
        Color yellowColor = new Color(0.95f, 0.85f, 0.15f);
        Color orangeColor = new Color(0.95f, 0.5f, 0.05f);

        RectTransform attackButton = UIHelper.CreateButton(canvasTransform, "AttackBtn", "ATTACK",
            new Vector2(-250, -335), new Vector2(155, 62), redColor);
        RectTransform heavyButton = UIHelper.CreateButton(canvasTransform, "HeavyBtn", "HEAVY ATK",
            new Vector2(-85, -335), new Vector2(155, 62), darkRedColor);
        RectTransform tauntButton = UIHelper.CreateButton(canvasTransform, "TauntBtn", "TAUNT",
            new Vector2(85, -335), new Vector2(155, 62), purpleColor);
        RectTransform defendButton = UIHelper.CreateButton(canvasTransform, "DefendBtn", "DEFEND",
            new Vector2(250, -335), new Vector2(155, 62), blueColor);

        RectTransform moveLeftButton = UIHelper.CreateButton(canvasTransform, "MoveLeftBtn", "< MOVE",
            new Vector2(-250, -415), new Vector2(155, 62), tealColor);
        RectTransform restButton = UIHelper.CreateButton(canvasTransform, "RestBtn", "REST",
            new Vector2(-85, -415), new Vector2(155, 62), greenColor);
        RectTransform potionButton = UIHelper.CreateButton(canvasTransform, "PotionBtn", "POTION",
            new Vector2(85, -415), new Vector2(155, 62), greenColor);
        RectTransform moveRightButton = UIHelper.CreateButton(canvasTransform, "MoveRightBtn", "MOVE >",
            new Vector2(250, -415), new Vector2(155, 62), tealColor);

        RectTransform restartButton = UIHelper.CreateButton(canvasTransform, "RestartBtn", "RESTART",
            new Vector2(-130, 160), new Vector2(220, 65), yellowColor, 24);
        RectTransform nextWaveButton = UIHelper.CreateButton(canvasTransform, "NextWaveBtn", "NEXT WAVE",
            new Vector2(130, 160), new Vector2(250, 65), orangeColor, 24);

        player.playerHealthText = playerHpText;
        player.playerEnergyText = playerEnergyText;
        player.actionLogText = actionLogText;
        player.gameOverText = gameOverText;
        player.waveText = waveText;
        player.distanceText = distanceText;
        player.attackButtonTransform = attackButton;
        player.heavyAttackButtonTransform = heavyButton;
        player.tauntButtonTransform = tauntButton;
        player.defendButtonTransform = defendButton;
        player.moveLeftButtonTransform = moveLeftButton;
        player.moveRightButtonTransform = moveRightButton;
        player.restButtonTransform = restButton;
        player.usePotionButtonTransform = potionButton;
        player.restartButtonTransform = restartButton;
        player.nextWaveButtonTransform = nextWaveButton;

        enemy.enemyHealthText = enemyHpText;
        enemy.enemyNameText = enemyNameText;
    }
}