# Renaissance — AI Agent Guidelines

## О проекте

**Название:** Renaissance (2D Tower Defense)
**Движок:** Unity 6000.4.10f1
**Тип:** Прототип
**Платформа:** Windows

### Команда

| Роль | Участник | Зона ответственности |
|------|---------|---------------------|
| Лид | Mikhail (Миша) | Архитектура, код-ревью, Git |
| Разработчик | Bogdan (Богдан) | Башни, апгрейды |
| Разработчик | Marina (Марина) | Карта, визуал |

### Связь

- Основной чат: команда использует AI в чате (не как агентов)
- PR и код-ревью: через GitHub
- Коммиты пушит **только пользователь** (для финальной проверки)

---

## Стандарты кода

### Именование

| Тип | Пример | Примечание |
|-----|--------|------------|
| Приватные поля | `_health`, `_coins` | Нижнее подчёркивание |
| Публичные поля | `MaxHealth` | PascalCase, свойства |
| Методы | `TakeDamage()`, `SpawnEnemy()` | PascalCase, глаголы |
| Классы | `TowerManager`, `EnemySpawner` | PascalCase, существительное |
| Константы | `MAX_WAVES` | UPPER_SNAKE_CASE |

### Принципы

- **Инкапсуляция:** все данные приватные, доступ через свойства/методы
- **ООП:** наследование где нужно, интерфейсы для общего поведения
- **Синглтоны:** только для менеджеров (`GameEconomy`, `UIManager`)
- **Single Responsibility:** один класс = одна задача

### Структура папок Scripts

```
Scripts/
├── GameScripts/
│   ├── Tower/           # Башни, снаряды, размещение
│   ├── Enemy/           # Враги, спавн
│   └── Path/            # Пути, чекпоинты
├── UI/                  # Интерфейс
├── Managers/            # Системные менеджеры
└── MenuScripts/         # Главное меню
```

---

## Git Workflow

### Conventional Commits

Формат: `<type>(<scope>): <description>`

**Type:**
| Type | Когда использовать |
|------|------------------|
| `feat` | Новая фича |
| `fix` | Исправление бага |
| `refactor` | Рефакторинг без смены поведения |
| `chore` | Настройки, зависимости, CI |
| `docs` | Документация |
| `perf` | Улучшение производительности |
| `test` | Тесты |

**Scope (опционально):** `tower`, `enemy`, `ui`, `spawner`, `economy`

**Breaking Changes:**
Используй `!` после типа: `feat!: change tower API`

Когда:
- Удаляешь публичный метод/поле
- Меняешь сигнатуру метода
- Меняешь поведение, которое другие части кода используют

Пример:
```
feat!: remove deprecated Tower.Upgrade() method
```

### Примеры коммитов

```
feat(tower): add upgrade panel UI
fix(spawner): correct wave interval calculation
refactor(economy): use events instead of direct reference
chore: update Unity version to 6000.4.10f1
docs: add architecture diagram to GAME_KNOWLEDGE.md
feat!: rename TowerData.Cost → TowerData.BaseCost
```

### Ветки

```
main                    # стабильная версия
├── feat/towers         # фичи башен
├── feat/waves          # система волн
└── bugfix/road-collision
```

---

## Для AI Агентов (Mavis и другие)

### Что можно делать

- Читать код, анализировать архитектуру
- Предлагать рефакторинг
- Писать новые классы/методы
- Создавать коммиты
- Обновлять документацию

### Что требует подтверждения пользователя

- **Push в remote** — всегда
- **Удаление файлов** — всегда
- **Изменение `.gitignore`** — всегда
- **Merge/pull request** — всегда

### Как работать

1. Прочитай `GAME_KNOWLEDGE.md` для контекста
2. Следуй стандартам кода ( имена, инкапсуляция)
3. Пиши коммиты по Conventional Commits
4. Перед пушем — показывай что будет запушено

---

## Текущее состояние (07.07.2026)

- Башни: ✅ работают, апгрейды работают
- Враги: ✅ двигаются по пути, урон от башен
- UI: ✅ панель апгрейда, HP слайдер
- Волны: 🔄 в разработке (основная задача)
- Публичный репозиторий: готовится к публикации

---

## Полезные ссылки

- [Conventional Commits](https://www.conventionalcommits.org/ru/v1.0.0-beta.2/)
- [Unity Scripting API](https://docs.unity3d.com/ScriptReference/)
