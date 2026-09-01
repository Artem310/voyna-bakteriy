using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Game/Config")]
public class GameConfig : ScriptableObject
{
    [Header("Gameplay Settings")]
    public float growthPerSecond = 3f;
    public float tentacleSpeed = 5f;
    public float unitTransferRate = 10f;
    
    [Header("Level Settings")]
    public int currentLevel = 1;
    public LevelDefinition[] levels;
    
    [Header("Colony Settings")]
    public float colonyRadius = 1f;
    public float captureThreshold = 1f;
    
    [Header("Tentacle Constraints")]
    public float tentacleMinMass = 10f;
    public int maxTentaclesPerColony = 2;
    
    [Header("Enemy AI")]
    public float aiActionInterval = 4f;
    public int aiEnabledFromLevel = 2;
    
    public LevelDefinition GetCurrentLevelDefinition()
    {
        if (levels == null || levels.Length == 0)
            return null;
        
        int index = Mathf.Clamp(currentLevel - 1, 0, levels.Length - 1);
        return levels[index];
    }
}
