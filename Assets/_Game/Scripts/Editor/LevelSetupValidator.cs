using UnityEngine;
using UnityEditor;

public class LevelSetupValidator
{
    [MenuItem("Game/Validate Level Setup")]
    public static void ValidateSetup()
    {
        bool allValid = true;
        
        GameConfig config = Resources.Load<GameConfig>("GameConfig");
        if (config == null)
        {
            Debug.LogError("❌ GameConfig not found in Resources!");
            allValid = false;
        }
        else
        {
            Debug.Log("✓ GameConfig found");
            
            if (config.levels == null || config.levels.Length != 6)
            {
                Debug.LogError($"❌ GameConfig.levels should have 6 elements, has {(config.levels?.Length ?? 0)}");
                allValid = false;
            }
            else
            {
                Debug.Log("✓ GameConfig has 6 level definitions");
                
                for (int i = 0; i < config.levels.Length; i++)
                {
                    if (config.levels[i] == null)
                    {
                        Debug.LogError($"❌ Level {i + 1} definition is null");
                        allValid = false;
                    }
                    else
                    {
                        LevelDefinition level = config.levels[i];
                        if (level.levelNumber != i + 1)
                        {
                            Debug.LogWarning($"⚠ Level {i + 1} has levelNumber {level.levelNumber}");
                        }
                        
                        if (level.colonies == null || level.colonies.Count == 0)
                        {
                            Debug.LogError($"❌ Level {i + 1} has no colonies defined");
                            allValid = false;
                        }
                        else
                        {
                            Debug.Log($"✓ Level {i + 1}: {level.colonies.Count} colonies, AI interval {level.aiActionInterval}s");
                        }
                    }
                }
            }
            
            if (config.aiEnabledFromLevel != 2)
            {
                Debug.LogWarning($"⚠ aiEnabledFromLevel is {config.aiEnabledFromLevel}, expected 2");
            }
        }
        
        BattleManager battleManager = Object.FindObjectOfType<BattleManager>();
        if (battleManager == null)
        {
            Debug.LogWarning("⚠ BattleManager not found in scene (open Battle.unity)");
        }
        else
        {
            Debug.Log("✓ BattleManager found");
            
            var colonyPrefabField = battleManager.GetType().GetField("colonyPrefab", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            if (colonyPrefabField != null)
            {
                GameObject colonyPrefab = (GameObject)colonyPrefabField.GetValue(battleManager);
                if (colonyPrefab == null)
                {
                    Debug.LogError("❌ BattleManager.colonyPrefab is not assigned");
                    allValid = false;
                }
                else
                {
                    Debug.Log("✓ Colony prefab assigned");
                }
            }
        }
        
        Sprite neutralSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/spr_colony_neutral.png");
        if (neutralSprite == null)
        {
            Debug.LogError("❌ spr_colony_neutral.png not found");
            allValid = false;
        }
        else
        {
            Debug.Log("✓ Neutral colony sprite found");
        }
        
        if (allValid)
        {
            Debug.Log("✅ All validations passed!");
        }
        else
        {
            Debug.LogError("❌ Some validations failed. Check the setup guide.");
        }
    }
}
