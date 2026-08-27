# Level 2-6 Setup Guide

This document describes the Unity Editor setup required to complete the levels 2-6 implementation.

## Required Steps

### 1. Generate Level Definitions

1. Open the Unity Editor
2. Go to menu: **Game → Generate Level Definitions**
3. This will create 6 level definition assets in `Assets/_Game/Resources/Levels/`

The generator creates the following levels with exact positions from the game design:
- **Level 1**: 2 colonies (50 vs 15), no AI
- **Level 2**: 4 colonies (1P, 2N, 1E), AI interval 3.0s
- **Level 3**: 5 colonies in cross formation, AI interval 2.5s
- **Level 4**: 6 colonies in two clusters, AI interval 2.0s
- **Level 5**: 7 colonies with bridge, AI interval 1.8s
- **Level 6**: 8 colonies in ring, AI interval 1.4s

### 2. Create Colony Prefab

Create a prefab at `Assets/_Game/Prefabs/Colony.prefab` with:

**Components:**
- Transform
- SpriteRenderer (any colony sprite as default)
- Colony script
- CircleCollider2D (radius 0.5)

**Colony Script Fields:**
- owner: Neutral
- units: 10
- spriteRenderer: reference to SpriteRenderer
- unitsText: null (optional - add TextMeshPro child if needed)
- playerSprite: will be set at runtime
- enemySprite: will be set at runtime
- neutralSprite: will be set at runtime
- playerColor: (0, 0.8, 1, 1)
- enemyColor: (1, 0.2, 0.5, 1)
- neutralColor: (0.5, 0.5, 0.5, 1)

### 3. Configure GameConfig Asset

Open `Assets/_Game/Resources/GameConfig.asset` and configure:

1. **Level Settings:**
   - currentLevel: 1 (default)
   - levels: Array of 6 elements, drag Level1-Level6 assets here

2. **Enemy AI:**
   - aiActionInterval: 4 (will be overridden by level definitions)
   - aiEnabledFromLevel: 2

### 4. Configure BattleManager in Battle Scene

Open `Assets/_Game/Scenes/Battle.unity` and select the BattleManager GameObject:

1. **Configuration:**
   - config: GameConfig asset

2. **References:**
   - victoryOverlay: VictoryOverlay reference
   - defeatOverlay: DefeatOverlay reference
   - tentaclePrefab: Tentacle prefab
   - **colonyPrefab**: Colony prefab (newly required)

3. **Colony Sprites:**
   - **playerSprite**: `spr_colony_player`
   - **enemySprite**: `spr_colony_enemy`
   - **neutralSprite**: `spr_colony_neutral` (commit 0e5f939)

4. **Tutorial:**
   - tutorialManager: TutorialManager reference (if exists)

### 5. Configure GameSceneManager

The GameSceneManager GameObject (in MainMenu or DontDestroyOnLoad) needs:
- **gameConfig**: Reference to GameConfig asset

This ensures "Play" button loads the first uncompleted level.

### 6. Victory Overlay - Continue Button

The VictoryOverlay already has the Continue button configured. It will:
- Show on victory for levels 2-5
- Hide for level 1 and after level 6
- Call `BattleManager.LoadNextLevel()` when clicked

## Testing Checklist

- [ ] Level 1: 2 colonies at (-2.5, 0) and (2.5, 0), 50 vs 15, no AI, no neutrals
- [ ] Level 1 victory: Shows "Заново" and "Меню" only, no "Дальше"
- [ ] Level 2: 4 colonies spawn at correct positions, neutrals use neutral sprite, AI active
- [ ] Level 2-5 victory: Shows "Дальше", "Заново", "Меню"
- [ ] "Дальше" button: Loads next level correctly
- [ ] Level 6 victory: No "Дальше" button shown
- [ ] Neutral colonies: Don't grow, can be captured, use spr_colony_neutral
- [ ] PlayerPrefs: Completed level persists, menu Play loads first uncompleted

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
