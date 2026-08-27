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
    
    [Header("Colonies")]
    [SerializeField] private List<Colony> allColonies = new List<Colony>();
    
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
        FindAllColonies();
        InitializeColonies();
    }
    
    private void FindAllColonies()
    {
        allColonies.Clear();
        allColonies.AddRange(FindObjectsOfType<Colony>());
    }
    
    private void InitializeColonies()
    {
        if (allColonies.Count >= 2)
        {
            allColonies[0].Initialize(ColonyOwner.Player, config.playerStartUnits);
            allColonies[1].Initialize(ColonyOwner.Enemy, config.enemyStartUnits);
        }
    }
    
    public void CreateTentacle(Colony source, Colony target, float unitPercentage)
    {
        if (source == null || target == null || gameEnded)
            return;
        
        if (source.Owner != ColonyOwner.Player)
            return;
        
        float unitsToSend = source.Units * unitPercentage;
        if (unitsToSend < 1f)
            return;
        
        if (source.TryRemoveUnits(unitsToSend))
        {
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
        if (victoryOverlay != null)
        {
            victoryOverlay.Show();
        }
    }
    
    private void Defeat()
    {
        gameEnded = true;
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
}
