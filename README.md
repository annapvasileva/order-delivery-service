# Сервис заказов

## Запуск приложения

### Требования системы

> Docker (version ≥ 28)
> 
> Docker Compose (version ≥ 2)

### Порядок действий

Для запуска приложения достаточно скопировать файл [docker-compose.yml](https://github.com/annapvasileva/order-delivery-service/blob/main/docker-compose.yml) на локальную машину и выполнить следующую команду в директории с файлом:
```bash
docker-compose up -d
```

Либо же можно клонировать репозиторий и запустить docker-compose следующим образом:
```bash
git clone https://github.com/annapvasileva/order-delivery-service.git
cd order-delivery-service
docker-compose up -d
```

Далее запустятся следующие сервисы:

- `order-delivery-service-frontend` – веб-интерфейс
- `order-delivery-service-backend` – бэкенд-сервис с доступом через REST API
- `postgres-order-delivery-service` – база данных PostgreSQL
- `pgadmin` – интерфейс управления БД

После запуска сервисов приложение будет доступно по следующим адресам:

| Сервис   | Адрес        | Изменение порта                                                                                                                       |
|----------|--------------|---------------------------------------------------------------------------------------------------------------------------------------|
| frontend | http://localhost:3001/ | [здесь](https://github.com/annapvasileva/order-delivery-service/blob/7cdcaca7964453d54df4746885b89db6aff11b03/docker-compose.yml#L44) |
| backend  | http://localhost:5002/swagger/index.html | [здесь](https://github.com/annapvasileva/order-delivery-service/blob/7cdcaca7964453d54df4746885b89db6aff11b03/docker-compose.yml#L29) |
| БД       | http://localhost:5010/ | [здесь](https://github.com/annapvasileva/order-delivery-service/blob/7cdcaca7964453d54df4746885b89db6aff11b03/docker-compose.yml#L52) |
