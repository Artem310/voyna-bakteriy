# Война бактерий — MVP-аудиопак

Органические / влажные / микроскопические скетчи (процедурный синтез). Это **не** студийные мастера: пригодны для прототипа, плейтестов и проверки гейт-логики. Финальный пасс лучше писать фоли + лёгкий синтез в DAW.

Поставка: **OGG Vorbis ~96 kbps** для билда; **WAV 44.1 kHz 16-bit** — мастера.

```
/workspace/audio/bacteria-war/
  sfx/     моно WAV + OGG
  bgm/     стерео WAV + OGG, зацикленные
  src/     генератор (Python), если нужно пересобрать
```

---

## Папки в Unity

| Здесь | В проекте |
|---|---|
| `sfx/` | `Assets/Audio/SFX/` |
| `bgm/` | `Assets/Audio/BGM/` |

Импортировать **OGG** в билд (мобильный размер). WAV можно держать рядом как Source Asset, но не включать в Addressables/StreamingAssets, если размер критичен.

---

## Import Settings

### SFX (короткие, ≤ 1.8 с)

- **Load Type:** `Decompress On Load` (короткие one-shot; не стримить)
- **Compression Format:** `Vorbis`, Quality ≈ 70 (или оставить исходный OGG)
- **Sample Rate Setting:** `Preserve Sample Rate` (44.1 kHz). Если упираетесь в размер: `Override` **22050 Hz** только для SFX — на казуальном RTS почти не слышно
- **Force To Mono:** On (файлы уже моно)
- **Preload Audio Data:** On
- **Spatialize / Ambisonic:** Off

Варианты `_02` вешать на тот же `AudioSource` через массив клипов и `Random.Range` (анти-усталость).

### BGM (30–35 с, луп)

- **Load Type:** `Streaming`
- **Compression Format:** `Vorbis`, Quality ≈ 70
- **Sample Rate Setting:** `Preserve Sample Rate`
- **Force To Mono:** Off (стерео)
- **Preload Audio Data:** Off
- На клипе: **Load In Background** On

В инспекторе `AudioSource` музыки: **Loop = On**. Точки лупа совпадают с началом/концом файла (фаза осцилляторов подогнана под длину петли). Не режьте клип в Unity.

---

## Audio Mixer

Группы:

| Группа | Назначение |
|---|---|
| `Master` | выход |
| `SFX` | геймплейные эффекты |
| `BGM` | музыка |
| `UI` | кнопки/меню (пока пусто в этом паке) |

Дублировать snapshot: `Default`, `Pause` (BGM −8 dB), `Result` (BGM −12 dB, SFX 0).

---

## AudioSource (2D)

Все казуальные RTS-события — **2D**:

- `spatialBlend = 0`
- `spatialize = false`
- `dopplerLevel = 0`
- `minDistance` / 3D rolloff не используются

Рекомендуемые уровни (fader клипа / mixer, относительно 0 dBFS клипа):

| Шина / клип | Volume |
|---|---|
| Mixer `SFX` | 0 dB |
| Mixer `BGM` | **−8 dB** (кровать под SFX) |
| Mixer `UI` | 0 dB |
| `sfx_grow` / `_02` | **−4 dB** (часто повторяется) |
| `sfx_tentacle_launch` / `_02` | 0 dB |
| `sfx_capture` / `_02` | −1 dB |
| `sfx_victory` | 0 dB |
| `sfx_defeat` | −1 dB |
| `bgm_battle` | 0 dB на клипе (глушим шиной BGM) |
| `bgm_menu` | 0 dB на клипе |

Pitch: 1.0. Для grow/tentacle можно лёгкий random pitch **0.97–1.03**, не больше — иначе ломается «влажный» тембр.

---

## Когда играть

| Событие | Клип | Когда |
|---|---|---|
| Рост биомассы | `sfx_grow` / `sfx_grow_02` | Каждый заметный прирост на ноде/юните (не каждый тик). Троттл ≥ 120 мс, рандом варианта |
| Запуск щупальца | `sfx_tentacle_launch` / `_02` | В момент **launch** (не hit). Один one-shot на выстрел |
| Захват территории | `sfx_capture` / `_02` | Когда нода **перешла** игроку (lock-in), не в процессе осады |
| Победа | `sfx_victory` | Экран результата, один раз. BGM battle fade-out 0.4 с |
| Поражение | `sfx_defeat` | Экран результата, один раз. BGM battle fade-out 0.4 с |
| Бой | `bgm_battle` | Сцена матча, loop. Старт с fade-in 0.6–1.0 с |
| Меню | `bgm_menu` | Меню / лобби / карта, loop. Кроссфейд с battle 0.8 с |

Не играть grow на каждом кадре симуляции — только на дискретном шаге UI/логики («биомасса +N»).

---

## Таблица имён (файл → событие)

| Файл | Event / ключ | Категория |
|---|---|---|
| `sfx_grow` | `sfx.grow` | SFX |
| `sfx_grow_02` | `sfx.grow` (variant) | SFX |
| `sfx_tentacle_launch` | `sfx.tentacle_launch` | SFX |
| `sfx_tentacle_launch_02` | `sfx.tentacle_launch` (variant) | SFX |
| `sfx_capture` | `sfx.capture` | SFX |
| `sfx_capture_02` | `sfx.capture` (variant) | SFX |
| `sfx_victory` | `sfx.victory` | SFX stinger |
| `sfx_defeat` | `sfx.defeat` | SFX stinger |
| `bgm_battle` | `bgm.battle` | BGM |
| `bgm_menu` | `bgm.menu` | BGM |

Префикс `sfx_` / `bgm_` совпадает с ключом без префикса папки.

---

## Формат и размеры (факт с диска)

SFX: моно, 44.1 kHz, 16-bit PCM WAV + Vorbis 96 kbps.  
BGM: стерео, 44.1 kHz, 16-bit PCM WAV + Vorbis 96 kbps.

| Файл | Длит. | WAV | OGG |
|---|---:|---:|---:|
| `sfx_grow` | 0.78 с | 67 KB | 12 KB |
| `sfx_grow_02` | 0.86 с | 74 KB | 13 KB |
| `sfx_tentacle_launch` | 0.42 с | 36 KB | 8 KB |
| `sfx_tentacle_launch_02` | 0.50 с | 43 KB | 9 KB |
| `sfx_capture` | 0.88 с | 76 KB | 13 KB |
| `sfx_capture_02` | 0.96 с | 83 KB | 13 KB |
| `sfx_victory` | 1.52 с | 131 KB | 20 KB |
| `sfx_defeat` | 1.58 с | 136 KB | 16 KB |
| `bgm_battle` | 34.29 с | 5.8 MB | 338 KB |
| `bgm_menu` | 30.00 с | 5.0 MB | 303 KB |
| **Итого в билде (только OGG)** | | | **~740 KB** |

Пики SFX ≈ −1.1…−1.8 dBTP, без клиппинга. Интегральная громкость one-shot ≈ −11…−18 LUFS (короткие; на шине будет ближе к −14). BGM ≈ −17.5 / −18 LUFS — специально тише, чтобы не спорить с эффектами.

---

## Заметки по качеству

- Тембр: пузырьки, влажный snap, низкий органик-дрон. Не хоррор, не cartoon boing, не chiptune.
- На SFX стоят fade in/out 5–15 мс против кликов.
- Лупы музыки фазово закрыты (частоты подогнаны под длину петли); шов ~−46 dB.
- Пересборка: `PYTHONPATH=... /usr/bin/python3.13 src/gen_sfx.py` и `src/gen_bgm.py`, затем `ffmpeg -c:a libvorbis -b:a 96k`.
