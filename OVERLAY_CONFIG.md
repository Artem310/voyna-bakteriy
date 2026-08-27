# UI Overlay Sprites Configuration

## Файлы спрайтов (коммит 020f5cd)

Подтверждено: все PNG-файлы ~1MB класса:
- `Assets/Sprites/UI/ui_panel_card.png` — 1,359,122 байт
- `Assets/Sprites/UI/ui_btn_fill_cyan.png` — 1,081,631 байт
- `Assets/Sprites/UI/ui_btn_fill_magenta.png` — 1,061,742 байт
- `Assets/Sprites/UI/ui_btn_outline.png` — 1,058,919 байт

## Настройки импорта

### ui_panel_card.png
- **Sprite Mode**: Sliced (9-slice)
- **Sprite Border**: {x: 64, y: 64, z: 64, w: 64}
- **Texture Type**: Sprite (2D and UI)

### ui_btn_fill_cyan.png, ui_btn_fill_magenta.png, ui_btn_outline.png
- **Sprite Mode**: Single
- **Texture Type**: Sprite (2D and UI)

## Спецификация оверлеев

### Victory Overlay
Полноэкранный диммер (60% черный), карточка с ui_panel_card (sliced).

**Кнопки** (сверху вниз, размер 280×56, вертикальный зазор 12px, отступ до низа карточки 24px):
1. **Дальше** (скрыта) — ui_btn_fill_cyan
2. **Заново** — ui_btn_fill_cyan
3. **Меню** — ui_btn_outline

### Defeat Overlay
Полноэкранный диммер (60% черный), карточка с ui_panel_card (sliced).

**Кнопки** (сверху вниз, размер 280×56, вертикальный зазор 12px, отступ до низа карточки 24px):
1. **Заново** — ui_btn_fill_magenta
2. **Меню** — ui_btn_outline

Кнопка "Дальше" отсутствует в Defeat.

## Примечания

- Текстовые метки — TextMeshPro компоненты, не включены в текстуры
- Дополнительные HUD-элементы из макетов не включены
- Unity-сцены (Battle.unity) и скрипты оверлеев отсутствуют в репозитории
- Для подключения спрайтов в Unity: назначить Image.sprite через Inspector или YAML сцены
