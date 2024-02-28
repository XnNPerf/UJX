<h1 align="center">Ultimate Jumps X <img src="https://github.com/blackcater/blackcater/raw/main/images/Hi.gif" height="32"/></h1>
<h3 align="center">Plugin server CS2 - <a href="https://github.com/roflmuffin/CounterStrikeSharp" target="_blank">CSSharp</a></h3>


[RU] Просто и динамичный плагин для CS2, Теперь не нужно использовать разные переменные на сервере ведь все сделает за вас данный плагин! 
<br>
[EN] Simple and dynamic plugin for CS2. Now you don’t need to use different variables on the server because this plugin will do everything for you!
<br>
<br>

<h2>!Commands</h2>

```c#
   !hud // включение,выключение игрового интерфейса
   !scout // выдача снаряжения ssg08,healthshoot
   !ujx_reload // перезагрузка конфига плагина (@css/root)
```
<h2>Configuration</h2>

Config - `addons/counterstrikesharp/configs/plugins/Jumps_X`
```json
{
  "TagServer": "[UJX]",         //Глобальный тег для команд и прочего 
  "ParramsBhop": true,          // Автоматическая распрышка (true,False) 
  "Parrams1": 2,                // Дополнительный прыжок (кол-во)
  "Parrams2": 300,              // Высота прыжка (стандарт 300-400) 
  "TabActive": true,            // Включение клан-тега игрокам (true,False) 
  "TabUsers": "[UJX - Hero]",   // Клан-Тег название 
  "ScoutActive": 1,             // включение команды (!scout) 
  "HUDMessage": "<br>info<br>message<br>hud<br>",  //Реклама в (!hud)
  "ConfigVersion": 1
}
```
Lang - `addons/counterstrikesharp/plugins/Jumps_X/lang`
```json
{
	"main.noneScout" : "Вы уже получили снаряжение в начале раунда",
	"main.configreload" : "Конфиг плагина был перезагружен! ",
	"main.giveScout" : "Вам выдали снаряжение! Удачного раунда! "
}
```
![image](https://github.com/XnNPerf/Jumps_X/assets/158213049/73ac2985-7dc4-45db-829d-745fae046e84)

<a href="https://github.com/roflmuffin/CounterStrikeSharp">CounterStrikeSharp</a> Tested v163
