using System.Text.Json.Serialization;
using CounterStrikeSharp.API.Core;

namespace Jumps_X;

public class PluginConfig : BasePluginConfig
{
    [JsonPropertyName("TagServer")] public string TagServer { get; set; } = "[UJX]";
    //bhop
    [JsonPropertyName("ParramsBhop")] public bool ParramsBhop { get; set; } = true;
    [JsonPropertyName("Parrams1")] public int Parrams1 { get; set; } = 2;
    [JsonPropertyName("Parrams2")] public int Parrams2 { get; set; } = 300; 
    //other
    [JsonPropertyName("TabActive")] public bool TabActive { get; set; } = true;
    [JsonPropertyName("TabUsers")] public string TabUsers { get; set; } = "[UJX - Hero]"; 
    [JsonPropertyName("ScoutActive")] public int ScoutActive { get; set; } = 1;
    [JsonPropertyName("HUDMessage")] public string HUDMessage { get; set; } = "<br> information message <br> for centerhtml<br>"; 

    
}