using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using CounterStrikeSharp.API.Modules.Config;
using CounterStrikeSharp.API.Modules.Admin;
using System.Drawing;


namespace UJX;
public partial class UJX : BasePlugin, IPluginConfig<PluginConfig>
{
    /*author*/
    public override string ModuleAuthor => "XnN.Prod";
    public override string ModuleName => "Ultimate Jumps X [UJX]";
    public override string ModuleVersion => "v0.2.0.3(fix)";
    public override string ModuleDescription => "Plugin for BHOP,MG servers.";

    //====================================
    public string UGlobal = "[Server]";
    public static int Setting1 = 2;
    public int Setting2 = 300;
    public bool Bhop_Settings = true;
    //module [1]
    public int UM1 = 1;
    public int UM1_Ammo = 1;
    //module [2]
    public bool UM2 = true;
    public string UM2_P  = "[ADMIN]";
    public string UM2_V = "[VIP]";
    public string UM2_A = "[USER]";
    //module [3]
    public bool UM3 = false;
     //module [4]
    public int UM4 = 1;
    //===================================
    private Dictionary<ulong, int> _tries = new();
    private Dictionary<ulong, int> _t1ries = new();
    private bool isHookEvent;
    private static readonly UserSettings?[] UserSettings = new UserSettings?[120];
    public PluginConfig Config { get; set; }

    public void OnConfigParsed(PluginConfig config)
    {
        config = ConfigManager.Load<PluginConfig>("UJX");

        UGlobal = config.GlobalTAG;
        Setting1 = config.Parrams1;
        Setting2 = config.Parrams2;
        Bhop_Settings = config.ParramsBhop;
        UM1 = config.ScoutActive;
        UM1_Ammo = config.ScoutAmmo;
        UM2 = config.ScoreBoardActive;
        UM2_P = config.ScoreAdmin;
        UM2_V = config.ScoreVIP;
        UM2_A = config.ScoreUsers;
        UM3 = config.HideActive;
        UM4 = config.RespawnActive;
        Console.WriteLine($"{UGlobal} - " + Localizer["main.configreload"]);
        Config = config;
    } 

    public override void Load(bool hotReload)
    {        
        Console.WriteLine($"Loading...");
        Console.WriteLine($"----------------------------------------");
        Console.WriteLine($" Plugin Name - {ModuleName}");
        Console.WriteLine($" Author - {ModuleAuthor}");
        Console.WriteLine($" ");
        Console.WriteLine($" Plugin version - {ModuleVersion}");
        Console.WriteLine($"-------------- [ ENABLE ] --------------");
        Console.WriteLine($" ConVars Status - ");
        if (Config.ScoreBoardActive == true)
        {
              Console.WriteLine("tags module (-None-)");
        }
        else if (Config.ScoreBoardActive == false)
        {
             Console.WriteLine("tags module (Enable)");
        }


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
            foreach (var player in Utilities.GetPlayers()) _tries.TryAdd(player.SteamID, UM1);
                      
            _t1ries.Clear();
            foreach (var player in Utilities.GetPlayers()) _t1ries.TryAdd(player.SteamID, UM4);
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
                    player.PrintToCenterHtml($"\n Speed:{Math.Round(player.PlayerPawn.Value.AbsVelocity.Length2D())} | Jumps:{UserSettings[client]!.JumpsHUD}");
                }
            }
        });        
    }

	private HookResult OnPlayerConnectFull(EventPlayerConnectFull @event, GameEventInfo info)
	{
		CCSPlayerController? player = @event.Userid;

		if (player == null  || !UM2 || !player.IsValid || player.IsBot || player.IsHLTV) return HookResult.Continue;

		AddTimer(1.05f, () => SetPlayerClanTag(player));

        if (UM3 == true){ AddTimer(0.66f, () => SetPlayerHide(player));}

		return HookResult.Continue;
	}

    public void GiveWeaponWithAmmo(CCSPlayerController player, string weaponName)
    {
        player.GiveNamedItem(weaponName);
        foreach (CHandle<CBasePlayerWeapon> weapon in player.PlayerPawn.Value!.WeaponServices!.MyWeapons)
        {
            if (weapon is { IsValid: true, Value.IsValid: true } && weapon.Value.DesignerName.Contains(weaponName))
            {
                weapon.Value.Clip1 = UM1_Ammo;
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

    public void TimeToRespawn(CCSPlayerController player)
    {
        AddTimer(3.0f, () => player.Respawn());
    }

	public void SetPlayerClanTag(CCSPlayerController player)
	{
		string steamid = player.SteamID!.ToString();

        bool GUM2_P = AdminManager.PlayerHasPermissions(player, "@css/root");
        bool GUM2_V = AdminManager.PlayerHasPermissions(player, "@css/reservation");

        if (GUM2_P)
        {
            var score_admin = UM2_P.ToString(); 
            if (!string.IsNullOrEmpty(score_admin))
            {
                player.Clan = score_admin; 
            }
        }  else if (GUM2_V)
        { 
            var score_vip = UM2_V.ToString();
            if (!string.IsNullOrEmpty(score_vip))
            { 
                player.Clan = score_vip; 
            }
        } else {
            var everyone = UM2_A.ToString();
            if (!string.IsNullOrEmpty(everyone))
            {
                player.Clan = everyone; 
            }
        }
	}

    private void SetPlayerHide(CCSPlayerController player)
    {
        if (player == null || !player.IsValid || player.IsBot || player.AuthorizedSteamID == null) return;

        player.PlayerPawn.Value.Render = Color.FromArgb(254, 255, 255, 255); 
        Utilities.SetStateChanged(player.PlayerPawn.Value, "CBaseModelEntity", "m_clrRender"); 
    }

    private HookResult OnPlayerSpawn(EventPlayerSpawn @event, GameEventInfo info)
    {
        var player = @event.Userid;
        if(!player.IsValid || player.IsBot)
            return HookResult.Continue;
        
        if (!_tries.ContainsKey(player.SteamID))
            _tries.Add(player.SteamID, UM1);
        if (!_t1ries.ContainsKey(player.SteamID))
            _t1ries.Add(player.SteamID, UM4);
            return HookResult.Continue;

    }
}

public class UserSettings
{
    public int JumpsCount { get; set; } = UJX.Setting1;
    public int NumberOfJumps { get; set; } = UJX.Setting1;
    public PlayerButtons LastButtons { get; set; }
    public PlayerFlags LastFlags { get; set; }
    public int JumpsHUD { get; set; }
    public bool ShowHUD { get; set; }
}
