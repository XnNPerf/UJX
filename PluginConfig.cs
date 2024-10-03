using System.Text.Json.Serialization;
using CounterStrikeSharp.API.Core;

namespace UJX;

public class PluginConfig : BasePluginConfig
{
    [JsonPropertyName("GlobalTAG")] public string GlobalTAG { get; set; } = "[UJX]";
    //bhop
    [JsonPropertyName("ParramsBhop")] public bool ParramsBhop { get; set; } = true;
    [JsonPropertyName("Parrams1")] public int Parrams1 { get; set; } = 2;
    [JsonPropertyName("Parrams2")] public int Parrams2 { get; set; } = 300; 
    //module 1 
    [JsonPropertyName("ScoutActive")] public int ScoutActive { get; set; } = 1; 
    [JsonPropertyName("ScoutAmmo")] public int ScoutAmmo { get; set; } = 0;  
    //module 2 
    [JsonPropertyName("ScoreBoardActive")] public bool ScoreBoardActive { get; set; } = true;
    [JsonPropertyName("ScoreAdmin")] public string ScoreAdmin { get; set; } = "[UJX - Admin]";
    [JsonPropertyName("ScoreVIP")] public string ScoreVIP { get; set; } = "[UJX - VIP]"; 
    [JsonPropertyName("ScoreUsers")] public string ScoreUsers { get; set; } = "[UJX - USER]";
    //module 3 
    [JsonPropertyName("HideActive")] public bool HideActive { get; set; } = true;
    //module 4 
    [JsonPropertyName("RespawnActive")] public int RespawnActive { get; set; } = 1; 
}