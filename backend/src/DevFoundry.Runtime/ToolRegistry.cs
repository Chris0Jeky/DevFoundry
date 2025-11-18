using DevFoundry.Core;

namespace DevFoundry.Runtime;

public sealed class ToolRegistry : IToolRegistry
{
    private readonly Dictionary<string, ITool> _tools;

    public ToolRegistry(IEnumerable<ITool> tools)
    {
        _tools = tools.ToDictionary(t => t.Descriptor.Id, StringComparer.OrdinalIgnoreCase);
    }

    public IEnumerable<ITool> GetAllTools() => _tools.Values;

    public ITool? GetTool(string id)
        => _tools.TryGetValue(id, out var tool) ? tool : null;
}
