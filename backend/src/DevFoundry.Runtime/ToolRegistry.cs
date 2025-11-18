using System;
using DevFoundry.Core;

namespace DevFoundry.Runtime;

public sealed class ToolRegistry : IToolRegistry
{
    private readonly Dictionary<string, ITool> _tools;

    public ToolRegistry(IEnumerable<ITool> tools)
        : this(tools, configuration: null)
    {
    }

    public ToolRegistry(IEnumerable<ITool> tools, PluginConfiguration? configuration)
    {
        var filteredTools = configuration != null
            ? tools.Where(t => configuration.IsToolEnabled(t.Descriptor.Id))
            : tools;

        _tools = CreateToolDictionary(filteredTools);
    }

    public IEnumerable<ITool> GetAllTools() => _tools.Values;

    public ITool? GetTool(string id)
        => _tools.TryGetValue(id, out var tool) ? tool : null;

    private static Dictionary<string, ITool> CreateToolDictionary(IEnumerable<ITool> tools)
    {
        var dictionary = new Dictionary<string, ITool>(StringComparer.OrdinalIgnoreCase);

        foreach (var tool in tools)
        {
            if (!dictionary.TryAdd(tool.Descriptor.Id, tool))
            {
                Console.Error.WriteLine($"Warning: Duplicate tool ID '{tool.Descriptor.Id}' detected. Using the first registered instance.");
            }
        }

        return dictionary;
    }
}
