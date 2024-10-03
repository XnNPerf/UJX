<h1 align="center">Ultimate Jumps X <img src="https://github.com/blackcater/blackcater/raw/main/images/Hi.gif" height="32"/></h1>
<h3 align="center">Plugin server CS2 - <a href="https://github.com/roflmuffin/CounterStrikeSharp" target="_blank">CSSharp</a></h3>


[RU] Простой и динамичный плагин для CS2, Теперь не нужно использовать разные переменные на сервере ведь все сделает за вас данный плагин! 
<br>
[EN] Simple and dynamic plugin for CS2. Now you don’t need to use different variables on the server because this plugin will do everything for you!
<br>
<br>

<h2>!Commands</h2>

```c#
   !hud // включение,выключение игрового интерфейса
   !scout // выдача снаряжения ssg08,healthshoot
   !ujx_reload // перезагрузка конфига плагина (@css/root)
   !respawn // вроде рабоатет а вроде и нет =)
```
<h2>Configuration</h2>

Config - `addons/counterstrikesharp/configs/plugins/UJX`
```json
{
  "GlobalTAG": "[UJX]",			// Глобальный тег для команд и прочего 
  "ParramsBhop": true,			// Автоматическая распрышка (true,False) 
  "Parrams1": 2,			// Дополнительный прыжок (кол-во)
  "Parrams2": 300,			// Высота прыжка (стандарт 300-400) 
  "ScoutActive": 1,			// Модуль scout
  "ScoutAmmo": 0,			// Патроны для scout
  "ScoreBoardActive": true,		// Модуль clantag
  "ScoreAdmin": "[UJX - Admin]",	// clantag - Администратор
  "ScoreVIP": "[UJX - VIP]",		// clantag - vip 
  "ScoreUsers": "[UJX - USER]",		// clantag - остальные игроки
  "HideActive": true,			// Модуль Lowbody
  "RespawnActive": 1,			// Модуль Respawn(50% позже доделаю)  
  "ConfigVersion": 1
}
```
Lang - `addons/counterstrikesharp/plugins/Jumps_X/lang`
```json
{
	"main.noneScout" : "Вы уже получили снаряжение в начале раунда",
	"main.configreload" : "Конфиг плагина был перезагружен! ",
	"main.giveScout" : "Вам выдали снаряжение! Удачного раунда! "
	"main.giveresp" : "Ваc возродили ! Удачного раунда! "
	"main.noneresp" : "Упс лимит исчерпан! "
}
```
![image](https://github.com/XnNPerf/Jumps_X/assets/158213049/73ac2985-7dc4-45db-829d-745fae046e84)

<a href="https://github.com/roflmuffin/CounterStrikeSharp">CounterStrikeSharp</a> Tested v234
