# Renaissance — Game Knowledge Base

## О проекте

**Название:** Renaissance
**Движок:** Unity 6000.4.10f1
**Тип:** 2D Tower Defense прототип
**Платформа:** Windows
**Дата обновления:** 07.07.2026

## Архитектура

```
Scripts/
├── GameScripts/
│   ├── Tower/
│   │   ├── TowerData.cs        — ScriptableObject с характеристиками башни
│   │   ├── Tower.cs            — Логика башни: цель, атака, апгрейд
│   │   ├── PlacementManager.cs — Расстановка, ghost-башня, панель апгрейда
│   │   ├── GameEconomy.cs     — Синглтон управления монетами
│   │   └── Projectile.cs      — Снаряды: поиск цели, урон
│   ├── Enemy.cs                — Здоровье, урон, смерть врага
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

## Стандарты кода

- **Приватные поля:** `_health`, `_coins` (нижнее подчёркивание)
- **Синглтоны:** только для менеджеров
- **Инкапсуляция:** все данные приватные, доступ через свойства/методы

Подробнее: [AGENTS.md](../AGENTS.md)

---

## История изменений

### 07.07.2026

#### Чистка репозитория

| Коммит | Изменение |
|--------|-----------|
| `52fa12f` | .gitignore перенесён в корень репозитория |
| `cd57b1b` | Удалён неиспользуемый `using Unity.VisualScripting` и поле `_baseCoins` в BaseManager |
| `bcb61cd` | Создан `.gitattributes` для нормализации LF |
| `6d88e08` | `*.bat`, `*.ps1` добавлены в .gitignore; удалён дубликат .gitattributes |
| — | Создан `AGENTS.md` с гайдлайнами для AI агентов |

### 26.06.2026

#### 1. Enemy и EnemySpawner разделены

**Было:** EnemySpawner содержал и логику спавна, и здоровье врага. Урон от снаряда попадал в EnemySpawner.
**Стало:** Создан отдельный `Enemy.cs` с `TakeDamage`, `Die`, `coinReward`. EnemySpawner теперь только спавнит.
**Файл:** `Enemy.cs` — новый, `EnemySpawner.cs` — переписан

#### 2. Слой башни

**Было:** `Mathf.RoundToInt(Mathf.Log(towerLayer.value, 2))` — ненадёжная формула
**Стало:** Явное поле `placedTowerLayerNumber` (int), присваивается напрямую
**Файл:** `PlacementManager.cs:9-10, 121`

#### 3. Ghost-башня перехватывала клики

**Было:** Отключался только `BoxCollider2D`
**Стало:** `foreach (Collider2D col in ghostTower.GetComponents<Collider2D>()) col.enabled = false;`
**Файл:** `PlacementManager.cs:85-88`

#### 4. UI feedback при недостатке монет

**Было:** `TryUpgradeSelectedTower` молча `return;` если денег нет
**Стало:** Добавлены `feedbackText`, `upgradeButtonImage`, цветовая индикация кнопки
**Новые поля в инспекторе:** `feedbackText`, `upgradeButtonImage`, `affordableColor`, `unaffordableColor`
**Файл:** `PlacementManager.cs`

---

## Инспектор — что подключить

### PlacementManager

| Поле | Что |
|------|-----|
| `placementLayer` | LayerMask — слой для размещения башен |
| `placedTowerLayerNumber` | Номер слоя башни (напр. 8) |
| `upgradePanel` | Панель апгрейда (Canvas/UI) |
| `upgradeCostText` | TMP текст цены |
| `feedbackText` | TMP текст ошибки ("Not enough coins!") |
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

---

## Известные рекомендации

| Приоритет | Файл | Проблема | Статус |
|-----------|------|----------|--------|
| Средний | `WaypointFollower2D` | `FindWithTag("GameController")` — заменить на сериализованную ссылку | ⏳ |
| Низкий | `PathContainer` | `OnEnable` — лучше `Awake` | ⏳ |
| Средний | `EnemySpawner` | Только бесконечный спавн — нужны волны | ⏳ В работе |
| Низкий | `Tower.cs`, `Projectile.cs` | Комментарии на украинском — привести к русскому | ⏳ |

---

## Актуальные задачи

### В разработке

- [ ] **Волны** — основная задача от Марины
- [ ] Причесать комментарии (единый язык — русский)

### Публичный репозиторий

Проект готовится к публикации как портфолио. Требуется:
- [x] Почистить .gitignore
- [x] Убрать мусорные файлы
- [x] Создать AGENTS.md
- [x] Добавить README.md с описанием проекта

---

## Шпаргалки (16.07.2026)

### Blend Tree для 4-направленной анимации

**Что:** Одно состояние аниматора, которое переключает клипы по двум float-параметрам (MoveX, MoveY).

**Когда использовать:** Когда персонаж двигается в 4 стороны и нужны разные анимации для каждой.

**Настройка:**
1. Параметры аниматора: MoveX (float), MoveY (float), IsMoving (bool)
2. Создать Blend Tree (2D Simple Directional)
3. 4 motion с координатами:
   - Right: (1, 0)
   - Left: (-1, 0)
   - Up: (0, 1)
   - Down: (0, -1)
4. Entry → Run (без условий)

**Код (WaypointFollower2D.cs):**
```csharp
Vector2 dir = (target - transform.position).normalized;
if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
{
    anim.SetFloat("MoveX", dir.x > 0 ? 1 : -1);
    anim.SetFloat("MoveY", 0);
}
else
{
    anim.SetFloat("MoveX", 0);
    anim.SetFloat("MoveY", dir.y > 0 ? 1 : -1);
}
anim.SetBool("IsMoving", true);
```

**Важно:**
- Loop Time должен быть включён на каждом .anim файле (иначе анимация проиграется 1 раз и встанет)
- Blend Tree = 0 транзишенов → 0 задержки
- Mathf.Abs(x) = модуль числа (расстояние от нуля)

### Mathf.Abs (модуль числа)

| x | Mathf.Abs(x) |
|---|-------------|
| 5 | 5 |
| -3 | 3 |
| 0 | 0 |

Используется чтобы понять: движение больше по горизонтали (`AbsX > AbsY`) или по вертикали.

### ShowFeedback (корутина)

Автоматическое скрытие сообщения через N секунд:
```csharp
private Coroutine _feedbackCoroutine;

private void ShowFeedback(string msg, float duration = 3f)
{
    if (_feedbackCoroutine != null) StopCoroutine(_feedbackCoroutine);
    _feedbackCoroutine = StartCoroutine(FeedbackRoutine(msg, duration));
}

private IEnumerator FeedbackRoutine(string msg, float duration)
{
    feedbackText.text = msg;
    yield return new WaitForSeconds(duration);
    feedbackText.text = "";
}
```

### Почему игнорировать транзишены в аниматоре = лучше
- Каждый Any State → State транзишен создаёт задержку 1-2 кадра
- 4 состояния × 3 транзишена = 12 задержкоопасных мест
- Blend Tree решает это полностью
