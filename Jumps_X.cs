using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using System.Text.RegularExpressions;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API.Modules.Config;
using CounterStrikeSharp.API.Modules.Admin;
using System.Drawing;
using Modularity;
using CounterStrikeSharp.API.Modules.Entities.Constants;

namespace Jumps_X;
public class Jumps_X : BasePlugin, IPluginConfig<PluginConfig>
{
    /*author*/
    public override string ModuleAuthor => "XnN.Prod";
    public override string ModuleName => "Ultimate Jumps X";
    public override string ModuleVersion => "v0.2.0";
    public override string ModuleDescription => "Plugin for BHOP,MG servers.";
    //Setting start
    public static int Setting1 = 2;
    public int Setting2 = 300;
    public int Scout = 1;
    public bool Bhop_Settings = true;
    public bool TAGPLAYER = false;
    public string PluginTag = "[Server]";
    public string Tagsuser = "[User]";
    public string PCenter = "-";
    //Setting end
    private Dictionary<ulong, int> _tries = new();
    private bool isHookEvent;
    private static readonly UserSettings?[] UserSettings = new UserSettings?[120];
    public PluginConfig Config { get; set; }

    public void OnConfigParsed(PluginConfig config)
    {
        config = ConfigManager.Load<PluginConfig>("Jumps_X");

        PluginTag = config.TagServer;
        //setting bhop
        Bhop_Settings = config.ParramsBhop;
        Setting1 = config.Parrams1;
        Setting2 = config.Parrams2;
        //other
        TAGPLAYER = config.TabActive;
        Tagsuser = config.TabUsers;
        Scout = config.ScoutActive;     
        PCenter = config.HUDMessage;

        Console.WriteLine($"{PluginTag} - " + Localizer["main.configreload"]);
        Config = config;
    } 

    public override void Load(bool hotReload)
    {
        Console.WriteLine($"Loading...");
        Console.WriteLine($" ");
        Console.WriteLine($"{ModuleName} [{ModuleVersion}] loaded!!  ");
        RegisterListener<Listeners.OnClientConnected>(((slot) =>
        {
            UserSettings[slot + 1] = new UserSettings { ShowHUD = true, JumpsHUD = 0 };
        }));
        RegisterListener<Listeners.OnClientAuthorized>((slot, id) => UserSettings[slot + 1] = new UserSettings());
        RegisterListener<Listeners.OnClientDisconnectPost>(slot => UserSettings[slot + 1] = null);
        RegisterEventHandler<EventPlayerConnectFull>(OnPlayerConnectFull);
        RegisterEventHandler<EventPlayerSpawn>(OnPlayerSpawn);
        RegisterListener<Listeners.OnTick>(() =>
        {
            foreach (var player in Utilities.GetPlayers()
                         .Where(player => player is { IsValid: true, IsBot: false, PawnIsAlive: true }))
            {
                if (UserSettings[player.Index] == null ||
                    player.TeamNum is not ((int)CsTeam.Terrorist or (int)CsTeam.CounterTerrorist)) continue;
                    OnTick(player);
            }
        });
        RegisterListener<Listeners.OnMapStart>((name =>
        {
            Server.ExecuteCommand($"sv_enablebunnyhopping {Bhop_Settings}");
            Server.ExecuteCommand($"sv_autobunnyhopping {Bhop_Settings}");
            if(isHookEvent) return;
            isHookEvent = true;
            
            RegisterEventHandler<EventPlayerJump>(((@event, info) =>
            {
                var controller = @event.Userid;
                var client = controller.Index;

                if (client == IntPtr.Zero) return HookResult.Continue;
                UserSettings[client]!.JumpsHUD++;

                return HookResult.Continue;
            }));
        }));
        RegisterEventHandler<EventRoundStart>(((@event, info) =>
        {
            var playerEntities = Utilities.FindAllEntitiesByDesignerName<CCSPlayerController>("cs_player_controller");
            foreach (var player in playerEntities)
            {
                var client = player.Index;
                UserSettings[client]!.JumpsHUD = 0;
            }
            _tries.Clear();
            foreach (var player in Utilities.GetPlayers()) _tries.TryAdd(player.SteamID, Scout);
            return HookResult.Continue;
        }));
        RegisterEventHandler<EventPlayerDeath>(((@event, info) =>
        {
            if (@event.Userid.Handle == IntPtr.Zero || @event.Userid.UserId == null) return HookResult.Continue;

            var controller = @event.Userid;
            var client = controller.Index;
            if (client == IntPtr.Zero) return HookResult.Continue;
            UserSettings[client]!.JumpsHUD = 0;

            return HookResult.Continue;
        }));
        RegisterListener<Listeners.OnTick>(() =>
        {
            for (var i = 1; i <= Server.MaxPlayers; ++i)
            {
                var player = new CCSPlayerController(NativeAPI.GetEntityFromIndex(i));

                if (player is { IsValid: true, IsBot: false, PawnIsAlive: true })
                {
                    var buttons = player.Buttons;
                    var client = player.Index;
                    if (client == IntPtr.Zero) return;
                    if (!UserSettings[client]!.ShowHUD) return;

                    if (player.PlayerPawn.Value == null) continue;
                    player.PrintToCenterHtml($"{PCenter}");
                    player.PrintToCenter($"Speed:{Math.Round(player.PlayerPawn.Value.AbsVelocity.Length2D())} | Jumps:{UserSettings[client]!.JumpsHUD}");
                }
            }
        });        
    }
    /*  */
    //  Command player
    //  !hud , !scout , !ujx_reload
    /*  */

    [RequiresPermissions("@css/root")]
    [ConsoleCommand("css_ujx_reload")]
    public void OnjumpxsCommand(CCSPlayerController? controller, CommandInfo info)
    {
        if (controller != null)
        {
            controller.PrintToChat($" \x07 {PluginTag} " + $" \x10 ===================== ");
            controller.PrintToChat($" \x07 {PluginTag} " + $" \x0f Counfig Reloaded    ");
            controller.PrintToChat($" \x07 {PluginTag} " + $" \x0f UJX - \x03 {ModuleVersion}");
            controller.PrintToChat($" \x07 {PluginTag} " + $" \x10 ===================== ");
        }
        
        OnConfigParsed(Config);
    }

    /* Временно убрали дабы краши, а так же нет времени исправить 
    [ConsoleCommand("css_hud")]
    public void OnSpeedsCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (player == null) return;
        var client = player.Index;
        UserSettings[client]!.ShowHUD = !UserSettings[client]!.ShowHUD;
        player.PrintToChat(UserSettings[client]!.ShowHUD ? $"\x10{PluginTag} - \x01HUD: \x06On" : $"\x10{PluginTag} - \x01HUD: \x02Off");
    }
      */  
    [ConsoleCommand("css_scout")]
    public void OnScoutCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (player == null) return;
        
        if(_tries[player.SteamID] <= 0)
        {
            player.PrintToChat($" \x07 {PluginTag} - \x01" + Localizer["main.noneScout"]);
        } else {
        _tries[player.SteamID]--;
        var client = player.Index;
        AddTimer(5.0f, () => GiveWeaponWithAmmo(player,"weapon_ssg08"));
        player.GiveNamedItem("weapon_healthshot");
        player.PrintToChat($" \x07 {PluginTag} - \x01" + Localizer["main.giveScout"]);
        }
    }

	private HookResult OnPlayerConnectFull(EventPlayerConnectFull @event, GameEventInfo info)
	{
		CCSPlayerController? player = @event.Userid;

		if (player == null  || !TAGPLAYER || !player.IsValid || player.IsBot || player.IsHLTV) return HookResult.Continue;

		AddTimer(2.0f, () => SetPlayerClanTag(player));

		return HookResult.Continue;
	}

    public void GiveWeaponWithAmmo(CCSPlayerController player, string weaponName)
    {
        player.GiveNamedItem(weaponName);
        foreach (CHandle<CBasePlayerWeapon> weapon in player.PlayerPawn.Value!.WeaponServices!.MyWeapons)
        {
            if (weapon is { IsValid: true, Value.IsValid: true } && weapon.Value.DesignerName.Contains(weaponName))
            {
                weapon.Value.Clip1 = 0;
                weapon.Value.ReserveAmmo[0] = 0;
            }
        }
    }
    private void OnTick(CCSPlayerController player)
    {

        var client = player.Index;
        var playerPawn = player.PlayerPawn.Value;
        
        if (playerPawn != null)
        {
            var flags = (PlayerFlags)playerPawn.Flags;
            var buttons = player.Buttons;

            if ((UserSettings[client]!.LastFlags & PlayerFlags.FL_ONGROUND) != 0 &&
                (flags & PlayerFlags.FL_ONGROUND) == 0 &&
                (UserSettings[client]!.LastButtons & PlayerButtons.Jump) == 0 && (buttons & PlayerButtons.Jump) != 0)
            {
            }
            else if ((flags & PlayerFlags.FL_ONGROUND) != 0)
                UserSettings[client]!.JumpsCount = 1;
            else if ((UserSettings[client]!.LastButtons & PlayerButtons.Jump) == 0 &&
                     (buttons & PlayerButtons.Jump) != 0 &&
                     UserSettings[client]!.JumpsCount < UserSettings[client]!.NumberOfJumps)
            {
                UserSettings[client]!.JumpsCount ++;
                playerPawn.AbsVelocity.Z = Setting2;
            }

            UserSettings[client]!.LastFlags = flags;
            UserSettings[client]!.LastButtons = buttons;
        }
    }

	private void SetPlayerClanTag(CCSPlayerController? player)
	{
		if (player == null || !player.IsValid || player.IsBot || player.IsHLTV || player.AuthorizedSteamID == null) return;

		string steamid = player.SteamID!.ToString();
		var scoreboardValue = Tagsuser.ToString();
		if (!string.IsNullOrEmpty(scoreboardValue))
		{
			player.Clan = scoreboardValue;
		}
	}

    private HookResult OnPlayerSpawn(EventPlayerSpawn @event, GameEventInfo info)
    {
        var player = @event.Userid;
        if(!player.IsValid || player.IsBot)
            return HookResult.Continue;
        
        if (!_tries.ContainsKey(player.SteamID))
            _tries.Add(player.SteamID, Scout);

        if (player != null && player.IsValid && player.PlayerPawn != null && player.PlayerPawn.IsValid && player.PlayerPawn.Value != null && player.PlayerPawn.Value.IsValid)
        player.PlayerPawn.Value.Render = Color.FromArgb(256, 256, 256, 256);   

        return HookResult.Continue;
    }
}

public class UserSettings
{
    public int JumpsCount { get; set; } = Jumps_X.Setting1;
    public int NumberOfJumps { get; set; } = Jumps_X.Setting1;
    public PlayerButtons LastButtons { get; set; }
    public PlayerFlags LastFlags { get; set; }
    public int JumpsHUD { get; set; }
    public bool ShowHUD { get; set; }
}