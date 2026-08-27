using UnityEngine;
using UnityEditor;
using System.IO;

public class LevelDefinitionGenerator
{
    [MenuItem("Game/Generate Level Definitions")]
    public static void GenerateLevels()
    {
        string folderPath = "Assets/_Game/Resources/Levels";
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        
        CreateLevel1(folderPath);
        CreateLevel2(folderPath);
        CreateLevel3(folderPath);
        CreateLevel4(folderPath);
        CreateLevel5(folderPath);
        CreateLevel6(folderPath);
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        Debug.Log("Level definitions generated successfully!");
    }
    
    private static void CreateLevel1(string folderPath)
    {
        LevelDefinition level = ScriptableObject.CreateInstance<LevelDefinition>();
        level.levelNumber = 1;
        level.aiActionInterval = 4f;
        
        level.colonies.Add(new ColonySpawnData { position = new Vector2(-2.5f, 0f), mass = 50f, owner = ColonyOwner.Player });
        level.colonies.Add(new ColonySpawnData { position = new Vector2(2.5f, 0f), mass = 15f, owner = ColonyOwner.Enemy });
        
        AssetDatabase.CreateAsset(level, $"{folderPath}/Level1.asset");
    }
    
    private static void CreateLevel2(string folderPath)
    {
        LevelDefinition level = ScriptableObject.CreateInstance<LevelDefinition>();
        level.levelNumber = 2;
        level.aiActionInterval = 3f;
        
        level.colonies.Add(new ColonySpawnData { position = new Vector2(-2.2f, -2.2f), mass = 30f, owner = ColonyOwner.Player });
        level.colonies.Add(new ColonySpawnData { position = new Vector2(-1.6f, 1.0f), mass = 20f, owner = ColonyOwner.Neutral });
        level.colonies.Add(new ColonySpawnData { position = new Vector2(1.8f, -0.6f), mass = 20f, owner = ColonyOwner.Neutral });
        level.colonies.Add(new ColonySpawnData { position = new Vector2(2.2f, 2.2f), mass = 35f, owner = ColonyOwner.Enemy });
        
        AssetDatabase.CreateAsset(level, $"{folderPath}/Level2.asset");
    }
    
    private static void CreateLevel3(string folderPath)
    {
        LevelDefinition level = ScriptableObject.CreateInstance<LevelDefinition>();
        level.levelNumber = 3;
        level.aiActionInterval = 2.5f;
        
        level.colonies.Add(new ColonySpawnData { position = new Vector2(0f, -3.0f), mass = 35f, owner = ColonyOwner.Player });
        level.colonies.Add(new ColonySpawnData { position = new Vector2(0f, 3.0f), mass = 35f, owner = ColonyOwner.Enemy });
        level.colonies.Add(new ColonySpawnData { position = new Vector2(-3.0f, 0f), mass = 15f, owner = ColonyOwner.Neutral });
        level.colonies.Add(new ColonySpawnData { position = new Vector2(3.0f, 0f), mass = 15f, owner = ColonyOwner.Neutral });
        level.colonies.Add(new ColonySpawnData { position = new Vector2(0f, 0f), mass = 15f, owner = ColonyOwner.Neutral });
        
        AssetDatabase.CreateAsset(level, $"{folderPath}/Level3.asset");
    }
    
    private static void CreateLevel4(string folderPath)
    {
        LevelDefinition level = ScriptableObject.CreateInstance<LevelDefinition>();
        level.levelNumber = 4;
        level.aiActionInterval = 2f;
        
        level.colonies.Add(new ColonySpawnData { position = new Vector2(-2.6f, -1.6f), mass = 30f, owner = ColonyOwner.Player });
        level.colonies.Add(new ColonySpawnData { position = new Vector2(-2.8f, 1.0f), mass = 20f, owner = ColonyOwner.Neutral });
        level.colonies.Add(new ColonySpawnData { position = new Vector2(-1.2f, -2.8f), mass = 20f, owner = ColonyOwner.Neutral });
        level.colonies.Add(new ColonySpawnData { position = new Vector2(2.6f, 1.6f), mass = 30f, owner = ColonyOwner.Enemy });
        level.colonies.Add(new ColonySpawnData { position = new Vector2(2.8f, -1.0f), mass = 20f, owner = ColonyOwner.Neutral });
        level.colonies.Add(new ColonySpawnData { position = new Vector2(1.2f, 2.8f), mass = 20f, owner = ColonyOwner.Neutral });
        
        AssetDatabase.CreateAsset(level, $"{folderPath}/Level4.asset");
    }
    
    private static void CreateLevel5(string folderPath)
    {
        LevelDefinition level = ScriptableObject.CreateInstance<LevelDefinition>();
        level.levelNumber = 5;
        level.aiActionInterval = 1.8f;
        
        level.colonies.Add(new ColonySpawnData { position = new Vector2(0f, 0f), mass = 20f, owner = ColonyOwner.Neutral });
        level.colonies.Add(new ColonySpawnData { position = new Vector2(-3.0f, 0f), mass = 40f, owner = ColonyOwner.Player });
        level.colonies.Add(new ColonySpawnData { position = new Vector2(-2.6f, 2.0f), mass = 15f, owner = ColonyOwner.Neutral });
        level.colonies.Add(new ColonySpawnData { position = new Vector2(-2.6f, -2.0f), mass = 15f, owner = ColonyOwner.Neutral });
        level.colonies.Add(new ColonySpawnData { position = new Vector2(3.0f, 0f), mass = 40f, owner = ColonyOwner.Enemy });
        level.colonies.Add(new ColonySpawnData { position = new Vector2(2.6f, 2.0f), mass = 15f, owner = ColonyOwner.Neutral });
        level.colonies.Add(new ColonySpawnData { position = new Vector2(2.6f, -2.0f), mass = 15f, owner = ColonyOwner.Neutral });
        
        AssetDatabase.CreateAsset(level, $"{folderPath}/Level5.asset");
    }
    
    private static void CreateLevel6(string folderPath)
    {
        LevelDefinition level = ScriptableObject.CreateInstance<LevelDefinition>();
        level.levelNumber = 6;
        level.aiActionInterval = 1.4f;
        
        level.colonies.Add(new ColonySpawnData { position = new Vector2(-2.3f, -2.3f), mass = 40f, owner = ColonyOwner.Player });
        level.colonies.Add(new ColonySpawnData { position = new Vector2(2.3f, 2.3f), mass = 40f, owner = ColonyOwner.Enemy });
        level.colonies.Add(new ColonySpawnData { position = new Vector2(0f, 3.2f), mass = 12f, owner = ColonyOwner.Neutral });
        level.colonies.Add(new ColonySpawnData { position = new Vector2(0f, -3.2f), mass = 12f, owner = ColonyOwner.Neutral });
        level.colonies.Add(new ColonySpawnData { position = new Vector2(-3.2f, 0f), mass = 12f, owner = ColonyOwner.Neutral });
        level.colonies.Add(new ColonySpawnData { position = new Vector2(3.2f, 0f), mass = 12f, owner = ColonyOwner.Neutral });
        level.colonies.Add(new ColonySpawnData { position = new Vector2(-2.3f, 2.3f), mass = 12f, owner = ColonyOwner.Neutral });
        level.colonies.Add(new ColonySpawnData { position = new Vector2(2.3f, -2.3f), mass = 12f, owner = ColonyOwner.Neutral });
        
        AssetDatabase.CreateAsset(level, $"{folderPath}/Level6.asset");
    }
}
