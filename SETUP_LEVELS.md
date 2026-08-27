# Level 2-6 Implementation

All level definitions, prefabs, and scene references are now baked into git. Clone + Play should work out of the box.

## What's Included

### Level Definitions

Six LevelDefinition assets are committed in `Assets/_Game/Resources/Levels/` with exact positions from game design:
- **Level 1**: 2 colonies (50 vs 15), no AI
- **Level 2**: 4 colonies (1P, 2N, 1E), AI interval 3.0s
- **Level 3**: 5 colonies in cross formation, AI interval 2.5s
- **Level 4**: 6 colonies in two clusters, AI interval 2.0s
- **Level 5**: 7 colonies with bridge, AI interval 1.8s
- **Level 6**: 8 colonies in ring, AI interval 1.4s

### Colony Prefab

`Assets/_Game/Prefabs/Colony.prefab` includes:
- Transform, SpriteRenderer, Colony script, CircleCollider2D
- Sprites set at runtime by BattleManager

### GameConfig Asset

`Assets/_Game/Resources/GameConfig.asset` configured with:
- levels: Array of 6 LevelDefinition references (Level1-Level6)
- aiEnabledFromLevel: 2

### Battle Scene

`Assets/_Game/Scenes/Battle.unity` includes:
- BattleManager with colonyPrefab and sprite references (player/enemy/neutral)
- VictoryOverlay with Continue button (shows L1-L5, hidden after L6)
- Level 1 colonies at (-3, 0) and (3, 0) with 50 vs 15 mass

## Testing

- Level 1: 2 colonies at (-3, 0) and (3, 0), 50 vs 15, no AI, no neutrals
- Level 1 victory: Shows "Дальше", "Заново", "Меню"
- Level 2-6: Spawn at correct GD positions, neutrals use spr_colony_neutral, AI active from L2
- Levels 1-5 victory: "Дальше" button visible and functional
- Level 6 victory: "Дальше" button hidden
- Neutral colonies: Don't grow, can be captured
- PlayerPrefs: Progress persists, menu Play loads first uncompleted

## Code Architecture

The implementation uses:
- **LevelDefinition** ScriptableObject: Stores colony positions, masses, owners, and AI interval
- **BattleManager.LoadCurrentLevel()**: Spawns colonies from level definition or initializes existing scene colonies
- **VictoryOverlay.Show(currentLevel)**: Shows/hides Continue button based on level
- **GameSceneManager.LoadBattle()**: Loads first uncompleted level from PlayerPrefs
- **Colony.SetSprites()**: Sets player/enemy/neutral sprites at runtime

## Coordinate System

- Origin: Petri dish center
- Y-axis: Up
- Units: World units
- Dish radius: ~4.5
- Format: (x, y, mass, owner P/E/N)
