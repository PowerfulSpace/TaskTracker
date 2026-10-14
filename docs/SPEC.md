# SPEC — требования MVP и API-контракты

## 1. Назначение документа

SPEC описывает, что делает система на уровне HTTP API: модель, статусы, endpoints, DTO и формат ошибок.
Доменные правила и инварианты — в `docs/DOMAIN.md`; слои, зависимости и реализация — в `docs/ARCHITECTURE.md`; здесь их нет.

## 2. Границы MVP

Входит:

- CRUD задач без аутентификации.
- Управление статусами через отдельные endpoints.
- ProblemDetails для ошибок.

Не входит:

- Аутентификация, пользователи, роли, владельцы задач.
- Комментарии, теги, назначение на других.
- Уведомления, напоминания.
- Пагинация, фильтры, сортировка.
- Интеграционные тесты, Docker, CI/CD.

## 3. Модель задачи (API-уровень)

| Поле | Тип | Обязательность | Описание |
|---|---|---|---|
| `id` | `Guid` | — | Идентификатор, назначается сервером |
| `title` | `string` | обязательно | Заголовок задачи |
| `description` | `string \| null` | необязательно | Описание, может отсутствовать |
| `status` | `string` | — | Одно из: `Todo`, `InProgress`, `Done` |
| `createdAtUtc` | `DateTime` | — | Момент создания, UTC |
| `updatedAtUtc` | `DateTime` | — | Момент последнего изменения, UTC |

Поля `ownerId` в MVP нет: аутентификации и понятия владельца задачи в первой версии не предусмотрено.

Лимиты длины `title` (≤ 200) и `description` (≤ 2000) — доменные инварианты, описаны в `docs/DOMAIN.md`. На API-уровне нарушение лимита возвращает `400` в общем формате ошибок валидации (§8).

## 4. Статусы и переходы

| Текущий | Операция | Результат |
|---|---|---|
| `Todo` | `Start()` | `InProgress` |
| `InProgress` | `Complete()` | `Done` |
| `Done` | `Reopen()` | `InProgress` |
| `InProgress` | `ReturnToTodo()` | `Todo` |
| `Done` | повторный `Complete()` | без изменений (идемпотентно) |

Начальный статус при создании — `Todo`.

`Complete()` на задаче в статусе `Done` идемпотентен: возвращает `200` с текущим состоянием, ничего не меняя и не создавая ошибку. Любой другой недопустимый переход — `409`. Прямой переход `Todo → Done` запрещён: завершить задачу можно только через `InProgress`.

Визуализация переходов — в `docs/DOMAIN.md` §5.

## 5. CRUD endpoints

| Метод | Путь | Успех | Ошибки |
|---|---|---|---|
| `POST` | `/api/tasks` | `201` + `Location` + `TaskResponse` | `400` |
| `GET` | `/api/tasks` | `200` + список `TaskResponse` | — |
| `GET` | `/api/tasks/{id}` | `200` + `TaskResponse` | `404` |
| `PUT` | `/api/tasks/{id}` | `200` + `TaskResponse` | `400`, `404` |
| `DELETE` | `/api/tasks/{id}` | `204` | `404` |

`PUT` полностью обновляет `title` и `description`. Поле `status` через `PUT` не меняется — статус меняется только через endpoints переходов (раздел 6).

### POST /api/tasks — создать задачу

Запрос:

```json
{
  "title": "Написать спецификацию API",
  "description": "Зафиксировать endpoints и формат ошибок"
}
```

Ответ `201 Created`, заголовок `Location: /api/tasks/{id}`:

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "title": "Написать спецификацию API",
  "description": "Зафиксировать endpoints и формат ошибок",
  "status": "Todo",
  "createdAtUtc": "2026-10-12T10:00:00Z",
  "updatedAtUtc": "2026-10-12T10:00:00Z"
}
```

`description` можно опустить в запросе — тогда в ответе он будет `null`.

### GET /api/tasks — список задач

Ответ `200 OK`:

```json
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "title": "Написать спецификацию API",
    "description": "Зафиксировать endpoints и формат ошибок",
    "status": "InProgress",
    "createdAtUtc": "2026-10-12T10:00:00Z",
    "updatedAtUtc": "2026-10-12T10:05:00Z"
  }
]
```

Пустой список — `200 OK` с пустым массивом.

### GET /api/tasks/{id} — получить задачу

Ответ `200 OK` — тело `TaskResponse` как в примерах выше. Несуществующий `id` — `404`.

### PUT /api/tasks/{id} — обновить задачу

Запрос:

```json
{
  "title": "Написать спецификацию API (обновлено)",
  "description": null
}
```

Ответ `200 OK`:

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "title": "Написать спецификацию API (обновлено)",
  "description": null,
  "status": "InProgress",
  "createdAtUtc": "2026-10-12T10:00:00Z",
  "updatedAtUtc": "2026-10-12T10:10:00Z"
}
```

`status` в ответе не изменился — `PUT` его не трогает. Несуществующий `id` — `404`, невалидное тело — `400`.

### DELETE /api/tasks/{id} — удалить задачу

Успех — `204 No Content` без тела. Несуществующий `id` — `404`.

## 6. Endpoints переходов статусов

| Метод | Путь | Действие | Успех | Ошибки |
|---|---|---|---|---|
| `POST` | `/api/tasks/{id}/start` | `Todo → InProgress` | `200` + `TaskResponse` | `404`, `409` |
| `POST` | `/api/tasks/{id}/complete` | `InProgress → Done` (идемпотентно на `Done`) | `200` + `TaskResponse` | `404`, `409` |
| `POST` | `/api/tasks/{id}/reopen` | `Done → InProgress` | `200` + `TaskResponse` | `404`, `409` |
| `POST` | `/api/tasks/{id}/reset` | `InProgress → Todo` | `200` + `TaskResponse` | `404`, `409` |

Тело запроса у этих endpoints отсутствует.

### POST /api/tasks/{id}/start

Запрос — без тела:

```
POST /api/tasks/3fa85f64-5717-4562-b3fc-2c963f66afa6/start
```

Ответ `200 OK`:

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "title": "Написать спецификацию API",
  "description": "Зафиксировать endpoints и формат ошибок",
  "status": "InProgress",
  "createdAtUtc": "2026-10-12T10:00:00Z",
  "updatedAtUtc": "2026-10-12T10:05:00Z"
}
```

Задача в статусе не `Todo` — `409`. Задача не найдена — `404`.

### POST /api/tasks/{id}/complete

Запрос — без тела:

```
POST /api/tasks/3fa85f64-5717-4562-b3fc-2c963f66afa6/complete
```

Ответ `200 OK` (задача была в `InProgress`):

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "title": "Написать спецификацию API",
  "description": "Зафиксировать endpoints и формат ошибок",
  "status": "Done",
  "createdAtUtc": "2026-10-12T10:00:00Z",
  "updatedAtUtc": "2026-10-12T10:15:00Z"
}
```

Задача уже в `Done` — повторный запрос вернёт `200 OK` с тем же телом, `updatedAtUtc` не изменится. Задача в `Todo` — `409`.

### POST /api/tasks/{id}/reopen

Запрос — без тела:

```
POST /api/tasks/3fa85f64-5717-4562-b3fc-2c963f66afa6/reopen
```

Ответ `200 OK`:

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "title": "Написать спецификацию API",
  "description": "Зафиксировать endpoints и формат ошибок",
  "status": "InProgress",
  "createdAtUtc": "2026-10-12T10:00:00Z",
  "updatedAtUtc": "2026-10-12T10:20:00Z"
}
```

Задача в статусе не `Done` — `409`. Задача не найдена — `404`.

### POST /api/tasks/{id}/reset

Запрос — без тела:

```
POST /api/tasks/3fa85f64-5717-4562-b3fc-2c963f66afa6/reset
```

Ответ `200 OK`:

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "title": "Написать спецификацию API",
  "description": "Зафиксировать endpoints и формат ошибок",
  "status": "Todo",
  "createdAtUtc": "2026-10-12T10:00:00Z",
  "updatedAtUtc": "2026-10-12T10:25:00Z"
}
```

Задача в статусе не `InProgress` — `409`. Задача не найдена — `404`.

## 7. DTO

```
CreateTaskRequest
    Title: string          // обязательно
    Description: string?   // опционально

UpdateTaskRequest
    Title: string          // обязательно
    Description: string?   // опционально

TaskResponse
    Id: Guid               // назначается сервером
    Title: string
    Description: string?
    Status: string         // "Todo" | "InProgress" | "Done"
    CreatedAtUtc: DateTime // UTC
    UpdatedAtUtc: DateTime // UTC
```

## 8. Формат ошибок

Ошибки возвращаются в формате ProblemDetails (RFC 7807): `type`, `title`, `status`, `detail`, `instance`. Для ошибок валидации дополнительно поле `errors` — словарь «поле → список сообщений».

### 400 — ошибка валидации

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "detail": "The request body is invalid.",
  "instance": "/api/tasks",
  "errors": {
    "Title": ["Title is required."]
  }
}
```

### 404 — задача не найдена

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.5",
  "title": "Resource not found.",
  "status": 404,
  "detail": "Task with id 3fa85f64-5717-4562-b3fc-2c963f66afa6 was not found.",
  "instance": "/api/tasks/3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

### 409 — недопустимый переход статуса

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.10",
  "title": "Status transition conflict.",
  "status": 409,
  "detail": "Cannot start task in status Done.",
  "instance": "/api/tasks/3fa85f64-5717-4562-b3fc-2c963f66afa6/start"
}
```

### 500 — необработанная ошибка

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.6.1",
  "title": "An unexpected error occurred.",
  "status": 500,
  "detail": "The server failed to handle the request.",
  "instance": "/api/tasks"
}
```

## 9. HTTP-статусы

| Код | Когда используется |
|---|---|
| `200` | Успех с телом ответа (GET, PUT, переходы статусов) |
| `201` | Задача создана (`POST /api/tasks`) |
| `204` | Успех без тела (`DELETE`) |
| `400` | Ошибка валидации входных данных |
| `404` | Задача не найдена |
| `409` | Недопустимый переход статуса |
| `500` | Необработанная ошибка |

## 10. Примеры сценариев

### Сценарий 1. Создание задачи

1. Запрос:

```
POST /api/tasks
{ "title": "Написать спецификацию API" }
```

2. Ответ: `201 Created`, `Location: /api/tasks/3fa85f64-...`, тело `TaskResponse` со `status: "Todo"`.

### Сценарий 2. Старт и завершение

1. Запрос: `POST /api/tasks/3fa85f64-.../start` — без тела.
2. Ответ: `200 OK`, `status: "InProgress"`.
3. Запрос: `POST /api/tasks/3fa85f64-.../complete` — без тела.
4. Ответ: `200 OK`, `status: "Done"`.

### Сценарий 3. Повторное завершение (идемпотентность)

1. Задача уже в `Done`. Запрос: `POST /api/tasks/3fa85f64-.../complete`.
2. Ответ: `200 OK`, `status: "Done"`, `updatedAtUtc` прежний. Ошибки нет.

### Сценарий 4. Недопустимый переход

1. Задача в `Todo`. Запрос: `POST /api/tasks/3fa85f64-.../complete`.
2. Ответ: `409 Conflict` в формате ProblemDetails с detail «Cannot complete task in status Todo.»

### Сценарий 5. Обновление через PUT

1. Запрос:

```
PUT /api/tasks/3fa85f64-...
{ "title": "Обновлённый заголовок", "description": null }
```

2. Ответ: `200 OK`, `title` обновлён, `description: null`, `status` остался прежним, `updatedAtUtc` обновился.

## 11. Открытые вопросы

Осознанно не решено в MVP и перенесено на последующие этапы:

- Пагинация, фильтры, сортировка списка задач.
- Аутентификация, пользователи, роли, `ownerId`.
- Оптимистичные блокировки (конкурентные изменения одной задачи).
