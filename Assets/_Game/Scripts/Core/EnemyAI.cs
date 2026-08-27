using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EnemyAI : MonoBehaviour
{
    private BattleManager battleManager;
    private GameConfig config;
    private float aiTimer = 0f;
    
    private void Start()
    {
        battleManager = BattleManager.Instance;
        if (battleManager != null)
        {
            config = battleManager.Config;
        }
    }
    
    private void Update()
    {
        if (battleManager == null || config == null || battleManager.IsGameEnded())
            return;
        
        if (Time.timeScale <= 0f)
            return;
        
        aiTimer += Time.deltaTime;
        
        if (aiTimer >= config.aiActionInterval)
        {
            aiTimer = 0f;
            ExecuteAIAction();
        }
    }
    
    private void ExecuteAIAction()
    {
        List<Colony> allColonies = battleManager.GetAllColonies();
        if (allColonies == null || allColonies.Count == 0)
            return;
        
        Colony fattestEnemyColony = FindFattestEnemyColony(allColonies);
        if (fattestEnemyColony == null)
            return;
        
        if (!fattestEnemyColony.CanLaunchTentacle())
            return;
        
        Colony nearestForeignColony = FindNearestForeignColony(fattestEnemyColony, allColonies);
        if (nearestForeignColony == null)
            return;
        
        battleManager.CreateTentacle(fattestEnemyColony, nearestForeignColony, 0.5f);
    }
    
    private Colony FindFattestEnemyColony(List<Colony> colonies)
    {
        Colony fattest = null;
        float maxUnits = 0f;
        
        foreach (var colony in colonies)
        {
            if (colony.Owner == ColonyOwner.Enemy && colony.Units > maxUnits)
            {
                maxUnits = colony.Units;
                fattest = colony;
            }
        }
        
        return fattest;
    }
    
    private Colony FindNearestForeignColony(Colony sourceColony, List<Colony> colonies)
    {
        Colony nearest = null;
        float minDistance = float.MaxValue;
        
        foreach (var colony in colonies)
        {
            if (colony.Owner != ColonyOwner.Enemy && colony != sourceColony)
            {
                float distance = Vector3.Distance(sourceColony.Position, colony.Position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearest = colony;
                }
            }
        }
        
        return nearest;
    }
}
