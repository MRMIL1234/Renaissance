# Renaissance Project — Unity 2D Tower Defense

## Архитектура (текущая)

```
Scripts/
├── GameScripts/
│   ├── Tower/
│   │   ├── TowerData.cs        — ScriptableObject с характеристиками башни
│   │   ├── Tower.cs            — Логика башни: цель, атака, апгрейд
│   │   ├── PlacementManager.cs — Расстановка, ghost-башня, панель апгрейда
│   │   ├── GameEconomy.cs     — Синглтон управления монетами
│   │   └── Projectile.cs      — Снаряды: поиск цели, урон
│   ├── Enemy.cs                — (СОЗДАНО) Здоровье, урон, смерть врага
│   ├── EnemySpawner.cs        — Спавн волн врагов (ТОЛЬКО спавн!)
│   ├── WaypointFollower2D.cs  — Движение врага по пути
│   └── PathContainer.cs       — Контейнер точек пути (дети объекта)
├── BaseManager.cs              — Здоровье базы, Game Over
├── UIManager.cs                — HP слайдер, панели
└── MenuScripts/MainMenuController.cs
```

## Ключевые связи
- `GameEconomy.Instance` — синглтон, доступен отовсюду
- `PlacementManager` — управляет расстановкой башен и панелью апгрейда
- `Tower` ищет врагов через `Physics2D.OverlapCircleAll`
- `Projectile.Seek(target, damage)` — снаряд летит к цели
- `Enemy.TakeDamage(dmg)` — получает урон от снаряда
- `Enemy.Die()` — награда монетами через `GameEconomy.AddCoins`
- `BaseManager.OnTriggerEnter2D` — враг касается базы, отнимает HP

## Что пофикшено (26.06.2025)

### 1. Enemy и EnemySpawner разделены
**Было:** EnemySpawner содержал и логику спавна, и здоровье врага. Урон от снаряда попадал в EnemySpawner.
**Стало:** Создан отдельный `Enemy.cs` с `TakeDamage`, `Die`, `coinReward`. EnemySpawner теперь только спавнит.
**Файл:** `Enemy.cs` — новый, `EnemySpawner.cs` — переписан

### 2. Слой башни
**Было:** `Mathf.RoundToInt(Mathf.Log(towerLayer.value, 2))` — ненадёжная формула
**Стало:** Явное поле `placedTowerLayerNumber` (int), присваивается напрямую
**Файл:** `PlacementManager.cs:9-10, 121`

### 3. Ghost-башня перехватывала клики
**Было:** Отключался только `BoxCollider2D`
**Стало:** `foreach (Collider2D col in ghostTower.GetComponents<Collider2D>()) col.enabled = false;`
**Файл:** `PlacementManager.cs:85-88`

### 4. UI feedback при недостатке монет
**Было:** `TryUpgradeSelectedTower` молча `return;` если денег нет
**Стало:** Добавлены `feedbackText`, `upgradeButtonImage`, цветовая индикация кнопки
**Новые поля в инспекторе:** `feedbackText`, `upgradeButtonImage`, `affordableColor`, `unaffordableColor`
**Файл:** `PlacementManager.cs`

## Инспектор — что подключить после фиксов

### PlacementManager
| Поле | Что |
|------|-----|
| `placementLayer` | LayerMask — слой для размещения башен |
| `placedTowerLayerNumber` | Номер слоя башни (напр. 8) |
| `upgradePanel` | Панель апгрейда (Canvas/UI) |
| `upgradeCostText` | TMP текст цены |
| `feedbackText` | TMP текст ошибки ("Недостаточно монет!") |
| `towerInfoText` | TMP текст характеристик башни |
| `upgradeButtonImage` | Image компонент кнопки апгрейда |
| `affordableColor` | Зелёный цвет кнопки (default: #34B233) |
| `unaffordableColor` | Красный цвет кнопки (default: #B23333) |

### Enemy (на вражеском префабе)
| Поле | Что |
|------|-----|
| `health` | Здоровье врага |
| `coinReward` | Награда за убийство |

### Tower (на префабе башни)
| Поле | Что |
|------|-----|
| `stats` | TowerData ScriptableObject |
| `enemyLayer` | LayerMask врагов |
| `projectilePrefab` | Префаб снаряда |
| `firePoint` | Точка спавна снаряда |

## Известные рекомендации (не критичные)
- `WaypointFollower2D.Start()` использует `FindWithTag("GameController")` — заменить на сериализованную ссылку на `PathContainer`
- `PathContainer` использует `OnEnable` — лучше `Awake`
- `WaypointFollower2D` не уведомляет `BaseManager` когда враг дошёл до конца — враги просто уничтожаются
