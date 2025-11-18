namespace DevFoundry.Core;

public sealed class ToolParameterDescriptor
{
    public string Name { get; init; } = default!;
    public string DisplayName { get; init; } = default!;
    public string Description { get; init; } = default!;
    public string Type { get; init; } = default!;
    public object? DefaultValue { get; init; }
}
