# Task Tracker — Frontend

Frontend приложения Task Tracker на Vue 3 + TypeScript + Vite + Pinia + Naive UI.

## Запуск

1. Установить зависимости: `npm install`
2. Запустить: `npm run dev`
3. Открыть: http://localhost:5173

Backend должен быть запущен на `http://localhost:5029` (см. `.env.development`).

## Структура

- `src/components/layout/` — каркас страницы (Header, Footer, MainLayout)
- `src/views/` — страницы
- `src/router/` — маршруты
- `src/stores/` — Pinia-сторы
- `src/services/api/` — HTTP-клиент
- `src/types/` — типы
- `src/utils/`, `src/composables/`, `src/constants/` — задел на будущее

## Сборка и проверка

- `npm run build` — production-сборка
- `npm run type-check` — проверка типов
- `npm run lint` — линтер
