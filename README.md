# Telegram Scheduler Bot

Telegram Scheduler Bot is a bot developed as a learning project as part of a C# course.  
This bot can schedule are events on specific date with some messages. 
The date of the event is selected interactively, to start it, you need to enter the command `/plan`.
After selecting a date, you must enter an event message with a maximum length of 256 characters.  
An event notification (message from bot) will be sent no more than 60 minutes before the start of the event.
After receiving an event, the bot will stop sending a notification about the same event until user react on event.  
The user can react to the event in the following ways: `Take` (i.e. notification about this event will no be sent), or shooze a new reminder time.

## How to 
* Before run application make sure that you save bot auth token gotten by [BotFather](https://core.telegram.org/bots) by path `TelegramBotServer/appsettings.json`  
Also you need replace `WebHookURL` property in `appsettings.json` by you webhook. For local run you may use [ngrok](https://ngrok.com/).  
* If you use ngrok for this purpose you can download ngrok application and rename them to `ngrok.exe` and place it to `Scripts` folder on root of repository. 
After that you can run PowerShell script which will replace `WebHookURL` propetry in `appsettings.json` automatically and run ngrok.  
* For run script you can use next command `powershell -executionpolicy RemoteSigned -file run_ngrok.ps1`. Script must be runned on `Scripts` folder on root of repository.


## Used tools
* [.NET Client for Telegram Bot API](https://github.com/TelegramBots/Telegram.Bot)
* PostgeSQL as databse
* Entity Framework as DBMS

## Screenshots
![Image 1](https://github.com/prafdin/dotnet-2/raw/images/1.jpg)
![Image 2](https://github.com/prafdin/dotnet-2/raw/images/2.jpg)
![Image 3](https://github.com/prafdin/dotnet-2/raw/images/3.jpg)
![Image 4](https://github.com/prafdin/dotnet-2/raw/images/4.jpg)
![Image 5](https://github.com/prafdin/dotnet-2/raw/images/5.jpg)
![Image 6](https://github.com/prafdin/dotnet-2/raw/images/6.jpg)
