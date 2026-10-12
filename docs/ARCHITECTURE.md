# ARCHITECTURE — слои, зависимости, ответственность

## 1. Назначение документа

ARCHITECTURE описывает, как устроен код: слои, зависимости, размещение компонентов, DI, потоки выполнения и обработку ошибок.
Что делает система — в `docs/SPEC.md`; доменные правила — в `docs/DOMAIN.md`; здесь их нет.

## 2. Слои и зависимости

```text
                ┌───────────────┐
                │      API      │
                └──┬─────────┬─┬┘
                   │         │ │
        ┌──────────┘         │ └───────────┐
        ▼                    │             ▼
┌───────────────┐            │  ┌────────────────────┐
│  Application  │◄───────────┼──│  Infrastructure    │
└──┬─────┬──────┘            │  └────────┬───────────┘
   │     │                   │             │
   │     └──────────┬────────┼─────────────┘
   │  ┌─────────────┘        │
   ▼  ▼                      ▼
┌─────────┐   ┌──────────┐
│ Domain  │   │  Common  │
└─────────┘   └──────────┘
```

Стрелки на диаграмме упрощены: показаны не все связи, чтобы не перегружать схему. Полный список зависимостей — в таблице ниже.

| Слой | Зависит от | Проект |
|---|---|---|
| Domain | ни от кого | `PS.TaskTracker.Domain` |
| Common | ни от кого | `PS.TaskTracker.Common` |
| Application | Domain, Common | `PS.TaskTracker.Application` |
| Infrastructure | Application, Domain, Common | `PS.TaskTracker.Infrastructure` |
| API | Application, Infrastructure, Domain, Common | `PS.TaskTracker.API` |

Направление зависимостей — внутрь: домен не знает ни о ком, инфраструктура знает о домене и приложении, API — о всех. Обратных ссылок нет: Application не ссылается на Infrastructure, Domain — ни на кого.

## 3. Domain

В `PS.TaskTracker.Domain` лежит:

- `TaskItem`, `TaskItemStatus` — сущность и enum статусов;
- `DomainException`, `DomainValidationException`, `DomainConflictException` — исключения домена, в подпапке `Exceptions/`, namespace `PS.TaskTracker.Domain.Exceptions`.

Не лежит (и не должно появляться): EF Core, DbContext, атрибуты, репозитории, `DateTime.UtcNow`, `ILogger`, `JsonConverter`, DTO, что-либо из ASP.NET Core.

Детали модели, инварианты и переходы — в `docs/DOMAIN.md`.

## 4. Common

В MVP проект `PS.TaskTracker.Common` пуст. Задел на будущее: общие утилиты, базовые типы логирования, абстракции, разделяемые между слоями.

Сейчас не используется ни одним слоем. Пустой проект сохранён, чтобы не ломать структуру solution при появлении общего кода.

## 5. Application

В `PS.TaskTracker.Application` лежит:

- `ITaskRepository`, `IUnitOfWork` — интерфейсы инфраструктуры (здесь только абстракции, реализации — в Infrastructure);
- Request-модели (`*Command` / `*Query`) — реализуют `IRequest<TResponse>` (MediatR);
- Handlers — по одному на use case, реализуют `IRequestHandler<TRequest, TResponse>` (MediatR): `CreateTaskHandler`, `GetTaskHandler`, `GetTasksHandler`, `UpdateTaskHandler`, `DeleteTaskHandler`, `StartTaskHandler`, `CompleteTaskHandler`, `ReopenTaskHandler`, `ReturnTaskToTodoHandler`;
- `ValidationBehavior<TRequest, TResponse>` — pipeline behavior MediatR для валидации;
- `TaskDto` — прикладная DTO, возвращается handlers наружу;
- FluentValidation-валидаторы — рядом с handlers;
- `NotFoundException`, `ValidationException` — исключения прикладного уровня.

Handlers регистрируются автоматически через `AddMediatR(cfg => cfg.RegisterServicesFromAssembly(...))`.

Для сценариев без результата (например, `DeleteTask`) используется `MediatR.Unit`.

### Вызов из контроллера

Через `IMediator` — контроллер отправляет команды/запросы в pipeline:

```csharp
public TasksController(IMediator mediator)
```

Вызов: `await mediator.Send(new CreateTaskCommand(...), cancellationToken)`.

### Валидация

FluentValidation-валидаторы живут в Application рядом с handler'ом. Валидаторы вызываются автоматически через `ValidationBehavior` в pipeline MediatR. Handler'ы о валидации не знают. При нарушении бросается `ValidationException` (Application-уровень, свой тип — не путать с `DomainValidationException` из Domain). Middleware маппит её в `400` + ProblemDetails + `errors`.

## 6. Infrastructure

В `PS.TaskTracker.Infrastructure` лежит:

- `AppDbContext` — контекст EF Core;
- Конфигурации сущностей — отдельные классы `IEntityTypeConfiguration<T>` (например, `TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>`);
- `TaskRepository : ITaskRepository` — реализация репозитория;
- `UnitOfWork : IUnitOfWork` — реализация UoW;
- Миграции EF Core;
- `AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>` — для генерации миграций из CLI, строка подключения захардкожена только для design-time;
- `DependencyInjection.AddInfrastructure(...)` — точка регистрации.

Репозиторий возвращает домен (`TaskItem`), без `SaveChanges` — сохранение делает UoW. Сценарии чтения (`GetByIdReadOnlyAsync`, `GetAllAsync`) идут через `AsNoTracking()`; сценарии изменения (`GetByIdAsync`) — с tracking (по умолчанию EF Core).

### Регистрация

`AddInfrastructure(connectionString)` регистрирует DbContext, репозиторий, UoW. Вызывается из `Program.cs` вместе с `AddApplication()`.

## 7. API

В `PS.TaskTracker.API` лежит:

- Controllers (`TasksController`);
- HTTP-DTO: `CreateTaskRequest`, `UpdateTaskRequest`, `TaskResponse`;
- `ExceptionHandlingMiddleware` — ловит исключения, возвращает ProblemDetails (RFC 7807);
- Вызов `AddApplication()` и `AddInfrastructure()` из `Program.cs` (реализации методов живут в Application и Infrastructure соответственно, см. §6 и §11);
- Настройка JSON: глобальный `JsonStringEnumConverter`, чтобы `TaskItemStatus` сериализовался как `"Todo"`, `"InProgress"`, `"Done"`;
- Swagger/OpenAPI.

Контроллер не содержит бизнес-логики: принимает HTTP-запрос, маппит в Application-модель, вызывает handler, маппит ответ обратно.

## 8. Поток выполнения сценария

### Create (создание задачи)

```text
HTTP POST /api/tasks
  → TasksController.Create
      → маппинг CreateTaskRequest (HTTP) → CreateTaskCommand (Application)
      → IMediator.Send(CreateTaskCommand)
          → ValidationBehavior (FluentValidation; нарушение → ValidationException → 400)
          → CreateTaskHandler
              → TaskItem.Create(title, description, utcNow)   [Domain]
                  (нарушение инвариантов → DomainValidationException → 400)
              → ITaskRepository.AddAsync(task)
              → IUnitOfWork.SaveChangesAsync()
              → маппинг TaskItem → TaskDto
      ← TaskDto
  ← маппинг TaskDto → TaskResponse (HTTP)
HTTP 201 Created + Location + TaskResponse
```

### Start (переход статуса)

```text
HTTP POST /api/tasks/{id}/start
  → TasksController.Start
      → маппинг: id из маршрута → StartTaskCommand (Application)
      → IMediator.Send(StartTaskCommand)
          → ValidationBehavior (валидаторов для команды нет → пропуск)
          → StartTaskHandler
              → ITaskRepository.GetByIdAsync(id)
                  (не найдено → NotFoundException → 404)
              → task.Start(utcNow)                              [Domain]
                  (недопустимый переход → DomainConflictException → 409)
              → IUnitOfWork.SaveChangesAsync()
              → маппинг TaskItem → TaskDto
      ← TaskDto
  ← маппинг TaskDto → TaskResponse (HTTP)
HTTP 200 OK + TaskResponse
```

## 9. Обработка ошибок

| Исключение | Слой | HTTP |
|---|---|---|
| `ValidationException` | Application | `400` + `errors` |
| `NotFoundException` | Application | `404` |
| `DomainValidationException` | Domain | `400` |
| `DomainConflictException` | Domain | `409` |
| всё остальное | — | `500` |

`ExceptionHandlingMiddleware` живёт в API. Ловит исключения, возвращает ProblemDetails (RFC 7807): `type`, `title`, `status`, `detail`, `instance`. Для ошибок валидации дополнительно `errors` — словарь «поле → список сообщений». Формат тел — в `docs/SPEC.md`, раздел 8.

## 10. Маппинг

Границы маппинга — три уровня:

```text
CreateTaskRequest (HTTP)  →  CreateTaskCommand (Application)  →  TaskItem (Domain)
TaskItem (Domain)         →  TaskDto (Application)            →  TaskResponse (HTTP)
```

- Handler принимает request-модель прикладного уровня (не API-DTO) и возвращает `TaskDto` (прикладную, не домен).
- API-слой маппит HTTP-DTO ↔ Application-DTO.
- Маппинг — вручную, extension-методами. Без AutoMapper.

Пример `TaskItem → TaskDto → TaskResponse`:

```text
TaskItem (Domain)                TaskDto (Application)         TaskResponse (HTTP)
  Id: Guid                  →     Id: Guid                 →     id: Guid
  Title: string             →     Title: string            →     title: string
  Description: string?      →     Description: string?     →     description: string?
  Status: TaskItemStatus    →     Status: TaskItemStatus   →     status: string
  CreatedAtUtc: DateTime    →     CreatedAtUtc: DateTime   →     createdAtUtc: DateTime
  UpdatedAtUtc: DateTime    →     UpdatedAtUtc: DateTime   →     updatedAtUtc: DateTime
```

TaskDto.Status — TaskItemStatus (enum). Enum → string выполняется на уровне API при маппинге TaskDto → TaskResponse; глобальный JsonStringEnumConverter (ADR-011) сериализует enum как строку. Application не знает про строковое представление статуса — это деталь HTTP-контракта.

## 11. Правила именования

| Что | Где живёт | Как называется | Примеры |
|---|---|---|---|
| HTTP-DTO запроса | API | `*Request` | `CreateTaskRequest`, `UpdateTaskRequest` |
| HTTP-DTO ответа | API | `*Response` | `TaskResponse` |
| Request-модель handler'а | Application | `*Command` / `*Query` | `CreateTaskCommand`, `GetTaskQuery` |
| Прикладная DTO | Application | `*Dto` | `TaskDto` |
| Handler | Application | `*Handler` | `CreateTaskHandler`, `StartTaskHandler` |
| Валидатор | Application | `*Validator` | `CreateTaskValidator` |
| Репозиторий | Application (интерфейс), Infrastructure (реализация) | `I*Repository`, `*Repository` | `ITaskRepository`, `TaskRepository` |
| UoW | Application (интерфейс), Infrastructure (реализация) | `IUnitOfWork`, `UnitOfWork` | — |
| Исключения домена | Domain (папка `Exceptions/`, namespace `PS.TaskTracker.Domain.Exceptions`) | `Domain*Exception` | `DomainValidationException` |
| Исключения приложения | Application | `ValidationException`, `NotFoundException` | — |
| Конфигурация EF | Infrastructure | `*Configuration` | `TaskItemConfiguration` |
| Точка входа DI | Application, Infrastructure | `DependencyInjection.Add*` | `AddApplication`, `AddInfrastructure` |

Правило именования request-моделей:

- `*Command` — сценарии, изменяющие состояние: `CreateTaskCommand`, `UpdateTaskCommand`, `DeleteTaskCommand`, `StartTaskCommand`, `CompleteTaskCommand`, `ReopenTaskCommand`, `ReturnTaskToTodoCommand`.
- `*Query` — сценарии только для чтения: `GetTaskQuery` (одна задача), `GetTasksQuery` (список).
- Суффикс `*Request` на уровне Application не используется — он зарезервирован за HTTP-DTO в API.

## 12. Что осознанно не используется

| Что | Почему |
|---|---|
| AutoMapper | Маппинг трёх DTO-уровней мал и прозрачен; вручную понятнее и без магии. |
| CQRS-фреймворки | Разделение command/query — соглашение об именовании, не отдельный фреймворк. |
| Generic Repository | Один `ITaskRepository` под конкретную сущность; generic-интерфейс добавит мёртвых методов. |
| Микросервисы | Монолит на один деплой; микросервисы — избыточно для учебного MVP. |
| Vertical Slice | Слои по Clean Architecture; vertical slice — другой способ организации, конфликтует с выбранным. |
| Minimal API | Controllers проще для группировки эндпоинтов и Swagger-интеграции в данном MVP. |
| Serilog | Стандартного `ILogger<T>` достаточно; Serilog — отдельное решение для логирования. |

## 13. Открытые вопросы

Вне MVP, осознанно отложено:

- Интеграционные тесты (сейчас только юнит-тесты Domain и Application).
- Кэширование.
- Пагинация, фильтры, сортировка списка задач.
- Аутентификация, пользователи, роли.
