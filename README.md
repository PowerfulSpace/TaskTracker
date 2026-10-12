# Task Tracker

## О проекте

Учебный backend-проект для практики Clean Architecture, EF Core и тестирования.
Управление задачами с переходами статусов: Todo → InProgress → Done.
Backend MVP завершён. Следующий этап — frontend на Vue 3.

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
    └── Frontend/
        └── PS.TaskTracker/   — frontend на Vue 3
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

Backend MVP завершён (Этапы 0–6). Все 44 теста зелёные, 14 сценариев API проверены через curl. Следующий этап — frontend на Vue 3.
Детали — в [docs/ROADMAP.md](docs/ROADMAP.md).

## Запуск

### Требования

- .NET 10 SDK
- SQL Server (Developer или Express), локальный инстанс
- `dotnet-ef` (глобальный инструмент): `dotnet tool install --global dotnet-ef`

### Шаги

1. Настроить строку подключения в `src/Backend/PS.TaskTracker/PS.TaskTracker.API/appsettings.Development.json`:

   ```json
   "ConnectionStrings": {
     "Default": "Server=localhost;Database=TaskTracker;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

2. Применить миграции:

   ```text
   dotnet ef database update --project src/Backend/PS.TaskTracker/PS.TaskTracker.Infrastructure --startup-project src/Backend/PS.TaskTracker/PS.TaskTracker.Infrastructure
   ```

3. Запустить API:

   ```text
   dotnet run --project src/Backend/PS.TaskTracker/PS.TaskTracker.API
   ```

4. Открыть Swagger UI: `https://localhost:7190/swagger` (или порт из launchSettings.json).

### Тесты

```text
dotnet test
```

Или через скрипт:

- Windows: `.\scripts\verify.ps1`
- Linux / macOS: `./scripts/verify.sh`
