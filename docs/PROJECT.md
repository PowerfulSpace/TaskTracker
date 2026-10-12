# Task Tracker — карта проекта

## 1. Что это

Task Tracker — учебный backend-проект для практики Clean Architecture, EF Core, тестирования и совместной работы с AI-агентом.
Цель — наработать процесс: сначала документация, потом код, итерациями и без лишних абстракций.
В результате получится backend MVP (CRUD задач) и отдельный frontend на Vue 3 после прохождения MVP.
Проект состоит из двух частей: backend (реализован) и frontend (в разработке).

## 2. Стек

| Область | Технология |
|---|---|
| Язык / платформа | C# / .NET 10 |
| Web | ASP.NET Core Web API + Controllers |
| БД | Microsoft SQL Server (локальная установка) |
| ORM | Entity Framework Core |
| Валидация | FluentValidation |
| Тесты | xUnit + NSubstitute |
| API-документация | OpenAPI / Swagger UI |
| Frontend | Vue 3 + TypeScript + Vite + Pinia + Naive UI |

## 3. Структура репозитория

```text
TaskTracker/
├── AGENTS.md
├── Directory.Build.props
├── Directory.Packages.props
├── global.json
├── .editorconfig
├── docs/
└── src/
    ├── Backend/
    │   └── PS.TaskTracker/
    │       ├── PS.TaskTracker.slnx
    │       ├── PS.TaskTracker.API/
    │       ├── PS.TaskTracker.Application/
    │       ├── PS.TaskTracker.Common/
    │       ├── PS.TaskTracker.Domain/
    │       └── PS.TaskTracker.Infrastructure/
    └── Frontend/
        └── PS.TaskTracker/   — frontend на Vue 3
```

Назначение слоёв:

- `PS.TaskTracker.Domain` — сущности и бизнес-правила, без зависимостей.
- `PS.TaskTracker.Common` — общие построения, без зависимостей.
- `PS.TaskTracker.Application` — сценарии использования, интерфейсы репозиториев.
- `PS.TaskTracker.Infrastructure` — EF Core, MSSQL, реализации репозиториев.
- `PS.TaskTracker.API` — контроллеры, точка входа, сборка приложения.

## 4. Принципы разработки

- Сначала документация, потом код.
- Итерации: одна законченная задача за раз.
- Clean Architecture с явными зависимостями между слоями.
- Никаких абстракций «на будущее» (AutoMapper, CQRS-фреймворки, микросервисы).
- Централизованная конфигурация сборки и версий пакетов.
- Строгий режим компилятора (`TreatWarningsAsErrors`).
- Документация обновляется вместе с кодом.

## 5. Навигация по документации

| Файл | Что описывает | Когда читать | Статус |
|---|---|---|---|
| `AGENTS.md` | Правила работы агента в репозитории | Перед любой задачей | создан |
| `docs/SPEC.md` | Требования MVP и API-контракты | Перед работой над API и функциональностью | создан |
| `docs/DOMAIN.md` | Доменная модель, статусы, инварианты | Перед работой над Domain и бизнес-правилами | создан |
| `docs/ARCHITECTURE.md` | Слои, зависимости, ответственность | При изменении структуры проекта | создан |
| `docs/DECISIONS.md` | Журнал архитектурных решений | Перед выбором подхода, после решения | создан |
| `docs/ROADMAP.md` | Этапы разработки с чекбоксами | Для оценки прогресса | создан |
| `docs/FRONTEND.md` | Стек, структура, архитектура frontend | Перед работой над frontend | создан |

## 6. Что вне MVP

- Аутентификация, пользователи, роли.
- Комментарии, теги, назначение на других.
- Уведомления, напоминания.
- Docker, CI/CD.
- Микросервисы.
- Пагинация, фильтры, сортировка.
- Интеграционные тесты.
