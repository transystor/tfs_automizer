# tfs_automizer

Локальная .NET-утилита для упрощения работы с TFS / Azure DevOps Server:
- просмотр своих задач,
- чтение истории списаний времени,
- быстрые операции по time tracking через внутренний `tsapi`.

## Текущий статус

Сейчас это стартовый PoC-каркас.

Цель ближайшего этапа:
1. подключиться к TFS / Azure DevOps Server;
2. проверить чтение задач;
3. проверить чтение истории списаний через `tsapi/WorkItemFormTab/...`;
4. подготовить безопасный путь для тестовой write-операции.

## Предполагаемая архитектура

- `TfsAutomizer.Web` — локальное web-приложение / dashboard.
- дальше можно выделить:
  - клиент для стандартного TFS API,
  - клиент для внутреннего `tsapi`,
  - модели задач и time-tracking записей.

## Безопасность

Это утилита для собственной рабочей учётки и разрешённого внутреннего API.
Не предполагаются массовые автоизменения без явного действия пользователя.

## Что уже добавлено

- `appsettings.example.json` с заготовкой секции `Tfs`;
- `TfsOptions` для настроек подключения;
- `TsApiClient` для read-only вызовов внутреннего `tsapi`;
- модели `TimeSheetEntryDto` и `OperatorTimeSummaryDto`;
- PoC endpoints:
  - `GET /health`
  - `GET /poc/notes`
  - `GET /poc/tsapi/entries/{workItemId}`
  - `GET /poc/tsapi/operators/{workItemId}`

## Следующие шаги

- заполнить локальный конфиг подключения к TFS;
- проверить аутентификацию до `tsapi` из .NET приложения;
- добавить чтение списка work items через стандартный TFS API;
- подготовить безопасный write PoC для `CUD_WI_TimeSheetByAD`.
