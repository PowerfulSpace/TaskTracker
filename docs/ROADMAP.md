# ROADMAP — план разработки

План фиксирует этапы проекта: что сделано, что предстоит и в каком порядке.
Обновляется по мере продвижения: задачи помечаются `[x]` сразу по завершении, выполненные пункты не удаляются — они часть истории.

## Текущий статус

**Текущий этап:** 2 (Domain), не начат.

**Что сделано:**
- Этап 0: solution настроен (пять проектов, ссылки по Clean Architecture, централизованная конфигурация, сборка проходит).
- Этап 1: закрыт. Созданы `AGENTS.md`, все документы `docs/` и обновлён `README.md`.

**Что предстоит:**
- Этап 2: реализация Domain.

## Этапы

### Этап 0. Подготовка solution — выполнен

**Цель:** Настроить solution и централизованную конфигурацию сборки.

**Задачи:**
- [x] Разведка репозитория.
- [x] Удаление шаблонного кода (WeatherForecast, Class1.cs).
- [x] Настройка ProjectReference по Clean Architecture.
- [x] Создание global.json.
- [x] Создание Directory.Build.props.
- [x] Создание Directory.Packages.props (CPM).
- [x] Создание .editorconfig.
- [x] Проверка сборки.

**Критерий завершения:** Solution собирается, структура соответствует Clean Architecture.

### Этап 1. Документация — выполнен

**Цель:** Зафиксировать требования, домен, архитектуру и решения до написания кода.

**Задачи:**
- [x] AGENTS.md.
- [x] docs/PROJECT.md.
- [x] docs/SPEC.md.
- [x] docs/DOMAIN.md.
- [x] docs/ARCHITECTURE.md.
- [x] docs/DECISIONS.md.
- [x] docs/ROADMAP.md (этот файл).
- [x] Обновить README.md — краткое описание проекта и ссылки на docs/.
- [x] Финальная вычитка документов.

**Критерий завершения:** Все документы согласованы, README.md не пустой, ссылки в PROJECT.md актуальны.

### Этап 2. Domain — не начат

**Цель:** Реализовать ядро: сущность `TaskItem`, статусы, инварианты, исключения — без зависимостей.

**Задачи:**
- [ ] `TaskItemStatus` — enum.
- [ ] `TaskItem` — сущность с приватными сеттерами.
- [ ] Фабричный метод `TaskItem.Create`.
- [ ] Методы переходов: `Start`, `Complete`, `Reopen`, `ReturnToTodo`.
- [ ] Метод `UpdateDetails`.
- [ ] Исключения: `DomainException`, `DomainValidationException`, `DomainConflictException`.
- [ ] Инварианты: `Title` ≤ 200, `Description` ≤ 2000.
- [ ] Тестовый проект `PS.TaskTracker.Domain.Tests` (xUnit + NSubstitute).
- [ ] Unit-тесты на создание, инварианты, все переходы, идемпотентность `Complete` на `Done`.

**Критерий завершения:** Домен собирается, unit-тесты проходят, домен ни от кого не зависит.

### Этап 3. Application — не начат

**Цель:** Реализовать сценарии использования: handlers, интерфейсы инфраструктуры, DTO, валидацию.

**Задачи:**
- [ ] `IHandler<TRequest, TResponse>`.
- [ ] `Unit`.
- [ ] `ITaskRepository`, `IUnitOfWork`.
- [ ] `TaskDto`.
- [ ] Команды и запросы: `CreateTaskCommand`, `UpdateTaskCommand`, `DeleteTaskCommand`, `StartTaskCommand`, `CompleteTaskCommand`, `ReopenTaskCommand`, `ReturnTaskToTodoCommand`, `GetTaskQuery`, `GetTasksQuery`.
- [ ] Handlers: по одному на сценарий.
- [ ] FluentValidation-валидаторы.
- [ ] `ValidationException`, `NotFoundException` (Application).
- [ ] `DependencyInjection.AddApplication()`.
- [ ] Тестовый проект `PS.TaskTracker.Application.Tests`.
- [ ] Unit-тесты handlers с подменёнными `ITaskRepository` и `IUnitOfWork`.

**Критерий завершения:** Сценарии работают с моками, тесты проходят, Application не зависит от Infrastructure.

### Этап 4. Infrastructure — не начат

**Цель:** Подключить EF Core + MSSQL: контекст, конфигурации, репозиторий, UoW, миграции.

**Задачи:**
- [ ] `AppDbContext`.
- [ ] `TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>`.
- [ ] `TaskRepository : ITaskRepository`.
- [ ] `UnitOfWork : IUnitOfWork`.
- [ ] Миграции EF Core.
- [ ] `DependencyInjection.AddInfrastructure(connectionString)`.

**Критерий завершения:** БД создаётся миграцией, данные сохраняются и читаются.

### Этап 5. API — не начат

**Цель:** Реализовать HTTP-слой: контроллеры, DTO, middleware, JSON, Swagger.

**Задачи:**
- [ ] `TasksController` — CRUD.
- [ ] Endpoints переходов статусов.
- [ ] HTTP-DTO: `CreateTaskRequest`, `UpdateTaskRequest`, `TaskResponse`.
- [ ] Маппинг HTTP-DTO ↔ Application-DTO вручную.
- [ ] `ExceptionHandlingMiddleware`.
- [ ] Настройка `JsonStringEnumConverter`.
- [ ] Swagger / OpenAPI.
- [ ] Вызов `AddApplication()` и `AddInfrastructure()` из `Program.cs`.
- [ ] Строка подключения в `appsettings.Development.json`.

**Критерий завершения:** Все endpoints отвечают согласно SPEC.md.

### Этап 6. Проверка MVP — не начат

**Цель:** Убедиться, что backend MVP работает целиком и соответствует документации.

**Задачи:**
- [ ] Прогон CRUD через Swagger.
- [ ] Прогон всех переходов статусов.
- [ ] Проверка ошибочных сценариев (404, 409, 400, 500).
- [ ] Прогон unit-тестов.
- [ ] Проверка маппинга ошибок в ProblemDetails.
- [ ] Проверка соответствия кода документации.
- [ ] Инструкция запуска в README.md.

**Критерий завершения:** Backend MVP работает, документация соответствует реализации, инструкция воспроизводима.

### Этап 7. Frontend (вне backend MVP) — не начат

**Цель:** После закрытия backend MVP — реализовать frontend.

Frontend на Vue 3 + TypeScript + Vite: подключение к API, UI для CRUD и переходов статусов. Детали — отдельный документ при старте этапа.

## Правила ведения ROADMAP

- Обновлять при завершении этапа.
- Отмечать задачи `- [x]` сразу по завершении.
- Не удалять выполненные пункты — они часть истории.
- При изменении scope — согласовать с заказчиком и обновить `docs/SPEC.md`, `docs/DECISIONS.md`.
