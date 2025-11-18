namespace DevFoundry.Core;

public sealed class ToolResult
{
    public bool Success { get; init; }
    public string? OutputText { get; init; }
    public string? SecondaryOutputText { get; init; }
    public string? ErrorMessage { get; init; }
    public IDictionary<string, object?> Metadata { get; init; } = new Dictionary<string, object?>();
}
