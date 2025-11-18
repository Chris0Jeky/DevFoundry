namespace DevFoundry.Api.DTOs;

public sealed class ToolRunResult
{
    public bool Success { get; set; }
    public string? OutputText { get; set; }
    public string? SecondaryOutputText { get; set; }
    public string? ErrorMessage { get; set; }
    public Dictionary<string, object?> Metadata { get; set; } = new();
}
