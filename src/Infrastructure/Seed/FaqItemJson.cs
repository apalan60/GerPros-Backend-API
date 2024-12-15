using System.Text.Json.Serialization;

namespace GerPros_Backend_API.Infrastructure.Seed;

public class FaqItemJson
{
    [JsonPropertyName("question")]
    public string Question { get; set; } = null!;
    [JsonPropertyName("answer")]
    public string Answer { get; set; } = null!;
}
