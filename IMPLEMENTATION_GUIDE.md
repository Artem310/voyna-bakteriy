# Victory/Defeat Overlay Implementation

## Статус
✅ PNG-спрайты подтверждены (~1MB класса)
✅ .meta файлы созданы с правильными настройками импорта
✅ C# скрипты созданы
⚠️ Unity сцена Battle.unity требует ручной настройки в редакторе

## Файлы спрайтов

| Файл | GUID | Размер | Назначение |
|------|------|--------|------------|
| ui_panel_card.png | `7a3c8e1f2b4d5e6f7a8b9c0d1e2f3a4b` | 1,359,122 bytes | Card background (9-slice, border 64×64×64×64) |
| ui_btn_fill_cyan.png | `4b8c9d0e1f2a3b4c5d6e7f8a9b0c1d2e` | 1,081,631 bytes | Victory Restart button |
| ui_btn_fill_magenta.png | `9e0f1a2b3c4d5e6f7a8b9c0d1e2f3a4c` | 1,061,742 bytes | Defeat Restart button |
| ui_btn_outline.png | `2c3d4e5f6a7b8c9d0e1f2a3b4c5d6e7f` | 1,058,919 bytes | Menu button (both overlays) |

## Скрипты

- **VictoryOverlay.cs** (GUID: `d4e5f678901234567890123456abcdef`)
- **DefeatOverlay.cs** (GUID: `e5f67890123456789012345678abcdef`)

## Настройка в Unity Editor

### Victory Overlay Hierarchy

```
VictoryOverlay (GameObject + VictoryOverlay script)
├── Dimmer (Image)
│   └── Color: rgba(0, 0, 0, 0.6), Full Rect Stretch
├── Card (Image)
│   └── Sprite: ui_panel_card, Type: Sliced, Size: 600×400, Centered
├── ContinueButton (GameObject + Button + Image)
│   └── Sprite: ui_btn_fill_cyan, Size: 280×56, Position: (0, -12), **HIDDEN**
├── RestartButton (GameObject + Button + Image)
│   └── Sprite: ui_btn_fill_cyan, Size: 280×56, Position: (0, -80)
└── MenuButton (GameObject + Button + Image)
    └── Sprite: ui_btn_outline, Size: 280×56, Position: (0, -148)
```

### Defeat Overlay Hierarchy

```
DefeatOverlay (GameObject + DefeatOverlay script)
├── Dimmer (Image)
│   └── Color: rgba(0, 0, 0, 0.6), Full Rect Stretch
├── Card (Image)
│   └── Sprite: ui_panel_card, Type: Sliced, Size: 600×400, Centered
├── RestartButton (GameObject + Button + Image)
│   └── Sprite: ui_btn_fill_magenta, Size: 280×56, Position: (0, -80)
└── MenuButton (GameObject + Button + Image)
    └── Sprite: ui_btn_outline, Size: 280×56, Position: (0, -148)
```

## Button Specifications

### Grid Layout (exact positions from card center)
- **Card**: 600×400, centered at (0, 0)
- **Button size**: 280×56 (Full Rect, не растянуто)
- **Vertical gap**: 12px
- **Bottom margin**: 24px from card bottom (-200)

### Position calculations:
- Card bottom: y = -200
- First button (Menu) bottom: y = -200 + 24 = -176, center: y = -176 + 28 = **-148**
- Second button (Restart) bottom: y = -176 + 56 + 12 = -108, center: y = -108 + 28 = **-80**
- Third button (Continue, Victory only) bottom: y = -108 + 56 + 12 = -40, center: y = -40 + 28 = **-12**

### Image Component Settings
- **Type**: Simple (NOT Sliced, NOT Tiled)
- **Preserve Aspect**: false
- **RectTransform Size Delta**: (280, 56) - EXACT, не меняй!

## TextMeshPro Labels

Все лейблы - дочерние объекты кнопок:
- **Font Size**: 32
- **Alignment**: Center/Middle
- **Color**: White
- **Text**:
  - Victory Continue: "Дальше"
  - Victory Restart: "Заново"
  - Defeat Restart: "Заново"
  - Menu (both): "Меню"

## Script References (assign in Inspector)

### VictoryOverlay
- overlayRoot: VictoryOverlay GameObject
- dimmer: Dimmer Image
- cardBackground: Card Image
- continueButton: ContinueButton Button
- restartButton: RestartButton Button
- menuButton: MenuButton Button
- continueLabel, restartLabel, menuLabel: TMP компоненты

### DefeatOverlay
- overlayRoot: DefeatOverlay GameObject
- dimmer: Dimmer Image
- cardBackground: Card Image
- restartButton: RestartButton Button
- menuButton: MenuButton Button
- restartLabel, menuLabel: TMP компоненты

## Button OnClick Events

### VictoryOverlay
- ContinueButton → VictoryOverlay.OnContinueClicked()
- RestartButton → VictoryOverlay.OnRestartClicked()
- MenuButton → VictoryOverlay.OnMenuClicked()

### DefeatOverlay
- RestartButton → DefeatOverlay.OnRestartClicked()
- MenuButton → DefeatOverlay.OnMenuClicked()

## Important Notes

✅ **Спрайты НЕ изменены** - используются оригинальные PNG из коммита 020f5cd
✅ **Сетка 280×56 сохранена** - gap 12px, margin 24px
✅ **9-slice настроен** - ui_panel_card с border 64px
✅ **Кнопки Full Rect** - не растянуты, точный размер 280×56
✅ **Continue скрыта** - SetActive(false) в Victory Awake()
