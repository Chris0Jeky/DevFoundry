namespace DevFoundry.Core;

public sealed class ToolDescriptor
{
    public string Id { get; init; } = default!;
    public string DisplayName { get; init; } = default!;
    public string Description { get; init; } = default!;
    public ToolCategory Category { get; init; }
    public IReadOnlyList<string> Tags { get; init; } = Array.Empty<string>();
    public IReadOnlyList<ToolParameterDescriptor> Parameters { get; init; } = Array.Empty<ToolParameterDescriptor>();
}
