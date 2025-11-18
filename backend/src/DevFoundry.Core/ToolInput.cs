namespace DevFoundry.Core;

public sealed class ToolInput
{
    public string? Text { get; init; }
    public string? SecondaryText { get; init; }
    public IDictionary<string, object?> Parameters { get; init; } = new Dictionary<string, object?>();
}
