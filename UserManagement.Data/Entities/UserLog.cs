using System;
using Newtonsoft.Json;

namespace UserManagement.Models;

public class UserLog
{
    [JsonProperty("id")]
    public int Id { get; set; } = default;
    [JsonProperty("@mt")]
    public string? Description { get; set; }
    [JsonProperty("@t")]
    public DateTime CreatedAt { get; set; }
    [JsonProperty("SourceContext")]
    public string? SourceContext { get; set; }
    [JsonProperty("RequestPath")]
    public string? RequestPath { get; set; }
}
