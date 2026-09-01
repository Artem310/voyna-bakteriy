using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }
    
    [Header("Configuration")]
    [SerializeField] private GameConfig config;
    
    [Header("References")]
    [SerializeField] private VictoryOverlay victoryOverlay;
    [SerializeField] private DefeatOverlay defeatOverlay;
    [SerializeField] private GameObject tentaclePrefab;
    [SerializeField] private GameObject colonyPrefab;
    
    [Header("Colony Sprites")]
    [SerializeField] private Sprite playerSprite;
    [SerializeField] private Sprite enemySprite;
    [SerializeField] private Sprite neutralSprite;
    
    [Header("Colonies")]
    [SerializeField] private List<Colony> allColonies = new List<Colony>();
    
    [Header("Tutorial")]
    [SerializeField] private TutorialManager tutorialManager;
    
    private bool gameEnded = false;
    
    public GameConfig Config => config;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    private void Start()
    {
        LoadCurrentLevel();
        InitializeEnemyAI();
        InitializeAudioService();
    }
    
    public void LoadCurrentLevel()
    {
        LevelDefinition levelDef = config.GetCurrentLevelDefinition();
        if (levelDef == null)
            return;
        
        config.aiActionInterval = levelDef.aiActionInterval;
        
        ClearColonies();
        SpawnColoniesFromDefinition(levelDef);
    }
    
    private void ClearColonies()
    {
        Colony[] existingColonies = FindObjectsOfType<Colony>();
        foreach (var colony in existingColonies)
        {
            if (colony != null)
                Destroy(colony.gameObject);
        }
        allColonies.Clear();
    }
    
    private void SpawnColoniesFromDefinition(LevelDefinition levelDef)
    {
        if (levelDef.colonies == null || colonyPrefab == null)
            return;
        
        foreach (var spawnData in levelDef.colonies)
        {
            Vector3 worldPos = new Vector3(spawnData.position.x, spawnData.position.y, 0f);
            GameObject colonyObj = Instantiate(colonyPrefab, worldPos, Quaternion.identity);
            Colony colony = colonyObj.GetComponent<Colony>();
            
            if (colony != null)
            {
                SetColonySprites(colony);
                colony.Initialize(spawnData.owner, spawnData.mass);
                allColonies.Add(colony);
            }
        }
    }
    
    private void SetColonySprites(Colony colony)
    {
        if (colony != null)
        {
            colony.SetSprites(playerSprite, enemySprite, neutralSprite);
        }
    }
    
    private void InitializeEnemyAI()
    {
        if (config == null || config.currentLevel < config.aiEnabledFromLevel)
            return;
        
        if (FindObjectOfType<EnemyAI>() == null)
        {
            gameObject.AddComponent<EnemyAI>();
        }
    }
    
    private void InitializeAudioService()
    {
        if (AudioService.Instance == null)
        {
            GameObject audioServiceObj = new GameObject("AudioService");
            audioServiceObj.AddComponent<AudioService>();
        }
    }
    
    public void CreateTentacle(Colony source, Colony target, float unitPercentage)
    {
        if (source == null || target == null || gameEnded)
            return;
        
        if (source.Owner == ColonyOwner.Neutral)
            return;
        
        if (!source.CanLaunchTentacle())
            return;
        
        float unitsToSend = source.Units * unitPercentage;
        if (unitsToSend < config.tentacleMinMass)
            return;
        
        if (source.TryRemoveUnits(unitsToSend))
        {
            if (tutorialManager != null && source.Owner == ColonyOwner.Player)
            {
                tutorialManager.OnTentacleLaunchRequested();
            }
            
            if (AudioService.Instance != null)
            {
                AudioService.Instance.PlayTentacleLaunch();
            }
            
            GameObject tentacleObj = Instantiate(tentaclePrefab, source.Position, Quaternion.identity);
            Tentacle tentacle = tentacleObj.GetComponent<Tentacle>();
            
            if (tentacle != null)
            {
                tentacle.Initialize(source, target, unitsToSend, config);
            }
        }
    }
    
    public void CheckVictoryConditions()
    {
        if (gameEnded)
            return;
        
        bool hasPlayerColony = allColonies.Any(c => c.Owner == ColonyOwner.Player);
        bool hasEnemyColony = allColonies.Any(c => c.Owner == ColonyOwner.Enemy);
        
        if (!hasEnemyColony && hasPlayerColony)
        {
            Victory();
        }
        else if (!hasPlayerColony && hasEnemyColony)
        {
            Defeat();
        }
    }
    
    private void Victory()
    {
        gameEnded = true;
        
        SaveLevelProgress();
        
        if (AudioService.Instance != null)
        {
            AudioService.Instance.PlayVictory();
        }
        
        if (victoryOverlay != null)
        {
            victoryOverlay.Show(config.currentLevel);
        }
    }
    
    private void SaveLevelProgress()
    {
        int completedLevel = PlayerPrefs.GetInt("CompletedLevel", 0);
        if (config.currentLevel > completedLevel)
        {
            PlayerPrefs.SetInt("CompletedLevel", config.currentLevel);
            PlayerPrefs.Save();
        }
    }
    
    public void LoadNextLevel()
    {
        if (config.currentLevel < 6)
        {
            config.currentLevel++;
            gameEnded = false;
            
            ClearColonies();
            LoadCurrentLevel();
            
            if (victoryOverlay != null)
            {
                victoryOverlay.Hide();
            }
            
            DestroyEnemyAI();
            InitializeEnemyAI();
        }
    }
    
    private void DestroyEnemyAI()
    {
        EnemyAI existingAI = FindObjectOfType<EnemyAI>();
        if (existingAI != null)
        {
            Destroy(existingAI);
        }
    }
    
    private void Defeat()
    {
        gameEnded = true;
        
        if (AudioService.Instance != null)
        {
            AudioService.Instance.PlayDefeat();
        }
        
        if (defeatOverlay != null)
        {
            defeatOverlay.Show();
        }
    }
    
    public Colony GetColonyAtPosition(Vector3 worldPosition)
    {
        foreach (var colony in allColonies)
        {
            float distance = Vector3.Distance(colony.Position, worldPosition);
            if (distance < config.colonyRadius)
            {
                return colony;
            }
        }
        return null;
    }
    
    public List<Colony> GetAllColonies()
    {
        return new List<Colony>(allColonies);
    }
    
    public bool IsGameEnded()
    {
        return gameEnded;
    }
}
