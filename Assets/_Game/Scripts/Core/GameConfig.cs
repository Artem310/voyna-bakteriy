using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Game/Config")]
public class GameConfig : ScriptableObject
{
    [Header("Gameplay Settings")]
    public float growthPerSecond = 3f;
    public float tentacleSpeed = 5f;
    public float unitTransferRate = 10f;
    
    [Header("Level 1 Settings")]
    public int playerStartUnits = 50;
    public int enemyStartUnits = 15;
    
    [Header("Colony Settings")]
    public float colonyRadius = 1f;
    public float captureThreshold = 1f;
    
    [Header("Tentacle Constraints")]
    public float tentacleMinMass = 10f;
    public int maxTentaclesPerColony = 2;
    
    [Header("Enemy AI")]
    public float aiActionInterval = 4f;
}
