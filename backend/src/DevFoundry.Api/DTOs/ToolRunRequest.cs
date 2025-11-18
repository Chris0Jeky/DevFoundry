namespace DevFoundry.Api.DTOs;

public sealed class ToolRunRequest
{
    public string? Text { get; set; }
    public string? SecondaryText { get; set; }
    public Dictionary<string, object?> Parameters { get; set; } = new();
}
