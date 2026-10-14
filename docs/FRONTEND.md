# FRONTEND — стек, структура, архитектура

## 1. Назначение

Frontend приложения Task Tracker: UI для управления задачами и переходами статусов.
Отдельное приложение в `src/Frontend/PS.TaskTracker/`, общается с backend по HTTP.

## 2. Стек

| Технология | Версия | Роль |
|---|---|---|
| Vue | 3.5 | UI-фреймворк, Composition API |
| TypeScript | 6 | Типизация |
| Vite | 8 | Сборка и dev-сервер |
| Vue Router | 5 | Маршрутизация |
| Pinia | 4 | Сторы (состояние) |
| Naive UI | 2.45 | Компоненты UI |
| axios | 1.20 | HTTP-клиент |
| ESLint + oxlint + Prettier | — | Линтинг и форматирование |
| Node.js | 24 (LTS) | Рантайм инструментов |

## 3. Структура репозитория

```text
src/Frontend/PS.TaskTracker/
├── public/
├── src/
│   ├── assets/         — стили, иконки
│   ├── components/     — Vue-компоненты
│   │   └── layout/     — каркас страницы
│   ├── composables/    — composition-функции (задел)
│   ├── constants/      — константы (задел)
│   ├── router/         — маршруты
│   ├── services/api/   — HTTP-клиент
│   ├── stores/         — Pinia-сторы
│   ├── types/          — TypeScript-типы
│   ├── utils/          — утилиты (задел)
│   ├── views/          — страницы
│   ├── App.vue
│   └── main.ts
├── .env.development
├── index.html
├── package.json
├── tsconfig.*.json
└── vite.config.ts
```

## 4. Слои frontend

- **Views** — страницы, привязанные к маршрутам.
- **Components** — переиспользуемые компоненты; `layout/` — каркас страницы.
- **Stores (Pinia)** — состояние, работа с API через services.
- **Services** — HTTP-клиент и сервисы, маппинг API ↔ типы.
- **Types** — TypeScript-типы, зеркало DTO из backend.
- **Composables** — переиспользуемая логика (задел).

## 5. Взаимодействие с API

- Базовый URL — `VITE_API_URL` из `.env.development`.
- HTTP-клиент — axios (`services/api/client.ts`).
- Формат ошибок — ProblemDetails (RFC 7807), см. `docs/SPEC.md` §8.
- Аутентификации нет (в MVP).

## 6. Соглашения

- Composition API + `<script setup>`.
- TypeScript strict.
- Именование компонентов — PascalCase, файлов — `PascalCase.vue`.
- Один компонент — один файл.
- Стили — `<style scoped>`, глобальные — в `assets/styles/`.
- Язык интерфейса — русский, идентификаторы — английский.

## 7. Что вне MVP

- Аутентификация.
- Многоязычность.
- Тёмная тема.
- Оптимизация бандла (naive-ui подключён глобально, чанк 1.4 MB).
- Тесты (Vitest, Playwright).
- Фильтрация и пагинация списка задач.

## 8. Навигация

- API-контракт и ошибки — [docs/SPEC.md](SPEC.md).
- Архитектурные решения — [docs/DECISIONS.md](DECISIONS.md) (ADR-028, ADR-029, ADR-030).
