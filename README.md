# Расписание ВКИ бот!

Данный бот используется для получения расписания занятий учебного заведения ВКИ НГУ путём парсинга данных с сайта.

[Docker Hub](https://hub.docker.com/r/hiter67/vki-telegram-bot)
## Установка
Скачиваем Docker image
```
$ docker pull hiter67/vki-telegram-bot
```
Создаём Docker contanier с нужными параметрами
```
$  run -it --name <имя контейнера> -e TELEGRAM_TOKEN=<токен> -e UPDATER_WAIT=1800000 -v <путь>:/app/data hiter67/vki-telegram-bot
```
## Использование

У бота есть 4 кнопки меню, нажатие на которые выполняет подписанные на ней списки. 
После нажатия бот выдаёт, выбранный вами список.
При нажатии на нужный вам документ, бот отправляет его в диалог

## Уведомления

Если расписание обновилось, бот отправит вам уведомление.

## Наша реализация бота

**Бот - [@raspisanie_vki_bot](https://t.me/raspisanie_vki_bot)**

# Авторы

[@Hiter67](https://github.com/Hiter67)
[@NicholasValentain](https://github.com/NicholasValentain)
