using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;

namespace UJX
{

    public partial class UJX
    {
        //===========================================
        [RequiresPermissions("@css/root")]
        [ConsoleCommand("css_ujx_reload")]
        public void OnjumpxsCommand(CCSPlayerController? player, CommandInfo info)
        {
            if (player != null)
            {
                player.PrintToChat($" \x07 {UGlobal} " + $" \x10 ===================== ");
                player.PrintToChat($" \x07 {UGlobal} " + Localizer["main.configreload"]);
                player.PrintToChat($" \x07 {UGlobal} " + $" \x0f UJX - \x03 {ModuleVersion}");
                player.PrintToChat($" \x07 {UGlobal} " + $" \x10 ===================== ");
            }
            
            OnConfigParsed(Config);
        }
        //===========================================
        [ConsoleCommand("css_hud")]
        public void OnSpeedsCommand(CCSPlayerController? player, CommandInfo info)
        {
            if (player == null) return;
            var client = player.Index;
            UserSettings[client]!.ShowHUD = !UserSettings[client]!.ShowHUD;
            player.PrintToChat(UserSettings[client]!.ShowHUD ? $"\x10{UGlobal} - \x01HUD: \x06On" : $"\x10{UGlobal} - \x01HUD: \x02Off");
        }
        //===========================================
        [ConsoleCommand("css_scout")]
        public void OnScoutCommand(CCSPlayerController player, CommandInfo info)
        {
            if(_tries[player.SteamID] <= 0)
            {
                player.PrintToChat($" \x07 {UGlobal} - \x01" + Localizer["main.noneScout"]);
            } else {
            _tries[player.SteamID]--;
            AddTimer(5.0f, () => GiveWeaponWithAmmo(player,"weapon_ssg08"));
            player.GiveNamedItem("weapon_healthshot");
            player.PrintToChat($" \x07 {UGlobal} - \x01" + Localizer["main.giveScout"]);
             return;
            }
        }
        //===========================================
        [ConsoleCommand("css_respawn", "Respawn player")]
        public void OnRespawnCommand(CCSPlayerController player, CommandInfo info)
        {
            if(_t1ries[player.SteamID] <= 0)
            {
                player.PrintToChat($" \x07 {UGlobal} - \x01" + Localizer["main.noneresp"]);
            } else {
            _t1ries[player.SteamID]--;
            TimeToRespawn(player);
            player.PrintToChat($" \x07 {UGlobal} - \x01" + Localizer["main.giveresp"]);
             return;
            }
        }
    }
}