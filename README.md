# Task Tracker

## О проекте

Учебный backend-проект для практики Clean Architecture, EF Core и тестирования.
Управление задачами с переходами статусов: Todo → InProgress → Done.
Backend MVP — в разработке; frontend на Vue 3 — после завершения backend MVP.

## Стек

| Область | Технология |
|---|---|
| Язык / платформа | C# / .NET 10 |
| Web | ASP.NET Core Web API (Controllers) |
| БД / ORM | Entity Framework Core + MSSQL |
| Валидация | FluentValidation |
| Тесты | xUnit + NSubstitute |
| API-документация | OpenAPI / Swagger UI |

## Структура репозитория

```text
TaskTracker/
├── AGENTS.md          — правила работы агента
├── docs/              — проектная документация
└── src/
    ├── Backend/
    │   └── PS.TaskTracker/   — solution backend'а
    └── Frontend/             — пусто, после MVP
```

## Документация

| Файл | Что описывает |
|---|---|
| [AGENTS.md](AGENTS.md) | Правила работы агента |
| [docs/PROJECT.md](docs/PROJECT.md) | Карта проекта |
| [docs/SPEC.md](docs/SPEC.md) | Требования MVP и API-контракты |
| [docs/DOMAIN.md](docs/DOMAIN.md) | Доменная модель |
| [docs/ARCHITECTURE.md](docs/ARCHITECTURE.md) | Слои и зависимости |
| [docs/DECISIONS.md](docs/DECISIONS.md) | Журнал архитектурных решений |
| [docs/ROADMAP.md](docs/ROADMAP.md) | План разработки |

## Текущий статус

Этап 4 (Infrastructure) завершён. Следующий — Этап 5: реализация API.
Детали — в [docs/ROADMAP.md](docs/ROADMAP.md).

## Запуск

Инструкция по запуску будет добавлена после завершения backend MVP (Этап 6 в docs/ROADMAP.md).
