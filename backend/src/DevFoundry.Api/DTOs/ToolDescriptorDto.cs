namespace DevFoundry.Api.DTOs;

public sealed class ToolDescriptorDto
{
    public string Id { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Category { get; set; } = default!;
    public IReadOnlyList<string> Tags { get; set; } = Array.Empty<string>();
    public IReadOnlyList<ToolParameterDescriptorDto> Parameters { get; set; } = Array.Empty<ToolParameterDescriptorDto>();
}

public sealed class ToolParameterDescriptorDto
{
    public string Name { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Type { get; set; } = default!;
    public object? DefaultValue { get; set; }
}
