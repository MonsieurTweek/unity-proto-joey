using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    private INIParser m_parser;

    public HUDManager HUDManager;

    public Text debugLogs;

    public List<SkinData> skins;
    private SkinData m_currentSkin;
    public SkinData CurrentSkin
    {
        get { return m_currentSkin; }
        set
        {
            m_currentSkin = value;
            player.PlayerSkinController = GameObject.Instantiate(m_currentSkin.Model, player.transform);
        }
    }

    private int m_score;
    public int Score
    {
        get { return m_score; }
        set
        {
            if(player.isAlive == true)
            {
                m_score = value;
                HUDManager.UpdateScore(m_score);
            }
        }
    }

    private int m_currency;
    public int Currency
    {
        get { return m_currency; }
        set
        {
            if (player.isAlive == true)
            {
                m_currency = value;
                HUDManager.UpdateCurrency(m_currency);
                SaveNewCurrency();
            }
        }
    }


    private int m_scoreMultiplier;
    public int ScoreMultiplier
    {
        get { return m_scoreMultiplier; }
        set
        {
            if (player.isAlive == true)
            {
                m_scoreMultiplier = value;
                HUDManager.UpdateScoreMultiplier(m_scoreMultiplier);
            }
        }
    }

    public Player player;
    public EnemyManager enemyManager;
    public BonusManager bonusManager;
    public BossManager bossManager;
    public CurrencyManager currencyManager;
    public EdgeColliderManager edgeColliderManager;

    private float m_delayBeforeReload = 2f;
    private float m_timeLeftBeforeReload;

    private bool m_debugMode = false;

    public enum GameState
    {
        Ready,
        Pause,
        Play,
        GameOver
    }
    public GameState State { get => m_state; set => m_state = value; }

    private GameState m_state;

    private void Awake()
    {
        m_parser = new INIParser();
        State = GameState.Ready;
        m_timeLeftBeforeReload = m_delayBeforeReload;

        m_parser.Open(Application.persistentDataPath + "/settings.ini");
        int currentSkinIndex = InitSetting("Player", "SkinSelected", 0);
        m_parser.Close();

        if (currentSkinIndex < 0 || currentSkinIndex >= skins.Count)
        {
            currentSkinIndex = 0;
        }
        CurrentSkin = skins[currentSkinIndex];
    }

    // Start is called before the first frame update
    private void Start()
    {
        Score = 0;
        ScoreMultiplier = 1;
        enemyManager.GameManager = this;
        bonusManager.GameManager = this;
        bossManager.GameManager = this;
        player.gameManager = this;
        currencyManager.GameManager = this;
        edgeColliderManager.gameManager = this;

        InitGameSettings();

        // Init High Score
        HUDManager.SetHighScore(m_parser.ReadValue("Score", "HighScore", 0));

        // Init Currency
        Currency = m_parser.ReadValue("Score", "Currency", 0);

    }

    public void StartGame()
    {
        State = GameState.Play;
        StartSpawners();
        edgeColliderManager.CanMove = true;
    }

    public void GameOver()
    {

        CustomDebug("Game over ! Score : " + Score);

        m_parser.Open(Application.persistentDataPath + "/settings.ini");

        // Save high score
        int previousHighScore = m_parser.ReadValue("Score", "HighScore", 0);
        if(previousHighScore < Score)
        {
            m_parser.WriteValue("Score", "HighScore", Score);
        }

        // Close the file
        m_parser.Close();

        State = GameState.GameOver;
    }

    public void PauseSpawners()
    {
        CustomDebug("PauseSpawners");
        enemyManager.SetSpawnInactive();
        bonusManager.SetSpawnInactive();
        bossManager.SetSpawnInactive();
    }

    public void StartSpawners()
    {
        CustomDebug("StartSpawners");
        enemyManager.SetSpawnActive();
        bonusManager.SetSpawnActive();
        bossManager.SetSpawnActive();
    }

    private void Update()
    {

        switch(State)
        {
            case GameState.Ready:
                break;

            case GameState.GameOver:
                m_timeLeftBeforeReload -= Time.deltaTime;
                if (m_timeLeftBeforeReload <= 0f)
                {
                    Scene scene = SceneManager.GetActiveScene();
                    SceneManager.LoadScene(scene.name);
                }
                break;
        }

    }

    private void SetDebugMode(bool status)
    {
        m_debugMode = status;
        debugLogs.enabled = m_debugMode;
    }

    private void InitGameSettings()
    {

        // Get information from INI
        m_parser.Open(Application.persistentDataPath + "/settings.ini");

        // ----- Game Manager -----
        // Debug mode
        bool debugMode = InitSetting("GameManager", "DebugMode", 0) == 1;
        SetDebugMode(debugMode);

        CustomDebug(Application.persistentDataPath + "/settings.ini");

        // ----- Player -----
        // God mode
        bool godMode = InitSetting("Player", "GodMode", 0) == 1;
        player.CanDie = godMode == false;
        // Skin selected

        // MinSize
        player.minSize = InitSetting("Player", "MinSize", player.minSize);
        // StepSize
        player.stepSize = InitSetting("Player", "StepSize", player.stepSize);
        // MoveForce
        player.moveForce = InitSetting("Player", "MoveForce", player.moveForce);
        // Mass
        player.mass = InitSetting("Player", "Mass", player.mass);
        // LinearDrag
        player.linearDrag = InitSetting("Player", "LinearDrag", player.linearDrag);
        // GravityScale
        player.gravityScale = InitSetting("Player", "GravityScale", player.gravityScale);
        // Distance Threshold
        player.SetDistanceThreshold(InitSetting("Player", "DistanceThreshold", 0.5f));
        // Update player rigidbody settings
        player.UpdateRigidbodySettings();

        // ----- Enemy Manager -----
        // MinDelay
        enemyManager.minDelay = InitSetting("EnemyManager", "MinDelay", enemyManager.minDelay);
        // MaxDelay
        enemyManager.maxDelay = InitSetting("EnemyManager", "MaxDelay", enemyManager.maxDelay);
        // MinSpeed
        enemyManager.minSpeed = InitSetting("Enemy", "MinSpeed", enemyManager.minSpeed);
        // MaxSpeed
        enemyManager.maxSpeed = InitSetting("Enemy", "MaxSpeed", enemyManager.maxSpeed);

        // ----- Edge Collider Manager -----
        // Speed
        edgeColliderManager.speed = InitSetting("EdgeColliderManager", "Speed", edgeColliderManager.speed);
        // MaxTop
        edgeColliderManager.maxTop = InitSetting("EdgeColliderManager", "MaxTop", edgeColliderManager.maxTop);

        // ----- Bonus Manager -----
        // ScoreTrigger
        bonusManager.SetScoreTrigger(InitSetting("BonusManager", "Trigger", bonusManager.Trigger));
        // PositionTrigger
        float defaultPositionTrigger = Mathf.Round((edgeColliderManager.maxTop / 4f) * 100f) / 100f;
        bonusManager.PositionTrigger = InitSetting("BonusManager", "PositionTrigger", defaultPositionTrigger);
        // MinSpeed
        bonusManager.minSpeed = InitSetting("Bonus", "MinSpeed", bonusManager.minSpeed);
        // MaxSpeed
        bonusManager.maxSpeed = InitSetting("Bonus", "MaxSpeed", bonusManager.maxSpeed);

        // ----- Boss Manager -----
        // ScoreTrigger
        bossManager.SetScoreTrigger(InitSetting("BossManager", "ScoreTrigger", bossManager.scoreTrigger));

        // ----- Currency Manager -----
        // Trigger
        currencyManager.Trigger = InitSetting("CurrencyManager", "Trigger", currencyManager.Trigger);

        // Close the file
        m_parser.Close();

    }

    public void CustomDebug(string log)
    {
        CustomDebugUpdateText(log.ToString());
    }

    public void CustomDebug(int log)
    {
        CustomDebugUpdateText(log.ToString());
    }

    public void CustomDebug(float log)
    {
        CustomDebugUpdateText(log.ToString());
    }

    private void CustomDebugUpdateText(string log)
    {
        if(m_debugMode == true)
        {
            Debug.Log(log);
            debugLogs.text += "\n" + log;
        }
    }

    private float InitSetting(string section, string key, float defaultValue)
    {

        if (m_parser.IsKeyExists(section, key) == false)
        {
            m_parser.WriteValue(section, key, defaultValue);
        }

        return (float) m_parser.ReadValue(section, key, defaultValue);
    }

    private int InitSetting(string section, string key, int defaultValue)
    {

        if (m_parser.IsKeyExists(section, key) == false)
        {
            m_parser.WriteValue(section, key, defaultValue);
        }

        return (int)m_parser.ReadValue(section, key, defaultValue);
    }

    private void SaveNewCurrency()
    {
        m_parser.Open(Application.persistentDataPath + "/settings.ini");
        m_parser.WriteValue("Score", "Currency", m_currency);
        m_parser.Close();
    }
}
