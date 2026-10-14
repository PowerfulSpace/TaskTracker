# ROADMAP — план разработки

План фиксирует этапы проекта: что сделано, что предстоит и в каком порядке.
Обновляется по мере продвижения: задачи помечаются `[x]` сразу по завершении, выполненные пункты не удаляются — они часть истории.

## Текущий статус

**Текущий этап:** 7 (Frontend) — выполнен. Backend + Frontend MVP готовы.

**Что сделано:**
- Этап 0: solution настроен (пять проектов, ссылки по Clean Architecture, централизованная конфигурация, сборка проходит).
- Этап 1: закрыт. Созданы `AGENTS.md`, все документы `docs/` и обновлён `README.md`.
- Этап 2: закрыт. Реализован Domain: `TaskItem`, `TaskItemStatus`, исключения, фабрика `Create`, переходы, `UpdateDetails`, unit-тесты (22 теста).
- Этап 3: закрыт. Реализован Application: MediatR, handlers (7 команд + 2 запроса), `ValidationBehavior`, валидаторы, DTO, исключения, unit-тесты (22 теста).
- Этап 4: закрыт. Реализован Infrastructure: `AppDbContext`, конфигурации, `TaskRepository`, `UnitOfWork`, `AddInfrastructure`, `AppDbContextFactory`, миграция `InitialCreate` (44 теста зелёные, `dotnet ef migrations list` показывает миграцию).
- Этап 5: закрыт. Реализован API: `TasksController` (CRUD + endpoints переходов статусов), HTTP-DTO, `ExceptionHandlingMiddleware`, `JsonStringEnumConverter`, Swagger (Swashbuckle), DI из `Program.cs`, строка подключения в `appsettings.Development.json`.
- Этап 6: закрыт. Проверка MVP: миграция применена, Swagger UI отвечает (9 endpoints), 14/14 сценариев через `curl` (CRUD, переходы, ошибочные), инструкция запуска в `README.md`.
- Этап 7: закрыт. Frontend на Vue 3: проект (Vue 3 + TypeScript + Vite + Pinia + Router + Naive UI + axios), `.env.development`, документация (`docs/FRONTEND.md`, ADR-028/029/030), каркас (типы, Pinia-стор, сервис API), UI (список задач, форма создания/редактирования, кнопки переходов статусов, удаление с подтверждением), обработка ошибок ProblemDetails → уведомления Naive UI, ручная проверка CRUD и переходов через UI.

**Что предстоит:**
—

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

### Этап 5. API — выполнен

**Цель:** Реализовать HTTP-слой: контроллеры, DTO, middleware, JSON, Swagger.

**Задачи:**
- [x] `TasksController` — CRUD.
- [x] Endpoints переходов статусов.
- [x] HTTP-DTO: `CreateTaskRequest`, `UpdateTaskRequest`, `TaskResponse`.
- [x] Маппинг HTTP-DTO ↔ Application-DTO вручную.
- [x] `ExceptionHandlingMiddleware`.
- [x] Настройка `JsonStringEnumConverter`.
- [x] Swagger / OpenAPI.
- [x] Вызов `AddApplication()` и `AddInfrastructure()` из `Program.cs`.
- [x] Строка подключения в `appsettings.Development.json`.

**Критерий завершения:** Все endpoints отвечают согласно SPEC.md.

### Этап 6. Проверка MVP — выполнен

**Цель:** Убедиться, что backend MVP работает целиком и соответствует документации.

**Задачи:**
- [x] Прогон CRUD через Swagger.
- [x] Прогон всех переходов статусов.
- [x] Проверка ошибочных сценариев (404, 409, 400, 500).
- [x] Прогон unit-тестов.
- [x] Проверка маппинга ошибок в ProblemDetails.
- [x] Проверка соответствия кода документации.
- [x] Инструкция запуска в README.md.
- [x] Прогон 14 сценариев через `curl` (CRUD + переходы + ошибочные) — 14/14 успешно.

**Критерий завершения:** Backend MVP работает, документация соответствует реализации, инструкция воспроизводима.

### Этап 7. Frontend — выполнен

**Цель:** Реализовать frontend на Vue 3, подключить к API, обеспечить управление задачами через UI.

**Задачи:**

- [x] 7.1. Создание проекта: Vue 3 + TypeScript + Vite + Pinia + Router + Naive UI + axios.
- [x] 7.2. Чистка шаблонного мусора, настройка `.env.development`, README.
- [x] 7.3. Документация: `docs/FRONTEND.md`, ADR-028/029/030.
- [x] 7.4. Каркас: типы (`types/task.ts`), Pinia-стор (`stores/tasks.ts`), сервис (`services/api/tasksService.ts`).
- [x] 7.5. UI: `TasksPage.vue` (список), `TaskFormModal.vue` (создание/редактирование), кнопки переходов статусов, подтверждение удаления.
- [x] 7.6. Обработка ошибок: маппинг `ProblemDetails` → уведомления Naive UI.
- [x] 7.7. Проверка: запуск backend + frontend, ручной прогон CRUD и переходов статусов.

**Критерий завершения:** Все сценарии CRUD и переходов статусов работают через UI, ошибки отображаются корректно.

## Правила ведения ROADMAP

- Обновлять при завершении этапа.
- Отмечать задачи `- [x]` сразу по завершении.
- Не удалять выполненные пункты — они часть истории.
- При изменении scope — согласовать с заказчиком и обновить `docs/SPEC.md`, `docs/DECISIONS.md`.
