# Промышленное программирование. Проект № 2

## Аннотация

### Цель
Знакомство со способами реализации клиент-серверных проектов.

### Задачи
* Реализация объектно-ориентированной модели данных
* Изучение сериализации данных в XML и JSON
* Изучение одного из способов реализации серверных приложений
  * WebAPI/OpenAPI
  * gRPC
  * SignalR (WebSockets)
* Реализация клиентского приложения с использованием WPF или других технологий
* Изучение паттернов проектирования
* Повторение основ работы с системами контроля версий
* Unit-тестирование



## Задание. Общая часть

**Целевая платформа**: .NET 5

**Целевой UI-фреймворк**: WPF или альтернатива (в случае альтернативы необходимо уведомить преподавателя)

**Хранение данных на сервере**: XML или JSON в зависимости от варианта.

**Обязательно**:
* Реализация как серверной, так и клиентской части системы
* Реализация серверной части на .NET 5
* Реализация серверной части на указанной в варианте серверной технологии
* Реализация unit-тестов
* Создание минимальной документации к проекту: страница на GitHub с информацией о задании, скриншоты приложения и прочая информация

**Факультативно**:
* Автоматизация тестирования на уровне репозитория через [GitHub Actions](https://docs.github.com/en/actions/learn-github-actions/understanding-github-actions)
* Интеграционное тестирование
* Реализация серверного хранилища в виде базы данных

<details>
  <summary><strong>Вариант 3: Чат</strong></summary>
  <p></p>

  Вы разрабатываете чат для обмена текстовыми сообщениями между пользователями.

  Минимальный функционал:
  - хранение информации о пользователях на сервере
  - хранение истории сообщений на клиентах
  - поддержка групповых чатов с сохранением истории и состава группы на сервере

  Варианты серверных технологий:  
  - gRPC
  - SignalR
</details>

*При запуске приложения можно как создать комнату, так и присоединиться к готовой. После создания необходимо будет перезапустить приложение, и затем зайти в нужную комнату, нажав на Join. 
*Для отключения от чата нажать на Disconnect.

Диалоговое окно что войти/создать комнату



![диалоговое окно](https://user-images.githubusercontent.com/73229151/172196584-18c975a4-0ad5-406e-88fb-cc5b1acf89fc.png)
![окно](https://user-images.githubusercontent.com/73229151/172197370-b7a9af96-ef42-420f-8a39-89afd8d47a44.png)



Основное окно


