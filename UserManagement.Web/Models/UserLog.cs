using Newtonsoft.Json;

namespace UserManagement.Models;

public class UserLog
{
    [JsonProperty("id")]
    public int Id { get; set; }
    [JsonProperty("@mt")]
    public string? Description { get; set; }
    [JsonProperty("SourceContext")]
    public string? SourceContext { get; set; }
    [JsonProperty("RequestPath")]
    public string? RequestPath { get; set; }
}
