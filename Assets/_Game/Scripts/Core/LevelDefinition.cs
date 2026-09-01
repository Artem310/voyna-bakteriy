using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ColonySpawnData
{
    public Vector2 position;
    public float mass;
    public ColonyOwner owner;
}

[CreateAssetMenu(fileName = "Level", menuName = "Game/Level Definition")]
public class LevelDefinition : ScriptableObject
{
    [Header("Level Info")]
    public int levelNumber = 1;
    
    [Header("AI Settings")]
    public float aiActionInterval = 4f;
    
    [Header("Colony Spawns")]
    public List<ColonySpawnData> colonies = new List<ColonySpawnData>();
}
