# ROADMAP — план разработки

План фиксирует этапы проекта: что сделано, что предстоит и в каком порядке.
Обновляется по мере продвижения: задачи помечаются `[x]` сразу по завершении, выполненные пункты не удаляются — они часть истории.

## Текущий статус

**Текущий этап:** 5 (API), не начат.

**Что сделано:**
- Этап 0: solution настроен (пять проектов, ссылки по Clean Architecture, централизованная конфигурация, сборка проходит).
- Этап 1: закрыт. Созданы `AGENTS.md`, все документы `docs/` и обновлён `README.md`.
- Этап 2: закрыт. Реализован Domain: `TaskItem`, `TaskItemStatus`, исключения, фабрика `Create`, переходы, `UpdateDetails`, unit-тесты (22 теста).
- Этап 3: закрыт. Реализован Application: MediatR, handlers (7 команд + 2 запроса), `ValidationBehavior`, валидаторы, DTO, исключения, unit-тесты (22 теста).
- Этап 4: закрыт. Реализован Infrastructure: `AppDbContext`, конфигурации, `TaskRepository`, `UnitOfWork`, `AddInfrastructure`, `AppDbContextFactory`, миграция `InitialCreate` (44 теста зелёные, `dotnet ef migrations list` показывает миграцию).

**Что предстоит:**
- Этап 5: реализация API.

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

### Этап 2. Domain — выполнен

**Цель:** Реализовать ядро: сущность `TaskItem`, статусы, инварианты, исключения — без зависимостей.

**Задачи:**
- [x] `TaskItemStatus` — enum.
- [x] `TaskItem` — сущность с приватными сеттерами.
- [x] Фабричный метод `TaskItem.Create`.
- [x] Методы переходов: `Start`, `Complete`, `Reopen`, `ReturnToTodo`.
- [x] Метод `UpdateDetails`.
- [x] Исключения: `DomainException`, `DomainValidationException`, `DomainConflictException`.
- [x] Инварианты: `Title` ≤ 200, `Description` ≤ 2000.
- [x] Тестовый проект `PS.TaskTracker.Domain.Tests` (xUnit + NSubstitute).
- [x] Unit-тесты на создание, инварианты, все переходы, идемпотентность `Complete` на `Done`.

**Критерий завершения:** Домен собирается, unit-тесты проходят, домен ни от кого не зависит.

### Этап 3. Application — выполнен

**Цель:** Реализовать сценарии использования: handlers, интерфейсы инфраструктуры, DTO, валидацию.

**Задачи:**
- [x] `ITaskRepository`, `IUnitOfWork`.
- [x] `TaskDto`.
- [x] Команды и запросы: `CreateTaskCommand`, `UpdateTaskCommand`, `DeleteTaskCommand`, `StartTaskCommand`, `CompleteTaskCommand`, `ReopenTaskCommand`, `ReturnTaskToTodoCommand`, `GetTaskQuery`, `GetTasksQuery`.
- [x] Handlers: по одному на сценарий.
- [x] FluentValidation-валидаторы.
- [x] `ValidationException`, `NotFoundException` (Application).
- [x] `DependencyInjection.AddApplication()`.
- [x] MediatR: request-модели `IRequest<T>`, handlers `IRequestHandler`, pipeline `ValidationBehavior`.
- [x] Тестовый проект `PS.TaskTracker.Application.Tests`.
- [x] Unit-тесты handlers с подменёнными `ITaskRepository` и `IUnitOfWork`.

**Критерий завершения:** Сценарии работают с моками, тесты проходят, Application не зависит от Infrastructure.

### Этап 4. Infrastructure — выполнен

**Цель:** Подключить EF Core + MSSQL: контекст, конфигурации, репозиторий, UoW, миграции.

**Задачи:**
- [x] `AppDbContext`.
- [x] `TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>`.
- [x] `TaskRepository : ITaskRepository`.
- [x] `UnitOfWork : IUnitOfWork`.
- [x] Миграции EF Core.
- [x] `DependencyInjection.AddInfrastructure(connectionString)`.
- [x] `AppDbContextFactory` для design-time (не был в исходном списке).

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
